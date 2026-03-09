using CSH_Monitor.Core.Entities.BasicEntities;

namespace CSH_Monitor.Core.Entities.BasicListEntities
{
    /// <summary>
    /// Данный класс является двойным маркированным списком объектов типа record
    /// </summary>
    public class MarkedDoubleRecList : List<MarkedRecord<double, double>> { }
}