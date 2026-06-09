using System.Windows;
using System.Windows.Controls;
using Models.UClipClip;

namespace UClipClip
{
    public partial class SettingsPage : System.Windows.Controls.UserControl
    {
        private AppSettings _settings = null!;
        public Action<string>? OnThemeChanged { get; set; }
        public Action? OnOpenAbout { get; set; }
        public Action? OnOpenDeveloper { get; set; }

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
            SoundOnCopyCheck.IsChecked = _settings.SoundOnCopy;
            ShowTimestampCheck.IsChecked = _settings.ShowTimestamp;
            MaxTextItemsBox.Text = _settings.MaxTextItems.ToString();
            MaxFileItemsBox.Text = _settings.MaxFileItems.ToString();

            foreach (ComboBoxItem item in ThemeCombo.Items)
            {
                if (item.Content.ToString() == _settings.Theme)
                {
                    ThemeCombo.SelectedItem = item;
                    break;
                }
            }
        }

        private void ThemeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeCombo.SelectedItem is ComboBoxItem selected)
            {
                string theme = selected.Content.ToString()!;
                OnThemeChanged?.Invoke(theme);
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            OnOpenAbout?.Invoke();
        }

        private void DeveloperButton_Click(object sender, RoutedEventArgs e)
        {
            OnOpenDeveloper?.Invoke();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            if (double.TryParse(WidthBox.Text, out double width) && width >= 200 && width <= 800)
                _settings.WindowWidth = width;
            else
            {
                System.Windows.MessageBox.Show("Width must be between 200 and 800", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                hasError = true;
            }

            if (double.TryParse(HeightBox.Text, out double height) && height >= 300 && height <= 1000)
                _settings.WindowHeight = height;
            else
            {
                System.Windows.MessageBox.Show("Height must be between 300 and 1000", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                hasError = true;
            }

            if (int.TryParse(MaxTextItemsBox.Text, out int maxText) && maxText >= 10 && maxText <= 500)
                _settings.MaxTextItems = maxText;
            else
            {
                System.Windows.MessageBox.Show("Max text items must be between 10 and 500", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                hasError = true;
            }

            if (int.TryParse(MaxFileItemsBox.Text, out int maxFile) && maxFile >= 10 && maxFile <= 300)
                _settings.MaxFileItems = maxFile;
            else
            {
                System.Windows.MessageBox.Show("Max files/images must be between 10 and 300", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                hasError = true;
            }

            if (hasError) return;

            _settings.AppearOnCopy = AppearOnCopyCheck.IsChecked == true;
            _settings.RunOnStartup = RunOnStartupCheck.IsChecked == true;
            _settings.SoundOnCopy = SoundOnCopyCheck.IsChecked == true;
            _settings.ShowTimestamp = ShowTimestampCheck.IsChecked == true;

            SettingsManager.Save(_settings);
            SettingsManager.SetRunOnStartup(_settings.RunOnStartup);

            System.Windows.MessageBox.Show("Settings saved. Please restart the application for all changes to take effect.",
                            "UClipClip", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}