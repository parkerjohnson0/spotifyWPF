using System;

namespace spotifyWPF.ViewModel;

public class PlayerVM : ViewModelBase
{
    public PlayerVM()
    {
        AppState.OnAuthorized += PlayerVMAuthorized;

    }

    private void PlayerVMAuthorized(object? sender, EventArgs e)
    {
    }
}