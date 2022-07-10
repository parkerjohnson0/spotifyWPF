using System;
using System.Windows;
using System.Windows.Controls;

namespace spotifyWPF.View.Controls;

public partial class DevicesUC : UserControl
{


    public bool ControlFocused
    {
        get { return (bool)GetValue(ControlFocusedProperty); }
        set { SetValue(ControlFocusedProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsFocused.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ControlFocusedProperty =
        DependencyProperty.Register("ControlFocused", typeof(bool), typeof(UserControl), new UIPropertyMetadata(false,OnControlFocusedChange));

    private static void OnControlFocusedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        UserControl control = d as UserControl;
        if ((bool)e.NewValue)
        {
            control.Focus();
        }
        else
        {
            control.Visibility = Visibility.Collapsed;
        }
    }

   
    public DevicesUC()
    {
        InitializeComponent();
    }
}