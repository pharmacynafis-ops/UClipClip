using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Models.UClipClip;

namespace UClipClip
{
    public partial class FilesPage : System.Windows.Controls.UserControl
    {
        private List<FileClip> _fileClips = new();

        public FilesPage()
        {
            InitializeComponent();
            Loaded += FilesPage_Loaded;
        }

        private void FilesPage_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        public void AddFiles(string[] paths)
        {
            var clip = new FileClip { Paths = paths.ToList() };
            _fileClips.Insert(0, clip);
            RefreshList();
        }

        private void RefreshList()
        {
            FilesItemsControl.ItemsSource = null;
            FilesItemsControl.ItemsSource = _fileClips;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Button)?.Tag is FileClip clip)
            {
                var paths = clip.Paths.ToArray();
                var dropList = new StringCollection();
                dropList.AddRange(paths);
                System.Windows.Forms.Clipboard.SetFileDropList(dropList);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Button)?.Tag is FileClip clip)
            {
                _fileClips.Remove(clip);
                RefreshList();
            }
        }
    }
}