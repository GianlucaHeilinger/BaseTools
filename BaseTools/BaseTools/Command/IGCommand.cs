using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BaseTools.Command
{

    public interface IGCommand<T> : ICommand
    {
        bool CanExecute(T parameter);

        void Execute(T parameter);
    }

    public interface IGCommand : ICommand
    {
    }
}
