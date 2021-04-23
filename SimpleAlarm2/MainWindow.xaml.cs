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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleAlarm2
{
    // TODO(Release): App.xaml에 Todo 있으니까 주의
    public partial class MainWindow : Window
    {
        private const int animationDuration = 200;
        private ThicknessAnimation _openMenuAnimation;
        private ThicknessAnimation _closeMenuAnimation;
        private bool _isOpen;

        public MainWindow()
        {
            InitializeComponent();
            _openMenuAnimation = new ThicknessAnimation(new Thickness(0), TimeSpan.FromMilliseconds(animationDuration));
            _openMenuAnimation.EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut };
            _closeMenuAnimation = new ThicknessAnimation(new Thickness(-48, 0, 0, 0), TimeSpan.FromMilliseconds(animationDuration));
            _closeMenuAnimation.EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseIn };

            _isOpen = false;
            menuBar.Margin = new Thickness(-48, 0, 0, 0);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            double mx = e.GetPosition(null).X;
            bool open = mx > 0 && mx < 50;
            if(_isOpen != open)
            {
                if (open)
                    menuBar.BeginAnimation(MarginProperty, _openMenuAnimation);
                else
                    menuBar.BeginAnimation(MarginProperty, _closeMenuAnimation);
                _isOpen = open;
            }
        }
    }
}
