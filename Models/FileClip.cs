namespace Models.UClipClip;

public class FileClip
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DisplayText => Paths.Count == 1 ? Paths[0] : $"{Paths[0]} (+{Paths.Count - 1} more)";
    public List<string> Paths { get; set; } = new();
    public DateTime CopiedAt { get; set; } = DateTime.Now;
}