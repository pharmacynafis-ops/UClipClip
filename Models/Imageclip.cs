using System.Windows.Media.Imaging;

namespace Models.UClipClip;

public class ImageClip
{
    public string   Id       { get; set; } = Guid.NewGuid().ToString();
    public DateTime CopiedAt { get; set; } = DateTime.Now;
    public byte[]   PngBytes { get; set; } = Array.Empty<byte>();
    public int      PixelWidth  { get; set; }
    public int      PixelHeight { get; set; }

    public string SizeLabel => $"{PixelWidth} × {PixelHeight}";

    public BitmapImage Thumbnail => ToBitmapImage();

    public BitmapImage ToBitmapImage()
    {
        if (PngBytes.Length == 0) return new BitmapImage();
        var bmp = new BitmapImage();
        using var ms = new System.IO.MemoryStream(PngBytes);
        bmp.BeginInit();
        bmp.CacheOption   = BitmapCacheOption.OnLoad;
        bmp.StreamSource  = ms;
        bmp.EndInit();
        bmp.Freeze();
        return bmp;
    }
}