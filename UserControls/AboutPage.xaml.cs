using System.Windows;
using System.Windows.Controls;

namespace UClipClip
{
    public partial class AboutPage : System.Windows.Controls.UserControl
    {
        public Action? OnBack { get; set; }

        public AboutPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OnBack?.Invoke();
        }
    }
}