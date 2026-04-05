using CSH_Monitor.Core.Entities.CommonListEntities;
using CSH_Monitor.Core.Interfaces.Infrastructure;
using CSH_Monitor.Core.Interfaces.Model;
using CSH_Monitor.GraphicsCore.Base;
using CSH_Monitor.GraphicsCore.Interfaces;
using OxyPlot;
using OxyPlot.Axes;
using System.Windows;

namespace CSH_Monitor.UI.Features.EntryWindow
{
    public partial class EntryWindowViewModel : ViewModelBase
    {
        // Подключаем нужные службы в конструкторе
        private IWindowService _windowService { get; }
        private ITabularParser _tabularParser { get; }
        private IStabilityCalculator _stabilityCalculator { get; }
        public IPlotController СustomController { get; private set; }

        // Конструктор
        public EntryWindowViewModel(
            IWindowService windowService, 
            ITabularParser tabularParser,
            IStabilityCalculator stabilityCalculator
        )
        {
            _windowService = windowService;
            _tabularParser = tabularParser;
            _stabilityCalculator = stabilityCalculator;

            var controller = new PlotController();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            СustomController = controller;

            InitalizeAllPlots();
        }
        

        // Инициализация всех графиков 
        private void InitalizeAllPlots()
        {
            StabilityPlotModel = new PlotModel { Title = "График стабильности" };

            StabilityPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            StabilityPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });
        }
    }
}