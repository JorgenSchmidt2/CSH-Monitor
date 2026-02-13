using CHS_Monitor.GraphicsCore.Interfaces;
using CSH_Monitor.UI.Features.EntryWindow;

namespace CSH_Monitor.UI.AppService
{
    public class ViewModelLocator
    {
        private readonly IWindowService _windowService;

        public ViewModelLocator(IWindowService windowService)
        {
            _windowService = windowService;
        }

        // Свойства для каждой ViewModel в папке
        public EntryWindowViewModel EntryWindowViewModel
            => new EntryWindowViewModel(_windowService);
    }
}