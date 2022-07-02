using System;
using System.Windows.Input;
using spotifyWPF.Model.Nav;

namespace spotifyWPF.ViewModel.Commands;

public class SelectPlaylistCommand : ICommand
{
    private NavVM vm;
    public SelectPlaylistCommand(NavVM vm)
    {
        this.vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        PlaylistItem item = parameter as PlaylistItem;
        vm.SelectPlaylist(item);
    }

    public event EventHandler? CanExecuteChanged;
}