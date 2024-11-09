using System;

namespace Sculptor.Models
{
    public class ActivityEntry
    {
        public DateTime Date { get; set; }
        public string ActivityName { get; set; }
        public double DurationMinutes { get; set; }
        public int CaloriesBurned { get; set; }

        // Propriété pour formater la date
        public string DateFormatted => Date.ToString("dd/MM/yyyy");

        public ActivityEntry()
        {
            Date = DateTime.Now;
        }
    }
}
