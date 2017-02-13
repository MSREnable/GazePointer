using System;
using System.Windows.Input;

///
/// Code for this library has been mostly borrowed from here:
/// http://social.technet.microsoft.com/wiki/contents/articles/13536.easy-mvvm-examples-in-extreme-detail.aspx
/// 

namespace Microsoft.HandsFree.MVVM
{
    /// <summary>
    /// Taken from http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030
    /// </summary>

    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }

}
