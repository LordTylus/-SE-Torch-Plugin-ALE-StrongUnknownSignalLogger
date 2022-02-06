using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace ALE_StrongUnknownSignalLogger {

    [Category("signallogger")]
    public class StrongUnknownSignalLoggerCommands : CommandModule {

        [Command("spawn", "Debug Command, to manually spawn signals.")]
        [Permission(MyPromoteLevel.Owner)]
        public void SpawnContainerDrop() {

            if(Context.Player == null) {
                Context.Respond("Ingame only!");
                return;
            }

            ulong steamID = Context.Player.SteamUserId;

            MySessionComponentContainerDropSystem.SpawnContainerDrop(steamID);

            Context.Respond("Done!");
        }
    }
}
