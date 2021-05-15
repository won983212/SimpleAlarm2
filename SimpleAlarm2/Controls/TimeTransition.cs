using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SimpleAlarm2.Controls
{
    class TimeTransition : Control
    {
        public static DependencyProperty CurrentTimeProperty
            = DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(TimeTransition),
                new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.AffectsRender, CurrentTimeChanged));

        private const int DigitCount = 4;
        private const double AnimationLength = 0.4;

        private FormattedText[] _prevText = new FormattedText[DigitCount];
        private FormattedText[] _text = null;
        private TranslateTransform[] _textTransform = null;
        private FormattedText _spliter;
        private Size _digitSize;
        private DoubleAnimation digitYAnimation;
        private DoubleAnimation digitOpacityAnimation;
        private DoubleAnimation digitOpacityRevAnimation;


        public TimeTransition() { }

        private FormattedText CreateFormattedText(string text)
        {
            Typeface type = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, type, FontSize, Foreground);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return new Size(_digitSize.Width * DigitCount + _spliter.Width, _digitSize.Height);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (_text == null)
                return;

            double x = 0;
            for (int i = 0; i < DigitCount; i++)
            {
                // draw digit
                if (App.Settings.UseClockAnimation && _prevText[i] != null && _prevText[i].Text != _text[i].Text)
                {
                    dc.PushTransform(_textTransform[i]);
                    dc.PushOpacity(1, digitOpacityRevAnimation.CreateClock());
                    dc.DrawText(_prevText[i], new Point(x, 0));
                    dc.Pop();
                    dc.PushOpacity(1, digitOpacityAnimation.CreateClock());
                    dc.DrawText(_text[i], new Point(x, _digitSize.Height));
                    dc.Pop();
                    dc.Pop();
                } 
                else
                {
                    dc.DrawText(_text[i], new Point(x, 0));
                }

                x += _digitSize.Width;

                // draw spliter
                if (i % 2 == 1 && i < DigitCount - 1)
                {
                    dc.DrawText(_spliter, new Point(x, 0));
                    x += _spliter.Width;
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // calculate digit textsize
            FormattedText zeroChar = CreateFormattedText("0");
            _digitSize = new Size(zeroChar.Width, zeroChar.Height);

            // create spliter text
            _spliter = CreateFormattedText(":");

            // create digitY up animation
            digitYAnimation = CreateDoubleAnimation(0, -_digitSize.Height);
            digitOpacityAnimation = CreateDoubleAnimation(0, 1);
            digitOpacityRevAnimation = CreateDoubleAnimation(1, 0);
        }

        private void GenerateTimeText(DateTime time)
        {
            string timeText = time.ToString("hh:mm");
            int i = 0;

            if (_text == null)
            {
                _text = new FormattedText[DigitCount];
                _textTransform = new TranslateTransform[DigitCount];
            }

            foreach (char c in timeText)
            {
                if (c != ':')
                {
                    _prevText[i] = _text[i];
                    _text[i] = CreateFormattedText(c.ToString());
                    _textTransform[i] = new TranslateTransform(0, 0);
                    if (App.Settings.UseClockAnimation && _prevText[i] != _text[i])
                        _textTransform[i].ApplyAnimationClock(TranslateTransform.YProperty, digitYAnimation.CreateClock());
                    i++;
                }
            }
        }

        public static void CurrentTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TimeTransition)d).GenerateTimeText((DateTime)e.NewValue);
        }

        private static DoubleAnimation CreateDoubleAnimation(double from, double to)
        {
            DoubleAnimation animation = new DoubleAnimation(from, to, new Duration(TimeSpan.FromSeconds(AnimationLength)));
            animation.EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut };
            return animation;
        }

        public DateTime CurrentTime
        {
            get => (DateTime)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }
    }
}
