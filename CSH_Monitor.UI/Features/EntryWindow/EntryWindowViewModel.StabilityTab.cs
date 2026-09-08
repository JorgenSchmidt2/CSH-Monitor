using CSH_Monitor.GraphicsCore.GraphicsEntities;
using OxyPlot;
using OxyPlot.Series;

namespace CSH_Monitor.UI.Features.EntryWindow
{
    public partial class EntryWindowViewModel
    {
        #region Данные вкладки
        // Целевой срок годности
        private string targetExpirationDate;
        public string TargetExpirationDate
        {
            get => targetExpirationDate;
            set => SetProperty(ref targetExpirationDate, value);
        }

        // Целевая неопределённость
        private string targetUncertanity;
        public string TargetUncertanity
        {
            get => targetUncertanity;
            set => SetProperty(ref targetUncertanity, value);
        }

        // Целевая погрешность
        private string targetError;
        public string TargetError
        {
            get => targetError;
            set => SetProperty(ref targetError, value);
        }

        // Свитчер вероятности 0.90, 0.95, 0.99
        public List<EnumerableGrapchicsItem<double>> ProbabilityItems { get; } = new List<EnumerableGrapchicsItem<double>>
        {
            new EnumerableGrapchicsItem<double>(0.90),
            new EnumerableGrapchicsItem<double>(0.95),
            new EnumerableGrapchicsItem<double>(0.99)
        };
        public double selectedProbabilityValue = 0.95;
        public double SelectedProbabilityValue
        {
            get => selectedProbabilityValue;
            set => SetProperty(ref selectedProbabilityValue, value);
        }
        #endregion

        #region 
        private void ReadMainStringAsStabilityData()
        {
            // !!!

            StabilityPlotModel.Series.Clear();
            StabilityPlotModel.InvalidatePlot(true);

            // 1 Точки
            var DataPoints = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 3,
                MarkerFill = OxyColors.Blue
            };

            // 2 Линия тренда
            var TrendLine = new LineSeries
            {
                MarkerStrokeThickness = 1.5,
                Color = OxyColors.Red
            };

            // 3 Нижняя граница
            var LowerLine = new LineSeries
            {
                MarkerStrokeThickness = 1.5,
                Color = OxyColors.Blue
            };

            // 4 Верхняя граница
            var UpperLine = new LineSeries
            {
                MarkerStrokeThickness = 1.5,
                Color = OxyColors.Blue
            };

            // Заполнение данных

            //!!!

            StabilityPlotModel.Series.Add(DataPoints);
            StabilityPlotModel.Series.Add(TrendLine);
            //StabilityPlotModel.Series.Add(LowerLine);
            //StabilityPlotModel.Series.Add(UpperLine);

            // Перезагрузка
            StabilityPlotModel.InvalidatePlot(true);
        }
        #endregion
    }
}