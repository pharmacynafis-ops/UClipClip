using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Models.UClipClip;

namespace UClipClip
{
    public partial class FilesPage : UserControl
    {
        private ObservableCollection<DayGroup<FileClip>> _allGroups = new();

        public FilesPage()
        {
            InitializeComponent();
            Loaded += (_, _) => RefreshBinding();
        }

        public void AddFiles(string[] paths)
        {
            var newClip = new FileClip { Paths = paths.ToList(), CopiedAt = DateTime.Now };
            InsertIntoGroups(newClip);
            TrimToLimit(SettingsManager.Load().MaxFileItems);
            RefreshBinding();
        }

        private void InsertIntoGroups(FileClip clip)
        {
            var date = clip.CopiedAt.Date;
            var group = _allGroups.FirstOrDefault(g => g.Date == date);
            if (group == null)
            {
                group = new DayGroup<FileClip> { Date = date, IsExpanded = true };
                _allGroups.Add(group);
                _allGroups = new ObservableCollection<DayGroup<FileClip>>(_allGroups.OrderByDescending(g => g.Date));
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
            GroupedFilesControl.ItemsSource = null;
            GroupedFilesControl.ItemsSource = _allGroups;
        }

        private void GroupHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is DayGroup<FileClip> group)
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
            if (sender is Button btn && btn.Tag is FileClip clip)
            {
                var dropList = new StringCollection();
                dropList.AddRange(clip.Paths.ToArray());
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        var dataObj = new System.Windows.DataObject();
                        dataObj.SetFileDropList(dropList);
                        System.Windows.Clipboard.SetDataObject(dataObj, true);
                        return;
                    }
                    catch (System.Runtime.InteropServices.ExternalException)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is FileClip clip)
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