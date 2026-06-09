using System.Windows;

namespace UClipClip;

public partial class App : System.Windows.Application { }

public static class ThemeManager
{
    public static void Apply(string themeName)
    {
        var uri = themeName switch
        {
            "Dark" => new Uri("Themes/DarkTheme.xaml", UriKind.Relative),
            "King" => new Uri("Themes/KingTheme.xaml", UriKind.Relative),
            _      => new Uri("Themes/LightTheme.xaml", UriKind.Relative)
        };

        var dict = new ResourceDictionary { Source = uri };

        var merged = System.Windows.Application.Current.Resources.MergedDictionaries;
        // Replace first merged dictionary (the theme slot)
        if (merged.Count > 0) merged[0] = dict;
        else                  merged.Add(dict);
    }
}