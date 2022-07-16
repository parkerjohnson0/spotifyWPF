using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using spotifyWPF.Model.App;
using spotifyWPF.Model.Nav;

namespace spotifyWPF.View.Behaviors;

public sealed class ScrollIntoViewBehavior : Behavior<ScrollViewer>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        ListView lv =  AssociatedObject.FindName("lv") as ListView;
        ((INotifyCollectionChanged)lv.Items).CollectionChanged += AssociatedObjectOnSourceUpdated;
        //((INotifyCollectionChanged) AssociatedObject.Items).CollectionChanged += AssociatedObjectOnSourceUpdated;
    }

    protected override void OnDetaching()
    {
        base.OnAttached();
        ListView lv =  AssociatedObject.FindName("lv") as ListView;
        ((INotifyCollectionChanged)lv.Items).CollectionChanged += AssociatedObjectOnSourceUpdated;
    }

    private void AssociatedObjectOnSourceUpdated(object? sender, NotifyCollectionChangedEventArgs e)
    {
        //AssociatedObject.ScrollToTop();
        
        //if (AssociatedObject.Items.Count == 0) return;
        //var test =VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(AssociatedObject));
       // Decorator border = VisualTreeHelper.GetChild(AssociatedObject, 0) as Decorator;
       // ScrollViewer scrollViewer = border.Child as ScrollViewer;
       // scrollViewer.ScrollToBottom();
        //AssociatedObject.ScrollIntoView();
    }
}