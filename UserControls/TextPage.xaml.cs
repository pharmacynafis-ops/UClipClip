using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Models.UClipClip;
using WpfButton = System.Windows.Controls.Button;

namespace UClipClip
{
    public partial class TextPage : System.Windows.Controls.UserControl
    {
        private List<TextClip> _allClips      = new();
        private List<TextClip> _filteredClips = new();
        private TextClip?      _editingClip;
        private string         _currentFilter = "All";

        public TextPage()
        {
            InitializeComponent();
            Loaded += (_, _) => RefreshList();
        }

        public new void AddText(string text)
        {
            if (_allClips.Count > 0 && _allClips[0].Content == text) return;
            _allClips.Insert(0, new TextClip { Content = text });
            var settings = SettingsManager.Load();
            while (_allClips.Count > settings.MaxTextItems) _allClips.RemoveAt(_allClips.Count - 1);
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var now = DateTime.Now;
            _filteredClips = _currentFilter switch
            {
                "Today"     => _allClips.Where(c => c.CopiedAt.Date == now.Date).ToList(),
                "This Week" => _allClips.Where(c => c.CopiedAt >= now.AddDays(-7)).ToList(),
                _           => _allClips.ToList()
            };
            RefreshList();
        }

        private void RefreshList()
        {
            var items = _filteredClips.Select(clip => new
            {
                clip.Id,
                clip.Content,
                ShortContent = clip.Content.Length > 80 ? clip.Content[..80] + "…" : clip.Content,
                clip.CopiedAt
            }).ToList();

            TextItemsControl.ItemsSource = null;
            TextItemsControl.ItemsSource = items;
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
                    ? (System.Windows.Media.Brush)FindResource("TabActiveBrush")
                    : (System.Windows.Media.Brush)FindResource("TabInactiveBrush");
                btn.Foreground = isActive
                    ? (System.Windows.Media.Brush)FindResource("TabActiveTextBrush")
                    : (System.Windows.Media.Brush)FindResource("TabInactiveTextBrush");
            }
        }

        private void CardHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border) return;
            var parent   = border.Parent as Grid;
            var expanded = parent?.Children.OfType<Border>()
                                  .FirstOrDefault(b => b.Name == "ExpandedContent");
            if (expanded == null) return;

            bool opening = expanded.Visibility != Visibility.Visible;
            expanded.Visibility = opening ? Visibility.Visible : Visibility.Collapsed;
            var icon = border.FindName("ExpandIcon") as TextBlock;
            if (icon != null) icon.Text = opening ? "▼" : "▶";
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not WpfButton btn) return;
            dynamic? item = btn.Tag;
            if (item == null) return;

            var original = _allClips.FirstOrDefault(c => c.Id == (string)item.Id);
            if (original == null) return;

            SetClipboardTextSafe(original.Content);
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
            dynamic? item = btn.Tag;
            if (item == null) return;

            _editingClip = _allClips.FirstOrDefault(c => c.Id == (string)item.Id);
            if (_editingClip != null)
            {
                EditTextBox.Text    = _editingClip.Content;
                EditPanel.Visibility = Visibility.Visible;
            }
        }

        private void SaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_editingClip != null)
            {
                _editingClip.Content = EditTextBox.Text;
                ApplyFilter();
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
            dynamic? item = btn.Tag;
            if (item == null) return;

            var clip = _allClips.FirstOrDefault(c => c.Id == (string)item.Id);
            if (clip != null)
            {
                _allClips.Remove(clip);
                ApplyFilter();
            }
        }
    }
}