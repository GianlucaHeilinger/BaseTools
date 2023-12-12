using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BaseTools.Command
{
    internal class GCommand<T> : IGCommand<T>, ICommand
    {
        private readonly Action<T?> _execute;

        private readonly Predicate<T?>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public GCommand(Action<T?> execute)
            : this(execute, null)
        {
        }

        public GCommand(Action<T?> execute, Predicate<T?>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute((T?)parameter);
            }
            return true;
        }

        public bool CanExecute(T? parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute((T?)parameter);
        }

        public void Execute(T? parameter)
        {
            _execute(parameter);
        }
    }

    public class GCommand : IGCommand, ICommand
    {
        private readonly Action<object?> _execute;

        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public GCommand(Action<object?> execute)
            : this(execute, null)
        {
        }

        public GCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
