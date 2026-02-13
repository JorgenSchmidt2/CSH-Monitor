namespace CHS_Monitor.GraphicsCore.Interfaces
{
    public interface IWindowService
    {
        void ShowWindow<TViewModel>(TViewModel viewModel) where TViewModel : class;
        void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class;
        bool? ShowDialogWithResult<TViewModel>(TViewModel viewModel) where TViewModel : class;
        void CloseWindow(object viewModel);
    }
}