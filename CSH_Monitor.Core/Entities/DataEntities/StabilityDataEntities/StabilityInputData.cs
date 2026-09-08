using CSH_Monitor.Core.Entities.CommonEnumerableEntities;

namespace CSH_Monitor.Core.Entities.DataEntities.StabilityDataEntities
{
    public class StabilityInputData
    {
        /// <summary>
        /// Основные считываемые данные по неопределённости
        /// </summary>
        public List<DataSeries<double, double>> Values { get; set; } = new();
        /// <summary>
        /// Целевой срок годности
        /// </summary>
        public double TargetExpirationDate;
        /// <summary>
        /// Стандартная неопределённость от нестабильности
        /// </summary>
        public double TargetUncertainty;
        /// <summary>
        /// Стандартная погрешность от нестабильности
        /// </summary>
        public double TargetError;
    }
}
