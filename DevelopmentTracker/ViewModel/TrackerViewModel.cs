using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

using Tracker = DevelopmentTracker.Model.Tracker;

namespace DevelopmentTracker.ViewModel;

class TrackerViewModel
{
    private IList<Tracker> trackers;
    public readonly string DevTrackerDirPath;
    public readonly string TrackerPath;


    public TrackerViewModel()
    {
        trackers = new List<Tracker>();
        DevTrackerDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DevTracker");

#if DEBUG
        TrackerPath = Path.Combine(DevTrackerDirPath, "dev.test.trk");
#else
        TrackerPath = Path.Combine(DevTrackerDirPath, "dev.trk");
#endif

        EnsureTrackCreated();
    }

    private void BackupTrackers()
    {
        var backupPath = Path.Combine(Path.GetDirectoryName(TrackerPath)!, "backup.json");

        var content = File.ReadAllText(TrackerPath);
        var backedUpContent = File.Exists(backupPath) ? File.ReadAllText(backupPath) : "";

        if (content.Length > backedUpContent.Length)
            File.WriteAllText(backupPath, content);
    }

    private void EnsureTrackCreated()
    {
        if (!Directory.Exists(DevTrackerDirPath))
            Directory.CreateDirectory(DevTrackerDirPath);

        if (!File.Exists(TrackerPath))
        {
            var content = JsonSerializer.Serialize(trackers);
            File.WriteAllText(TrackerPath, content);
        }
#if !DEBUG
        else
        {
            BackupTrackers();
        }
#endif

        try
        {
            //trackers = JsonSerializer.Deserialize<IList<Tracker>>(File.OpenRead(TrackerPath))!;
            trackers = Tracker.ReadTrackersFile(TrackerPath);
            trackers ??= new List<Tracker>();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Fatal", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    public IList<Tracker> Trackers
    {
        get { return trackers; }
        set { trackers = value; }
    }

    private ICommand? mUpdater;
    public ICommand UpdateCommand
    {
        get
        {
            mUpdater ??= new Updater();
            return mUpdater;
        }
        set
        {
            mUpdater = value;
        }
    }

    private class Updater : ICommand
    {
        #region ICommand Members  

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public event EventHandler? CanExecuteChanged;

        public void Execute(object? parameter)
        {

        }

        #endregion
    }
}
