namespace CSH_Monitor.GraphicsCore.GraphicsEntities
{
    public class EnumerableGrapchicsItem <T>
    {
        public T Value { get; set; }
        public EnumerableGrapchicsItem(T val)
        {
            Value = val;
        }
    }
}