using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace Sculptor.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private ApplicationDataContainer localSettings;

        // Propriété pour l'unité par défaut
        public int SelectedUnitIndex { get; set; } // 0 pour métrique, 1 pour impérial

        public ICommand SaveSettingsCommand { get; }

        public SettingsViewModel()
        {
            localSettings = ApplicationData.Current.LocalSettings;
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Charger les paramètres enregistrés
            if (localSettings.Values.ContainsKey("SelectedUnitIndex"))
            {
                SelectedUnitIndex = (int)localSettings.Values["SelectedUnitIndex"];
            }
            else
            {
                SelectedUnitIndex = 0; // Valeur par défaut
            }

            OnPropertyChanged(nameof(SelectedUnitIndex));
        }

        private void SaveSettings()
        {
            // Enregistrer les paramètres
            localSettings.Values["SelectedUnitIndex"] = SelectedUnitIndex;

            // Afficher une notification à l'utilisateur
            var dialog = new ContentDialog
            {
                Title = "Paramètres enregistrés",
                Content = "Vos paramètres ont été enregistrés avec succès.",
                CloseButtonText = "OK",
                //XamlRoot = App.MainWindow.Content.XamlRoot
            };
            _ = dialog.ShowAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
