using Sculptor.Models;
using Sculptor.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace Sculptor.ViewModels
{
    public class GoalsViewModel : INotifyPropertyChanged
    {
        private DataService dataService;

        // Propriétés pour le nouvel objectif
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset TargetDate { get; set; }
        public string TargetWeight { get; set; }
        public string TargetBodyFatPercentage { get; set; }

        public string CurrentGoalSummary { get; set; }

        public ICommand SaveGoalCommand { get; }

        public GoalsViewModel()
        {
            dataService = new DataService();
            SaveGoalCommand = new RelayCommand(SaveGoal);
            _ = LoadGoalAsync();
        }

        private async Task LoadGoalAsync()
        {
            await dataService.LoadDataAsync();
            var goal = dataService.UserGoal;
            if (goal != null && goal.TargetDate != DateTime.MinValue)
            {
                StartDate = goal.StartDate;
                TargetDate = goal.TargetDate;
                TargetWeight = goal.TargetWeight.ToString();
                TargetBodyFatPercentage = goal.TargetBodyFatPercentage.ToString();
                UpdateCurrentGoalSummary();
            }
            else
            {
                StartDate = DateTimeOffset.Now;
                TargetDate = DateTimeOffset.Now.AddMonths(1);
            }
            OnPropertyChanged(nameof(StartDate));
            OnPropertyChanged(nameof(TargetDate));
            OnPropertyChanged(nameof(TargetWeight));
            OnPropertyChanged(nameof(TargetBodyFatPercentage));
        }

        private async void SaveGoal()
        {
            if (!double.TryParse(TargetWeight, out double weight) ||
                !double.TryParse(TargetBodyFatPercentage, out double bfPercentage))
            {
                // Afficher un message d'erreur à l'utilisateur
                var dialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Veuillez saisir des informations valides pour l'objectif.",
                    CloseButtonText = "OK",
                    //XamlRoot = App.MainWindow.Content.XamlRoot
                };
                _ = dialog.ShowAsync();
                return;
            }

            var goal = new Goal
            {
                StartDate = StartDate.DateTime,
                TargetDate = TargetDate.DateTime,
                TargetWeight = weight,
                TargetBodyFatPercentage = bfPercentage
            };

            dataService.UserGoal = goal;
            await dataService.SaveDataAsync();

            UpdateCurrentGoalSummary();

            var successDialog = new ContentDialog
            {
                Title = "Objectif enregistré",
                Content = "Votre objectif a été enregistré avec succès.",
                CloseButtonText = "OK",
                //XamlRoot = App.MainWindow.Content.XamlRoot
            };
            _ = successDialog.ShowAsync();
        }

        private void UpdateCurrentGoalSummary()
        {
            CurrentGoalSummary = $"Début : {StartDate:dd/MM/yyyy}\n" +
                                 $"Cible : {TargetDate:dd/MM/yyyy}\n" +
                                 $"Poids cible : {TargetWeight} kg\n" +
                                 $"BF% cible : {TargetBodyFatPercentage}%";
            OnPropertyChanged(nameof(CurrentGoalSummary));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
