using LiteDB;

namespace ParlamataUI.XPSystem
{
    public class PlayerXP
    {
        [BsonId]
        public string UserId { get; set; } = string.Empty;

        public int XP { get; set; } = 0;

        public int Level { get; set; } = 1;
    }
}
