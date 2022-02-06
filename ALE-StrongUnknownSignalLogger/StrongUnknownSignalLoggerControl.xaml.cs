using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ALE_StrongUnknownSignalLogger {
    public partial class StrongUnknownSignalLoggerControl : UserControl {

        private StrongUnknownSignalLoggerPlugin Plugin { get; }

        private StrongUnknownSignalLoggerControl() {
            InitializeComponent();
        }

        public StrongUnknownSignalLoggerControl(StrongUnknownSignalLoggerPlugin plugin) : this() {
            Plugin = plugin;
            DataContext = plugin.Config;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e) {
            Plugin.Save();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
