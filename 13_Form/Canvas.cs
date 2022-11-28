using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _13_Form
{
    public partial class Canvas : UserControl
    {
        List<Crash> Crashes = new List<Crash>();

        TrackCell[,] grid = null;

        public Canvas()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        public void UpdateGrid(TrackCell[,] newGrid, List<Location> crashLocations = null)
        {
            grid = newGrid;
            if (crashLocations != null)
            {
                foreach (Location crashLocation in crashLocations)
                {
                    Crashes.Add(new Crash(crashLocation, 20));
                }
            }
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (grid != null)
            {
                int sizeX = grid.GetLength(1);
                int sizeY = grid.GetLength(0);

                int cellDimension = (int)Math.Floor((double)(this.Height - 20) / sizeY);

                using (Pen trackPen = new Pen(Color.Black, 1))
                using (SolidBrush cartBrush = new SolidBrush(Color.Black))
                using (SolidBrush crashBrush = new SolidBrush(Color.Red))
                {
                    for (int gridY = 0; gridY < sizeY; gridY++)
                    {
                        for (int gridX = 0; gridX < sizeX; gridX++)
                        {
                            int x = 10 + gridX * cellDimension;
                            int y = 10 + gridY * cellDimension;



                            switch (grid[gridY, gridX].Type)
                            {
                                case TrackType.EastWest:
                                    e.Graphics.DrawLine(trackPen, x, y + cellDimension / 2f, x + cellDimension, y + cellDimension / 2f);
                                    break;
                                case TrackType.NorthSouth:
                                    e.Graphics.DrawLine(trackPen, x + cellDimension / 2f, y, x + cellDimension / 2f, y + cellDimension);
                                    break;
                                case TrackType.NorthEast:
                                    e.Graphics.DrawLine(trackPen, x + cellDimension / 2f, y, x + cellDimension / 2f, y + cellDimension / 2f);
                                    e.Graphics.DrawLine(trackPen, x + cellDimension / 2f, y + cellDimension / 2f, x + cellDimension, y + cellDimension / 2f);
                                    break;
                                case TrackType.EastSouth:
                                    e.Graphics.DrawLine(trackPen, x + cellDimension / 2f, y + cellDimension / 2f, x + cellDimension, y + cellDimension / 2f);
                                    e.Graphics.DrawLine(trackPen, x + cellDimension / 2f, y + cellDimension / 2f, x + cellDimension / 2f, y + cellDimension);
                                    break;
                                case TrackType.SouthWest:
                                    e.Graphics.DrawLine(trackPen, x + cellDimension / 2f, y + cellDimension / 2f, x + cellDimension / 2f, y + cellDimension);
                                    e.Graphics.DrawLine(trackPen, x, y + cellDimension / 2f, x + cellDimension / 2f, y + cellDimension / 2f);
                                    break;
                                case TrackType.NorthWest:
                                    e.Graphics.DrawLine(trackPen, x + cellDimension / 2f, y, x + cellDimension / 2f, y + cellDimension / 2f);
                                    e.Graphics.DrawLine(trackPen, x, y + cellDimension / 2f, x + cellDimension / 2f, y + cellDimension / 2f);
                                    break;
                                case TrackType.NorthEastSouthWest:
                                    e.Graphics.DrawLine(trackPen, x, y + cellDimension / 2f, x + cellDimension, y + cellDimension / 2f);
                                    e.Graphics.DrawLine(trackPen, x + cellDimension / 2f, y, x + cellDimension / 2f, y + cellDimension);
                                    break;
                            }

                            if (grid[gridY, gridX].Carts.Count > 0)
                            {
                                e.Graphics.FillEllipse(cartBrush, x, y, cellDimension, cellDimension);
                            }
                        }
                    }

                    for (int i = 0; i < Crashes.Count;)
                    {
                        if (Crashes[i].Value > 0)
                        {
                            int x = 10 + Crashes[i].Location.X * cellDimension;
                            int y = 10 + Crashes[i].Location.Y * cellDimension;
                            e.Graphics.FillEllipse(crashBrush, x - cellDimension, y - cellDimension, cellDimension * 3, cellDimension * 3);
                            Crashes[i].Value--;
                            i++;
                        }
                        else
                        {
                            Crashes.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }

    public class Crash
    {
        public Location Location;
        public int Value;

        public Crash(Location location, int value)
        {
            Location = location;
            Value = value;
        }
    }
}
