using NLog;
using System;
using System.IO;
using System.Windows.Controls;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Session;

namespace ALE_StrongUnknownSignalLogger {
    public class StrongUnknownSignalLoggerPlugin : TorchPluginBase, IWpfPlugin {

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly string CONFIG_FILE_NAME = "ALE_StrongUnknownSignalLoggerConfig.cfg";

        private StrongUnknownSignalLoggerControl _control;
        public UserControl GetControl() => _control ?? (_control = new StrongUnknownSignalLoggerControl(this));

        private Persistent<ALE_StrongUnknownSignalLoggerConfig> _config;
        public ALE_StrongUnknownSignalLoggerConfig Config => _config?.Data;

        private StrongUnknownSignalLoggerLogic logic;

        public override void Init(ITorchBase torch) {
            
            base.Init(torch);

            SetupConfig();

            this.logic = new StrongUnknownSignalLoggerLogic(this);
            this.logic.ApplyLogging();

            var sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (sessionManager != null)
                sessionManager.SessionStateChanged += SessionChanged;
            else
                Log.Warn("No session manager loaded!");
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state) {

            switch (state) {

                case TorchSessionState.Loaded:
                    logic.StartLogging();
                    Log.Info("Signal Logging started!");
                    break;

                case TorchSessionState.Unloading:
                    logic.StopLogging();
                    Log.Info("Signal Logging stopped!");
                    break;
            }
        }

        private void SetupConfig() {

            var configFile = Path.Combine(StoragePath, CONFIG_FILE_NAME);

            try {

                _config = Persistent<ALE_StrongUnknownSignalLoggerConfig>.Load(configFile);

            } catch (Exception e) {
                Log.Warn(e);
            }

            if (_config?.Data == null) {

                Log.Info("Create Default Config, because none was found!");

                _config = new Persistent<ALE_StrongUnknownSignalLoggerConfig>(configFile, new ALE_StrongUnknownSignalLoggerConfig());
                _config.Save();
            }
        }

        public void Save() {
            
            try {
                
                _config.Save();
                this.logic.ApplyLogging();

                Log.Info("Configuration Saved.");
            } catch (IOException e) {
                Log.Warn(e, "Configuration failed to save");
            }
        }
    }
}
