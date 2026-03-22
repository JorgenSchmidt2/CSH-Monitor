using CSH_Monitor.Core.Generics.CommonGenerics.TwoParamGenerics;

namespace CSH_Monitor.Core.Entities.CommonListEntities
{
    /// <summary>
    /// Данный класс является двойным маркированным списком объектов типа record
    /// </summary>
    public sealed class MarkedDoubleRecList : List<MarkedRecord<double, double>> { }
}