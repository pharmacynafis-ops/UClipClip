using System.Windows;
using System.Windows.Controls;
using Models.UClipClip;

namespace UClipClip
{
    public partial class MainWindow : Window
    {
        private TextPage? _textPage;
        private FilesPage? _filesPage;
        private SettingsPage? _settingsPage;
        private bool _isInitialized = false;
        private AppSettings _appSettings = null!;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load settings
            _appSettings = SettingsManager.Load();

            // Apply saved size
            this.Width = _appSettings.WindowWidth;
            this.Height = _appSettings.WindowHeight;

            // Position top-right after size is applied
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Top;

            // Start monitoring
            ClipboardMonitor.Start();
            ClipboardMonitor.TextCopied += OnTextCopied;
            ClipboardMonitor.FilesCopied += OnFilesCopied;

            // Create single instances of each page
            _textPage = new TextPage();
            _filesPage = new FilesPage();
            _settingsPage = new SettingsPage();

            // Show default page
            ContentFrame.Navigate(_textPage);

            _isInitialized = true;
        }

        private void OnTextCopied(string text)
        {
            Dispatcher.Invoke(() =>
            {
                if (_appSettings.AppearOnCopy)
                {
                    Show();
                    Topmost = true;
                    Topmost = false;
                }
                _textPage?.AddText(text);
            });
        }

        private void OnFilesCopied(string[] files)
        {
            Dispatcher.Invoke(() =>
            {
                if (_appSettings.AppearOnCopy)
                {
                    Show();
                    Topmost = true;
                    Topmost = false;
                }
                _filesPage?.AddFiles(files);
            });
        }

        private void Tab_Checked(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized) return;

            if (sender == TabText)
                ContentFrame.Navigate(_textPage);
            else if (sender == TabFiles)
                ContentFrame.Navigate(_filesPage);
            else if (sender == TabSettings)
                ContentFrame.Navigate(_settingsPage);
        }
        private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
{
    DragMove();
}

private void CloseButton_Click(object sender, RoutedEventArgs e)
{
    System.Windows.Application.Current.Shutdown();
}
    }
}