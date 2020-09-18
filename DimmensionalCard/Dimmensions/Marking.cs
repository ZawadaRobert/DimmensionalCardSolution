using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DimmensionalCard.Dimmensions {
    class Marking {
        public double Width { get; set; }
        public double Height { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double EndPosX { get => PosX + Width; }
        public double EndPosY { get => PosY + Height; }
        public MarkSide MarkSide { get; set; }

        public Marking(Rectangle rect) {
            Width = rect.Width;
            Height = rect.Height;
            PosX = Canvas.GetLeft(rect);
            PosY = Canvas.GetTop(rect);
            MarkSide = MarkSide.DownLeft;
        }
    }
    enum MarkSide {
        TopLeft,
        TopRight,
        DownLeft,
        DownRight
    }
}
