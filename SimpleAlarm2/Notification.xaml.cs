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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SimpleAlarm2
{
    public partial class Notification : Window
    {
        public Notification(string title, string description)
        {
            InitializeComponent();
            tblTitle.Text = title;
            tblDescription.Text = description;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (o, e) => StartCloseAnimation();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var workingArea = SystemParameters.WorkArea;
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

            Left = corner.X - ActualWidth - 50;
            Top = corner.Y - ActualHeight - 30;
        }

        private void StartCloseAnimation()
        {
            BeginStoryboard((Storyboard)FindResource("ClosingStoryboard"));
        }

        private void ClosingAnimation_Completed(object sender, EventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartCloseAnimation();
        }
    }
}
