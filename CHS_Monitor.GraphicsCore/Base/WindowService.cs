using CHS_Monitor.GraphicsCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows;

namespace CHS_Monitor.GraphicsCore.Base
{
    public class WindowService : IWindowService
    {
        private readonly Dictionary<object, Window> _openWindows = new();
        private readonly IServiceProvider _serviceProvider;

        public WindowService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ShowWindow<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            var window = CreateWindow(viewModel);
            window.Show();
            _openWindows[viewModel] = window;
        }

        public void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            var window = CreateWindow(viewModel);
            window.ShowDialog();
        }

        public bool? ShowDialogWithResult<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            var window = CreateWindow(viewModel);
            return window.ShowDialog();
        }

        public void CloseWindow(object viewModel)
        {
            if (_openWindows.TryGetValue(viewModel, out var window))
            {
                window.Close();
                _openWindows.Remove(viewModel);
            }
        }

        private Window CreateWindow<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            // Определяем тип окна на основе типа ViewModel
            var windowType = GetWindowTypeForViewModel(typeof(TViewModel));
            var window = (Window)_serviceProvider.GetRequiredService(windowType);

            window.DataContext = viewModel;

            // Подписываемся на закрытие окна для очистки
            window.Closed += (s, e) =>
            {
                if (viewModel != null)
                    _openWindows.Remove(viewModel);
            };

            return window;
        }

        private Type GetWindowTypeForViewModel(Type viewModelType)
        {
            // Конвенция: MainViewModel -> MainWindow, SettingsViewModel -> SettingsWindow
            var windowTypeName = viewModelType.Name.Replace("ViewModel", "Window");
            var windowType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == windowTypeName && typeof(Window).IsAssignableFrom(t));

            return windowType ?? throw new InvalidOperationException(
                $"Не найден соответствующий Window для {viewModelType.Name}");
        }
    }
}