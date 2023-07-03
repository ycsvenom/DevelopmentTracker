using System.Diagnostics;

namespace DevelopmentTracker.Model;

public class BookTracker : Tracker
{
    private string m_SectionName;

    public string SectionName
    {
        get { return m_SectionName; }
        set
        {
            m_SectionName = value;
            OnPropertyChanged(nameof(SectionName));
        }
    }

    public BookTracker() : base()
    {
        m_SectionName = "";
    }

    public BookTracker(string name, string description, string url, float total) : base(name, description, url, total)
    {
        m_SectionName = "";
    }

    public override void Open()
    {
        base.Open();
        Process.Start(new ProcessStartInfo
        {
            FileName = "acrobat.exe",
            Arguments = $"/A \"page={Reached}\" \"{Url}\"",
            UseShellExecute = true
        });
    }
}
