using CSH_Monitor.Core.Entities.CommonListEntities;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple;
using OxyPlot;
using OxyPlot.Series;
using System.Windows;

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

        #region Логика чтения данных
        // 
        private void ReadMainData(string MainData, ref MarkedDoubleRecList DataList)
        {
            var CurrentResponce = _tabularParser.GetDoubleMeasuredData(MainData);
            if (!CurrentResponce.Status || CurrentResponce.Data == null)
            {
                MessageBox.Show(CurrentResponce.Message);
                return;
            }
            DataList = CurrentResponce.Data;
        }

        private void ReadMainStringAsStabilityData()
        {
            var DataList = new MarkedDoubleRecList();
            ReadMainData(MainDataString, ref DataList);

            var ModelResponse = _statisticsCalculator.GetLinearRegressionEstimate(DataList);
            if (!ModelResponse.Status || ModelResponse.Data == null)
            {
                MessageBox.Show(ModelResponse.Message);
                return;
            }
            var Model = ModelResponse.Data;

            var ResultResponce = _stabilityCalculator.GetRegressionModelData(DataList, ref Model);
            if (!ResultResponce.Status || ResultResponce.Data == null)
            {
                MessageBox.Show(ResultResponce.Message);
                return;
            }

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
            foreach (var Item in ResultResponce.Data)
            {
                DataPoints.Points.Add(new ScatterPoint(Item.Marker, Item.Value.MeansuredValue));
                TrendLine.Points.Add(new DataPoint(Item.Marker, Item.Value.RegressionLinePoint));
                LowerLine.Points.Add(new DataPoint(Item.Marker, Item.Value.RegressionLinePoint - Item.Value.TrustInterval));
                UpperLine.Points.Add(new DataPoint(Item.Marker, Item.Value.RegressionLinePoint + Item.Value.TrustInterval));
            }
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