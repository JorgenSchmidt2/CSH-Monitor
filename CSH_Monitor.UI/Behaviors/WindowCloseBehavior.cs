using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CSH_Monitor.UI.Behaviors
{
    public static class WindowCloseBehavior
    {
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached(
                "CloseCommand",
                typeof(ICommand),
                typeof(WindowCloseBehavior),
                new PropertyMetadata(null, OnCloseCommandChanged));

        public static void SetCloseCommand(DependencyObject d, ICommand value)
            => d.SetValue(CloseCommandProperty, value);

        public static ICommand GetCloseCommand(DependencyObject d)
            => (ICommand)d.GetValue(CloseCommandProperty);

        private static void OnCloseCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                if (e.OldValue != null)
                    window.Closing -= Window_Closing;

                if (e.NewValue != null)
                    window.Closing += Window_Closing;
            }
        }

        private static void Window_Closing(object sender, CancelEventArgs e)
        {
            var window = (Window)sender;
            var command = GetCloseCommand(window);

            if (command != null && command.CanExecute(e))
            {
                // Выполняем команду из ViewModel
                command.Execute(e);

                // Если команда установила e.Cancel = true, окно не закроется
                // Это позволяет ViewModel отменить закрытие
            }
        }
    }
}

/*
B XAML
xmlns:behaviors="clr-namespace:YourApp.Behaviors"
behaviors:WindowCloseBehavior.CloseCommand="{Binding CloseCommand}">

В ViewModel
private ICommand? _closeCommand;

public ICommand CloseCommand => _closeCommand ??= new RelayCommand<CancelEventArgs>(OnClose);

private void OnClose(CancelEventArgs? e)
{
    // Проверяем, можно ли закрыть окно
    if (HasUnsavedChanges)
    {
        var result = MessageBox.Show("Сохранить изменения?", 
            "Подтверждение", 
            MessageBoxButton.YesNoCancel);

        if (result == MessageBoxResult.Cancel)
        {
            e?.Cancel(); // Отменяем закрытие
            return;
        }

        if (result == MessageBoxResult.Yes)
            SaveData();
    }

    // Дополнительные действия при закрытии
    CleanupResources();

    // Можно вызвать закрытие через WindowService
    CloseCurrentWindow();
}
 */