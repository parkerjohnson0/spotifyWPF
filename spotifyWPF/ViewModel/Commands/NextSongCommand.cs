using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class NextSongCommand :ICommand
{
    private PlayerVM _vm;
    public NextSongCommand(PlayerVM vm)
    {
        _vm = vm;
    }
    
    public bool CanExecute(object? parameter)
    {
        return _vm.AppState.AccessToken != null;
    }

    public async void Execute(object? parameter)
    {
        if (await _vm.AppState.SpotifyRequest.NextSong())
        {
            await Task.Delay(500);
           _vm.AppState.GetPlaybackState(); 
        }
    }

    public event EventHandler? CanExecuteChanged;
}