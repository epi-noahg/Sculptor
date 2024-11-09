using System;

namespace Sculptor.Models
{
    public class Goal
    {
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public double TargetWeight { get; set; }
        public double TargetBodyFatPercentage { get; set; }

        public Goal()
        {
            StartDate = DateTime.Now;
        }
    }
}
