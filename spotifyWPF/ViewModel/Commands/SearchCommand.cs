using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands
{
    public class SearchCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private NavVM vm;
        public SearchCommand(NavVM vm)
        {
            this.vm = vm;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            vm.SearchClicked();
        }
    }
}
