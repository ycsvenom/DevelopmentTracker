using System.Diagnostics;

namespace DevelopmentTracker.Model;

public class OnlineCourseTracker : Tracker
{
    public OnlineCourseTracker() : base() { }

    public OnlineCourseTracker(string name, string description, string url, float total) : base(name, description, url, total) { }

    public override void Open()
    {
        base.Open();
        Process.Start(new ProcessStartInfo
        {
            FileName = Url,
            UseShellExecute = true
        });
    }
}
