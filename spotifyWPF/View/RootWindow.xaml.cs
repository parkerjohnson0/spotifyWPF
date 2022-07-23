using spotifyWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using spotifyWPF.View.Controls;

namespace spotifyWPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RootWindow : Window
    {
        public RootWindow()
        {
            InitializeComponent();
            //if (Unauthorized())
            //{
            //    AuthWindow auth = new AuthWindow();
            //    auth.Show();
            //}
        }

        //stackoverflow mvvm nerds hate this
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double yPos = e.MouseDevice.GetPosition(this).Y;
            //use source instead of original source. source is the name of the parent, original source is the specific grid control within the parent.
            if (yPos < 50 && e.Source.GetType() != typeof(WindowButtonsUC))
            {
                //todo this is triggering dragging when maximized and only on single click. if maximized should only drag after mousemovement and not on the click
                if (App.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Point mousePoint = App.Current.MainWindow.PointToScreen(Mouse.GetPosition(Application.Current.MainWindow));
                    App.Current.MainWindow.WindowState = WindowState.Normal;
                    App.Current.MainWindow.Left =mousePoint.X -  App.Current.MainWindow.Width / 2;
                    App.Current.MainWindow.Top = mousePoint.Y;
                }

                this.DragMove();
            }
        }
    }
}