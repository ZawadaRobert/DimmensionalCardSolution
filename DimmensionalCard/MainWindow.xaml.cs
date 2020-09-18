using DimmensionalCard.Dimmensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DimmensionalCard {
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow, INotifyPropertyChanged {

        private Point startPoint;
        private Rectangle rect;
        private List<Marking> markings = new List<Marking>();
        private List<MarkingShape> markingShapes = new List<MarkingShape>();
        public event PropertyChangedEventHandler PropertyChanged;
        private BitmapImage _drawingImage;

        public BitmapImage DrawingImage {
            get { return _drawingImage; }
            set {
                _drawingImage = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindow() {
            InitializeComponent();
            LoadNewDrawing();
        }

        private void LoadNewDrawing() {

            DrawingCanvas.Children.Clear();
            DrawingImage = new BitmapImage(new Uri(@"Images\drw_example2.jpg", UriKind.Relative));

            ImageBrush ib = new ImageBrush();
            ib.ImageSource = DrawingImage;
            DrawingCanvas.Background = ib;

            DrawingViewbox.Width = ib.ImageSource.Width;
            DrawingViewbox.Height = ib.ImageSource.Height;
            DrawingCanvas.Width = ib.ImageSource.Width;
            DrawingCanvas.Height = ib.ImageSource.Height;
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region WPF_Events
        private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {

            if ((bool)MarkingModeButton.IsChecked) {
                startPoint = e.GetPosition(DrawingCanvas);

                rect = new Rectangle {
                    Stroke = Brushes.Violet,
                    StrokeThickness = 2,
                    StrokeDashCap = PenLineCap.Round,
                    StrokeDashArray = new DoubleCollection() { 2, 3 }
                };
                Canvas.SetLeft(rect, startPoint.X);
                Canvas.SetTop(rect, startPoint.Y);
                DrawingCanvas.Children.Add(rect);
            }
        }
        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            rect = null;
        }
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Released || rect == null)
                return;

            var pos = e.GetPosition(DrawingCanvas);

            var x = Math.Min(pos.X, startPoint.X);
            var y = Math.Min(pos.Y, startPoint.Y);

            var w = Math.Max(pos.X, startPoint.X) - x;
            var h = Math.Max(pos.Y, startPoint.Y) - y;

            rect.Width = w;
            rect.Height = h;

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }
        private void DrawingViewbox_MouseWheel(object sender, MouseWheelEventArgs e) {

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
                double change = (e.Delta > 0) ? 1.2 : 0.8;

                //Check if drawing is not too small already
                if ((DrawingViewbox.Width < 300 || DrawingViewbox.Height < 200) && change < 1)
                    return;

                DrawingViewbox.Width *= change;
                DrawingViewbox.Height *= change;
            }
        }
        private void MarkingModeButton_Checked(object sender, RoutedEventArgs e) {

            var collection = new List<Rectangle>();

            foreach (MarkingShape shape in DrawingCanvas.Children.OfType<MarkingShape>()) {

                Rectangle rect2 = new Rectangle {
                    Width = shape.Marking.Width,
                    Height = shape.Marking.Height,
                    Stroke = Brushes.Violet,
                    StrokeThickness = 2,
                    StrokeDashCap = PenLineCap.Round,
                    StrokeDashArray = new DoubleCollection() { 2, 3 }
                };

                Canvas.SetLeft(rect2, shape.Marking.PosX);
                Canvas.SetTop(rect2, shape.Marking.PosY);

                collection.Add(rect2);
            }

            DrawingCanvas.Children.Clear();

            foreach (Rectangle shape in collection) {
                DrawingCanvas.Children.Add(shape);
            }

        }
        private void MarkingModeButton_Unchecked(object sender, RoutedEventArgs e) {
            foreach (Rectangle rect in DrawingCanvas.Children.OfType<Rectangle>()) {

                Marking marking = new Marking(rect);
                markings.Add(marking);

                MarkingShape shape = new MarkingShape(marking);
                markingShapes.Add(shape);
            }
            AddMarkingsToCanvas();
        }
        private void DrawingCanvas_MouseEnter(object sender, MouseEventArgs e) {
            if (Cursor != Cursors.Wait && (bool)MarkingModeButton.IsChecked)
                Mouse.OverrideCursor = Cursors.Cross;
        }
        private void DrawingCanvas_MouseLeave(object sender, MouseEventArgs e) {
            if (Cursor != Cursors.Wait)
                Mouse.OverrideCursor = null;
        }
        #endregion

        private void TestButton_Click(object sender, RoutedEventArgs e) {

            DrawingCanvas.Children.Clear();

            int count = 0;
            foreach (MarkingShape shape in markingShapes) {
                count++;
                shape.Number = count;
                shape.Stroke = Brushes.Red;
                shape.StrokeThickness = 2;
                shape.Fill = Brushes.White;
                DrawingCanvas.Children.Add(shape);
            }
        }

        public void AddMarkingsToCanvas() {
            foreach (Marking marking in markings) {
                Rectangle rect2 = new Rectangle {
                    Width = marking.Width,
                    Height = marking.Height,
                    Stroke = Brushes.Violet,
                    StrokeThickness = 2,
                    StrokeDashCap = PenLineCap.Round,
                    StrokeDashArray = new DoubleCollection() { 2, 3 }
                };
                Canvas.SetLeft(rect2, marking.PosX);
                Canvas.SetTop(rect2, marking.PosY);
            }
            markings.Clear();
        }
    }
}
