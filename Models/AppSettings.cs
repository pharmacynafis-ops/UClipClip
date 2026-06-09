namespace Models.UClipClip;

public class AppSettings
{
    public double WindowWidth  { get; set; } = 320;
    public double WindowHeight { get; set; } = 520;
    public bool   AppearOnCopy { get; set; } = true;
    public bool   RunOnStartup { get; set; } = true;
    public string Theme        { get; set; } = "Light";   // Light | Dark | King
    public int    MaxTextItems { get; set; } = 200;
    public int    MaxFileItems { get; set; } = 100;
    public bool   SoundOnCopy  { get; set; } = false;
    public bool   ShowTimestamp{ get; set; } = true;
}