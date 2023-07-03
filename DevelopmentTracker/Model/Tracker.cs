using DevelopmentTracker.Model.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DevelopmentTracker.Model;

public class Tracker : INotifyPropertyChanged
{
    #region Members

    private string m_Name;
    private string m_Description;
    private string m_Url;
    private float m_Total;
    private DateTime m_CreationDate;
    private List<Progress> m_ProgressionHistory;
    public event PropertyChangedEventHandler? PropertyChanged;

    public string TrackerType { get; protected set; }

    public string Name
    {
        get { return m_Name; }
        set
        {
            m_Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public string Description
    {
        get { return m_Description; }
        set
        {
            m_Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public string Url
    {
        get { return m_Url; }
        set
        {
            m_Url = value;
            OnPropertyChanged(nameof(Url));
        }
    }

    public float Total
    {
        get { return m_Total; }
        set
        {
            m_Total = value;
            OnPropertyChanged(nameof(Total));
        }
    }

    public float Reached
    {
        get
        {
            return m_ProgressionHistory.Count != 0 ? m_ProgressionHistory.Last().Advancement : 0;
        }
        protected set
        {
            OnPropertyChanged(nameof(Reached));
        }
    }

    public DateTime CreationDate
    {
        get { return m_CreationDate; }
        set
        {
            m_CreationDate = value;
            OnPropertyChanged(nameof(CreationDate));
        }
    }

    public List<Progress> ProgressionHistory
    {
        get { return m_ProgressionHistory; }
        set
        {
            m_ProgressionHistory = value;
            OnPropertyChanged(nameof(ProgressionHistory));
        }
    }

    #endregion

    #region Constructors

    public Tracker()
    {
        m_Name = "";
        m_Description = "";
        m_Url = "";
        m_ProgressionHistory = new List<Progress>();
        m_CreationDate = DateTime.Now;
        TrackerType = GetType().Name;
    }

    public Tracker(string name, string description, string url, float total)
    {
        m_Name = name;
        m_Description = description;
        m_Url = url;
        m_Total = total;
        m_ProgressionHistory = new List<Progress>();
        m_CreationDate = DateTime.Now;
        TrackerType = GetType().Name;
    }

    #endregion

    #region Operations

    public virtual void Open()
    {
    }

    public static IList<Tracker> ReadTrackersFile(string path)
    {
        var trackers = new List<Tracker>();

        foreach (JsonElement tracker in JsonSerializer.Deserialize<List<object>>(File.ReadAllText(path))!)
        {
            var trackerType = tracker.GetProperty(nameof(TrackerType)).ToString();
            var trackerObject = (Tracker)tracker.Deserialize(trackerType)!;

            //switch (trackerType)
            //{
            //    case "Tracker":
            //        trackerObject = tracker.Deserialize<Tracker>()!;
            //        break;
            //    case "BookTracker":
            //        trackerObject = tracker.Deserialize<BookTracker>()!;
            //        break;
            //    case "OnlineCourseTracker":
            //        trackerObject = tracker.Deserialize<OnlineCourseTracker>()!;
            //        break;
            //}

            trackers.Add(trackerObject);
        }

        return trackers;
    }

    public string GetTypeName()
    {
        return (TrackerType.StartsWith("Tracker") ? TrackerType : TrackerType.Replace("Tracker", "")).ToSentenceCase();
    }

    public virtual string Export()
    {
        throw new NotImplementedException();
    }

    #endregion

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}