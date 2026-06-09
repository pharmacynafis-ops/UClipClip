using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Models.UClipClip;

public static class ClipboardMonitor
{
    private static System.Windows.Forms.Timer? _timer;
    private static string   _lastText     = string.Empty;
    private static string[] _lastFileList = Array.Empty<string>();
    private static string   _lastImageHash = string.Empty;

    public static event Action<string>?   TextCopied;
    public static event Action<string[]>? FilesCopied;
    public static event Action<ImageClip>? ImageCopied;

    public static void Start()
    {
        if (_timer != null) return;
        _timer = new System.Windows.Forms.Timer { Interval = 400 };
        _timer.Tick += (_, _) => CheckClipboard();
        _timer.Start();
    }

    public static void Stop()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }

    private static void CheckClipboard()
    {
        try
        {
            if (System.Windows.Forms.Clipboard.ContainsText())
            {
                string text = System.Windows.Forms.Clipboard.GetText();
                if (!string.IsNullOrEmpty(text) && text != _lastText)
                {
                    _lastText = text;
                    _lastImageHash = string.Empty;
                    TextCopied?.Invoke(text);
                }
                return;
            }

            if (System.Windows.Forms.Clipboard.ContainsFileDropList())
            {
                var col   = System.Windows.Forms.Clipboard.GetFileDropList();
                var paths = new string[col.Count];
                col.CopyTo(paths, 0);
                if (!ArrayEqual(paths, _lastFileList))
                {
                    _lastFileList  = paths;
                    _lastImageHash = string.Empty;
                    FilesCopied?.Invoke(paths);
                }
                return;
            }

            if (System.Windows.Forms.Clipboard.ContainsImage())
            {
                var sd = System.Windows.Forms.Clipboard.GetDataObject();
                if (sd == null) return;

                byte[]? pngBytes = null;
                if (sd.GetDataPresent("PNG", false))
                {
                    using var ms2 = sd.GetData("PNG", false) as Stream;
                    if (ms2 != null)
                    {
                        using var buf = new MemoryStream();
                        ms2.CopyTo(buf);
                        pngBytes = buf.ToArray();
                    }
                }

                if (pngBytes == null)
                {
                    var sdbmp = System.Windows.Forms.Clipboard.GetImage();
                    if (sdbmp == null) return;
                    using var ms = new MemoryStream();
                    sdbmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    pngBytes = ms.ToArray();
                    sdbmp.Dispose();
                }

                string hash = ComputeHash(pngBytes);
                if (hash == _lastImageHash) return;
                _lastImageHash = hash;
                _lastText      = string.Empty;

                int w = 0, h = 0;
                try
                {
                    var bmp = new BitmapImage();
                    using var ms3 = new MemoryStream(pngBytes);
                    bmp.BeginInit();
                    bmp.CacheOption  = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = ms3;
                    bmp.EndInit();
                    bmp.Freeze();
                    w = bmp.PixelWidth;
                    h = bmp.PixelHeight;
                }
                catch { }

                ImageCopied?.Invoke(new ImageClip
                {
                    PngBytes    = pngBytes,
                    PixelWidth  = w,
                    PixelHeight = h
                });
            }
        }
        catch { }
    }

    private static string ComputeHash(byte[] data)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        return Convert.ToBase64String(md5.ComputeHash(data));
    }

    private static bool ArrayEqual(string[] a, string[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
            if (a[i] != b[i]) return false;
        return true;
    }
}