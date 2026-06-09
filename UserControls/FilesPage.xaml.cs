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
            Loaded += (_, _) => RefreshList();
        }

        public void AddFiles(string[] paths)
        {
            var clip = new FileClip { Paths = paths.ToList() };
            _fileClips.Insert(0, clip);

            var settings = SettingsManager.Load();
            while (_fileClips.Count > settings.MaxFileItems) _fileClips.RemoveAt(_fileClips.Count - 1);
            RefreshList();
        }

        private void RefreshList()
        {
            FilesItemsControl.ItemsSource = null;
            FilesItemsControl.ItemsSource = _fileClips;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.Button btn) return;
            if (btn.Tag is not FileClip clip) return;

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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.Button btn) return;
            if (btn.Tag is not FileClip clip) return;
            _fileClips.Remove(clip);
            RefreshList();
        }
    }
}