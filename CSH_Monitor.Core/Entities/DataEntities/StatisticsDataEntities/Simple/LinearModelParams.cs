namespace CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple
{
    /// <summary>
    /// Содержит необходимое для построения линии тренда по линейному закону (полином первого порядка)
    /// Объект неизбежно будет мутировать из за необходимости заполнить поле для СКО текущей модели
    /// </summary>
    public class LinearModelParams
    {
        /// <summary>
        /// Указывает на среднее занчение по опорной оси (маркированной)
        /// </summary>
        public double MarkerAverage { get; set; }
        /// <summary>
        /// Указывает на среднее значение целевых значений в выборке
        /// </summary>
        public double ValueAverage { get; set; }
        /// <summary>
        /// Начальная точка построения линейной зависимости
        /// </summary>
        public double Intercept { get; set; }
        /// <summary>
        /// Коэфициент наклона прямой (закона изменения линии тренда по оси Y)
        /// </summary>
        public double Slope { get; set; }
        /// <summary>
        /// Оценка СКО погрешностей
        /// Должен быть заполнен на второй итерации, как следствие будет мутировать
        /// </summary>
        public double SlopeStandartError { get; set; }

        /// <summary>
        /// Сеттер, заполняющий значение СКО для модели
        /// </summary>
        public void SetSlopeStandartError (double _slopeStandartError) => SlopeStandartError = _slopeStandartError;
        
    }
}