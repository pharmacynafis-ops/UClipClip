using System.Collections.ObjectModel;

namespace UClipClip
{
    public class DayGroup<T>
    {
        public DateTime Date { get; set; }
        public ObservableCollection<T> Items { get; set; } = new();
        public bool IsExpanded { get; set; } = true;
        public string HeaderText => Date.ToString("MMMM dd, yyyy") + (Date == DateTime.Today ? " (Today)" : "");
    }
}