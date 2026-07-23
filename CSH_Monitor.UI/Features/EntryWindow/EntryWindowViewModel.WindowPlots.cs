using OxyPlot;
using OxyPlot.Series;

namespace CSH_Monitor.UI.Features.EntryWindow
{
    public partial class EntryWindowViewModel
    {
        #region Объявления графиков

        // Инициализация графиков идёт в конструкторе класса!!! Где подключаются службы и т.п.
        private PlotModel sertificationPlotModel;
        public PlotModel SertificationPlotModel
        {
            get => sertificationPlotModel;
            set => SetField(ref sertificationPlotModel, value);
        }

        private PlotModel stabilityPlotModel;
        public PlotModel StabilityPlotModel
        {
            get => stabilityPlotModel;
            set => SetField(ref stabilityPlotModel, value);
        }

        private PlotModel homohenityPlotModel;
        public PlotModel HomohenityPlotModel
        {
            get => homohenityPlotModel;
            set => SetField(ref homohenityPlotModel, value);
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