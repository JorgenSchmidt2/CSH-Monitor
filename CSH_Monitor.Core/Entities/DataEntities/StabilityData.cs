using CSH_Monitor.Core.Entities.BasicListEntities;

namespace CSH_Monitor.Core.Entities.DataEntities
{
    public class StabilityData
    {
        /// <summary>
        /// Значение маркировано, т.к. пара "время-значение"
        /// </summary>
        public MarkedDoubleRecList InitialValues { get; } = new();

        public string? StabilityId { get; set; }
    }
}