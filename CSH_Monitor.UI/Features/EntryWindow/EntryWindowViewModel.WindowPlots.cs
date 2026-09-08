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
        

        #endregion
    }
}