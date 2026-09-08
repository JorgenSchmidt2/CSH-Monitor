namespace CSH_Monitor.Core.Entities.CommonEnumerableEntities
{
    /// <summary>
    /// Является контейнером для хранения данных по шаблону "ключ-набор значений"
    /// </summary>
    public class DataSeries<TKey,TValue>
    {
        public TKey Definitor { get; set; } 
        public List<TValue> Values { get; set; } = new ();
    }
}
