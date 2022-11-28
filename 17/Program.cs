#define OVERRIDE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _17;
using AdventOfCodeUtilities;

namespace _17
{
    public static class Program
    {
        public static int MapMinimumX;
        public static int MapMinimumY;
        public static int MapMaximumX;
        public static int MapMaximumY;
        public static int MapDepth;

        public static int activeMonitorStart = 0;

        public static List<ScanCell>[] Map;

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            Console.SetBufferSize(500, 2000);

            List<Vein> veins = new List<Vein>();

            MapMinimumX = int.MaxValue;
            MapMinimumY = int.MaxValue;
            MapMaximumX = -1;
            MapMaximumY = -1;

            for (int i = 0; i < inputList.Count; i++)
            {
                string[] veinStringParts = inputList[i].Split(' ');
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

            PrintMap();
            Console.ReadLine();

            List<ScanCell> sourceCells = new List<ScanCell> { GetMapScanCell(0, 500) };

            while (sourceCells.Count > 0)
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

                        //PrintMap();
                        //Console.ReadLine();
                    }

                    //PrintMap();
                    //Console.ReadLine();
                }

                //PrintMap();
                //Console.ReadLine();

                sourceCells.RemoveAt(0);
            }

            //PrintMap();
            Console.WriteLine("Done");

            int cellsWaterReached = 0;
            int restingWater = 0;
            for (int y = MapMinimumY; y <= MapMaximumY; y++)
            {
                for (int x = MapMinimumX; x <= MapMaximumX; x++)
                {
                    ScanCell cell = GetMapScanCell(y, x);
                    if (cell.Type == ScanCellType.DrySand)
                    {
                        cellsWaterReached++;
                    }
                    else if (cell.Type == ScanCellType.Water)
                    {
                        restingWater++;
                        cellsWaterReached++;
                    }
                }
            }

            Console.WriteLine("Water has reached {0} cells", cellsWaterReached);
            Console.WriteLine("Water cells that will be left as t -> (infinity) cells: {0}", restingWater);
            //PrintMap();
            Console.ReadLine();
        }

        public static ScanCell GetMapScanCell(int y, int x)
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

        public static void PrintMap()
        {
#if DEBUG || OVERRIDE

            string toPrint = "";

            toPrint += "   ";
            for (int x = MapMinimumX; x <= MapMaximumX; x++)
            {
                toPrint += string.Format("{0}", x / 100);
            }
            toPrint += "\n";
            toPrint += "   ";
            for (int x = MapMinimumX; x <= MapMaximumX; x++)
            {
                toPrint += string.Format("{0}", (x - ((x / 100) * 100)) / 10);
            }
            toPrint += "\n";
            toPrint += "   ";
            for (int x = MapMinimumX; x <= MapMaximumX; x++)
            {
                toPrint += string.Format("{0}", (x - ((x / 100) * 100)) % 10);
            }
            toPrint += "\n";

            for (int y = Program.activeMonitorStart; y <= Program.activeMonitorStart + 50; y++)
            {
                toPrint += string.Format("{0} ", y.ToString().PadLeft(4, ' '));
                for (int x = MapMinimumX; x <= MapMaximumX; x++)
                {
                    toPrint += string.Format("{0}", (char)GetMapScanCell(y, x).Type);
                }
                toPrint += "\n";
            }
            toPrint += "\n";
            Console.Clear();
            Console.Write(toPrint);

            /*
            for (int x = MapMinimumX; x <= MapMaximumX; x++)
            {
                AoCUtilities.DebugWrite("{0}", x / 100);
            }
            AoCUtilities.DebugWriteLine();
            AoCUtilities.DebugWrite("   ");
            for (int x = MapMinimumX; x <= MapMaximumX; x++)
            {
                AoCUtilities.DebugWrite("{0}", (x - ((x / 100) * 100)) / 10);
            }
            AoCUtilities.DebugWriteLine();
            AoCUtilities.DebugWrite("   ");
            for (int x = MapMinimumX; x <= MapMaximumX; x++)
            {
                AoCUtilities.DebugWrite("{0}", (x - ((x / 100) * 100)) % 10);
            }
            AoCUtilities.DebugWriteLine();

            for (int y = 0; y <= MapMaximumY; y++)
            {
                AoCUtilities.DebugWrite("{0} ", y.ToString().PadLeft(4, ' '));
                for (int x = MapMinimumX; x <= MapMaximumX; x++)
                {
                    AoCUtilities.DebugWrite("{0}", (char)GetMapScanCell(y, x).Type);
                }
                AoCUtilities.DebugWriteLine();
            }
            AoCUtilities.DebugWrite("   ");
            AoCUtilities.DebugWriteLine();
            */
#endif
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
