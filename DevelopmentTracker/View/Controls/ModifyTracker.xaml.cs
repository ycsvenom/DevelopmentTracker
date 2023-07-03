using DevelopmentTracker.Model;
using System.Windows;
using System.Windows.Controls;

namespace DevelopmentTracker.View.Controls;

/// <summary>
/// Interaction logic for ModifyTracker.xaml
/// </summary>
public partial class ModifyTracker : UserControl
{
    #region Members

    private Tracker? SelectedTracker { get { return (DataContext as Tracker); } }

    #endregion

    public RoutedEventHandler FinishClick
    {
        get { return (RoutedEventHandler)GetValue(FinishClickProperty); }
        set { SetValue(FinishClickProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FinishClick.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FinishClickProperty =
        DependencyProperty.Register("FinishClick", typeof(RoutedEventHandler), typeof(ModifyTracker), new PropertyMetadata(null));

    public ModifyTracker()
    {
        InitializeComponent();
    }

    private void BtnFinishModifyTracker_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedTracker == null)
            return;

        var isValidTotal = int.TryParse(txtModifyProgressTotal.Text, out int total);

        if (!isValidTotal)
        {
            Utils.ReportError("Total must be a number!");
            return;
        }

        SelectedTracker.Name = txtModifyName.Text;
        SelectedTracker.Description = txtModifyDesc.Text;
        SelectedTracker.Url = txtModifyUrl.Text;
        SelectedTracker.Total = total;

        FinishClick?.Invoke(sender, e);
    }

    private void ModifyTrackerResetContent_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        bool isVisible = (bool)e.NewValue;
        if (!isVisible)
            return;

        var updatedProgress = SelectedTracker!;

        txtModifyName.Text = updatedProgress.Name;
        txtModifyDesc.Text = updatedProgress.Description;
        txtModifyUrl.Text = updatedProgress.Url;
        txtModifyProgressTotal.Text = updatedProgress.Total.ToString();
    }
}
