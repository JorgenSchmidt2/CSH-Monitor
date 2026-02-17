using CSH_Monitor.Core.Interfaces.Infrastructure;
using CSH_Monitor.Core.Interfaces.Model;
using CSH_Monitor.GraphicsCore.Interfaces;
using CSH_Monitor.GraphicsCore.Presentation;
using CSH_Monitor.Infrastructure.Messaging;
using CSH_Monitor.Model.Calculations;
using CSH_Monitor.UI.Features.EntryWindow;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CSH_Monitor.UI
{

    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App ()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices (IServiceCollection services)
        {
            // Регистрация служб
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IMessenger, Messenger>();
            services.AddSingleton<IListMessenger, ListMessenger>();

            // Регистрация классов с математической логикой
            services.AddSingleton<ICertificationCalculator, CertificationCalculator>();
            services.AddSingleton<IHomogenityCalculator, HomogenityCalculator>();
            services.AddSingleton<IStabilityCalculator, StabilityCalculator>();


            // Регистрация ViewModels
            services.AddTransient<EntryWindowViewModel>();

            // Регистрация окон со связыванием их с моделями представления
            services.AddTransient(s => new EntryWindow
            {
                DataContext = s.GetRequiredService<EntryWindowViewModel>()
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var entryWindow = _serviceProvider.GetRequiredService<EntryWindow>();
            entryWindow.Show();
        }
    }
}