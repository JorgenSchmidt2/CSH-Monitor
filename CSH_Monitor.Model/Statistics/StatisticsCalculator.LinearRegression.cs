using CSH_Monitor.Core.Entities.CommonListEntities;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple;
using CSH_Monitor.Core.Responses;

namespace CSH_Monitor.Model.Statistics
{
    public partial class StatisticsCalculator
    {
        /// <summary>
        /// Вычисляет параметры линейной регрессии для пары "Маркер-Значение"
        /// </summary>
        public DataResponse<LinearModelParams> GetLinearRegressionEstimate(MarkedDoubleRecList Records)
        {
            var Result = new DataResponse<LinearModelParams>
            {
                Data = new LinearModelParams(),
                Message = "Установление закона линейной регрессии",
            };

            try
            {
                Result.Data = GetAllParams(Records);
                Result.Message += ": успешно.";
                Result.Status = true;
            }
            catch (Exception e)
            {
                Result.Message += ", возникла ошибка: \n" + e.Message;
                Result.Status = false;
            }

            return Result;
        }

        private LinearModelParams GetAllParams(MarkedDoubleRecList Records)
        {
            var Result = new LinearModelParams();

            Result.MarkerAverage = Records.Average(x => x.Marker);
            Result.ValueAverage = Records.Average(x => x.Value);
            Result.Slope = GetSlope(Records, Result.MarkerAverage, Result.ValueAverage);
            Result.Intercept = GetIntercept(Result.MarkerAverage, Result.ValueAverage, Result.Slope);

            return Result;
        }

        private double GetSlope (MarkedDoubleRecList Records, double MarkerAverage, double ValueAverage)
        {
            double Numerator = 0;
            double Denominator = 0;

            // Выполняем расчёты по методу наименьших квадратов
            foreach (var item in Records)
            {
                Numerator += (item.Value - ValueAverage) * (item.Marker - MarkerAverage); 
                Denominator += Math.Pow(item.Marker - MarkerAverage, 2);
            }

            return Numerator / Denominator;
        }

        private double GetIntercept(double MarkerAverage, double ValueAverage, double Slope) 
            => ValueAverage - Slope * MarkerAverage;

    }
}
