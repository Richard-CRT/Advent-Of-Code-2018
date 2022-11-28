using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventOfCodeUtilities;

namespace _17_Form
{
    public partial class Canvas : UserControl
    {
        public int MapMinimumX;
        public int MapMinimumY;
        public int MapMaximumX;
        public int MapMaximumY;
        public int MapDepth;

        private int CellDim = 2;

        public List<ScanCell>[] Map;

        public List<ScanCell> sourceCells;

        public Day17 parent;

        public Canvas()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        public void Canvas_Setup()
        {
            List<Vein> veins = new List<Vein>();

            MapMinimumX = int.MaxValue;
            MapMinimumY = int.MaxValue;
            MapMaximumX = -1;
            MapMaximumY = -1;

            for (int i = 0; i < parent.inputList.Count; i++)
            {
                string[] veinStringParts = parent.inputList[i].Split(' ');
                string fixedCoordinateString = veinStringParts[0].Substring(0, veinStringParts[0].Length - 1);
                string variableCoordinateString = veinStringParts[1];

                int xStart;
                int xEnd;
                int yStart;
                int yEnd;

                string[] variableCoordinateStringParts = variableCoordinateString.Substring(2).Split('.');

                if (fixedCoordinateString.Substring(0, 1) == "x")
                {
                    xStart = Int32.Parse(fixedCoordinateString.Substring(2, fixedCoordinateString.Length - 2));
                    xEnd = xStart;
                    yStart = Int32.Parse(variableCoordinateStringParts[0]);
                    yEnd = Int32.Parse(variableCoordinateStringParts[2]);
                }
                else
                {
                    yStart = Int32.Parse(fixedCoordinateString.Substring(2, fixedCoordinateString.Length - 2));
                    yEnd = yStart;
                    xStart = Int32.Parse(variableCoordinateStringParts[0]);
                    xEnd = Int32.Parse(variableCoordinateStringParts[2]);
                }

                //if (yEnd > 50)
                //{
                //    continue;
                //}

                if (xStart < MapMinimumX)
                {
                    MapMinimumX = xStart;
                }
                if (xEnd > MapMaximumX)
                {
                    MapMaximumX = xEnd;
                }
                if (yStart < MapMinimumY)
                {
                    MapMinimumY = yStart;
                }
                if (yEnd > MapMaximumY)
                {
                    MapMaximumY = yEnd;
                }

                veins.Add(new Vein(xStart, xEnd, yStart, yEnd));
            }

            MapMinimumX = 360;
            MapMaximumX = 595;

            MapDepth = MapMaximumY + 1;
            Map = new List<ScanCell>[MapDepth];

            for (int y = 0; y < MapDepth; y++)
            {
                Map[y] = new List<ScanCell>();
                for (int x = MapMinimumX; x <= MapMaximumX; x++)
                {
                    Map[y].Add(new ScanCell(y, x));
                    if (y == 0 && x == 500)
                    {
                        GetMapScanCell(y, x).Type = ScanCellType.Spring;
                    }
                }
            }

            foreach (Vein vein in veins)
            {
                for (int y = vein.YStart; y <= vein.YEnd; y++)
                {
                    for (int x = vein.XStart; x <= vein.XEnd; x++)
                    {
                        GetMapScanCell(y, x).Type = ScanCellType.Clay;
                    }
                }
            }

            this.Refresh();

            sourceCells = new List<ScanCell> { GetMapScanCell(0, 500) };
        }

        public void Tick()
        {
            if (sourceCells.Count > 0)
            {
                ScanCell scanningCell = sourceCells[0];
                while (true)
                {
                    scanningCell = GetMapScanCell(scanningCell.Y + 1, scanningCell.X);
                    if (scanningCell == null || scanningCell.Type == ScanCellType.Clay)
                    {
                        break;
                    }
                    if (scanningCell.Type != ScanCellType.Water)
                    {
                        scanningCell.Type = ScanCellType.DrySand;
                    }
                }


                if (scanningCell != null)
                {

                    ScanCell rightCell = null;
                    ScanCell leftCell = null;

                    bool bounded = true;

                    while (bounded)
                    {
                        scanningCell = GetMapScanCell(scanningCell.Y - 1, scanningCell.X);

                        bool rightBounded = false;
                        rightCell = GetMapScanCell(scanningCell.Y, scanningCell.X);
                        while (true)
                        {
                            rightCell = GetMapScanCell(rightCell.Y, rightCell.X + 1);
                            if (rightCell.Type == ScanCellType.Clay)
                            {
                                rightBounded = true;
                                break;
                            }
                            else if (GetMapScanCell(rightCell.Y + 1, rightCell.X).Type == ScanCellType.DrySand || GetMapScanCell(rightCell.Y + 1, rightCell.X).Type == ScanCellType.UntouchedSand)
                            {
                                break;
                            }
                        }

                        bool leftBounded = false;
                        leftCell = GetMapScanCell(scanningCell.Y, scanningCell.X);
                        while (true)
                        {
                            leftCell = GetMapScanCell(leftCell.Y, leftCell.X - 1);
                            if (leftCell.Type == ScanCellType.Clay)
                            {
                                leftBounded = true;
                                break;
                            }
                            else if (GetMapScanCell(leftCell.Y + 1, leftCell.X).Type == ScanCellType.DrySand || GetMapScanCell(leftCell.Y + 1, leftCell.X).Type == ScanCellType.UntouchedSand)
                            {
                                break;
                            }
                        }

                        bounded = leftBounded && rightBounded;

                        int leftLimit;
                        int rightLimit;

                        if (leftBounded)
                        {
                            leftLimit = leftCell.X + 1;
                        }
                        else
                        {
                            leftLimit = leftCell.X;
                            if (leftCell.Type == ScanCellType.UntouchedSand && !sourceCells.Contains(leftCell))
                                sourceCells.Add(leftCell);
                        }

                        if (rightBounded)
                        {
                            rightLimit = rightCell.X - 1;
                        }
                        else
                        {
                            rightLimit = rightCell.X;
                            if (rightCell.Type == ScanCellType.UntouchedSand && !sourceCells.Contains(rightCell))
                                sourceCells.Add(rightCell);
                        }

                        for (int x = leftLimit; x <= rightLimit; x++)
                        {
                            if (bounded)
                            {
                                GetMapScanCell(scanningCell.Y, x).Type = ScanCellType.Water;
                            }
                            else
                            {
                                GetMapScanCell(scanningCell.Y, x).Type = ScanCellType.DrySand;
                            }
                        }

                        this.Refresh();
                        //Console.ReadLine();
                    }
                }

                this.Refresh();
                //Console.ReadLine();

                sourceCells.RemoveAt(0);
            }
        }

        public ScanCell GetMapScanCell(int y, int x)
        {
            if (y < MapDepth)
            {
                if (x < MapMinimumX)
                {
                    for (int yi = 0; yi < MapDepth; yi++)
                    {
                        for (int i = 0; i < MapMinimumX - x; i++)
                        {
                            Map[yi].Insert(0, new ScanCell(yi, (MapMinimumX - 1) - i));
                        }
                    }
                    MapMinimumX = x;
                }
                if (x > MapMaximumX)
                {
                    for (int yi = 0; yi < MapDepth; yi++)
                    {
                        for (int i = 0; i < x - MapMaximumX; i++)
                        {
                            Map[yi].Add(new ScanCell(yi, MapMaximumX + i + 1));
                        }
                    }
                    MapMaximumX += x - MapMaximumX;
                }
                int shiftedX = x - MapMinimumX;
                return Map[y][shiftedX];
            }
            else
            {
                return null;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            //using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(32, 32, 0)))
            using (SolidBrush clayBrush = new SolidBrush(Color.SaddleBrown))
            //using (Pen clayPen = new Pen(Color.SaddleBrown, 1))
            using (SolidBrush waterBrush = new SolidBrush(Color.Blue))
            //using (Pen waterPen = new Pen(Color.SaddleBrown, 1))
            using (SolidBrush flowingWaterBrush = new SolidBrush(Color.LightBlue))
            //using (Pen flowingWaterPen = new Pen(Color.SaddleBrown, 1))
            using (SolidBrush springBrush = new SolidBrush(Color.Yellow))
            //using (Pen springPen = new Pen(Color.SaddleBrown, 1))
            {
                for (int yGrid = 0; yGrid <= MapMaximumY; yGrid++)
                {
                    for (int xGrid = MapMinimumX; xGrid <= MapMaximumX; xGrid++)
                    {
                        ScanCell cell = Map[yGrid][xGrid - MapMinimumX];
                        int y = (cell.X - MapMinimumX) * CellDim + 5;
                        int x = cell.Y * CellDim + 5;

                        if (yGrid < 879)
                        {
                            y += 475;
                        }
                        else
                        {
                            x -= 879 * CellDim;
                        }

                        if (x <= this.Width)
                        {
                            switch (cell.Type)
                            {
                                case ScanCellType.Clay:
                                    //e.Graphics.DrawRectangle(clayPen, x, y, CellDim, CellDim);
                                    e.Graphics.FillRectangle(clayBrush, x, y, CellDim, CellDim);
                                    break;
                                case ScanCellType.Water:
                                    //e.Graphics.DrawRectangle(clayPen, x, y, CellDim, CellDim);
                                    e.Graphics.FillRectangle(waterBrush, x, y, CellDim, CellDim);
                                    break;
                                case ScanCellType.DrySand:
                                    //e.Graphics.DrawRectangle(clayPen, x, y, CellDim, CellDim);
                                    e.Graphics.FillRectangle(flowingWaterBrush, x, y, CellDim, CellDim);
                                    break;
                                case ScanCellType.Spring:
                                    //e.Graphics.DrawRectangle(clayPen, x, y, CellDim, CellDim);
                                    e.Graphics.FillRectangle(springBrush, x, y, CellDim, CellDim);
                                    break;
                                    /*
                                default:
                                    //e.Graphics.DrawRectangle(clayPen, x, y, CellDim, CellDim);
                                    e.Graphics.FillRectangle(bgBrush, x, y, CellDim, CellDim);
                                    break;
                                    */
                            }
                        }
                    }
                }
            }
            using (Pen dividerPen = new Pen(Color.White, 5))
            {
                e.Graphics.DrawLine(dividerPen, 5, 478, 1763, 478);
            }
        }
    }

    public enum ScanCellType { Spring = '+', UntouchedSand = ' ', Clay = '▓', DrySand = '▒', Water = '█' };

    public class ScanCell
    {
        public int Y;
        public int X;

        public ScanCellType Type = ScanCellType.UntouchedSand;

        public ScanCell(int y, int x)
        {
            Y = y;
            X = x;
        }
    }

    public class Vein
    {
        public int XStart;
        public int XEnd;
        public int YStart;
        public int YEnd;

        public Vein(int xStart, int xEnd, int yStart, int yEnd)
        {
            XStart = xStart;
            XEnd = xEnd;
            YStart = yStart;
            YEnd = yEnd;
        }
    }

}
