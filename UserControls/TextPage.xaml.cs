using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Models.UClipClip;

namespace UClipClip
{
    public partial class TextPage : System.Windows.Controls.UserControl
    {
        private List<TextClip> _allTextClips = new();
        private List<TextClip> _filteredClips = new();
        private TextClip? _editingClip;
        private string _currentFilter = "All";

        public TextPage()
        {
            InitializeComponent();
            Loaded += TextPage_Loaded;
            HighlightFilterButton(FilterAll);
        }

        private void TextPage_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        public new void AddText(string text)
        {
            _allTextClips.Insert(0, new TextClip { Content = text });
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var now = DateTime.Now;
            _filteredClips = _currentFilter switch
            {
                "Today" => _allTextClips.Where(c => c.CopiedAt.Date == now.Date).ToList(),
                "This Week" => _allTextClips.Where(c => c.CopiedAt >= now.AddDays(-7)).ToList(),
                _ => _allTextClips.ToList()
            };
            RefreshList();
        }

        private void RefreshList()
        {
            var items = _filteredClips.Select(clip => new
            {
                clip.Id,
                clip.Content,
                ShortContent = clip.Content.Length > 80 ? clip.Content.Substring(0, 80) + "..." : clip.Content,
                clip.CopiedAt
            }).ToList();

            TextItemsControl.ItemsSource = null;
            TextItemsControl.ItemsSource = items;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            if (btn == null) return;

            _currentFilter = btn.Content.ToString()!;
            ApplyFilter();
            HighlightFilterButton(btn);
        }

        private void HighlightFilterButton(System.Windows.Controls.Button active)
{
    foreach (var btn in new[] { FilterAll, FilterToday, FilterWeek })
    {
        if (btn == active)
        {
            btn.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 120, 212));
            btn.Foreground = System.Windows.Media.Brushes.White;
        }
        else
        {
            btn.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 240, 240));
            btn.Foreground = System.Windows.Media.Brushes.Black;
        }
    }
}

        private void CardHeader_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border?.Tag == null) return;

            var parent = border.Parent as Grid;
            var expanded = parent?.Children.OfType<Border>().FirstOrDefault(b => b.Name == "ExpandedContent");
            if (expanded != null)
            {
                bool isExpanded = expanded.Visibility == Visibility.Visible;
                expanded.Visibility = isExpanded ? Visibility.Collapsed : Visibility.Visible;

                var icon = border.FindName("ExpandIcon") as TextBlock;
                if (icon != null)
                    icon.Text = isExpanded ? "▶" : "▼";
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            dynamic? item = btn?.Tag;
            if (item != null)
            {
                var original = _allTextClips.FirstOrDefault(c => c.Id == item.Id);
                if (original != null)
                    System.Windows.Forms.Clipboard.SetText(original.Content);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            dynamic? item = btn?.Tag;
            if (item != null)
            {
                _editingClip = _allTextClips.FirstOrDefault(c => c.Id == item.Id);
                if (_editingClip != null)
                {
                    EditTextBox.Text = _editingClip.Content;
                    EditPanel.Visibility = Visibility.Visible;
                }
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
    }
}