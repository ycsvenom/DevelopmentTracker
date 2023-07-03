using System.Windows;

namespace DevelopmentTracker
{
    public static class Utils
    {
        public static void ReportError(string message)
        {
            MessageBox.Show(
                    message,
                    "Fatal",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
            );
        }
    }
}
