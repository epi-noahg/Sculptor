using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Sculptor.Models;
using Sculptor.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Sculptor.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public ISeries[] WeightSeries { get; set; }
        public Axis[] DateAxis { get; set; }
        public double CurrentWeight { get; set; }
        public double CurrentBodyFatPercentage { get; set; }

        private DataService dataService;

        public DashboardViewModel()
        {
            dataService = new DataService();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await dataService.LoadDataAsync();

            if (dataService.Measurements.Any())
            {
                var weights = dataService.Measurements.Select(m => m.Weight).ToArray();
                var dates = dataService.Measurements.Select(m => m.Timestamp.ToString("dd/MM")).ToArray();

                WeightSeries = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Values = weights,
                        Fill = null,
                        GeometrySize = 8,
                        LineSmoothness = 0.6,
                        Stroke = new SolidColorPaint(SKColors.Blue),
                        GeometryStroke = new SolidColorPaint(SKColors.Blue)
                    }
                };

                DateAxis = new Axis[]
                {
                    new Axis
                    {
                        Labels = dates,
                        LabelsRotation = 15,
                        SeparatorsPaint = new SolidColorPaint
                        {
                            Color = SKColors.LightGray
                        }
                    }
                };

                CurrentWeight = weights.Last();
                CurrentBodyFatPercentage = dataService.Measurements.Last().BodyFatPercentage;

                OnPropertyChanged(nameof(WeightSeries));
                OnPropertyChanged(nameof(DateAxis));
                OnPropertyChanged(nameof(CurrentWeight));
                OnPropertyChanged(nameof(CurrentBodyFatPercentage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
