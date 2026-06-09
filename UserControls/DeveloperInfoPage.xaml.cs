using System.Windows;
using System.Windows.Controls;

namespace UClipClip
{
    public partial class DeveloperInfoPage : System.Windows.Controls.UserControl
    {
        public Action? OnBack { get; set; }

        public DeveloperInfoPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OnBack?.Invoke();
        }
    }
}