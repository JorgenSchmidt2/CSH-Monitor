namespace CSH_Monitor.Core.Generics.CommonGenerics.ListGenerics
{
    /// <summary>
    /// Нужен для категоризации данных, где для одного маркера имеется несколько однотипных значений
    /// </summary>
    public record CategorizedRecord<TMarker, TItem>  (TMarker Marker, List<TItem> ObjectList);
}