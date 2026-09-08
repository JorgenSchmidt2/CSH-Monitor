namespace CSH_Monitor.Core.Entities.CommonEnumerableEntities
{
    public class SharedRelation<TL,TR>
    {
        public List<TL> LeftValues { get; set; } = new ();
        public List<TR> RightValues { get; set; } = new ();
    }
}
