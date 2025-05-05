using System.ComponentModel;

namespace ParlamataUI.XPSystem
{
    public class XPRewardsConfig
    {
        [Description("XP when killing a player.")]
        public int OnKill { get; set; } = 10;

        [Description("XP when dying.")]
        public int OnDeath { get; set; } = 2;

        [Description("XP when escaping.")]
        public int OnEscape { get; set; } = 25;

        [Description("XP when winning the round.")]
        public int OnWin { get; set; } = 20;

        [Description("XP when opening or interacting with a door.")]
        public int OnDoorOpen { get; set; } = 1;

        [Description("XP when picking up an item.")]
        public int OnPickupItem { get; set; } = 2;

        [Description("XP when dropping an item.")]
        public int OnDropItem { get; set; } = 1;

        [Description("XP when throwing a grenade.")]
        public int OnThrowGrenade { get; set; } = 2;

        [Description("XP when activating a generator.")]
        public int OnGeneratorActivate { get; set; } = 5;

        [Description("XP when using SCP-914.")]
        public int OnUpgradeItem { get; set; } = 3;

        [Description("XP when using medical items.")]
        public int OnUseMedical { get; set; } = 4;

        [Description("XP when spawning.")]
        public int OnSpawn { get; set; } = 3;

        [Description("XP when resurrecting a player as SCP-049.")]
        public int OnResurrect { get; set; } = 10;
    }
}
