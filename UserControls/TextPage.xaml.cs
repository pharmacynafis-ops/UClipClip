using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Models.UClipClip;
using WpfButton = System.Windows.Controls.Button;
using System.Collections.ObjectModel;

namespace UClipClip
{
    public partial class TextPage : UserControl
    {
        private ObservableCollection<DayGroup<TextClip>> _allGroups = new();
        private List<DayGroup<TextClip>> _filteredGroups = new();
        private TextClip? _editingClip;
        private string _currentFilter = "All";

        public TextPage()
        {
            InitializeComponent();
            Loaded += (_, _) => ApplyFilter();
        }

        public new void AddText(string text)
        {
            // Avoid duplicate of the very last copied item
            if (_allGroups.Count > 0 && _allGroups[0].Items.Count > 0 &&
                _allGroups[0].Items[0].Content == text)
                return;

            var newClip = new TextClip { Content = text, CopiedAt = DateTime.Now };
            InsertIntoGroups(newClip);
            TrimToLimit(SettingsManager.Load().MaxTextItems);
            ApplyFilter();
        }

        private void InsertIntoGroups(TextClip clip)
        {
            var date = clip.CopiedAt.Date;
            var group = _allGroups.FirstOrDefault(g => g.Date == date);
            if (group == null)
            {
                group = new DayGroup<TextClip> { Date = date, IsExpanded = true };
                _allGroups.Add(group);
                // Keep groups sorted descending by date
                _allGroups = new ObservableCollection<DayGroup<TextClip>>(
                    _allGroups.OrderByDescending(g => g.Date));
            }
            group.Items.Insert(0, clip); // newest on top
        }

        private void TrimToLimit(int maxItems)
        {
            var allItems = _allGroups.SelectMany(g => g.Items).ToList();
            if (allItems.Count <= maxItems) return;

            int toRemove = allItems.Count - maxItems;
            // Remove from the oldest groups first
            var oldestGroups = _allGroups.OrderBy(g => g.Date).ToList();
            foreach (var group in oldestGroups)
            {
                while (toRemove > 0 && group.Items.Count > 0)
                {
                    group.Items.RemoveAt(group.Items.Count - 1); // remove oldest inside group
                    toRemove--;
                }
                if (toRemove == 0) break;
            }
            // Remove empty groups
            for (int i = _allGroups.Count - 1; i >= 0; i--)
                if (_allGroups[i].Items.Count == 0)
                    _allGroups.RemoveAt(i);
        }

        private void ApplyFilter()
        {
            var now = DateTime.Now;
            _filteredGroups = _currentFilter switch
            {
                "Today" => _allGroups.Where(g => g.Date == now.Date).ToList(),
                "This Week" => _allGroups.Where(g => g.Date >= now.AddDays(-7)).ToList(),
                _ => _allGroups.ToList()
            };
            GroupedItemsControl.ItemsSource = _filteredGroups;
        }

        private void RefreshGroupDisplay()
        {
            // Replaces the binding to refresh the UI
            GroupedItemsControl.ItemsSource = null;
            GroupedItemsControl.ItemsSource = _filteredGroups;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not WpfButton btn) return;
            _currentFilter = btn.Content.ToString()!;
            ApplyFilter();
            HighlightFilter(btn);
        }

        private void HighlightFilter(WpfButton active)
        {
            foreach (var btn in new[] { FilterAll, FilterToday, FilterWeek })
            {
                bool isActive = btn == active;
                btn.Background = isActive
                    ? (Brush)FindResource("TabActiveBrush")
                    : (Brush)FindResource("TabInactiveBrush");
                btn.Foreground = isActive
                    ? (Brush)FindResource("TabActiveTextBrush")
                    : (Brush)FindResource("TabInactiveTextBrush");
            }
        }

        private void GroupHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is DayGroup<TextClip> group)
            {
                group.IsExpanded = !group.IsExpanded;
                var icon = border.FindName("GroupExpandIcon") as TextBlock;
                if (icon != null) icon.Text = group.IsExpanded ? "▼" : "▶";
                var content = (border.Parent as Border)?.FindName("GroupContent") as Border;
                if (content != null) content.Visibility = group.IsExpanded ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void CardHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border) return;
            var parentGrid = border.Parent as Grid;
            var expanded = parentGrid?.Children.OfType<Border>().FirstOrDefault(b => b.Name == "ExpandedContent");
            if (expanded == null) return;

            bool opening = expanded.Visibility != Visibility.Visible;
            expanded.Visibility = opening ? Visibility.Visible : Visibility.Collapsed;
            var icon = border.FindName("ExpandIcon") as TextBlock;
            if (icon != null) icon.Text = opening ? "▼" : "▶";
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not WpfButton btn) return;
            if (btn.Tag is TextClip clip)
                SetClipboardTextSafe(clip.Content);
        }

        internal static void SetClipboardTextSafe(string text)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    System.Windows.Clipboard.SetDataObject(text, true);
                    return;
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not WpfButton btn) return;
            if (btn.Tag is TextClip clip)
            {
                _editingClip = clip;
                EditTextBox.Text = clip.Content;
                EditPanel.Visibility = Visibility.Visible;
            }
        }

        private void SaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_editingClip != null)
            {
                _editingClip.Content = EditTextBox.Text;
                // Update the ShortContent binding (handled via UI refresh)
                RefreshGroupDisplay();
            }
            EditPanel.Visibility = Visibility.Collapsed;
            _editingClip = null;
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = Visibility.Collapsed;
            _editingClip = null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not WpfButton btn) return;
            if (btn.Tag is TextClip clip)
            {
                // Remove from its group
                var group = _allGroups.FirstOrDefault(g => g.Items.Contains(clip));
                if (group != null)
                {
                    group.Items.Remove(clip);
                    if (group.Items.Count == 0)
                        _allGroups.Remove(group);
                }
                ApplyFilter();
            }
        }
    }
}