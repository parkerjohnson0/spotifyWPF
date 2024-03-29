﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class PreviousSongCommand : ICommand
{
    private PlayerVM _vm;
    public PreviousSongCommand(PlayerVM vm)
    {

        _vm = vm;
    }

    public bool CanExecute(object? parameter)
    {
        return _vm.AppState.AccessToken != null;
    }

    public async void Execute(object? parameter)
    {
        if (await _vm.AppState.SpotifyRequest.PreviousSong())
        {
            await Task.Delay(1000);
           _vm.AppState.GetPlaybackState(); 
        }
    }

    public event EventHandler? CanExecuteChanged;
}