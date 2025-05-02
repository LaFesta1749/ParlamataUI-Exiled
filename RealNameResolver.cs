using Exiled.API.Features;

namespace ParlamataUI
{
    public static class RealNameResolver
    {
        public static string GetDisplayName(Player player, bool showRealName)
        {
            if (player == null)
                return string.Empty;

            if (!showRealName || player.Nickname == player.DisplayNickname)
                return player.Nickname;

            // Показва реалното име в скоби след setnick
            return $"{player.DisplayNickname} ({player.Nickname})";
        }
    }
}
