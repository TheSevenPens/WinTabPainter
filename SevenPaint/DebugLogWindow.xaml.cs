using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SevenPaint
{
    public partial class DebugLogWindow : Window
    {
        private ObservableCollection<string> _logEntries = new ObservableCollection<string>();
        private const int MaxLogEntries = 4000;

        public DebugLogWindow()
        {
            InitializeComponent();
            LstLog.ItemsSource = _logEntries;
        }

        public bool OnlyLogDown => ChkLogDownOnly.IsChecked == true;

        public void Log(string message)
        {
            if (ChkPause.IsChecked == true) return;

            _logEntries.Add(message);

            // Keep log size manageable
            while (_logEntries.Count > MaxLogEntries)
            {
                _logEntries.RemoveAt(0);
            }

            // Auto-scroll to bottom
            if (VisualTreeHelper.GetChildrenCount(LstLog) > 0)
            {
                var border = VisualTreeHelper.GetChild(LstLog, 0) as Decorator;
                if (border?.Child is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToBottom();
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            _logEntries.Clear();
        }
    }
}
