using CHS_Monitor.GraphicsCore.Base;
using CHS_Monitor.GraphicsCore.Interfaces;
using CSH_Monitor.UI.AppService;
using CSH_Monitor.UI.Features.EntryWindow;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CSH_Monitor.UI
{

    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        public static ViewModelLocator Locator { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Настройка DI контейнера
            var services = new ServiceCollection();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<ViewModelLocator>();

            _serviceProvider = services.BuildServiceProvider();

            // Сохраняем Locator в статическом свойстве
            Locator = _serviceProvider.GetService<ViewModelLocator>();

            var mainWindow = new EntryWindow();
            mainWindow.Show();

        }
    }
}