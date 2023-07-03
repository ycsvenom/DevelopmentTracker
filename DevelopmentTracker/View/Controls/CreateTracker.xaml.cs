using DevelopmentTracker.Model;
using DevelopmentTracker.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DevelopmentTracker.View.Controls;

/// <summary>
/// Interaction logic for CreateTracker.xaml
/// </summary>
public partial class CreateTracker : UserControl
{
    private IList<Tracker> Trackers { get => (DataContext as IList<Tracker>)!; }

    public RoutedEventHandler FinishClick
    {
        get { return (RoutedEventHandler)GetValue(FinishClickProperty); }
        set { SetValue(FinishClickProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FinishClick.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FinishClickProperty =
        DependencyProperty.Register("FinishClick", typeof(RoutedEventHandler), typeof(CreateTracker), new PropertyMetadata(null));


    public CreateTracker()
    {
        InitializeComponent();

        foreach (var type in typeof(Tracker).GetInheritedClasses())
            comboTrackerType.Items.Add(type.ToTitleCase());
    }

    private void BtnFinishCreateTracker_Click(object sender, RoutedEventArgs e)
    {
        if (comboTrackerType.SelectedItem == null)
        {
            Utils.ReportError("You have to Choose a Tracker type!");
            return;
        }

        var type = comboTrackerType.SelectedValue.ToString()!.ToPascalCase();
        var name = txtCreateName.Text;
        var desc = txtCreateDesc.Text;
        var url = txtCreateUrl.Text;
        var isValidTotal = int.TryParse(txtCreateProgressTotal.Text, out int intTotal);
        var isValidAdvancement = int.TryParse(txtCreateProgressAdvancement.Text, out int advancement);

        if (!isValidTotal || !isValidAdvancement)
        {
            Utils.ReportError($"{(isValidTotal ? nameof(isValidTotal) : nameof(isValidAdvancement)).Replace("isValid", "")} must be a number!");
            return;
        }

        var tracker = (Tracker)Activator.CreateInstance(Type.GetType($"{typeof(Tracker).Namespace}.{type}")!, name, desc, url, intTotal)!;

        Trackers.Add(tracker);
        Trackers.Last().ProgressionHistory.Add(new Progress(advancement));

        FinishClick?.Invoke(sender, e);
    }

    private void CreateTrackerResetContent_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        bool isVisible = (bool)e.NewValue;
        if (!isVisible)
            return;

        txtCreateName.Text = "";
        txtCreateDesc.Text = "";
        txtCreateUrl.Text = "";
        txtCreateProgressTotal.Text = "";
        txtCreateProgressAdvancement.Text = "";
        comboTrackerType.SelectedIndex = -1;
    }
}
