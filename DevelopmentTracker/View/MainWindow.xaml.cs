using DevelopmentTracker.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Tracker = DevelopmentTracker.Model.Tracker;

namespace DevelopmentTracker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    #region Members

    private IList<Tracker> Trackers { get => (DataContext as TrackerViewModel)!.Trackers; }
    private bool IsCreateActivated { get { return createTracker.Visibility == Visibility.Visible; } }
    private bool IsModifyActivated { get { return modifyTracker.Visibility == Visibility.Visible; } }
    private bool IsRegisterActivated { get { return registerProgress.Visibility == Visibility.Visible; } }

    private Tracker? SelectedTracker { get { return listTrackersGrid.SelectedIndex == -1 ? null : Trackers[listTrackersGrid.SelectedIndex]; } }

    #endregion

    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartRegisterProgress_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedTracker == null)
        {
            MessageBox.Show("You have to select a tracker before to register progress");
            return;
        }

        registerProgress.DataContext = SelectedTracker;

        if (!IsRegisterActivated)
            ShowContent(registerProgress, btnStartRegisterProgress, "End");
        else
            CancelContent();
    }

    private void StartCreateTracker_Click(object sender, RoutedEventArgs e)
    {
        if (!IsCreateActivated)
            ShowContent(createTracker, btnStartCreateTracker);
        else
            CancelContent();
    }

    private void FinishCreateTracker_Click(object sender, RoutedEventArgs e)
    {
        CancelContent();
    }

    private void StartModifyTracker_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedTracker == null)
        {
            MessageBox.Show("You have to select a tracker before to update");
            return;
        }

        modifyTracker.DataContext = SelectedTracker;

        if (!IsModifyActivated)
            ShowContent(modifyTracker, btnStartModifyTracker);
        else
            CancelContent();
    }

    private void FinishModifyTracker_Click(object sender, RoutedEventArgs e)
    {
        CancelContent();
        RefreshMetrics();
    }

    #region WindowEvents

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        RefreshMetrics();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        SaveSession();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Ctrl + S
        if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
        {
            SaveSession();
            MessageBox.Show("Save Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Alt + F4
        else if (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.F4)
            Close();
    }

    #endregion

    #region ListTrackersEvents

    private void ListTrackersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        StartRegisterProgress_Click(sender, e);
    }

    private void ListTrackersGrid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (SelectedTracker == null)
            return;

        if (e.MiddleButton != MouseButtonState.Pressed)
            return;

        SelectedTracker.Open();
    }

    private void ListTrackersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshMetrics();
    }

    private void OnListViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {

    }

    #endregion

    #region ContextMenuEvents

    private void ContextDeleteTracker_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedTracker == null)
            return;

        if (MessageBox.Show(
                "Are you sure you want to delete the selected Trackers?",
                "Confirmation!",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            ) == MessageBoxResult.No)
            return;

        Trackers.RemoveAt(listTrackersGrid.SelectedIndex);
        listTrackersGrid.SelectedIndex = -1;
        RefreshMetrics();
    }

    private void ContextOpenTracker_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedTracker == null)
            return;

        SelectedTracker.Open();
    }

    private void ContextRegisterProgress_Click(object sender, RoutedEventArgs e)
    {
        StartRegisterProgress_Click(sender, e);
    }

    private void ContextModifyTracker_Click(object sender, RoutedEventArgs e)
    {
        StartModifyTracker_Click(sender, e);
    }

    private void ContextExportTracker_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var content = SelectedTracker!.Export();

            var sfd = new SaveFileDialog
            {
                AddExtension = true,
                Filter = "Markdown|*.md",
            };

            if (sfd.ShowDialog() == false)
                return;

            using var writer = new StreamWriter(sfd.FileName);
            writer.Write(content);
        }
        catch (NotImplementedException)
        {
            MessageBox.Show("Feature is Coming soon!", "Not Implemented yet", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    #endregion

    #region Utilities

    private void SaveSession()
    {
        var viewModel = (DataContext as TrackerViewModel)!;
        var content = JsonSerializer.Serialize(Trackers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(viewModel.TrackerPath, content);
    }

    private void ShowContent(UIElement elem, Button? sender = null, string content = "Cancel")
    {
        ResetContent();

        listTrackersGrid.Visibility = Visibility.Collapsed;
        registerProgress.Visibility = Visibility.Collapsed;
        createTracker.Visibility = Visibility.Collapsed;
        modifyTracker.Visibility = Visibility.Collapsed;

        if (sender != null)
            sender.Content = content;

        elem.Visibility = Visibility.Visible;
        listTrackersGrid.Items.Refresh();
    }

    private void ResetContent()
    {
        btnStartCreateTracker.Content = "Create Tracker";
        btnStartModifyTracker.Content = "Modify Tracker";
        btnStartRegisterProgress.Content = "Register Progress";

        btnStartCreateTracker.Background = (Brush)FindResource("Neon");
        btnStartCreateTracker.Foreground = (Brush)FindResource("LightBlack");

        btnStartModifyTracker.Background = (Brush)FindResource("Neon");
        btnStartModifyTracker.Foreground = (Brush)FindResource("LightBlack");

        btnStartRegisterProgress.Background = (Brush)FindResource("Neon");
        btnStartRegisterProgress.Foreground = (Brush)FindResource("LightBlack");
    }

    private void CancelContent()
    {
        ResetContent();
        ShowContent(listTrackersGrid);
    }

    private void RefreshMetrics()
    {
        float totalTotal = Trackers.Sum(d => d.Total);
        float totalReached = Trackers.Sum(d => d.Reached);
        float advancement = totalReached / Math.Max(totalTotal, 1) * 100;
        int remaining = (int)Math.Max(totalTotal - totalReached, 0);

        if (SelectedTracker != null)
        {
            advancement = SelectedTracker.Reached / SelectedTracker.Total * 100;
            remaining = (int)Math.Max(SelectedTracker.Total - SelectedTracker.Reached, 0);
        }

        prgsProgress.Value = Math.Min(advancement, 100);
        lblRemaining.Content = $"Remaining: {remaining}";
        listTrackersGrid.Items.Refresh();
    }

    #endregion

    #region MenuBar

    private void MenuSave_Click(object sender, RoutedEventArgs e)
    {
        SaveSession();
        MessageBox.Show("Save Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void MenuExit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    #endregion
}
