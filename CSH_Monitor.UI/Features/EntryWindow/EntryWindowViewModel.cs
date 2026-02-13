using CHS_Monitor.GraphicsCore.Base;
using CHS_Monitor.GraphicsCore.Interfaces;

namespace CSH_Monitor.UI.Features.EntryWindow
{
    public class EntryWindowViewModel : ViewModelBase
    {
        public EntryWindowViewModel(IWindowService windowService) : base(windowService)
        {

        }

        public int hei = 450;
        public int Hei
        {
            get { return hei; }
            set { hei = value; CheckChanges(); }
        }

        public int wid = 600;
        public int Wid
        {
            get {  return wid; }
            set { wid = value; CheckChanges(); }
        }

    }
}