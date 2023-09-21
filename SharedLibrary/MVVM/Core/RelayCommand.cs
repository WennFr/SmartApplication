using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedLibrary.MVVM.Core
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public event EventHandler? CanExecuteChanged;



        public RelayCommand(Action execute, Func< bool> canExecute = null!) 
            : this (_ => execute(), canExecute != null ? _ => canExecute() : null!)
        {
            
        }



        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null!)
        {
            _execute = execute ?? throw new ArgumentException(nameof(execute));
            _canExecute = canExecute ?? (_ => true);
        }




        public bool CanExecute(object? parameter) => _canExecute(parameter!);

        public void Execute(object? parameter) => _execute(parameter!);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        
    }
}
