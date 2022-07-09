using System;
using System.Linq;
using System.Windows;
using spotifyWPF.View.Controls;
using spotifyWPF.ViewModel;

namespace spotifyWPF.View.Services;

public class WindowService : IWindowService
{
    public void ShowWindow(object viewModel)
    {
        var win = new Window();
        win.WindowStyle = WindowStyle.None;
        win.SizeToContent = SizeToContent.WidthAndHeight;
        win.ResizeMode = ResizeMode.NoResize;
        win.Content = viewModel;
        win.Show();
    }

    public void CloseWindowByType(Type t)
    {
        Window win = App.Current.Windows.Cast<Window>().FirstOrDefault(x => x.Content.GetType() == typeof(DevicesVM));
        win?.Close();
    }
}