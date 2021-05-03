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
    // TODO(Release): App.xaml에 Todo 있으니까 주의 (폰트 설정)
    public partial class MainWindow : Window
    {
        private const int animationDuration = 200;
        private const int snapTolerance = 30;

        private ThicknessAnimation _openMenuAnimation;
        private ThicknessAnimation _closeMenuAnimation;
        private bool _isOpen;
        private Point _offset = new Point();

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

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            _offset = e.GetPosition(this);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            // window dragmove
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mouseLoc = PointToScreen(e.GetPosition(this));
                double x = mouseLoc.X - _offset.X;
                double y = mouseLoc.Y - _offset.Y;

                if (App.Settings.UseSnappingWindow)
                {
                    Rect r = SystemParameters.WorkArea;
                    x = snapTo(r.Left, x);
                    x = snapTo(r.Right - Width, x);
                    y = snapTo(r.Top, y);
                    y = snapTo(r.Bottom - Height, y);
                }

                Left = x;
                Top = y;
            }

            // animation
            double mx = e.GetPosition(null).X;
            bool open = mx > 0 && mx < 50;
            if (_isOpen != open)
            {
                if (open)
                    menuBar.BeginAnimation(MarginProperty, _openMenuAnimation);
                else
                    menuBar.BeginAnimation(MarginProperty, _closeMenuAnimation);
                _isOpen = open;
            }
        }

        private double snapTo(double criteria, double value)
        {
            if (value >= criteria - snapTolerance && value <= criteria + snapTolerance)
                return criteria;
            return value;
        }
    }
}
