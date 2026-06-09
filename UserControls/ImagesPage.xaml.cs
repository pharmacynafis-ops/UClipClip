using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Models.UClipClip;
using WpfButton = System.Windows.Controls.Button;

namespace UClipClip
{
    public partial class ImagesPage : System.Windows.Controls.UserControl
    {
        private List<ImageClip> _imageClips = new();

        public ImagesPage()
        {
            InitializeComponent();
            Loaded += (_, _) => RefreshList();
        }

        public void AddImage(ImageClip clip)
        {
            _imageClips.Insert(0, clip);
            var settings = SettingsManager.Load();
            while (_imageClips.Count > settings.MaxFileItems) // reuse max file limit
                _imageClips.RemoveAt(_imageClips.Count - 1);
            RefreshList();
        }

        private void RefreshList()
        {
            ImagesItemsControl.ItemsSource = null;
            ImagesItemsControl.ItemsSource = _imageClips;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not WpfButton btn) return;
            if (btn.Tag is not ImageClip clip) return;

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var bmp = clip.ToBitmapImage();
                    System.Windows.Clipboard.SetImage(bmp);
                    return;
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not WpfButton btn) return;
            if (btn.Tag is not ImageClip clip) return;

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Save Image As",
                FileName = $"UClipClip_{clip.CopiedAt:yyyyMMdd_HHmmss}.png"
            };
            if (dialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllBytes(dialog.FileName, clip.PngBytes);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not WpfButton btn) return;
            if (btn.Tag is not ImageClip clip) return;
            _imageClips.Remove(clip);
            RefreshList();
        }
    }
}