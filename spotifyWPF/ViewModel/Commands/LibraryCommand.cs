using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands
{
    public class LibraryCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private NavVM vm;
        public LibraryCommand(NavVM vm)
        {
            this.vm = vm;
        }
        public bool CanExecute(object? parameter)
        {
            return vm.Authorized;
        }

        public void Execute(object? parameter)
        {
            vm.LibraryClicked();
        }
    }
}
