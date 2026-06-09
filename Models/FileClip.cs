namespace Models.UClipClip;

public class FileClip
{
    public string      Id          { get; set; } = Guid.NewGuid().ToString();
    public List<string>Paths       { get; set; } = new();
    public DateTime    CopiedAt    { get; set; } = DateTime.Now;

    public string DisplayText =>
        Paths.Count == 0 ? "(empty)"
      : Paths.Count == 1 ? System.IO.Path.GetFileName(Paths[0])
      : $"{System.IO.Path.GetFileName(Paths[0])}  (+{Paths.Count - 1} more)";

    public string FullPath => Paths.Count > 0 ? Paths[0] : string.Empty;
}