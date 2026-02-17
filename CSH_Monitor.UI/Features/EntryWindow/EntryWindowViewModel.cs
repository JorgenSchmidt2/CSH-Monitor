using CSH_Monitor.GraphicsCore.Base;
using CSH_Monitor.GraphicsCore.Interfaces;

namespace CSH_Monitor.UI.Features.EntryWindow
{
    public partial class EntryWindowViewModel : ViewModelBase
    {
        // Подключаем нужные службы в конструкторе
        private IWindowService _windowService;
        public EntryWindowViewModel(IWindowService windowService)
        {
            UpdateContentForSelectedTab();
            _windowService = windowService;
        }
        
    }
}