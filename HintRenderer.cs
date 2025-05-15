using Exiled.API.Features;
using HintServiceMeow.Core;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Utilities;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HSMHint = HintServiceMeow.Core.Models.Hints.Hint;
using ParlamataUI.XPSystem;
using MEC;
using PlayerRoles;

namespace ParlamataUI
{
    public static class HintRenderer
    {
        private static Plugin Plugin => ParlamataUI.Plugin.Instance;
        private static Config Config => Plugin.Config;

        private static readonly Dictionary<string, HSMHint> ActiveHints = new();
        private static readonly Dictionary<string, HSMHint> ServerNameHints = new();
        private static readonly Dictionary<string, HSMHint> EffectHints = new();
        private static readonly Dictionary<string, HSMHint> XpHints = new();
        private static readonly Dictionary<string, HSMHint> XpFeedbackHints = new();

        public static void RenderUI(Player player)
        {
            if (!player.IsAlive && !Config.EnableForSpectators)
                return;

            var target = GetTargetPlayer(player);
            if (target == null)
                return;

            string roleColor = target.Role.Color.ToHex();
            var sb = new StringBuilder();

            // === XP/Level Hint (център-горе) ===
            if (Config.ShowXP)
            {
                var xpData = XPManager.GetData(player);
                float aspectRatio = player.ReferenceHub.aspectRatioSync.AspectRatio;
                string xpUserId = player.UserId;

                int currentLevel = xpData.Level;
                int currentXP = xpData.XP;
                int neededXP = XPManager.GetXPRequired(currentLevel + 1);

                string xpText = $"<b><color=#FFD700>⭐ Level {currentLevel}</color></b> <color=#AAAAAA>|</color> " +
                                $"<color=#00C3FF>XP: {currentXP}</color> <color=#AAAAAA>/</color> <color=#00FFAA>{neededXP}</color>";

                if (!XpHints.TryGetValue(xpUserId, out var xpHint))
                {
                    xpHint = new HSMHint
                    {
                        FontSize = 24,
                        YCoordinate = 20,
                        XCoordinate = GetCenterXPosition(aspectRatio),
                        Alignment = HintAlignment.Center
                    };

                    PlayerDisplay.Get(player).AddHint(xpHint);
                    XpHints[xpUserId] = xpHint;

                    if (Config.Debug)
                        Log.Debug($"[ParlamataUI] Added XP hint for {player.Nickname}.");
                }

                xpHint.Text = xpText;
            }



            // === ЛЯВО-ДОЛУ - основен info хинт ===
            sb.AppendLine($"<color={roleColor}>{Config.EmojiIcons.Name}</color> | {RealNameResolver.GetDisplayName(target, Config.ShowRealName)}");

            if (Config.ShowRole)
                sb.AppendLine($"<color={roleColor}>{Config.EmojiIcons.Role}</color> | {target.Role.Name}");

            if (Config.ShowSpectators && target.IsAlive)
                sb.AppendLine($"<color={roleColor}>{Config.EmojiIcons.Spectators}</color> | {SpectatorTracker.GetSpectatorCount(target)}");

            if (Config.ShowKills && target.IsAlive)
            {
                int kills = KillTracker.GetKills(target);
                int traps = KillTracker.GetTrappedVictims(target);

                if (target.Role.Type == RoleTypeId.Scp106)
                {
                    int total = kills + traps;
                    sb.AppendLine($"<color={roleColor}>{Config.EmojiIcons.Traps}</color> | {total}");
                }
                else
                {
                    sb.AppendLine($"<color={roleColor}>{Config.EmojiIcons.Kills}</color> | {kills}");
                }
            }

            if (Config.ShowElapsedRoundTime)
                sb.AppendLine($"<color={roleColor}>{Config.EmojiIcons.Timer}</color> | {RoundTimer.GetFormattedElapsedTime()}");

            float aspect = player.ReferenceHub.aspectRatioSync.AspectRatio;

            // === Основен Hint (ляво-долу) ===
            string userId = player.UserId;
            if (!ActiveHints.TryGetValue(userId, out var hint))
            {
                hint = new HSMHint
                {
                    FontSize = 20,
                    YCoordinate = 920,
                    XCoordinate = GetLeftXPosition(aspect),
                    Alignment = HintAlignment.Left
                };

                PlayerDisplay.Get(player).AddHint(hint);
                ActiveHints[userId] = hint;

                if (Config.Debug)
                    Log.Debug($"[ParlamataUI] Added main hint for {player.Nickname}.");
            }

            hint.Text = sb.ToString();

            // === Server Name Hint (център-долу) ===
            if (!ServerNameHints.ContainsKey(userId))
            {
                var serverHint = new HSMHint
                {
                    FontSize = 25,
                    YCoordinate = 1060,
                    XCoordinate = GetCenterXPosition(aspect),
                    Alignment = HintAlignment.Center,
                    Text = $"<size=30>{Config.ServerName}</size>"
                };

                PlayerDisplay.Get(player).AddHint(serverHint);
                ServerNameHints[userId] = serverHint;

                if (Config.Debug)
                    Log.Debug($"[ParlamataUI] Added ServerName hint for {player.Nickname}.");
            }

            // === Ефекти върху играча (горе-дясно) ===
            var effectsList = new StringBuilder();

            foreach (var effect in player.ActiveEffects)
            {
                if (effect.IsEnabled)
                {
                    int remaining = Mathf.CeilToInt(effect.TimeLeft);
                    string effectName = effect.GetType().Name;
                    effectsList.AppendLine($"<color=#ff6a00>{effectName}</color> <size=20>[{remaining}s]</size>");
                }
            }

            string effectUserId = player.UserId;

            if (effectsList.Length == 0)
            {
                // Няма ефекти — махни Hint-а, ако съществува
                if (EffectHints.TryGetValue(effectUserId, out var activeHint))
                {
                    PlayerDisplay.Get(player).RemoveHint(activeHint);
                    EffectHints.Remove(effectUserId);

                    if (Config.Debug)
                        Log.Debug($"[ParlamataUI] Removed Effect hint for {player.Nickname}.");
                }
            }
            else
            {
                // Има ефекти — създай/ъпдейтни Hint-а
                if (!EffectHints.TryGetValue(effectUserId, out var effectHint))
                {
                    effectHint = new HSMHint
                    {
                        FontSize = 20,
                        YCoordinate = 720,
                        XCoordinate = GetLeftXPosition(aspect),
                        Alignment = HintAlignment.Left
                    };

                    PlayerDisplay.Get(player).AddHint(effectHint);
                    EffectHints[effectUserId] = effectHint;

                    if (Config.Debug)
                        Log.Debug($"[ParlamataUI] Added Effect hint for {player.Nickname}.");
                }

                effectHint.Text = effectsList.ToString();
            }
        }

        // === Поддържаща функция за адаптивна X позиция на ляв хинт ===
        private static float GetLeftXPosition(float aspectRatio)
        {
            return (622.27444f * Mathf.Pow(aspectRatio, 3f)) +
                   (-2869.08991f * Mathf.Pow(aspectRatio, 2f)) +
                   (3827.03102f * aspectRatio) -
                   1580.21554f;
        }

        // === Центриране (може да е винаги 0, но запазваме за бъдеща логика) ===
        private static float GetCenterXPosition(float aspectRatio)
        {
            return 0f;
        }

        private static Player GetTargetPlayer(Player player)
        {
            if (player.IsAlive)
                return player;

            foreach (var target in Player.List)
                if (target.IsAlive && target.CurrentSpectatingPlayers.Contains(player))
                    return target;

            return null!;
        }

        public static void ClearAllHints()
        {
            ActiveHints.Clear();
            ServerNameHints.Clear();
            EffectHints.Clear();
            XpHints.Clear();
            XpFeedbackHints.Clear();

            if (Plugin.Config.Debug)
                Log.Debug("[ParlamataUI] Cleared hint caches on round end.");
        }

        public static void RemoveHintsFor(string userId)
        {
            ActiveHints.Remove(userId);
            ServerNameHints.Remove(userId);
            EffectHints.Remove(userId);
            XpHints.Remove(userId);
            XpFeedbackHints.Remove(userId);

            if (Plugin.Config.Debug)
                Log.Debug($"[ParlamataUI] Removed cached hints for {userId} (player left/destroyed).");
        }

        public static void ShowXPMessage(Player player, string message)
        {
            string userId = player.UserId;
            float aspect = player.ReferenceHub.aspectRatioSync.AspectRatio;

            if (!XpFeedbackHints.TryGetValue(userId, out var hint))
            {
                hint = new HSMHint
                {
                    FontSize = 18,
                    YCoordinate = 50,
                    XCoordinate = GetCenterXPosition(aspect),
                    Alignment = HintAlignment.Center
                };

                PlayerDisplay.Get(player).AddHint(hint);
                XpFeedbackHints[userId] = hint;

                if (Plugin.Config.Debug)
                    Log.Debug($"[ParlamataUI] Added XP Feedback hint for {player.Nickname}.");
            }

            hint.Text = $"<color=#7af57a>{message}</color>";

            // → Скрий го след 3 секунди
            DestroyHintIn(player, hint, 3f);
        }

        public static void DestroyHintIn(Player player, HSMHint hint, float delay)
        {
            string tag = $"xp_msg_{player.UserId}";
            Timing.KillCoroutines(tag);
            CoroutineHandle handle = Timing.RunCoroutine(DestroyHintCoroutine(player, hint, delay));
            handle.Tag = tag;
        }

        private static IEnumerator<float> DestroyHintCoroutine(Player player, HSMHint hint, float delay)
        {
            yield return Timing.WaitForSeconds(delay);

            PlayerDisplay.Get(player).RemoveHint(hint);

            if (XpFeedbackHints.TryGetValue(player.UserId, out var stored) && stored == hint)
                XpFeedbackHints.Remove(player.UserId);

            if (Plugin.Config.Debug)
                Log.Debug($"[ParlamataUI] XP feedback hint destroyed for {player.Nickname}.");
        }
    }
}
