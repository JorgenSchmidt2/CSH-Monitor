using CSH_Monitor.Core.Entities.CommonListEntities;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Enumerable;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple;
using CSH_Monitor.Core.Generics.CommonGenerics.TwoParamGenerics;
using CSH_Monitor.Core.Interfaces.Model;
using CSH_Monitor.Core.Responses;
using MathNet.Numerics.Distributions;

namespace CSH_Monitor.Model.Metrology.StabilityCalculator
{
    public partial class StabilityCalculator : IStabilityCalculator
    {
        #region Основная часть
        
        public DataResponse<RegressionModelData> GetRegressionModelData(MarkedDoubleRecList Data, ref LinearModelParams ModelParams)
        {
            var Result = new DataResponse<RegressionModelData>
            {
                Data = new(),
                Message = "Установление закона линейной регрессии",
            };

            try
            {
                Result.Data = GetAllParams(Data, ref ModelParams);
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

        private RegressionModelData GetAllParams(MarkedDoubleRecList Data, ref LinearModelParams ModelParams)
        {
            var Result = new RegressionModelData();

            SimplifiedRegressionList TemporaryRegressionList = GetRegression(Data, ModelParams);
            
            var SlopeStandartError = GetSlopeStandartError(TemporaryRegressionList);
            ModelParams.SetSlopeStandartError(SlopeStandartError);

            Result = GetStandartErrorData(TemporaryRegressionList, ModelParams);

            return Result;
            
        }
        #endregion

        #region
        private SimplifiedRegressionList GetRegression(MarkedDoubleRecList Data, LinearModelParams ModelParams)
        {
            var Result = new SimplifiedRegressionList();
            foreach (var Item in Data) 
            {
                var pointValue = ModelParams.Intercept + ModelParams.Slope * Item.Marker;
                Result.Add(
                new MarkedRecord<double, SimplifiedRegressionPoint>(
                    // Значение времени
                    Item.Marker,
                    // Измеренные/расчитанные значения
                    new SimplifiedRegressionPoint
                    {
                        MeansuredValue = Item.Value,
                        RegressionLinePoint = ModelParams.Intercept + ModelParams.Slope * Item.Marker
                    }
                )
            );
            }
            return Result;
        }

        #endregion

        #region
        /// <summary>
        /// Присваевает входной 
        /// </summary>
        private double GetSlopeStandartError(SimplifiedRegressionList RegressionData)
        {
            double Result = 0;
            double numerator = 0;

            foreach (var Item in RegressionData)
            {
                numerator += Math.Pow(Item.Value.MeansuredValue - Item.Value.RegressionLinePoint, 2);
            }

            Result = Math.Sqrt(numerator / (RegressionData.Count - 2));
            return Result;
        }
        #endregion

        #region
        private RegressionModelData GetStandartErrorData (SimplifiedRegressionList RegressionData, LinearModelParams ModelParams)
        {
            var Result = new RegressionModelData();
            var CDF = StudentT.InvCDF(0, 1, RegressionData.Count - 2, 0.95);
            
            // Считаем сумму квадратов отклонений значений от среднего
            double SumSquaredDeviation = 0;
            foreach (var Item in RegressionData)
            {
                SumSquaredDeviation += Math.Pow(Item.Value.MeansuredValue - ModelParams.ValueAverage, 2);
            }

            // Считаем границы доверительных интервалов + добавляем старые данные
            foreach (var Item in RegressionData)
            {
                double ModelStandartError = GetModelStandartError(ModelParams.SlopeStandartError, RegressionData.Count, Item.Marker, ModelParams.ValueAverage, SumSquaredDeviation);
                double TrustInterval = GetStabilityError(ModelParams.Slope, Item.Marker, CDF, ModelStandartError);
                Result.Add(
                    new MarkedRecord<double, RegressionPoint>
                    ( 
                        Item.Marker, 
                        new RegressionPoint {
                            MeansuredValue = Item.Value.MeansuredValue,
                            RegressionLinePoint = Item.Value.RegressionLinePoint,
                            ModelEstimate = ModelStandartError,
                            TrustInterval = TrustInterval
                        }
                    )
                );
            }

            return Result;
        }

        private double GetModelStandartError(double SlopeStandartError, int DataCount, double CurTime, double MedTime, double SumSquaredDeviation) 
            => SlopeStandartError * Math.Sqrt(1/Convert.ToDouble(DataCount) + Math.Pow(CurTime - MedTime, 2)/SumSquaredDeviation );

        private double GetStabilityError(double Slope, double CurTime, double CDF, double ModelStandartError) 
            => Math.Abs(Slope) * CurTime + CDF * ModelStandartError;
        #endregion

        
    }
}