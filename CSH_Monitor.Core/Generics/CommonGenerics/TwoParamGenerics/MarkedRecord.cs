namespace CSH_Monitor.Core.Generics.CommonGenerics.TwoParamGenerics
{
    /// <summary>
    /// Маркированный record (значение "Маркер" должно быть уникально), различие делается только для того, 
    /// чтобы по названию различать маркированные и немаркированные списки
    /// </summary>
    public record MarkedRecord<TMarker,TValue> (TMarker Marker, TValue Value);
}