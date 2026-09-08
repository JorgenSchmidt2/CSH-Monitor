using CSH_Monitor.Core.Entities.DataEntities.StabilityDataEntities;
using CSH_Monitor.Core.Responses;

namespace CSH_Monitor.Core.Interfaces.Infrastructure
{
    public interface ITabularParser
    {
        public EntityResponse<StabilityInputData> ReadStabilityData(string InputData);
    }
}