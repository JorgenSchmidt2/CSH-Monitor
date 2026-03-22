namespace CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple
{
    /// <summary>
    /// Чертёж типичного набора данных для модели регрессии по оси Y
    /// </summary>
    public class RegressionPoint
    {
        /// <summary>
        /// Измеренное значение
        /// </summary>
        public double MeansuredValue { get; set; }
        /// <summary>
        /// Точка для построения линии тренда
        /// </summary>
        public double RegressionLinePoint { get; set; }
        /// <summary>
        /// Оценка СКО модели регрессии для текущего значения
        /// </summary>
        public double ModelEstimate { get; set; }
        /// <summary>
        /// Точка для доверительного интервала
        /// </summary>
        public double TrustInterval { get; set; }
    }
}