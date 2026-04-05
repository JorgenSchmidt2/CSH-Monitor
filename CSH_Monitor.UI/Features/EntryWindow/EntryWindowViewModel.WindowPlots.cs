using CSH_Monitor.Core.Entities.CommonListEntities;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple;
using OxyPlot;
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

        private PlotModel stabilityplotModel;
        public PlotModel StabilityPlotModel
        {
            get => stabilityplotModel;
            set => SetField(ref stabilityplotModel, value);
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

            var Model = new LinearModelParams();
            var Result = _stabilityCalculator.GetRegressionModelData(DataList, ref Model);
        }

        #endregion
    }
}