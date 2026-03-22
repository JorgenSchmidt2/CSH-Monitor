namespace CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple
{
    public class SimplifiedRegressionPoint
    {
        /// <summary>
        /// Измеренное значение
        /// </summary>
        public double MeansuredValue { get; set; }
        /// <summary>
        /// Точка для построения линии тренда
        /// </summary>
        public double RegressionLinePoint { get; set; }
    }
}