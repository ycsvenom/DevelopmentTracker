using DevelopmentTracker.ViewModel;
using System.Windows;

namespace DevelopmentTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StartMainWindow();
        }

        private static void StartMainWindow()
        {
            var window = new MainWindow();
            var viewModel = new TrackerViewModel();
            window.DataContext = viewModel;
            window.Show();
        }
    }
}
