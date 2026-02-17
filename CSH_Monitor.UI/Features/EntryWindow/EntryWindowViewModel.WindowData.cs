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


    }
}
