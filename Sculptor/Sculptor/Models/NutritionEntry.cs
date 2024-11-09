using System;

namespace Sculptor.Models
{
    public class NutritionEntry
    {
        public DateTime Date { get; set; }
        public int CaloriesConsumed { get; set; }
        public int ProteinGrams { get; set; }
        public int CarbsGrams { get; set; }
        public int FatGrams { get; set; }
        public string DateFormatted => Date.ToString("dd/MM/yyyy");

        public NutritionEntry()
        {
            Date = DateTime.Now;
        }
    }
}
