using System.Text.Json;
using System.IO;

namespace Models.UClipClip;

public static class SettingsManager
{
    private static readonly string AppDataPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "UClipClip");
    private static readonly string SettingsFile = Path.Combine(AppDataPath, "settings.json");

    static SettingsManager()
    {
        if (!Directory.Exists(AppDataPath))
            Directory.CreateDirectory(AppDataPath);
    }

    public static AppSettings Load()
    {
        if (!File.Exists(SettingsFile))
            return new AppSettings();

        try
        {
            var json = File.ReadAllText(SettingsFile);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public static void Save(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsFile, json);
    }

    public static void SetRunOnStartup(bool enable)
    {
        var keyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        var appName = "UClipClip";
        var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? 
                      System.Reflection.Assembly.GetExecutingAssembly().Location;

        using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyPath, true);
        if (key == null) return;

        if (enable)
            key.SetValue(appName, exePath);
        else
            key.DeleteValue(appName, false);
    }
}