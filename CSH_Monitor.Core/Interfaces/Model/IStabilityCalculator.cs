using CSH_Monitor.Core.Entities.CommonListEntities;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Enumerable;
using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple;
using CSH_Monitor.Core.Responses;

namespace CSH_Monitor.Core.Interfaces.Model
{
    public interface IStabilityCalculator
    {
        public DataResponse<RegressionModelData> GetRegressionModelData(MarkedDoubleRecList Data, ref LinearModelParams ModelParams);
    }
}