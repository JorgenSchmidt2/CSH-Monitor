using CSH_Monitor.GraphicsCore.Presentation;
using System.Windows;

namespace CSH_Monitor.UI.Features.EntryWindow
{
    public partial class EntryWindowViewModel
    {
        #region Размеры окна

        public int windowHeight;
        public int WindowHeight
        {
            get => windowHeight;
            set => SetProperty(ref windowHeight, value);
        }

        public int windowWidth;
        public int WindowWidth
        {
            get => windowWidth;
            set => SetProperty(ref windowWidth, value);
        }
        #endregion

        #region Центральный правый элемент
        public string mainDataString;
        public string MainDataString
        {
            get => mainDataString;
            set => SetProperty(ref mainDataString, value);
        }

        public Command GetData { 
            get {
                return new Command(
                    obj =>
                    {
                        switch (CurrentPlotTab)
                        {
                            case EPlotTab.Stability:
                                ReadMainStringAsStabilityData();
                                break;
                            default:
                                MessageBox.Show("Вы находитесь на неучтённой вкладке.");
                                break;
                        }
                    }
                );
            } 
        }
        #endregion
    }
}
