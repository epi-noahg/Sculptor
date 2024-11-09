using System;

namespace Sculptor.Models
{
    public class MeasurementEntry
    {
        public DateTime Timestamp { get; set; }
        public bool IsMetric { get; set; }

        // Mesures générales
        public double Height { get; set; }
        public double Weight { get; set; }

        // Haut du corps
        public double Neck { get; set; }
        public double Shoulders { get; set; }
        public double Chest { get; set; }
        public double Abdomen { get; set; }
        public double Waist { get; set; }
        public double Hips { get; set; }

        // Bras droit
        public double RightArm { get; set; }
        public double RightForearm { get; set; }
        public double RightWrist { get; set; }

        // Bras gauche
        public double LeftArm { get; set; }
        public double LeftForearm { get; set; }
        public double LeftWrist { get; set; }

        // Jambe droite
        public double RightThigh { get; set; }
        public double RightLowerThigh { get; set; }
        public double RightCalf { get; set; }
        public double RightAnkle { get; set; }

        // Jambe gauche
        public double LeftThigh { get; set; }
        public double LeftLowerThigh { get; set; }
        public double LeftCalf { get; set; }
        public double LeftAnkle { get; set; }

        // Pourcentage de masse grasse (optionnel)
        public double BodyFatPercentage { get; set; }

        // Propriété formatée pour l'affichage de la date
        public string TimestampFormatted => Timestamp.ToString("dd/MM/yyyy HH:mm");

        public MeasurementEntry()
        {
            Timestamp = DateTime.Now;
            IsMetric = true;
        }
    }
}
