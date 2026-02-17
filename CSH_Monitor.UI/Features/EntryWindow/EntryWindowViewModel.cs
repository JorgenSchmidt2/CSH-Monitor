using CSH_Monitor.GraphicsCore.Base;
using CSH_Monitor.GraphicsCore.Interfaces;

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
            get => hei; 
            set { hei = value; CheckChanges(); }
        }

        public int wid = 600;
        public int Wid
        {
            get => wid; 
            set { wid = value; CheckChanges(); }
        }


        public string testr = "Test_pos_1";
        public string Testr
        {
            get => testr;
            set { testr = value; CheckChanges(); }
        }
    }
}