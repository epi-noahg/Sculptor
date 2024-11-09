using Sculptor.Models;
using Sculptor.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Sculptor.ViewModels
{
    public class ActivitiesViewModel : INotifyPropertyChanged
    {
        private DataService dataService;

        public ObservableCollection<ActivityEntry> Activities { get; set; }

        // Propriétés pour la nouvelle activité
        public string NewActivityName { get; set; }
        public string NewActivityDuration { get; set; } // On utilise string pour gérer la conversion
        public string NewActivityCalories { get; set; }

        public ICommand AddActivityCommand { get; }

        public ActivitiesViewModel()
        {
            dataService = new DataService();
            Activities = new ObservableCollection<ActivityEntry>();
            AddActivityCommand = new RelayCommand(AddActivity);
            _ = LoadActivitiesAsync();
        }

        private async Task LoadActivitiesAsync()
        {
            await dataService.LoadDataAsync();
            foreach (var activity in dataService.Activities.OrderByDescending(a => a.Date))
            {
                Activities.Add(activity);
            }
        }

        private async void AddActivity()
        {
            if (string.IsNullOrWhiteSpace(NewActivityName) ||
                !double.TryParse(NewActivityDuration, out double duration) ||
                !int.TryParse(NewActivityCalories, out int calories))
            {
                // Afficher un message d'erreur à l'utilisateur
                var dialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Veuillez saisir des informations valides pour l'activité.",
                    CloseButtonText = "OK",
                    //XamlRoot = App.MainWindow.Content.XamlRoot
                };
                _ = dialog.ShowAsync();
                return;
            }

            var activity = new ActivityEntry
            {
                ActivityName = NewActivityName,
                DurationMinutes = duration,
                CaloriesBurned = calories,
                Date = DateTime.Now
            };

            dataService.Activities.Add(activity);
            Activities.Insert(0, activity); // Ajouter au début de la liste

            await dataService.SaveDataAsync();

            // Réinitialiser les champs
            NewActivityName = string.Empty;
            NewActivityDuration = string.Empty;
            NewActivityCalories = string.Empty;
            OnPropertyChanged(nameof(NewActivityName));
            OnPropertyChanged(nameof(NewActivityDuration));
            OnPropertyChanged(nameof(NewActivityCalories));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
