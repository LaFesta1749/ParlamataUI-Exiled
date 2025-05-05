using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using ParlamataUI.XPSystem;

namespace ParlamataUI
{
    public class Config : IConfig
    {
        [Description("Enable or disable the entire plugin.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Enable debug logs.")]
        public bool Debug { get; set; } = false;

        [Description("Show XP and Level info at the top-center of the screen.")]
        public bool ShowXP { get; set; } = true;

        [Description("XP rewards for different events.")]
        public XPRewardsConfig XPRewards { get; set; } = new();

        [Description("How often the UI should update (in seconds).")]
        public float UpdateInterval { get; set; } = 1f;

        [Description("Should the UI be shown for spectators observing a player?")]
        public bool EnableForSpectators { get; set; } = true;

        [Description("Show the player's kill count.")]
        public bool ShowKills { get; set; } = true;

        [Description("Show how many people are spectating you.")]
        public bool ShowSpectators { get; set; } = true;

        [Description("Show the real name of the player if a nickname is set.")]
        public bool ShowRealName { get; set; } = true;

        [Description("Show the role/class of the player.")]
        public bool ShowRole { get; set; } = true;

        [Description("Show the elapsed round time.")]
        public bool ShowElapsedRoundTime { get; set; } = true;

        [Description("Emoji icons used for various UI lines.")]
        public EmojiConfig EmojiIcons { get; set; } = new();

        [Description("Formatted server name shown at the bottom of the UI.")]
        public string ServerName { get; set; } = "<size=25><b><color=#A8FFF4>[</color><color=#FFFFFF>B</color><color=#4BFF36>U</color><color=#FF4242>L</color><color=#A8FFF4>/</color><color=#FFE542>ENG</color><color=#A8FFF4>]</color> <color=#ffffff>B</color><color=#c1e8d2>u</color><color=#84d1a4>l</color><color=#46ba77>g</color><color=#42a05b>a</color><color=#798451>r</color><color=#b06846>i</color><color=#e74c3c>a</color> - <color=#3498db>ПАРЛАМАТА</color></size>";
    }

    public class EmojiConfig
    {
        [Description("Emoji for the player's name.")]
        public string Name { get; set; } = "👤";

        [Description("Emoji for the player's role.")]
        public string Role { get; set; } = "🎭";

        [Description("Emoji for how many spectators are watching.")]
        public string Spectators { get; set; } = "👥";

        [Description("Emoji for kill count.")]
        public string Kills { get; set; } = "🔪";

        [Description("Emoji for the round timer.")]
        public string Timer { get; set; } = "🕒";
    }
}
