using System;
using System.Collections.Generic;
using Torch;

namespace ALE_StrongUnknownSignalLogger {
    public class ALE_StrongUnknownSignalLoggerConfig : ViewModel {


        private string _loggingFileName = "signals-${shortdate}.log";

        private bool _logRegularSignals = false;

        public string LoggingFileName { get => _loggingFileName; set => SetValue(ref _loggingFileName, value); }

        public bool LogRegularSignals { get => _logRegularSignals; set => SetValue(ref _logRegularSignals, value); }
    }
}
