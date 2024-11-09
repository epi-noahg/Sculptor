using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sculptor.Views;
using Windows.UI.ApplicationSettings;

namespace Sculptor
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            MainNavigationView.SelectedItem = MainNavigationView.MenuItems[0];
            ContentFrame.Navigate(typeof(DashboardPage));
        }

        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                switch (selectedItem.Tag)
                {
                    case "DashboardPage":
                        ContentFrame.Navigate(typeof(DashboardPage));
                        break;
                    case "MeasurementsPage":
                        ContentFrame.Navigate(typeof(MeasurementsPage));
                        break;
                    case "ActivitiesPage":
                        ContentFrame.Navigate(typeof(ActivitiesPage));
                        break;
                    case "NutritionPage":
                        ContentFrame.Navigate(typeof(NutritionPage));
                        break;
                    case "GoalsPage":
                        ContentFrame.Navigate(typeof(GoalsPage));
                        break;
                    case "SettingsPage":
                        ContentFrame.Navigate(typeof(SettingsPage));
                        break;
                }
            }
        }


        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            // Code pour gérer l'activation de la fenêtre si nécessaire
        }
    }
}
