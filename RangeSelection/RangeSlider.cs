﻿using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static System.Math;
using static Xamarin.Forms.Device;
using static Xamarin.Forms.AbsoluteLayout;
using System.Runtime.CompilerServices;

namespace RangeSelection
{
    public class RangeSlider : TemplatedView
    {
        const double EnabledOpacity = 1;

        const double DisabledOpacity = .6;

        public static BindableProperty MinimumValueProperty
            = BindableProperty.Create(nameof(MinimumValue), typeof(double), typeof(RangeSlider), .0, propertyChanged: OnMinimumMaximumValuePropertyChanged);

        public static BindableProperty MaximumValueProperty
            = BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(RangeSlider), 1.0, propertyChanged: OnMinimumMaximumValuePropertyChanged);

        public static BindableProperty LowerValueProperty
            = BindableProperty.Create(nameof(LowerValue), typeof(double), typeof(RangeSlider), .0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);

        public static BindableProperty UpperValueProperty
            = BindableProperty.Create(nameof(UpperValue), typeof(double), typeof(RangeSlider), 1.0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);

        public static BindableProperty ThumbSizeProperty
            = BindableProperty.Create(nameof(ThumbSize), typeof(double), typeof(RangeSlider), 30.0, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty LowerThumbSizeProperty
            = BindableProperty.Create(nameof(LowerThumbSize), typeof(double), typeof(RangeSlider), -1.0, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty UpperThumbSizeProperty
            = BindableProperty.Create(nameof(UpperThumbSize), typeof(double), typeof(RangeSlider), -1.0, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty TrackSizeProperty
            = BindableProperty.Create(nameof(TrackSize), typeof(double), typeof(RangeSlider), 3.0, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty ThumbColorProperty
            = BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty LowerThumbColorProperty
            = BindableProperty.Create(nameof(LowerThumbColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty UpperThumbColorProperty
            = BindableProperty.Create(nameof(UpperThumbColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty TrackColorProperty
            = BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty TrackHighlightColorProperty
            = BindableProperty.Create(nameof(TrackHighlightColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty ThumbBorderColorProperty
            = BindableProperty.Create(nameof(ThumbBorderColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty LowerThumbBorderColorProperty
            = BindableProperty.Create(nameof(LowerThumbBorderColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty UpperThumbBorderColorProperty
            = BindableProperty.Create(nameof(UpperThumbBorderColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty TrackBorderColorProperty
            = BindableProperty.Create(nameof(TrackBorderColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty TrackHighlightBorderColorProperty
            = BindableProperty.Create(nameof(TrackHighlightBorderColor), typeof(Color), typeof(RangeSlider), Color.Default, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty ValueLabelStyleProperty
            = BindableProperty.Create(nameof(ValueLabelStyle), typeof(Style), typeof(RangeSlider), propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty LowerValueLabelStyleProperty
            = BindableProperty.Create(nameof(LowerValueLabelStyle), typeof(Style), typeof(RangeSlider), propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty UpperValueLabelStyleProperty
            = BindableProperty.Create(nameof(UpperValueLabelStyle), typeof(Style), typeof(RangeSlider), propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty ValueLabelStringFormatProperty
            = BindableProperty.Create(nameof(ValueLabelStringFormat), typeof(string), typeof(RangeSlider), "{0:0.##}", propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty LowerThumbViewProperty
            = BindableProperty.Create(nameof(LowerThumbView), typeof(View), typeof(RangeSlider), propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty UpperThumbViewProperty
            = BindableProperty.Create(nameof(UpperThumbView), typeof(View), typeof(RangeSlider), propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty ValueLabelSpacingProperty
            = BindableProperty.Create(nameof(ValueLabelSpacing), typeof(double), typeof(RangeSlider), 5.0, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty ThumbRadiusProperty
           = BindableProperty.Create(nameof(ThumbRadius), typeof(double), typeof(RangeSlider), -1.0, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty LowerThumbRadiusProperty
            = BindableProperty.Create(nameof(LowerThumbRadius), typeof(double), typeof(RangeSlider), -1.0, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty UpperThumbRadiusProperty
            = BindableProperty.Create(nameof(UpperThumbRadius), typeof(double), typeof(RangeSlider), -1.0, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty TrackRadiusProperty
            = BindableProperty.Create(nameof(TrackRadius), typeof(double), typeof(RangeSlider), -1.0, propertyChanged: OnLayoutPropertyChanged);

        readonly Dictionary<View, double> thumbPositionMap = new Dictionary<View, double>();

        readonly PanGestureRecognizer lowerThumbGestureRecognizer = new PanGestureRecognizer();

        readonly PanGestureRecognizer upperThumbGestureRecognizer = new PanGestureRecognizer();

        Size allocatedSize;

        double labelMaxHeight;

        double lowerTranslation;

        double upperTranslation;

        public RangeSlider()
            => ControlTemplate = new ControlTemplate(typeof(RangeSliderLayout));

        public double MinimumValue
        {
            get => (double)GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }

        public double MaximumValue
        {
            get => (double)GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }

        public double LowerValue
        {
            get => (double)GetValue(LowerValueProperty);
            set => SetValue(LowerValueProperty, value);
        }

        public double UpperValue
        {
            get => (double)GetValue(UpperValueProperty);
            set => SetValue(UpperValueProperty, value);
        }

        public double ThumbSize
        {
            get => (double)GetValue(ThumbSizeProperty);
            set => SetValue(ThumbSizeProperty, value);
        }

        public double LowerThumbSize
        {
            get => (double)GetValue(LowerThumbSizeProperty);
            set => SetValue(LowerThumbSizeProperty, value);
        }

        public double UpperThumbSize
        {
            get => (double)GetValue(UpperThumbSizeProperty);
            set => SetValue(UpperThumbSizeProperty, value);
        }

        public double TrackSize
        {
            get => (double)GetValue(TrackSizeProperty);
            set => SetValue(TrackSizeProperty, value);
        }

        public Color ThumbColor
        {
            get => (Color)GetValue(ThumbColorProperty);
            set => SetValue(ThumbColorProperty, value);
        }

        public Color LowerThumbColor
        {
            get => (Color)GetValue(LowerThumbColorProperty);
            set => SetValue(LowerThumbColorProperty, value);
        }

        public Color UpperThumbColor
        {
            get => (Color)GetValue(UpperThumbColorProperty);
            set => SetValue(UpperThumbColorProperty, value);
        }

        public Color TrackColor
        {
            get => (Color)GetValue(TrackColorProperty);
            set => SetValue(TrackColorProperty, value);
        }

        public Color TrackHighlightColor
        {
            get => (Color)GetValue(TrackHighlightColorProperty);
            set => SetValue(TrackHighlightColorProperty, value);
        }

        public Color ThumbBorderColor
        {
            get => (Color)GetValue(ThumbBorderColorProperty);
            set => SetValue(ThumbBorderColorProperty, value);
        }

        public Color LowerThumbBorderColor
        {
            get => (Color)GetValue(LowerThumbBorderColorProperty);
            set => SetValue(LowerThumbBorderColorProperty, value);
        }

        public Color UpperThumbBorderColor
        {
            get => (Color)GetValue(UpperThumbBorderColorProperty);
            set => SetValue(UpperThumbBorderColorProperty, value);
        }

        public Color TrackBorderColor
        {
            get => (Color)GetValue(TrackBorderColorProperty);
            set => SetValue(TrackBorderColorProperty, value);
        }

        public Color TrackHighlightBorderColor
        {
            get => (Color)GetValue(TrackHighlightBorderColorProperty);
            set => SetValue(TrackHighlightBorderColorProperty, value);
        }

        public Style ValueLabelStyle
        {
            get => (Style)GetValue(ValueLabelStyleProperty);
            set => SetValue(ValueLabelStyleProperty, value);
        }

        public Style LowerValueLabelStyle
        {
            get => (Style)GetValue(LowerValueLabelStyleProperty);
            set => SetValue(LowerValueLabelStyleProperty, value);
        }

        public Style UpperValueLabelStyle
        {
            get => (Style)GetValue(UpperValueLabelStyleProperty);
            set => SetValue(UpperValueLabelStyleProperty, value);
        }

        public string ValueLabelStringFormat
        {
            get => (string)GetValue(ValueLabelStringFormatProperty);
            set => SetValue(ValueLabelStringFormatProperty, value);
        }

        public View LowerThumbView
        {
            get => (View)GetValue(LowerThumbViewProperty);
            set => SetValue(LowerThumbViewProperty, value);
        }

        public View UpperThumbView
        {
            get => (View)GetValue(UpperThumbViewProperty);
            set => SetValue(UpperThumbViewProperty, value);
        }

        public double ValueLabelSpacing
        {
            get => (double)GetValue(ValueLabelSpacingProperty);
            set => SetValue(ValueLabelSpacingProperty, value);
        }

        public double ThumbRadius
        {
            get => (double)GetValue(ThumbRadiusProperty);
            set => SetValue(ThumbRadiusProperty, value);
        }

        public double LowerThumbRadius
        {
            get => (double)GetValue(LowerThumbRadiusProperty);
            set => SetValue(LowerThumbRadiusProperty, value);
        }

        public double UpperThumbRadius
        {
            get => (double)GetValue(UpperThumbRadiusProperty);
            set => SetValue(UpperThumbRadiusProperty, value);
        }

        public double TrackRadius
        {
            get => (double)GetValue(TrackRadiusProperty);
            set => SetValue(TrackRadiusProperty, value);
        }

        RangeSliderLayout Content { get; set; }

        Frame Track => Content.Track;

        Frame TrackHighlight => Content.TrackHighlight;

        Frame LowerThumb => Content.LowerThumb;

        Frame UpperThumb => Content.UpperThumb;

        Label LowerValueLabel => Content.LowerValueLabel;

        Label UpperValueLabel => Content.UpperValueLabel;

        double TrackWidth => Width - LowerThumb.Width - UpperThumb.Width;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == IsEnabledProperty.PropertyName)
                OnIsEnabledChanged();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width == allocatedSize.Width && height == allocatedSize.Height)
                return;

            allocatedSize = new Size(width, height);
            OnLayoutPropertyChanged();
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            Content = child as RangeSliderLayout;
            if (Content == null)
                return;

            AddGestureRecognizer(LowerThumb, lowerThumbGestureRecognizer);
            AddGestureRecognizer(UpperThumb, upperThumbGestureRecognizer);
            Track.SizeChanged += OnViewSizeChanged;
            LowerThumb.SizeChanged += OnViewSizeChanged;
            UpperThumb.SizeChanged += OnViewSizeChanged;
            LowerValueLabel.SizeChanged += OnViewSizeChanged;
            UpperValueLabel.SizeChanged += OnViewSizeChanged;
            OnIsEnabledChanged();
            OnLayoutPropertyChanged();
        }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);
            if (Content == null)
                return;

            LowerValueLabel.RemoveBinding(Label.TextProperty);
            UpperValueLabel.RemoveBinding(Label.TextProperty);
            RemoveGestureRecognizer(LowerThumb, lowerThumbGestureRecognizer);
            RemoveGestureRecognizer(UpperThumb, upperThumbGestureRecognizer);
            Track.SizeChanged -= OnViewSizeChanged;
            LowerThumb.SizeChanged -= OnViewSizeChanged;
            UpperThumb.SizeChanged -= OnViewSizeChanged;
            LowerValueLabel.SizeChanged -= OnViewSizeChanged;
            UpperValueLabel.SizeChanged -= OnViewSizeChanged;
            Content = null;
        }

        static object CoerceValue(BindableObject bindable, object value)
            => ((RangeSlider)bindable).CoerceValue((double)value);

        static void OnMinimumMaximumValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((RangeSlider)bindable).OnMinimumMaximumValuePropertyChanged();
            OnLowerUpperValuePropertyChanged(bindable, oldValue, newValue);
        }

        static void OnLowerUpperValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((RangeSlider)bindable).OnLowerUpperValuePropertyChanged();

        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((RangeSlider)bindable).OnLayoutPropertyChanged();

        void OnIsEnabledChanged()
        {
            if (Content == null)
                return;

            Content.Opacity = IsEnabled
                ? EnabledOpacity
                : DisabledOpacity;
        }

        double CoerceValue(double value)
            => value.Clamp(MinimumValue, MaximumValue);

        void OnMinimumMaximumValuePropertyChanged()
        {
            LowerValue = CoerceValue(LowerValue);
            UpperValue = CoerceValue(UpperValue);
        }

        void OnLowerUpperValuePropertyChanged()
        {
            var rangeValue = MaximumValue - MinimumValue;
            var trackWidth = TrackWidth;

            lowerTranslation = (LowerValue - MinimumValue) / rangeValue * trackWidth;
            upperTranslation = (UpperValue - MinimumValue) / rangeValue * trackWidth + LowerThumb.Width;

            LowerThumb.TranslationX = lowerTranslation;
            UpperThumb.TranslationX = upperTranslation;
            OnValueLabelTranslationChanged();

            var bounds = GetLayoutBounds(TrackHighlight);
            SetLayoutBounds(TrackHighlight, new Rectangle(lowerTranslation, bounds.Y, upperTranslation - lowerTranslation + UpperThumb.Width, bounds.Height));
        }

        void OnValueLabelTranslationChanged()
        {
            var labelSpacing = 5;
            var lowerLabelTranslation = lowerTranslation + (LowerThumb.Width - LowerValueLabel.Width) / 2;
            var upperLabelTranslation = upperTranslation + (UpperThumb.Width - UpperValueLabel.Width) / 2;
            LowerValueLabel.TranslationX = Min(Max(lowerLabelTranslation, 0), Width - LowerValueLabel.Width - UpperValueLabel.Width - labelSpacing);
            UpperValueLabel.TranslationX = Min(Max(upperLabelTranslation, LowerValueLabel.TranslationX + LowerValueLabel.Width + labelSpacing), Width - UpperValueLabel.Width);
        }

        void OnLayoutPropertyChanged()
        {
            BatchBegin();
            Track.BatchBegin();
            TrackHighlight.BatchBegin();
            LowerThumb.BatchBegin();
            UpperThumb.BatchBegin();
            LowerValueLabel.BatchBegin();
            UpperValueLabel.BatchBegin();

            var lowerThumbColor = GetColorOrDefault(LowerThumbColor, ThumbColor);
            var upperThumbColor = GetColorOrDefault(UpperThumbColor, ThumbColor);
            var lowerThumbBorderColor = GetColorOrDefault(LowerThumbBorderColor, ThumbBorderColor);
            var upperThumbBorderColor = GetColorOrDefault(UpperThumbBorderColor, ThumbBorderColor);
            LowerThumb.BackgroundColor = GetColorOrDefault(lowerThumbColor, Color.White);
            UpperThumb.BackgroundColor = GetColorOrDefault(upperThumbColor, Color.White);
            LowerThumb.BorderColor = GetColorOrDefault(lowerThumbBorderColor, Color.FromRgb(182, 182, 182));
            UpperThumb.BorderColor = GetColorOrDefault(upperThumbBorderColor, Color.FromRgb(182, 182, 182));
            Track.BackgroundColor = GetColorOrDefault(TrackColor, Color.FromRgb(182, 182, 182));
            TrackHighlight.BackgroundColor = GetColorOrDefault(TrackHighlightColor, Color.FromRgb(46, 124, 246));
            Track.BorderColor = GetColorOrDefault(TrackBorderColor, Color.Default);
            TrackHighlight.BorderColor = GetColorOrDefault(TrackHighlightBorderColor, Color.Default);

            var trackSize = TrackSize;
            var trackRadius = (float)GetDoubleOrDefault(TrackRadius, trackSize / 2);
            var lowerThumbSize = GetDoubleOrDefault(LowerThumbSize, ThumbSize);
            var upperThumbSize = GetDoubleOrDefault(UpperThumbSize, ThumbSize);
            Track.CornerRadius = trackRadius;
            TrackHighlight.CornerRadius = trackRadius;
            LowerThumb.CornerRadius = (float)GetDoubleOrDefault(GetDoubleOrDefault(LowerThumbRadius, ThumbRadius), lowerThumbSize / 2);
            UpperThumb.CornerRadius = (float)GetDoubleOrDefault(GetDoubleOrDefault(UpperThumbRadius, ThumbRadius), upperThumbSize / 2);

            LowerThumb.Content = LowerThumbView;
            UpperThumb.Content = UpperThumbView;

            var labelWithSpacingHeight = Max(Max(LowerValueLabel.Height, UpperValueLabel.Height), 0);
            if (labelWithSpacingHeight > 0)
                labelWithSpacingHeight += ValueLabelSpacing;

            var trackThumbHeight = Max(Max(lowerThumbSize, upperThumbSize), trackSize);
            var trackVerticalPosition = labelWithSpacingHeight + (trackThumbHeight - trackSize) / 2;
            var lowerThumbVerticalPosition = labelWithSpacingHeight + (trackThumbHeight - lowerThumbSize) / 2;
            var upperThumbVerticalPosition = labelWithSpacingHeight + (trackThumbHeight - upperThumbSize) / 2;

            Content.HeightRequest = labelWithSpacingHeight + trackThumbHeight;

            var trackHighlightBounds = GetLayoutBounds(TrackHighlight);
            SetLayoutBounds(TrackHighlight, new Rectangle(trackHighlightBounds.X, trackVerticalPosition, trackHighlightBounds.Width, trackSize));
            SetLayoutBounds(Track, new Rectangle(0, trackVerticalPosition, Width, trackSize));
            SetLayoutBounds(LowerThumb, new Rectangle(0, lowerThumbVerticalPosition, lowerThumbSize, lowerThumbSize));
            SetLayoutBounds(UpperThumb, new Rectangle(0, upperThumbVerticalPosition, upperThumbSize, upperThumbSize));
            SetLayoutBounds(LowerValueLabel, new Rectangle(0, 0, -1, -1));
            SetLayoutBounds(UpperValueLabel, new Rectangle(0, 0, -1, -1));
            SetValueLabelBinding(LowerValueLabel, LowerValueProperty);
            SetValueLabelBinding(UpperValueLabel, UpperValueProperty);
            LowerValueLabel.Style = LowerValueLabelStyle ?? ValueLabelStyle;
            UpperValueLabel.Style = UpperValueLabelStyle ?? ValueLabelStyle;

            OnLowerUpperValuePropertyChanged();

            Track.BatchCommit();
            TrackHighlight.BatchCommit();
            LowerThumb.BatchCommit();
            UpperThumb.BatchCommit();
            LowerValueLabel.BatchCommit();
            UpperValueLabel.BatchCommit();
            BatchCommit();
        }

        void OnViewSizeChanged(object sender, System.EventArgs e)
        {
            var maxHeight = Max(LowerValueLabel.Height, UpperValueLabel.Height);
            if ((sender == LowerValueLabel || sender == UpperValueLabel) && labelMaxHeight == maxHeight)
            {
                BeginInvokeOnMainThread(OnValueLabelTranslationChanged);
                return;
            }

            labelMaxHeight = maxHeight;
            OnLayoutPropertyChanged();
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var view = (View)sender;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    OnPanStarted(view);
                    break;
                case GestureStatus.Running:
                    OnPanRunning(view, e.TotalX);
                    break;
                case GestureStatus.Completed:
                    OnPanCompleted(view);
                    break;
                case GestureStatus.Canceled:
                    OnPanCanceled(view);
                    break;
            }
        }

        void OnPanStarted(View view)
            => thumbPositionMap[view] = view.TranslationX;

        void OnPanRunning(View view, double value)
            => UpdateValue(view, value + GetPanShiftValue(view));

        void OnPanCompleted(View view)
            => thumbPositionMap[view] = view.TranslationX;

        void OnPanCanceled(View view)
            => UpdateValue(view, thumbPositionMap[view]);

        void UpdateValue(View view, double value)
        {
            var rangeValue = MaximumValue - MinimumValue;
            if (view == LowerThumb)
            {
                LowerValue = Min(Max(MinimumValue, value / TrackWidth * rangeValue + MinimumValue), UpperValue);
                return;
            }
            UpperValue = Min(Max(LowerValue, (value - LowerThumb.Width) / TrackWidth * rangeValue + MinimumValue), MaximumValue);
        }

        double GetPanShiftValue(View view)
            => RuntimePlatform == Android
                ? view.TranslationX
                : thumbPositionMap[view];

        void SetValueLabelBinding(Label label, BindableProperty bindableProperty)
            => label.SetBinding(Label.TextProperty, new Binding
            {
                Path = bindableProperty.PropertyName,
                Source = this,
                StringFormat = ValueLabelStringFormat
            });

        void AddGestureRecognizer(View view, PanGestureRecognizer gestureRecognizer)
        {
            gestureRecognizer.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(gestureRecognizer);
        }

        void RemoveGestureRecognizer(View view, PanGestureRecognizer gestureRecognizer)
        {
            gestureRecognizer.PanUpdated -= OnPanUpdated;
            view.GestureRecognizers.Remove(gestureRecognizer);
        }

        Color GetColorOrDefault(Color color, Color defaultColor)
            => color == Color.Default
                ? defaultColor
                : color;

        double GetDoubleOrDefault(double value, double defaultSize)
            => value < 0
                ? defaultSize
                : value;

        sealed class RangeSliderLayout : AbsoluteLayout
        {
            public RangeSliderLayout()
            {
                Children.Add(Track);
                Children.Add(TrackHighlight);
                Children.Add(LowerThumb);
                Children.Add(UpperThumb);
                Children.Add(LowerValueLabel);
                Children.Add(UpperValueLabel);
            }

            internal Frame Track { get; } = CreateFrameElement();

            internal Frame TrackHighlight { get; } = CreateFrameElement();

            internal Frame LowerThumb { get; } = CreateFrameElement();

            internal Frame UpperThumb { get; } = CreateFrameElement();

            internal Label LowerValueLabel { get; } = CreateLabelElement();

            internal Label UpperValueLabel { get; } = CreateLabelElement();

            static Frame CreateFrameElement()
                => new Frame
                {
                    Padding = 0,
                    HasShadow = false,
                    IsClippedToBounds = true
                };

            static Label CreateLabelElement()
                => new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = GetNamedSize(NamedSize.Small, typeof(Label))
                };
        }
    }
}
