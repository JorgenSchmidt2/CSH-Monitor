using CSH_Monitor.GraphicsCore.Presentation;

namespace CSH_Monitor.UI.Features.EntryWindow
{
    public partial class EntryWindowViewModel
    {
        #region Данные вкладки
        private string sampleMass;
        public string SampleMass
        {
            get => sampleMass;
            set => SetProperty(ref sampleMass, value);
        }

        private string detectionLimit;
        public string DetectionLimit
        {
            get => detectionLimit;
            set => SetProperty(ref detectionLimit, value);
        }

        public Command RemoveOutliers => new Command(x =>
        {

        });
        #endregion
    }
}