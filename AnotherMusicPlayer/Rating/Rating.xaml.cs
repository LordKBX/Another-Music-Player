using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Logique d'interaction pour Rating.xaml
    /// </summary>
    public partial class Rating : UserControl
    {
        private double Max = 5.0;
        private double StarCaseWidth = 30.0;
        private PointCollection StarPoints = new PointCollection() { new Point(2.0, 9.0), new Point(28.0, 9.0), new Point(5.0, 25.0), new Point(15.0, 0.0), new Point(25.0, 25.0) };

        public static readonly SolidColorBrush DefaultStarBackground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
        public static readonly SolidColorBrush DefaultStarForeground = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        public static readonly SolidColorBrush DefaultStarSelectionForeground = new SolidColorBrush(Color.FromRgb(255, 0, 0));

        public SolidColorBrush StarBackground = DefaultStarBackground;
        public SolidColorBrush StarForeground = DefaultStarForeground;
        public SolidColorBrush StarSelectionForeground = DefaultStarSelectionForeground;

        public Rating()
        {
            InitializeComponent();
            this.MouseMove += StarGrid_MouseMove;
            this.MouseLeave += StarGrid_MouseLeave;
            this.MouseLeftButtonUp += StarGrid_MouseLeftButtonUp;
        }

        public void setAltLeftClick() { this.MouseLeftButtonDown += StarGrid_MouseLeftButtonUp; }

        private void StarGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsReadOnly) { return; }
            ChangeGrid2StarsColor(StarForeground);
            reDraw();
        }

        private void StarGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsReadOnly) { return; }
            Point pos = e.GetPosition(StarGrid);
            //Debug.WriteLine("Mouse Position = " + pos.X + ":" + pos.Y);
            ChangeGrid2StarsColor(StarSelectionForeground);
            double caseW = StarCaseWidth * _Zoom;
            double posx = (int)(pos.X / caseW) + ((((pos.X / caseW) - (int)(pos.X / caseW)) >= 0.5) ? 0.5 : 0);
            Grid2.Width = posx * caseW;
        }

        private void StarGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("StarGrid_MouseLeftButtonUp");
            if (IsReadOnly) { return; }
            Point pos = e.GetPosition(StarGrid);
            double caseW = StarCaseWidth * _Zoom;
            double posx = (int)(pos.X / caseW) + ((((pos.X / caseW) - (int)(pos.X / caseW)) >= 0.5) ? 0.5 : 0);
            RaiseEvent(new RoutedPropertyChangedEventArgs<double>(_Rate, posx) { RoutedEvent = RateChangedEvent });
            _Rate = posx;
            ChangeGrid2StarsColor(StarForeground);
            reDraw();
        }

        public bool setRate(double rate)
        {
            if (rate < 0 || rate > Max) { return false; }
            SetValue(RateProperty, rate);
            _Rate = rate; reDraw();
            return true;
        }

        private void ChangeGrid1StarsColor(SolidColorBrush color)
        {
            foreach (Polygon star in Grid1.Children) { star.Fill = color; }
            reDraw();
        }

        private void ChangeGrid2StarsColor(SolidColorBrush color)
        {
            foreach (Polygon star in Grid2.Children) { star.Fill = color; }
            reDraw();
        }

        public void reDraw() { Grid2.Width = StarCaseWidth * _Zoom * _Rate; }

        #region Rate Property
        /// <summary>
        /// StarForegroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty RateProperty = DependencyProperty.Register(
            "Rate", typeof(double), typeof(Rating), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnRateChanged)));

        private double _Rate = 0;
        public double Rate
        {
            get { return _Rate; }
            set { setRate(value); }
        }

        /// <summary>
        /// Handles changes to the StarForegroundColor property.
        /// </summary>
        private static void OnRateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { ((Rating)d).setRate((double)e.NewValue); }

        public static readonly RoutedEvent RateChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(_Rate),
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<double>),
                typeof(Rating));

        public event RoutedPropertyChangedEventHandler<double> RateChanged
        {
            add => AddHandler(RateChangedEvent, value);
            remove => RemoveHandler(RateChangedEvent, value);
        }

        #endregion

        #region Zoom Property
        /// <summary>
        /// StarForegroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            "Zoom", typeof(double), typeof(Rating), new FrameworkPropertyMetadata(1.0, new PropertyChangedCallback(OnZoomChanged)));

        /// <summary>
        /// Handles changes to the StarForegroundColor property.
        /// </summary>
        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { ((Rating)d).setZoom((double)e.NewValue); }

        private double _Zoom = 1.0;
        public double Zoom
        {
            get { return _Zoom; }
            set { setZoom(value); }
        }

        public void setZoom(double multiply)
        {
            double caseSize = StarCaseWidth * multiply;
            PointCollection StarMatrix = StarPoints;
            for (int i = 0; i < 5; i++) { StarMatrix[i] = new Point(StarPoints[i].X * multiply, StarPoints[i].Y * multiply); }
            foreach (Polygon star in Grid1.Children) { star.Width = caseSize; star.Points = StarMatrix; }
            foreach (Polygon star in Grid2.Children) { star.Width = caseSize; star.Points = StarMatrix; }
            _Zoom = multiply; reDraw();
        }
        #endregion

        #region StarBackgroundColor
        /// <summary>
        /// BackgroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty StarBackgroundColorProperty = DependencyProperty.Register(
            "StarBackgroundColor", typeof(SolidColorBrush), typeof(Rating), new FrameworkPropertyMetadata(Rating.DefaultStarBackground, new PropertyChangedCallback(OnStarBackgroundColorChanged)));

        /// <summary>
        /// Gets or sets the BackgroundColor property.  
        /// </summary>
        public SolidColorBrush StarBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(StarBackgroundColorProperty); }
            set { SetValue(StarBackgroundColorProperty, value); }
        }

        /// <summary>
        /// Handles changes to the BackgroundColor property.
        /// </summary>
        private static void OnStarBackgroundColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Rating control = (Rating)d;
            control.StarBackground = (SolidColorBrush)e.NewValue;
            control.ChangeGrid1StarsColor(control.StarBackground);
        }
        #endregion

        #region StarForegroundColor
        /// <summary>
        /// StarForegroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty StarForegroundColorProperty = DependencyProperty.Register(
            "StarForegroundColor", typeof(SolidColorBrush), typeof(Rating), new FrameworkPropertyMetadata(Rating.DefaultStarForeground, new PropertyChangedCallback(OnStarForegroundColorChanged)));

        /// <summary>
        /// Gets or sets the StarForegroundColor property.  
        /// </summary>
        public SolidColorBrush StarForegroundColor
        {
            get { return (SolidColorBrush)GetValue(StarForegroundColorProperty); }
            set { SetValue(StarForegroundColorProperty, value); }
        }

        /// <summary>
        /// Handles changes to the StarForegroundColor property.
        /// </summary>
        private static void OnStarForegroundColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Rating control = (Rating)d;
            control.StarForeground = (SolidColorBrush)e.NewValue;
            control.ChangeGrid2StarsColor(control.StarForeground);
        }
        #endregion

        #region StarSelectionForegroundColor
        /// <summary>
        /// StarForegroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty StarSelectionForegroundColorProperty = DependencyProperty.Register(
            "StarSelectionForegroundColor", typeof(SolidColorBrush), typeof(Rating), new FrameworkPropertyMetadata(Rating.DefaultStarForeground, new PropertyChangedCallback(OnStarSelectionForegroundColorChanged)));

        /// <summary>
        /// Gets or sets the StarForegroundColor property.  
        /// </summary>
        public SolidColorBrush StarSelectionForegroundColor
        {
            get { return (SolidColorBrush)GetValue(StarSelectionForegroundColorProperty); }
            set { SetValue(StarSelectionForegroundColorProperty, value); }
        }

        /// <summary>
        /// Handles changes to the StarForegroundColor property.
        /// </summary>
        private static void OnStarSelectionForegroundColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Rating control = (Rating)d;
            control.StarSelectionForeground = (SolidColorBrush)e.NewValue;
        }
        #endregion

        #region IsReadOnly
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            nameof(IsReadOnly), typeof(bool), typeof(Rating), new PropertyMetadata(default(bool)));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }
        #endregion  
    }
}
