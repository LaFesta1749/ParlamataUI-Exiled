namespace ParlamataUI.XPSystem
{
    public static class XPRewards
    {
        public static int OnKill => Plugin.Instance.Config.XPRewards.OnKill;
        public static int OnDeath => Plugin.Instance.Config.XPRewards.OnDeath;
        public static int OnEscape => Plugin.Instance.Config.XPRewards.OnEscape;
        public static int OnWin => Plugin.Instance.Config.XPRewards.OnWin;
        public static int OnDoorOpen => Plugin.Instance.Config.XPRewards.OnDoorOpen;
        public static int OnPickupItem => Plugin.Instance.Config.XPRewards.OnPickupItem;
        public static int OnDropItem => Plugin.Instance.Config.XPRewards.OnDropItem;
        public static int OnThrowGrenade => Plugin.Instance.Config.XPRewards.OnThrowGrenade;
        public static int OnGeneratorActivate => Plugin.Instance.Config.XPRewards.OnGeneratorActivate;
        public static int OnUpgradeItem => Plugin.Instance.Config.XPRewards.OnUpgradeItem;
        public static int OnUseMedical => Plugin.Instance.Config.XPRewards.OnUseMedical;
        public static int OnSpawn => Plugin.Instance.Config.XPRewards.OnSpawn;
        public static int OnResurrect => Plugin.Instance.Config.XPRewards.OnResurrect;
    }
}
