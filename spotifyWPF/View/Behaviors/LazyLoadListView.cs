using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using spotifyWPF.ViewModel;
using spotifyWPF.ViewModel.Commands;

namespace spotifyWPF.View.Behaviors;

public class LazyLoadListView : Behavior<ListView>
{
    private double LOAD_TRIGGER_UPPER_BOUND = 0.75;
    private double LOAD_TRIGGER_LOWER_BOUND = 0.25;
    private DisplayVM _vm;
    private bool _loading;

    protected override void OnAttached()
    {
        base.OnAttached();
        ScrollViewer scrollViewer = AssociatedObject.FindName("ScrollBar") as ScrollViewer;
        scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;

        _vm = scrollViewer.DataContext as DisplayVM;
        // LazyListViewLoadCommand lazyListViewLoadCommand = new LazyListViewLoadCommand(scrollViewer.DataContext); 
        // CommandBinding binding = new CommandBinding(lazyListViewLoadCommand);
        // scrollViewer.CommandBindings.Add(binding);
    }


    protected override void OnDetaching()
    {
        base.OnDetaching();
    }
    private async void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        //use scrollviewer height vertical offset to get the current scroll position. 
        //use scrollviewer extentheight to get the maximum position of the bar
        //if within 25% of the bottom, unload from the beginning, load more to the end
        //if within 25& of the top, unload from the end, load more to the beginning
        //find someway to trigger getting next tracks
       // if (AssociatedObject.Items.Count == 0) return;
        double ratio = e.VerticalOffset / (sender as ScrollViewer).ScrollableHeight;
        if (ratio > LOAD_TRIGGER_UPPER_BOUND && e.VerticalChange > 0
            && !_loading)
        {
            _loading = true;
            await _vm.LoadSongs();
            _loading = false;
            
        }
    }
}