using CSH_Monitor.Core.Entities.BasicListEntities;
using CSH_Monitor.Core.Responses;

namespace CSH_Monitor.Core.Interfaces.Infrastructure
{
    public interface ITabularParser
    {
        public DataResponse<MarkedDoubleRecList> GetDoubleMeasuredData(string InputData);
    }
}