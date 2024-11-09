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
    public class NutritionViewModel : INotifyPropertyChanged
    {
        private DataService dataService;

        public ObservableCollection<NutritionEntry> NutritionEntries { get; set; }

        // Propriétés pour l'entrée du jour
        public string CaloriesConsumed { get; set; }
        public string ProteinGrams { get; set; }
        public string CarbsGrams { get; set; }
        public string FatGrams { get; set; }

        public ICommand SaveNutritionCommand { get; }

        public NutritionViewModel()
        {
            dataService = new DataService();
            NutritionEntries = new ObservableCollection<NutritionEntry>();
            SaveNutritionCommand = new RelayCommand(SaveNutrition);
            _ = LoadNutritionAsync();
        }

        private async Task LoadNutritionAsync()
        {
            await dataService.LoadDataAsync();
            foreach (var entry in dataService.NutritionEntries.OrderByDescending(n => n.Date))
            {
                NutritionEntries.Add(entry);
            }
        }

        private async void SaveNutrition()
        {
            if (!int.TryParse(CaloriesConsumed, out int calories) ||
                !int.TryParse(ProteinGrams, out int protein) ||
                !int.TryParse(CarbsGrams, out int carbs) ||
                !int.TryParse(FatGrams, out int fat))
            {
                // Afficher un message d'erreur à l'utilisateur
                var dialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Veuillez saisir des informations valides pour la nutrition.",
                    CloseButtonText = "OK",
                    //XamlRoot = App.MainWindow.Content.XamlRoot
                };
                _ = dialog.ShowAsync();
                return;
            }

            var nutritionEntry = new NutritionEntry
            {
                Date = DateTime.Now,
                CaloriesConsumed = calories,
                ProteinGrams = protein,
                CarbsGrams = carbs,
                FatGrams = fat
            };

            dataService.NutritionEntries.Add(nutritionEntry);
            NutritionEntries.Insert(0, nutritionEntry);

            await dataService.SaveDataAsync();

            // Réinitialiser les champs
            CaloriesConsumed = string.Empty;
            ProteinGrams = string.Empty;
            CarbsGrams = string.Empty;
            FatGrams = string.Empty;
            OnPropertyChanged(nameof(CaloriesConsumed));
            OnPropertyChanged(nameof(ProteinGrams));
            OnPropertyChanged(nameof(CarbsGrams));
            OnPropertyChanged(nameof(FatGrams));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Implémentation de RelayCommand (si non déjà définie)
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;
        public event EventHandler CanExecuteChanged;
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => canExecute == null || canExecute();
        public void Execute(object parameter) => execute();
    }
}
