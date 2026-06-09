namespace Models.UClipClip;

public class TextClip
{
    public string   Id        { get; set; } = Guid.NewGuid().ToString();
    public string   Content   { get; set; } = string.Empty;
    public DateTime CopiedAt  { get; set; } = DateTime.Now;
    public bool     Pinned    { get; set; } = false;
}