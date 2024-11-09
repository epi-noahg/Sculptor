using Sculptor.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sculptor.Services
{
    public class DataService
    {
        private const string MeasurementsFileName = "measurements.json";
        private const string ActivitiesFileName = "activities.json";
        private const string NutritionFileName = "nutrition.json";
        private const string GoalsFileName = "goals.json";

        private string AppDataFolder => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Sculptor");

        public List<MeasurementEntry> Measurements { get; private set; }
        public List<ActivityEntry> Activities { get; private set; }
        public List<NutritionEntry> NutritionEntries { get; private set; }
        public Goal UserGoal { get; set; }

        public DataService()
        {
            Measurements = new List<MeasurementEntry>();
            Activities = new List<ActivityEntry>();
            NutritionEntries = new List<NutritionEntry>();
            UserGoal = new Goal();
            Directory.CreateDirectory(AppDataFolder);
        }

        public async Task LoadDataAsync()
        {
            Measurements = await LoadAsync<List<MeasurementEntry>>(MeasurementsFileName) ?? new List<MeasurementEntry>();
            Activities = await LoadAsync<List<ActivityEntry>>(ActivitiesFileName) ?? new List<ActivityEntry>();
            NutritionEntries = await LoadAsync<List<NutritionEntry>>(NutritionFileName) ?? new List<NutritionEntry>();
            UserGoal = await LoadAsync<Goal>(GoalsFileName) ?? new Goal();
        }

        public async Task SaveDataAsync()
        {
            await SaveAsync(MeasurementsFileName, Measurements);
            await SaveAsync(ActivitiesFileName, Activities);
            await SaveAsync(NutritionFileName, NutritionEntries);
            await SaveAsync(GoalsFileName, UserGoal);
        }

        private async Task<T> LoadAsync<T>(string fileName)
        {
            string filePath = Path.Combine(AppDataFolder, fileName);
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<T>(json);
            }
            return default;
        }

        private async Task SaveAsync<T>(string fileName, T data)
        {
            string filePath = Path.Combine(AppDataFolder, fileName);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
