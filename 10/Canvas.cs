using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace _10
{
    public partial class Canvas : UserControl
    {
        List<Point> Points = new List<Point>();

        public Canvas()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        public void UpdatePoints(List<Point> points)
        {
            Points = points;
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (Point point in Points)
            {
                if (point.X < minX)
                {
                    minX = point.X;
                }
                if (point.X > maxX)
                {
                    maxX = point.X;
                }
                if (point.Y < minY)
                {
                    minY = point.Y;
                }
                if (point.Y > maxY)
                {
                    maxY = point.Y;
                }
            }

            int trueWidth = maxX - minX;
            int trueHeight = maxY - minY;
            double trueCentreX = minX + (trueWidth / 2.0);
            double trueCentreY = minY + (trueHeight / 2.0);

            double xScalar = (((double)this.Width*0.9) / trueWidth);
            double yScalar = (((double)this.Height*0.7) / trueHeight);

            using (SolidBrush pointBrush = new SolidBrush(Color.Black))
            {
                foreach (Point point in Points)
                {
                    double trueXOffset = point.X - trueCentreX;
                    double xOffset = (trueXOffset * xScalar);
                    int scaledX = (int)Math.Round((this.Width / 2.0) + xOffset);
                    double trueYOffset = point.Y - trueCentreY;
                    double yOffset = (trueYOffset * yScalar);
                    int scaledY = (int)Math.Round((this.Height / 2.0) + yOffset);

                    e.Graphics.FillEllipse(pointBrush, scaledX - 10 - 2, scaledY - 10 - 2, 21, 21);
                }
            }
        }

        private void Canvas_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
