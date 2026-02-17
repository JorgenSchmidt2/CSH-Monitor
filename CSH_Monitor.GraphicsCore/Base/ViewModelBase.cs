using CSH_Monitor.GraphicsCore.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSH_Monitor.GraphicsCore.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected IWindowService? _windowService { get; }

        protected ViewModelBase(IWindowService windowService)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));        }

        protected virtual void CheckChanges([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            CheckChanges(propertyName);
            return true;
        }

        protected void ShowWindow<TViewModel>(TViewModel viewModel) where TViewModel : class
            => _windowService?.ShowWindow(viewModel);

        protected void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class
            => _windowService?.ShowDialog(viewModel);

        protected bool? ShowDialogWithResult<TViewModel>(TViewModel viewModel) where TViewModel : class
            => _windowService?.ShowDialogWithResult(viewModel);

        protected void CloseCurrentWindow()
        {
            if (_windowService != null)
                _windowService?.CloseWindow(this);
        }
    }
}