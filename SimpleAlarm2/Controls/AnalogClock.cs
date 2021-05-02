using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SimpleAlarm2.Controls
{
    public class AnalogClock : Control
    {
        public static DependencyProperty TimeProperty
            = DependencyProperty.Register("Time", typeof(DateTime), typeof(AnalogClock),
                new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.AffectsRender));

        private Point?[] _prevPoints = new Point?[6];

        public AnalogClock()
        {
            Width = 180;
            Height = 180;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            Point center = new Point(RenderSize.Width / 2, RenderSize.Height / 2);

            // render background
            dc.DrawEllipse(Background, null, center, center.X, center.X);

            // render unit
            double endOfUnit = RenderSize.Width / 2 - 7;
            double startOfUnit = endOfUnit - 8;
            double startOfBigUnit = endOfUnit - 12;

            dc.PushTransform(new TranslateTransform(center.X, center.Y));
            for (int deg = 0; deg < 360; deg += 6)
            {
                double rad = deg * Math.PI / 180;
                double dx = Math.Cos(rad);
                double dy = Math.Sin(rad);
                double endUnit = (deg % 30 == 0) ? startOfBigUnit : startOfUnit;

                Point p1 = new Point(dx * endOfUnit, dy * endOfUnit);
                Point p2 = new Point(dx * endUnit, dy * endUnit);
                dc.DrawLine(new Pen(Foreground, (deg % 30 == 0) ? 2 : 1), p1, p2);
            }

            // render hands
            int totalSeconds = Time.Second + Time.Minute * 60 + Time.Hour * 3600;

            dc.DrawEllipse(Foreground, null, new Point(), 5, 5);
            RenderHand(dc, new Pen(Foreground, 2), totalSeconds / 60.0 / 60.0, endOfUnit - 30, 10, 0); // minute
            RenderHand(dc, new Pen(Foreground, 3), totalSeconds / 3600.0 / 12.0, endOfUnit - 40, 10, 1); // hour

            dc.DrawEllipse(Brushes.OrangeRed, null, new Point(), 3, 3);
            RenderHand(dc, new Pen(Brushes.OrangeRed, 2), Time.Second / 60.0, endOfUnit - 20, 20, 2); // second

            dc.Pop();
        }

        private void RenderHand(DrawingContext dc, Pen pen, double ratio, double length, double extendedLength, int prevIndex)
        {
            Point p1 = GetStraightPoint(ratio, -extendedLength);
            Point p2 = GetStraightPoint(ratio, length);
            prevIndex *= 2;

            if(!Properties.Settings.Default.UseClockAnimation || _prevPoints[prevIndex] == null)
            {
                dc.DrawLine(pen, p1, p2);
            }
            else
            {
                AnimationClock p1Clock = CreateHandAnimationClock(_prevPoints[prevIndex].Value, p1);
                AnimationClock p2Clock = CreateHandAnimationClock(_prevPoints[prevIndex + 1].Value, p2);
                dc.DrawLine(pen, p1, p1Clock, p2, p2Clock);
            }

            _prevPoints[prevIndex] = p1;
            _prevPoints[prevIndex + 1] = p2;
        }

        private Point GetStraightPoint(double t, double length)
        {
            double rad = t * 2 * Math.PI - Math.PI / 2;
            return new Point(Math.Cos(rad) * length, Math.Sin(rad) * length);
        }

        private AnimationClock CreateHandAnimationClock(Point from, Point to)
        {
            PointAnimation animation = new PointAnimation(from, to, new Duration(TimeSpan.FromSeconds(0.1)));
            animation.EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut };
            return animation.CreateClock();
        }


        #region Properties

        public DateTime Time
        {
            get => (DateTime)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        #endregion
    }

}
