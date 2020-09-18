using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DimmensionalCard.Dimmensions {
    class MarkingShape : Shape {

        public Marking Marking { get; set; }
        public int Number { get; set; } = 0;
        public MarkingShape(Marking marking, int number = 0) {
            Marking = marking;
            Number = number;
        }

        protected override Geometry DefiningGeometry {
            get {

                Point p1 = new Point(Marking.PosX, Marking.PosY);
                Point p2 = new Point(Marking.EndPosX, Marking.PosY);
                Point p3 = new Point(Marking.EndPosX, Marking.EndPosY);
                Point p4 = new Point(Marking.PosX, Marking.EndPosY);

                Point p5;
                switch (Marking.MarkSide) {
                    case MarkSide.TopLeft:
                        p5 = Point.Add(p1, new Vector(-10, -10));
                        break;
                    case MarkSide.TopRight:
                        p5 = Point.Add(p2, new Vector(10, -10));
                        break;
                    case MarkSide.DownRight:
                        p5 = Point.Add(p3, new Vector(10, 10));
                        break;
                    case MarkSide.DownLeft:
                        p5 = Point.Add(p4, new Vector(-10, 10));
                        break;
                    default:
                        p5 = Point.Add(p1, new Vector(-10, -10));
                        break;
                }

                var text = new FormattedText(Number.ToString(), CultureInfo.GetCultureInfo("en-us"),
                                                FlowDirection.LeftToRight, new Typeface("Tahoma"), 12, Brushes.Black,
                                                VisualTreeHelper.GetDpi(this).PixelsPerDip);
                text.TextAlignment = TextAlignment.Center;

                var geom1 = new RectangleGeometry(new Rect(p1, p3));
                var geom2 = new EllipseGeometry(p5, 10, 10);
                var geom3 = text.BuildGeometry(Point.Add(p5, new Vector(0, -text.Height / 2)));

                GeometryGroup geomGroup = new GeometryGroup();

                // geom1 added twice for transparency
                geomGroup.Children.Add(geom1);
                geomGroup.Children.Add(geom1);
                geomGroup.Children.Add(geom2);
                geomGroup.Children.Add(geom3);

                return geomGroup;
            }
        }
    }
}