using CSH_Monitor.Core.Entities.CommonListEntities;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Enumerable;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple;

namespace CSH_Monitor.Core.Entities.DataEntities.StabilityDataEntities
{
    public class StabilityData
    {
        /// <summary>
        /// Значение маркировано, т.к. пара "время-значение"
        /// </summary>
        public MarkedDoubleRecList InitialValues { get; } = new();
        /// <summary>
        /// Содержит необходимое для построения линейной модели тренда
        /// </summary>
        public LinearModelParams LinearModelParams { get; set; } = new();
        /// <summary>
        /// Содержит полный набор данных по графикам
        /// </summary>
        public RegressionModelData StabilityModelData { get; } = new();
    }
}