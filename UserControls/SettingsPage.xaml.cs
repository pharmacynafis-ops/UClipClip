using System.Windows;
using System.Windows.Controls;
using Models.UClipClip;

namespace UClipClip
{
    public partial class SettingsPage : System.Windows.Controls.UserControl
    {
        private AppSettings _settings = null!;

        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            _settings = SettingsManager.Load();
            WidthBox.Text = _settings.WindowWidth.ToString();
            HeightBox.Text = _settings.WindowHeight.ToString();
            AppearOnCopyCheck.IsChecked = _settings.AppearOnCopy;
            RunOnStartupCheck.IsChecked = _settings.RunOnStartup;
        }

        private void SettingChanged(object sender, RoutedEventArgs e)
        {
            // Not used; Apply button does all
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            if (double.TryParse(WidthBox.Text, out double width) && width >= 200 && width <= 800)
                _settings.WindowWidth = width;
            else
            {
                System.Windows.MessageBox.Show("Width must be between 200 and 800", "Invalid", 
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
                hasError = true;
            }

            if (double.TryParse(HeightBox.Text, out double height) && height >= 300 && height <= 1000)
                _settings.WindowHeight = height;
            else
            {
                System.Windows.MessageBox.Show("Height must be between 300 and 1000", "Invalid", 
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
                hasError = true;
            }

            if (hasError) return;

            _settings.AppearOnCopy = AppearOnCopyCheck.IsChecked == true;
            _settings.RunOnStartup = RunOnStartupCheck.IsChecked == true;

            SettingsManager.Save(_settings);
            SettingsManager.SetRunOnStartup(_settings.RunOnStartup);

            System.Windows.MessageBox.Show("Settings saved. Please restart the application for all changes to take effect.", 
                                            "UClipClip", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}