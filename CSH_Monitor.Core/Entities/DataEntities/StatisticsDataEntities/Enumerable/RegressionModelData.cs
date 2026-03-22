using CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Simple;
using CSH_Monitor.Core.Generics.CommonGenerics.TwoParamGenerics;

namespace CSH_Monitor.Core.Entities.DataEntities.StatisticsDataEntities.Enumerable
{
    public sealed class RegressionModelData : List<MarkedRecord<double, RegressionPoint>> { }
}