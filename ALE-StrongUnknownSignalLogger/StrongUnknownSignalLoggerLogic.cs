using ALE_Core.Utils;
using NLog;
using NLog.Config;
using NLog.Targets;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace ALE_StrongUnknownSignalLogger {
    
    public class StrongUnknownSignalLoggerLogic {

        private static readonly Logger Log = LogManager.GetLogger("StrongUnknownSignalLogger");

        private readonly StrongUnknownSignalLoggerPlugin plugin;

        public StrongUnknownSignalLoggerLogic(StrongUnknownSignalLoggerPlugin plugin) {
            this.plugin = plugin;
        }

        public void ApplyLogging() {

            var rules = LogManager.Configuration.LoggingRules;

            for (int i = rules.Count - 1; i >= 0; i--) {

                var rule = rules[i];

                if (rule.LoggerNamePattern == "OwnershipLogger")
                    rules.RemoveAt(i);
            }

            var config = plugin.Config;

            var logTarget = new FileTarget {
                FileName = "Logs/" + config.LoggingFileName,
                Layout = "${var:logStamp} ${var:logContent}"
            };

            var logRule = new LoggingRule("StrongUnknownSignalLogger", LogLevel.Debug, logTarget) {
                Final = true
            };

            rules.Insert(0, logRule);

            LogManager.Configuration.Reload();
        }

        public void StartLogging() {
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed += PrefabSpawned;
        }

        public void StopLogging() {
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed -= PrefabSpawned;
        }

        private void PrefabSpawned(long entityId, string prefabName) {

            var entity = MyEntities.GetEntityById(entityId);

            if (entity == null || !(entity is MyCubeGrid))
                return;

            MyCubeGrid grid = entity as MyCubeGrid;

            foreach(var block in grid.CubeBlocks) {

                var cubeBlock = block.FatBlock;
                 
                if (!(cubeBlock is MyButtonPanel buttonPanel))
                    continue;

                buttonPanel.Components.ComponentAdded += ActionComponentAdded;
                buttonPanel.OnMarkForClose += ActionMarkedForClose;
            }
        }

        private void ActionMarkedForClose(MyEntity obj) {

            obj.Components.ComponentAdded -= ActionComponentAdded;
            obj.OnMarkForClose -= ActionMarkedForClose;
        }

        private void ActionComponentAdded(Type type, MyEntityComponentBase componentBase) {

            var entity = componentBase.Entity;

            if (!(componentBase is MyContainerDropComponent dropComponent))
                return;

            try {

                if(!plugin.Config.LogRegularSignals && !dropComponent.Competetive)
                    return;

                if (!(entity is MyButtonPanel buttonPanel))
                    return;

                string ownerName = PlayerUtils.GetPlayerNameById(dropComponent.Owner);

                var grid = buttonPanel.CubeGrid;
                var position = buttonPanel.PositionComp.GetPosition();

                Log.Info(ownerName + " - " + grid.DisplayName + " - GPS:" + ownerName + " - " + grid.DisplayName + ":" + position.X + ":" + position.Y + ":" + position.Z + ":#FFFFFF00:");

            } finally {
                entity.Components.ComponentAdded -= ActionComponentAdded;
            }
        }
    }
}
