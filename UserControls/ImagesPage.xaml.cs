using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Models.UClipClip;

namespace UClipClip
{
    public partial class ImagesPage : UserControl
    {
        private ObservableCollection<DayGroup<ImageClip>> _allGroups = new();

        public ImagesPage()
        {
            InitializeComponent();
            Loaded += (_, _) => RefreshBinding();
        }

        public void AddImage(ImageClip clip)
        {
            // Avoid duplicate of the very last copied image
            if (_allGroups.Count > 0 && _allGroups[0].Items.Count > 0 &&
                _allGroups[0].Items[0].PngBytes.SequenceEqual(clip.PngBytes))
                return;

            clip.CopiedAt = DateTime.Now;
            InsertIntoGroups(clip);
            TrimToLimit(SettingsManager.Load().MaxFileItems); // reuse max file setting
            RefreshBinding();
        }

        private void InsertIntoGroups(ImageClip clip)
        {
            var date = clip.CopiedAt.Date;
            var group = _allGroups.FirstOrDefault(g => g.Date == date);
            if (group == null)
            {
                group = new DayGroup<ImageClip> { Date = date, IsExpanded = true };
                _allGroups.Add(group);
                _allGroups = new ObservableCollection<DayGroup<ImageClip>>(_allGroups.OrderByDescending(g => g.Date));
            }
            group.Items.Insert(0, clip);
        }

        private void TrimToLimit(int maxItems)
        {
            var allItems = _allGroups.SelectMany(g => g.Items).ToList();
            if (allItems.Count <= maxItems) return;
            int toRemove = allItems.Count - maxItems;
            var oldestGroups = _allGroups.OrderBy(g => g.Date).ToList();
            foreach (var group in oldestGroups)
            {
                while (toRemove > 0 && group.Items.Count > 0)
                {
                    group.Items.RemoveAt(group.Items.Count - 1);
                    toRemove--;
                }
                if (toRemove == 0) break;
            }
            for (int i = _allGroups.Count - 1; i >= 0; i--)
                if (_allGroups[i].Items.Count == 0)
                    _allGroups.RemoveAt(i);
        }

        private void RefreshBinding()
        {
            GroupedImagesControl.ItemsSource = null;
            GroupedImagesControl.ItemsSource = _allGroups;
        }

        private void GroupHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is DayGroup<ImageClip> group)
            {
                group.IsExpanded = !group.IsExpanded;
                var icon = border.FindName("GroupExpandIcon") as TextBlock;
                if (icon != null) icon.Text = group.IsExpanded ? "▼" : "▶";
                var content = (border.Parent as Border)?.FindName("GroupContent") as Border;
                if (content != null) content.Visibility = group.IsExpanded ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ImageClip clip)
            {
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
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ImageClip clip)
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PNG Image|*.png",
                    Title = "Save Image As",
                    FileName = $"UClipClip_{clip.CopiedAt:yyyyMMdd_HHmmss}.png"
                };
                if (dialog.ShowDialog() == true)
                    System.IO.File.WriteAllBytes(dialog.FileName, clip.PngBytes);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ImageClip clip)
            {
                var group = _allGroups.FirstOrDefault(g => g.Items.Contains(clip));
                if (group != null)
                {
                    group.Items.Remove(clip);
                    if (group.Items.Count == 0)
                        _allGroups.Remove(group);
                }
                RefreshBinding();
            }
        }
    }
}