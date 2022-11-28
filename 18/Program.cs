using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _18
{
    public enum CellType { Clearing = ' ', Wooded = '▒', Lumberyard = '█' };
    class Program
    {
        public static int MapXSize = 0;
        public static int MapYSize = 0;
        public static CellType[,] Map;

        static void Main(string[] args)
        {
            Console.ReadLine();
            List<string> inputList = AoCUtilities.GetInput();

            MapXSize = inputList[0].Length;
            MapYSize = inputList.Count;

            Map = new CellType[MapYSize, MapXSize];

            for (int y = 0; y < MapYSize; y++)
            {
                for (int x = 0; x < MapXSize; x++)
                {
                    switch (inputList[y][x])
                    {
                        case '.':
                            Map[y, x] = CellType.Clearing;
                            break;
                        case '|':
                            Map[y, x] = CellType.Wooded;
                            break;
                        case '#':
                            Map[y, x] = CellType.Lumberyard;
                            break;
                    }
                }
            }

            PrintMap();


            // resource value repeats every 28th minute
            // so once at 1,000th-ish generation skip to 1,000,000,000

            int skip28Count = (1000000000 - 500) / 28; // int division
            int skipCount = skip28Count * 28;
            int skipThreshold = 1000000000 - skipCount;

            int clearings = 0;
            int woodedAreas = 0;
            int lumberyards = 0;

            for (int minute = 1; minute <= 1000000000; minute++)
            {
                clearings = 0;
                woodedAreas = 0;
                lumberyards = 0;

                CellType[,] newMap = new CellType[MapYSize, MapXSize];
                for (int y = 0; y < MapYSize; y++)
                {
                    for (int x = 0; x < MapXSize; x++)
                    {
                        List<CellType> adjacentCells = new List<CellType>();

                        if (x > 0 && y > 0)
                        {
                            adjacentCells.Add(Map[y - 1, x - 1]);
                        }
                        if (y > 0)
                        {
                            adjacentCells.Add(Map[y - 1, x]);
                        }
                        if (x < MapXSize - 1 && y > 0)
                        {
                            adjacentCells.Add(Map[y - 1, x + 1]);
                        }
                        if (x < MapXSize - 1)
                        {
                            adjacentCells.Add(Map[y, x + 1]);
                        }
                        if (x < MapXSize - 1 && y < MapYSize - 1)
                        {
                            adjacentCells.Add(Map[y + 1, x + 1]);
                        }
                        if (y < MapYSize - 1)
                        {
                            adjacentCells.Add(Map[y + 1, x]);
                        }
                        if (x > 0 && y < MapYSize - 1)
                        {
                            adjacentCells.Add(Map[y + 1, x - 1]);
                        }
                        if (x > 0)
                        {
                            adjacentCells.Add(Map[y, x - 1]);
                        }

                        int adjacentWooded = 0;
                        int adjacentClearing = 0;
                        int adjacentLumberyard = 0;
                        foreach (CellType adjacentCell in adjacentCells)
                        {
                            switch (adjacentCell)
                            {
                                case CellType.Clearing:
                                    adjacentClearing++;
                                    break;
                                case CellType.Wooded:
                                    adjacentWooded++;
                                    break;
                                case CellType.Lumberyard:
                                    adjacentLumberyard++;
                                    break;
                            }
                        }

                        newMap[y, x] = Map[y, x];
                        switch (Map[y, x])
                        {
                            case CellType.Clearing:
                                if (adjacentWooded >= 3)
                                {
                                    woodedAreas++;
                                    newMap[y, x] = CellType.Wooded;
                                }
                                else
                                {
                                    clearings++;
                                }
                                break;
                            case CellType.Wooded:
                                if (adjacentLumberyard >= 3)
                                {
                                    lumberyards++;
                                    newMap[y, x] = CellType.Lumberyard;
                                }
                                else
                                {
                                    woodedAreas++;
                                }
                                break;
                            case CellType.Lumberyard:
                                if (adjacentLumberyard < 1 || adjacentWooded < 1)
                                {
                                    clearings++;
                                    newMap[y, x] = CellType.Clearing;
                                }
                                else
                                {
                                    lumberyards++;
                                }
                                break;
                        }
                    }
                }

                Map = newMap;
                PrintMap();
                Console.WriteLine("    Minute {0}", minute);
                AoCUtilities.DebugReadLine();

                if (minute == skipThreshold - 1)
                {
                    minute = 1000000000 - 1; // -1 to allow for loop to increment
                }
            }

            Console.WriteLine("    Pattern repeated every 28 minutes so can skip straight to minute 1,000,000,000");
            Console.WriteLine("    {0} Clearings", clearings);
            Console.WriteLine("    {0} Wooded Areas", woodedAreas);
            Console.WriteLine("    {0} Lumberyards", lumberyards);
            Console.WriteLine("    Resource Value: {0}", woodedAreas * lumberyards);

            Console.ReadLine();
        }

        static void PrintMap()
        {
#if DEBUG
            string toPrint = "";
            toPrint += "\n";
            toPrint += "\n";
            for (int y = 0; y < MapYSize; y++)
            {
                toPrint += "    ";
                for (int x = 0; x < MapXSize; x++)
                {
                    toPrint += string.Format("{0}{0}", (char)Map[y, x]);
                }
                toPrint += "\n";
            }
            AoCUtilities.DebugClear();
            AoCUtilities.DebugWrite(toPrint);
#endif
        }
    }
}
