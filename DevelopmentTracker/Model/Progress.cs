using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevelopmentTracker.Model;

public class Progress : INotifyPropertyChanged
{
    #region Members

    private float m_Advancement;
    private DateTime m_Date;
    private List<string> m_Notes = new();

    public float Advancement
    {
        get { return m_Advancement; }
        set
        {
            m_Advancement = value;
            OnPropertyChanged(nameof(Advancement));
        }
    }

    public DateTime Date
    {
        get { return m_Date; }
        set
        {
            m_Date = value;
            OnPropertyChanged(nameof(Date));
        }
    }

    public List<string> Notes
    {
        get { return m_Notes; }
        set
        {
            m_Notes = value;
            OnPropertyChanged(nameof(Notes));
        }
    }

    #endregion

    #region Constructors

    public Progress()
    {
        Constructor(0, DateTime.Now, new List<string>());
    }

    public Progress(float advancement)
    {
        Constructor(advancement, DateTime.Now, new List<string>());
    }

    public Progress(float advancement, DateTime date)
    {
        Constructor(advancement, date, new List<string>());
    }

    public Progress(float advancement, List<string> notes)
    {
        Constructor(advancement, DateTime.Now, notes);
    }

    public Progress(float advancement, DateTime date, List<string> notes)
    {
        Constructor(advancement, date, notes);
    }

    private void Constructor(float advancement, DateTime date, List<string> notes)
    {
        Advancement = advancement;
        Date = date;
        Notes = notes;
    }

    #endregion

    #region INotifyPropertyChangedImp

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
