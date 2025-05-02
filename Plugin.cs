using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;

namespace ParlamataUI
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "LaFesta1749";
        public override string Name => "ParlamataUI";
        public override Version Version => new(1, 0, 2);
        public override Version RequiredExiledVersion => new(9, 6, 0);

        public static Plugin Instance { get; private set; } = null!;

        private CoroutineHandle _renderCoroutine;

        public override void OnEnabled()
        {
            Instance = this;

            if (Config.Debug)
                Log.Info("ParlamataUI: Plugin enabled. Initializing systems...");

            RoundTimer.Enable();
            KillTracker.Enable();

            _renderCoroutine = Timing.RunCoroutine(RenderLoop());

            Exiled.Events.Handlers.Server.RoundEnded += RoundResetHandler.OnRoundEnded;

            Exiled.Events.Handlers.Player.Destroying += PlayerCleanupHandler.OnPlayerDestroying;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Timing.KillCoroutines(_renderCoroutine);

            KillTracker.Disable();
            RoundTimer.Disable();

            if (Config.Debug)
                Log.Info("ParlamataUI: Plugin disabled.");

            Instance = null!;

            Exiled.Events.Handlers.Server.RoundEnded -= RoundResetHandler.OnRoundEnded;

            Exiled.Events.Handlers.Player.Destroying -= PlayerCleanupHandler.OnPlayerDestroying;

            base.OnDisabled();
        }

        private IEnumerator<float> RenderLoop()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(Config.UpdateInterval);

                foreach (var player in Player.List)
                {
                    if (!player.IsConnected)
                        continue;

                    HintRenderer.RenderUI(player);
                }
            }
        }
    }
}
