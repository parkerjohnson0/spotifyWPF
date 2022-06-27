using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands
{
    public class HomeCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private NavVM vm;
        public HomeCommand(NavVM vm)
        {
            this.vm = vm;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            vm.HomeClicked();
        }
    }
}
