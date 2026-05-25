using System;
using System.Windows.Forms;

namespace Models.UClipClip
{
    public static class ClipboardMonitor
    {
        private static System.Windows.Forms.Timer? _timer;
        private static string _lastText = string.Empty;
        private static string[] _lastFileList = Array.Empty<string>();

        public static event Action<string>? TextCopied;
        public static event Action<string[]>? FilesCopied;

        public static void Start()
        {
            if (_timer != null) return;
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 500;
            _timer.Tick += (s, e) => CheckClipboard();
            _timer.Start();
        }

        public static void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        private static void CheckClipboard()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    string text = Clipboard.GetText();
                    if (text != _lastText)
                    {
                        _lastText = text;
                        TextCopied?.Invoke(text);
                    }
                }
                else if (Clipboard.ContainsFileDropList())
                {
                    var files = Clipboard.GetFileDropList();
                    var paths = new string[files.Count];
                    files.CopyTo(paths, 0);
                    if (paths.Length != _lastFileList.Length || !CompareArrays(paths, _lastFileList))
                    {
                        _lastFileList = paths;
                        FilesCopied?.Invoke(paths);
                    }
                }
            }
            catch (Exception) { }
        }

        private static bool CompareArrays(string[] a, string[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }
    }
}