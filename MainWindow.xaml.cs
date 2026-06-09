using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Models.UClipClip;
using WpfButton = System.Windows.Controls.Button;
using System.IO;
using System.Windows.Media.Imaging;

namespace UClipClip
{
    public partial class MainWindow : Window
    {
        private WpfButton? _activeTab;
        private bool _settingsOpen = false;

        public MainWindow()
        {
            InitializeComponent();

            var settings = SettingsManager.Load();
            ApplyTheme(settings.Theme);
            Width  = settings.WindowWidth;
            Height = settings.WindowHeight;

            ClipboardMonitor.TextCopied   += OnTextCopied;
            ClipboardMonitor.FilesCopied  += OnFilesCopied;
            ClipboardMonitor.ImageCopied  += OnImageCopied;
            ClipboardMonitor.Start();

            _activeTab = TabText;
            Loaded += (_, _) => SwitchPage("Text");
        }

         private void PlayCopySound()
        {
            if (SettingsManager.Load().SoundOnCopy)
                System.Media.SystemSounds.Beep.Play();
        }

        protected override void OnClosed(EventArgs e)
        {
            ClipboardMonitor.Stop();
            base.OnClosed(e);
        }

        private void OnTextCopied(string text)
            => Dispatcher.Invoke(() =>
            {
                 PlayCopySound();
                PageText.AddText(text);
                if (SettingsManager.Load().AppearOnCopy) ShowWindow();
            });

        private void OnFilesCopied(string[] paths)
    => Dispatcher.Invoke(() =>
    {
        if (AreAllImages(paths))
        {
            // Treat as images – add each to ImagesPage
            foreach (var path in paths)
            {
                var clip = LoadImageFileToClip(path);
                if (clip != null)
                 PlayCopySound();
                    PageImages.AddImage(clip);

            }
        }
        else
        {
            // Normal file(s) – add to FilesPage
            PageFiles.AddFiles(paths);
        }

        if (SettingsManager.Load().AppearOnCopy) ShowWindow();
    });

        private void OnImageCopied(ImageClip clip)
            => Dispatcher.Invoke(() =>
            {
                 PlayCopySound();
                PageImages.AddImage(clip);
                if (SettingsManager.Load().AppearOnCopy) ShowWindow();
            });

        private void ShowWindow()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is WpfButton btn)
                SwitchPage(btn.Tag?.ToString() ?? "Text");
        }

        public void SwitchPage(string page)
        {
            CloseOverlays();
            CloseDeveloper();

            PageText.Visibility   = Visibility.Collapsed;
            PageFiles.Visibility  = Visibility.Collapsed;
            PageImages.Visibility = Visibility.Collapsed;

            switch (page)
            {
                case "Text":   PageText.Visibility   = Visibility.Visible; _activeTab = TabText;   break;
                case "Files":  PageFiles.Visibility  = Visibility.Visible; _activeTab = TabFiles;  break;
                case "Images": PageImages.Visibility = Visibility.Visible; _activeTab = TabImages; break;
            }

            RefreshTabColors();
        }

        private void RefreshTabColors()
        {
            foreach (var btn in new[] { TabText, TabFiles, TabImages })
            {
                bool active = btn == _activeTab;
                btn.Background = active
                    ? (System.Windows.Media.Brush)FindResource("TabActiveBrush")
                    : (System.Windows.Media.Brush)FindResource("TabInactiveBrush");
                btn.Foreground = active
                    ? (System.Windows.Media.Brush)FindResource("TabActiveTextBrush")
                    : (System.Windows.Media.Brush)FindResource("TabInactiveTextBrush");
            }
        }

        private void SettingsFab_Click(object sender, RoutedEventArgs e)
        {
            if (_settingsOpen)
                CloseOverlays();
            else
            {
                CloseOverlays();
                CloseDeveloper();
                PageSettings.Visibility = Visibility.Visible;
                PageSettings.OnThemeChanged = ApplyTheme;
                PageSettings.OnOpenAbout    = OpenAbout;
                PageSettings.OnOpenDeveloper = OpenDeveloper;
                _settingsOpen = true;
            }
        }

        public void OpenAbout()
        {
            PageSettings.Visibility = Visibility.Collapsed;
            _settingsOpen = false;
            PageAbout.Visibility = Visibility.Visible;
            PageAbout.OnBack = () =>
            {
                PageAbout.Visibility = Visibility.Collapsed;
                PageSettings.Visibility = Visibility.Visible;
                _settingsOpen = true;
            };
        }

        public void OpenDeveloper()
        {
            PageSettings.Visibility = Visibility.Collapsed;
            _settingsOpen = false;
            PageDeveloper.Visibility = Visibility.Visible;
            PageDeveloper.OnBack = () =>
            {
                PageDeveloper.Visibility = Visibility.Collapsed;
                PageSettings.Visibility = Visibility.Visible;
                _settingsOpen = true;
            };
        }

        private void CloseOverlays()
        {
            PageSettings.Visibility = Visibility.Collapsed;
            PageAbout.Visibility    = Visibility.Collapsed;
            _settingsOpen = false;
        }

        private void CloseDeveloper()
        {
            PageDeveloper.Visibility = Visibility.Collapsed;
        }

        public void ApplyTheme(string themeName)
        {
            var uri = themeName switch
            {
                "Dark" => new Uri("Themes/DarkTheme.xaml",  UriKind.Relative),
                "King" => new Uri("Themes/KingTheme.xaml",  UriKind.Relative),
                _      => new Uri("Themes/LightTheme.xaml", UriKind.Relative),
            };

            var dict = new ResourceDictionary { Source = uri };
            if (System.Windows.Application.Current.Resources.MergedDictionaries.Count > 0)
                System.Windows.Application.Current.Resources.MergedDictionaries[0] = dict;
            else
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(dict);

            var s = SettingsManager.Load();
            s.Theme = themeName;
            SettingsManager.Save(s);

            RefreshTabColors();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
        private static readonly string[] ImageExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff" };

private bool AreAllImages(string[] paths)
{
    return paths.All(p => ImageExtensions.Contains(Path.GetExtension(p)?.ToLower()));
}

private ImageClip? LoadImageFileToClip(string filePath)
{
    try
    {
        // Load the image file into a BitmapImage
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.UriSource = new Uri(filePath);
        bitmap.EndInit();
        bitmap.Freeze();

        // Convert to PNG bytes for storage (so we can later paste it back as an image)
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        using var ms = new MemoryStream();
        encoder.Save(ms);
        byte[] pngBytes = ms.ToArray();

        return new ImageClip
        {
            PngBytes = pngBytes,
            PixelWidth = bitmap.PixelWidth,
            PixelHeight = bitmap.PixelHeight,
            CopiedAt = DateTime.Now
        };
    }
    catch
    {
        return null;
    }
}

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
            => Hide();
    }
}