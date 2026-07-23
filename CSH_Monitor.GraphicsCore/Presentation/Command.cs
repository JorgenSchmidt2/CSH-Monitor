using System.Windows.Input;

namespace CSH_Monitor.GraphicsCore.Presentation
{
    public class Command : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;
        private readonly Action? _executeSimple;
        private readonly Func<bool>? _canExecuteSimple;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public Command(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public Command(Action execute, Func<bool>? canExecute = null)
        {
            _executeSimple = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteSimple = canExecute;
            _execute = _ => _executeSimple();
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute != null)
                return _canExecute(parameter);

            if (_canExecuteSimple != null)
                return _canExecuteSimple();

            return true;
        }

        public void Execute(object? parameter)
        {
            if (_execute != null)
                _execute(parameter);
            else
                _executeSimple?.Invoke();
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
