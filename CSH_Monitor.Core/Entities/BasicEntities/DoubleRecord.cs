namespace CSH_Monitor.Core.Entities.BasicEntities
{
    /// <summary>
    /// Немаркированный record (оба значения неуникальны), различие делается только для того, 
    /// чтобы по названию различать маркированные и немаркированные списки
    /// </summary>
    public record DoubleRecord<T1, T2> (T1 X, T2 Y);
}
