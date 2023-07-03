using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevelopmentTracker.Model;

public partial class YoutubeCourseTracker : Tracker
{
    private readonly string m_Domain = "https://www.youtube.com";

    public new string Url
    {
        get { return base.Url; }
        set
        {
            base.Url = FormUrl(value);
            OnPropertyChanged(nameof(Url));
        }
    }

    public YoutubeCourseTracker() : base()
    {
    }

    public YoutubeCourseTracker(string name, string description, string url, float total) : base(name, description, url, total)
    {
        Url = url;
    }

    // will convert to following syntax
    // https://www.youtube.com/playlist?list=SOME_ID
    private string FormUrl(string url)
    {
        if (MatchYoutubeWatchUrl().IsMatch(url))
        {
            string playlistId = MatchYoutubePlaylistId().Match(url).Groups["listId"].Value;
            url = playlistId;
        }

        if (url.StartsWith("https://") || url.StartsWith("www."))
            url = $"{m_Domain}/{url.Split('/').Last()}";

        if (MatchYoutubeIdForm().IsMatch(url))
            url = $"{m_Domain}/playlist?list={url}";

        return url;
    }

    public override void Open()
    {
        base.Open();
        var listId = Url.Split("=").Last();

        var parameters = string.Join("&", new List<string>()
        {
            $"v=gg",
            $"list={listId}",
            $"index={Reached-1}"
        });

        var newUrl = Reached switch
        {
            var r when r > 1 && r < Total => $"{m_Domain}/watch?{parameters}",
            _ => Url
        };

        Process.Start(new ProcessStartInfo
        {
            FileName = newUrl,
            UseShellExecute = true
        });
    }

    [GeneratedRegex(@"&list=(?<listId>[0-9A-Za-z_-]+)")]
    private static partial Regex MatchYoutubePlaylistId();

    [GeneratedRegex(@"watch\?v=[0-9A-Za-z_-]+&list=")]
    private static partial Regex MatchYoutubeWatchUrl();

    [GeneratedRegex(@"[0-9A-Za-z_-]+")]
    private static partial Regex MatchYoutubeIdForm();
}
