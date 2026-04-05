namespace CSH_Monitor.UI.Features.EntryWindow
{
    public partial class EntryWindowViewModel
    {
        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set
            {
                if (selectedTabIndex != value)
                {
                    SetProperty(ref selectedTabIndex, value);
                    UpdateContentForSelectedTab();
                }
            }
        }

        private EPlotTab CurrentPlotTab { get; set; }

        private void UpdateContentForSelectedTab()
        {
            switch (selectedTabIndex)
            {
                case 0:
                    CurrentPlotTab = EPlotTab.Sertification;
                    break;
                case 1:
                    CurrentPlotTab = EPlotTab.Stability;
                    break;
                case 2:
                    CurrentPlotTab = EPlotTab.Homogenity;
                    break;
            }
        }

    }

    public enum EPlotTab
    {
        Sertification,
        Stability,
        Homogenity
    }
}