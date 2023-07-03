using DevelopmentTracker.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DevelopmentTracker.View.Controls;

/// <summary>
/// Interaction logic for RegisterProgress.xaml
/// </summary>
public partial class RegisterProgress : UserControl
{
    #region Members

    private int SelectedProgressIndex = -1;
    private bool IsProgressSelected { get { return SelectedProgressIndex != -1; } }
    private Tracker? SelectedTracker { get { return (DataContext as Tracker); } }

    #endregion

    public RegisterProgress()
    {
        InitializeComponent();
    }

    private void Progress_SelectionChanged(object sender, RoutedEventArgs e)
    {
        var button = (sender as Button)!;
        var selected = (int)button.Tag;

        SelectProgress(selected);

        if (!IsProgressSelected)
            return;

        var progress = SelectedTracker!.ProgressionHistory[SelectedProgressIndex];

        txtUpdateProgressAdvancement.Text = progress.Advancement.ToString();
        txtUpdateProgressNotes.Text = string.Join("\n", progress.Notes);
        dateUpdateProgressDate.SelectedDate = progress.Date;
    }

    private void UpdateProgressResetContent_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        bool isVisible = (bool)e.NewValue;
        if (!isVisible)
            return;

        var updatedProgress = SelectedTracker!;

        txtUpdateProgressAdvancement.Text = "";
        txtUpdateProgressNotes.Text = "";
        dateUpdateProgressDate.SelectedDate = DateTime.Now;
        stackProgress.Children.Clear();

        foreach (var progress in updatedProgress.ProgressionHistory)
            CreateProgressButton(progress.Advancement);
    }

    #region UpdateProgressButtons

    private void BtnUpdateProgress_Click(object sender, RoutedEventArgs e)
    {
        if (!IsProgressSelected)
        {
            Utils.ReportError("You have to Select Progress first!");
            return;
        }

        var progress = SelectedTracker!.ProgressionHistory[SelectedProgressIndex];

        var isValidAdvancement = int.TryParse(txtUpdateProgressAdvancement.Text, out int advancement);

        if (!isValidAdvancement)
        {
            Utils.ReportError("Progress Advancement must be a number!");
            return;
        }

        var notes = txtUpdateProgressNotes.Text;
        var date = dateUpdateProgressDate.SelectedDate ?? DateTime.Now;

        progress.Advancement = advancement;
        progress.Date = date;
        progress.Notes.Clear();
        progress.Notes.Add(notes);

        (stackProgress.Children[SelectedProgressIndex] as Button)!.Content = advancement;

        DeselectAllProgress();
    }

    private void BtnDeleteResetProgress_Click(object sender, RoutedEventArgs e)
    {
        // reset
        if (!IsProgressSelected)
        {
            txtUpdateProgressAdvancement.Text = "";
            txtUpdateProgressNotes.Text = "";
            dateUpdateProgressDate.SelectedDate = DateTime.Now;

            return;
        }

        // delete
        if (SelectedTracker == null)
            return;

        if (MessageBox.Show(
                "Are you sure you want to delete the selected Progress?",
                "Confirmation!",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            ) != MessageBoxResult.Yes)
            return;

        SelectedTracker.ProgressionHistory.RemoveAt(SelectedProgressIndex);
        stackProgress.Children.RemoveAt(SelectedProgressIndex);

        for (int i = 0; i < stackProgress.Children.Count; i++)
            (stackProgress.Children[i] as Button)!.Tag = i;

        DeselectAllProgress();
    }

    private void BtnRegisterProgress_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedTracker == null)
            return;

        var isValidAdvancement = int.TryParse(txtUpdateProgressAdvancement.Text, out int advancement);

        if (!isValidAdvancement)
        {
            Utils.ReportError("Progress Advancement must be a number!");
            return;
        }

        var notes = txtUpdateProgressNotes.Text;
        var date = dateUpdateProgressDate.SelectedDate ?? DateTime.Now;

        date = new DateTime(
            date.Year, date.Month, date.Day,
            DateTime.Now.Hour,
            DateTime.Now.Minute,
            DateTime.Now.Second,
            DateTime.Now.Millisecond
        );

        SelectedTracker.ProgressionHistory.Add(new Progress(advancement, date, new List<string> { notes }));

        CreateProgressButton(advancement);
        DeselectAllProgress();
    }

    #endregion

    #region Utilities

    private void DeselectAllProgress()
    {
        foreach (Button progress in stackProgress.Children)
            progress.Foreground = Brushes.White;

        SelectedProgressIndex = -1;
    }

    private void SelectProgress(int index)
    {
        var button = (stackProgress.Children[index] as Button)!;
        var selected = (int)button.Tag;

        if (selected == SelectedProgressIndex)
        {
            button.Foreground = Brushes.White;
            SelectedProgressIndex = -1;
            return;
        }

        DeselectAllProgress();

        SelectedProgressIndex = selected;
        button.Foreground = Brushes.LimeGreen;
    }

    private void CreateProgressButton(float advancement)
    {
        if (SelectedTracker == null)
            return;

        int index = stackProgress.Children.Count;
        var button = new Button { Content = advancement, Tag = index };
        button.Click += Progress_SelectionChanged;
        stackProgress.Children.Add(button);
    }

    #endregion
}
