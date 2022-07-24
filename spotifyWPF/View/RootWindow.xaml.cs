using spotifyWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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

        //https://engy.us/blog/2020/01/01/implementing-a-custom-window-title-bar-in-wpf/ 

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
            
        }
        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002; 
        private const int WM_GETMINMAXINFO = 0x0024;
        private IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == WM_GETMINMAXINFO)
            {
                MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lparam, typeof(MINMAXINFO));
                IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
                if (monitor != IntPtr.Zero)
                {
                    MONITORINFO monitorInfo = new MONITORINFO();
                    monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
                    GetMonitorInfo(monitor, ref monitorInfo);
                    RECT rcWorkArea = monitorInfo.rcWork;
                    RECT rcMonitorArea = monitorInfo.rcMonitor;

                    mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
                }
                Marshal.StructureToPtr(mmi, lparam, true);
            }
            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);


        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }
        
        
        
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
               this.Left = left;
               this.Top = top;
               this.Right = right;
               this.Bottom = bottom;
            }
        }
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
            
        }

        //stackoverflow mvvm nerds hate this
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // double yPos = e.MouseDevice.GetPosition(this).Y;
           // //use source instead of original source. source is the name of the parent, original source is the specific grid control within the parent.
           // if (yPos < 50 && e.Source.GetType() != typeof(WindowButtonsUC))
           // {
           //     //todo this is triggering dragging when maximized and only on single click. if maximized should only drag after mousemovement and not on the click
           //     if (App.Current.MainWindow.WindowState == WindowState.Maximized)
           //     {
           //         Point mousePoint = App.Current.MainWindow.PointToScreen(Mouse.GetPosition(Application.Current.MainWindow));
           //         App.Current.MainWindow.WindowState = WindowState.Normal;
           //         App.Current.MainWindow.Left =mousePoint.X -  App.Current.MainWindow.Width / 2;
           //         App.Current.MainWindow.Top = mousePoint.Y;
           //     }

           //     this.DragMove();
           // }
        }
    }
}