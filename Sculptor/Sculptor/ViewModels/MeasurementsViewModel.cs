using Sculptor.Models;
using Sculptor.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage;

namespace Sculptor.ViewModels
{
    public class MeasurementsViewModel : INotifyPropertyChanged
    {
        private DataService dataService;

        public int SelectedUnitIndex { get; set; } // 0 pour métrique, 1 pour impérial

        // Propriétés pour les mesures
        public double Height { get; set; }
        public double Weight { get; set; }
        public double Neck { get; set; }
        public double Shoulders { get; set; }
        public double Chest { get; set; }
        public double Abdomen { get; set; }
        public double Waist { get; set; }
        public double Hips { get; set; }
        public double RightArm { get; set; }
        public double RightForearm { get; set; }
        public double RightWrist { get; set; }
        public double LeftArm { get; set; }
        public double LeftForearm { get; set; }
        public double LeftWrist { get; set; }
        // Ajoutez les autres propriétés pour les jambes

        public ICommand SaveCommand { get; }

        public MeasurementsViewModel()
        {
            dataService = new DataService();
            SaveCommand = new RelayCommand(SaveMeasurements);

            // Charger l'unité par défaut à partir des paramètres
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("SelectedUnitIndex"))
            {
                SelectedUnitIndex = (int)localSettings.Values["SelectedUnitIndex"];
            }
            else
            {
                SelectedUnitIndex = 0; // Par défaut à métrique
            }

            _ = LoadLastMeasurementAsync();
        }

        private async Task LoadLastMeasurementAsync()
        {
            await dataService.LoadDataAsync();
            if (dataService.Measurements.Any())
            {
                var lastMeasurement = dataService.Measurements.Last();
                // Charger les valeurs dans les propriétés
                Height = lastMeasurement.Height;
                Weight = lastMeasurement.Weight;
                Neck = lastMeasurement.Neck;
                Shoulders = lastMeasurement.Shoulders;
                Chest = lastMeasurement.Chest;
                Abdomen = lastMeasurement.Abdomen;
                Waist = lastMeasurement.Waist;
                Hips = lastMeasurement.Hips;
                RightArm = lastMeasurement.RightArm;
                RightForearm = lastMeasurement.RightForearm;
                RightWrist = lastMeasurement.RightWrist;
                LeftArm = lastMeasurement.LeftArm;
                LeftForearm = lastMeasurement.LeftForearm;
                LeftWrist = lastMeasurement.LeftWrist;
                // Charger les autres propriétés
                SelectedUnitIndex = lastMeasurement.IsMetric ? 0 : 1;
                OnPropertyChanged(string.Empty); // Mettre à jour toutes les propriétés
            }
        }

        private async void SaveMeasurements()
        {
            try
            {
                var measurement = new MeasurementEntry
                {
                    IsMetric = SelectedUnitIndex == 0,
                    Height = Height,
                    Weight = Weight,
                    Neck = Neck,
                    Shoulders = Shoulders,
                    Chest = Chest,
                    Abdomen = Abdomen,
                    Waist = Waist,
                    Hips = Hips,
                    RightArm = RightArm,
                    RightForearm = RightForearm,
                    RightWrist = RightWrist,
                    LeftArm = LeftArm,
                    LeftForearm = LeftForearm,
                    LeftWrist = LeftWrist,
                    // Initialize other properties
                    RightThigh = 0, // Example value, replace with actual data
                    LeftThigh = 0,  // Example value, replace with actual data
                                    // Continue initializing other properties...
                };

                dataService.Measurements.Add(measurement);
                await dataService.SaveDataAsync();

                // Show notification on the UI thread
                await ShowSaveDialogAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show a message to the user)
                System.Diagnostics.Debug.WriteLine($"Error saving measurements: {ex.Message}");
            }
        }

        private async Task ShowSaveDialogAsync()
        {
            var dialog = new ContentDialog
            {
                Title = "Mesures enregistrées",
                Content = "Vos mesures ont été enregistrées avec succès.",
                CloseButtonText = "OK",
                //XamlRoot = App.MainWindow.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
