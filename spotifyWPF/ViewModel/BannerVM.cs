using System;
using System.Threading;
using spotifyWPF.Model.App;

namespace spotifyWPF.ViewModel;

public class BannerVM : ViewModelBase
{
    private User _user;

    public User User
    {
        get => _user;
        set { _user = value; NotifyPropertyChanged(); }
    }

    public BannerVM()
    {
        AppState.OnAuthorized += BannerVMAuthorized;
    }

    private async void BannerVMAuthorized(object? sender, EventArgs e)
    {
        User = await AppState.GetUser();
    }
}