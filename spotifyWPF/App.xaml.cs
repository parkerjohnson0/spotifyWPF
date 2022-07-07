using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using spotifyWPF.View;
using spotifyWPF.ViewModel;

namespace spotifyWPF
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    ///
    public partial class App : Application
    {
        private void Lv_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //prevents innate scrollviewer in listview from capturing scroll event. need so do this to properly emulate the playlist look 
            // illallowit.gif
            if (sender is ListView && !e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}