using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _22
{
    public enum RegionType { Rocky = '.', Wet = '=', Narrow = '|' };
    public enum Tools { ClimbingGear = 0, Torch = 1, Neither = 2 };
    class Program
    {
        public static Dictionary<RegionType, int> RiskLevel = new Dictionary<RegionType, int>
        {
            { RegionType.Rocky, 0 },
            { RegionType.Wet, 1 },
            { RegionType.Narrow, 2 },
        };

        public static List<List<int>> GeologicalIndexMap = new List<List<int>>();
        public static List<List<int>> ErosionLevelMap = new List<List<int>>();
        public static List<List<RegionType>> RegionTypeMap = new List<List<RegionType>>();
        public static int CaveDepth;
        public static int TargetX;
        public static int TargetY;
        public static int MaximumX;
        public static int MaximumY;

        static void Main(string[] args)
        {
            Console.SetBufferSize(500, 20000);

            List<string> inputList = AoCUtilities.GetInput();

            CaveDepth = Int32.Parse(inputList[0].Substring(7));
            string[] targetCoordinateParts = inputList[1].Substring(8).Split(',');
            TargetX = Int32.Parse(targetCoordinateParts[0]);
            TargetY = Int32.Parse(targetCoordinateParts[1]);
            MaximumX = TargetX + 50;
            MaximumY = TargetY + 50;

            for (int y = 0; y <= MaximumY; y++)
            {
                List<int> geologicalIndexRow = new List<int>();
                List<int> erosionLevelRow = new List<int>();
                List<RegionType> regionTypeRow = new List<RegionType>();
                for (int x = 0; x <= MaximumX; x++)
                {
                    int index = 0;
                    if ((x == 0 && y == 0) || (x == TargetX && y == TargetY))
                    {
                        index = 0;
                    }
                    else if (x == 0)
                    {
                        index = y * 48271;
                    }
                    else if (y == 0)
                    {
                        index = x * 16807;
                    }
                    else
                    {
                        index = erosionLevelRow[x - 1] * ErosionLevelMap[y - 1][x];
                    }
                    geologicalIndexRow.Add(index);
                    int erosionLevel = (index + CaveDepth) % 20183;
                    erosionLevelRow.Add(erosionLevel);

                    switch (erosionLevel % 3)
                    {
                        case 0:
                            regionTypeRow.Add(RegionType.Rocky);
                            break;
                        case 1:
                            regionTypeRow.Add(RegionType.Wet);
                            break;
                        case 2:
                            regionTypeRow.Add(RegionType.Narrow);
                            break;
                    }
                }
                GeologicalIndexMap.Add(geologicalIndexRow);
                ErosionLevelMap.Add(erosionLevelRow);
                RegionTypeMap.Add(regionTypeRow);
            }

            PrintMap();

            int risk = 0;
            for (int y = 0; y <= TargetY; y++)
            {
                for (int x = 0; x <= TargetX; x++)
                {
                    risk += RiskLevel[RegionTypeMap[y][x]];
                }
            }
            Console.WriteLine("Risk Level: {0}", risk);

            DijkstraCell[,,] dijkstraMap1 = Dijkstra(0, 0, Tools.Torch);
            /*
            Console.WriteLine("Dijkstra's 1 from Start:");
            for (int y = 200; y < 230; y++)
            {
                for (int x = 20; x <= 41; x++)
                {
                    Console.Write(dijkstraMap1[y, x].Distance.ToString().PadLeft(5, ' '));
                }
                Console.WriteLine();
            }
            */
            Console.WriteLine("Time Taken to Reach Target: {0}", dijkstraMap1[TargetY, TargetX, (int)Tools.Torch].Distance);

            /*
            for (int y = 0; y <= 721; y++)
            {
                for (int x = 0; x <= 41; x++)
                {
                    if (dijkstraMap1[y,x].Distance > dijkstraMap2[y,x].Distance)
                    {
                        Console.WriteLine("Bigger Map Longer distance at {0},{1}", x, y);
                    }
                }
            }
            */

            Console.ReadLine();
        }

        public static DijkstraCell[,,] Dijkstra(int startX, int startY, Tools startTool)
        {
            // Dijkstra's

            List<DijkstraCell> unvisitedPoints = new List<DijkstraCell>();
            DijkstraCell[,,] distanceMap = new DijkstraCell[MaximumY + 1, MaximumX + 1, 3];
            for (int y = 0; y <= MaximumY; y++)
            {
                for (int x = 0; x <= MaximumX; x++)
                {
                    DijkstraCell newCell;

                    newCell = new DijkstraCell(x, y, Tools.ClimbingGear);
                    distanceMap[y, x, (int)Tools.ClimbingGear] = newCell;
                    unvisitedPoints.Add(newCell);

                    newCell = new DijkstraCell(x, y, Tools.Torch);
                    distanceMap[y, x, (int)Tools.Torch] = newCell;
                    unvisitedPoints.Add(newCell);

                    newCell = new DijkstraCell(x, y, Tools.Neither);
                    distanceMap[y, x, (int)Tools.Neither] = newCell;
                    unvisitedPoints.Add(newCell);
                }
            }

            distanceMap[startY, startX, (int)startTool].Distance = new InfinityInt(0);

            while (unvisitedPoints.Count > 0)
            {
                int unvisitedPointsLen = unvisitedPoints.Count;
                if (unvisitedPointsLen % 1000 == 0)
                {
                    Console.WriteLine("{0} unvisited points remaining", unvisitedPointsLen);
                }
                // Pick shortest distance vertex
                DijkstraCell nearestVertex = null;
                foreach (DijkstraCell unvisitedCell in unvisitedPoints)
                {
                    if (nearestVertex == null || unvisitedCell.Distance < nearestVertex.Distance)
                    {
                        nearestVertex = unvisitedCell;
                    }
                }

                // Visit shortest distance vertex
                unvisitedPoints.Remove(nearestVertex);

                // Update adjacent cell's total distances
                if (nearestVertex.Y > 0)
                {
                    DijkstraCell dijkstraCellAbove = distanceMap[nearestVertex.Y - 1, nearestVertex.X, (int)nearestVertex.Tool];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellAbove);
                }
                if (nearestVertex.Y < MaximumY)
                {
                    DijkstraCell dijkstraCellBelow = distanceMap[nearestVertex.Y + 1, nearestVertex.X, (int)nearestVertex.Tool];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellBelow);
                }
                if (nearestVertex.X > 0)
                {
                    DijkstraCell dijkstraCellLeft = distanceMap[nearestVertex.Y, nearestVertex.X - 1, (int)nearestVertex.Tool];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellLeft);
                }
                if (nearestVertex.X < MaximumX)
                {
                    DijkstraCell dijkstraCellRight = distanceMap[nearestVertex.Y, nearestVertex.X + 1, (int)nearestVertex.Tool];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellRight);
                }
                if (nearestVertex.Tool != Tools.ClimbingGear)
                {
                    DijkstraCell dijkstraCellClimbingGear = distanceMap[nearestVertex.Y, nearestVertex.X, (int)Tools.ClimbingGear];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellClimbingGear);
                }
                if (nearestVertex.Tool != Tools.Torch)
                {
                    DijkstraCell dijkstraCellTorch = distanceMap[nearestVertex.Y, nearestVertex.X, (int)Tools.Torch];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellTorch);
                }
                if (nearestVertex.Tool != Tools.Neither)
                {
                    DijkstraCell dijkstraCellNeither = distanceMap[nearestVertex.Y, nearestVertex.X, (int)Tools.Neither];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellNeither);
                }
            }

            return distanceMap;
        }

        public static void UpdateCell(DijkstraCell[,,] distanceMap, DijkstraCell currentCell, DijkstraCell relativeCell)
        {
            InfinityInt edgeCost;

            switch (RegionTypeMap[currentCell.Y][currentCell.X])
            {
                case RegionType.Rocky:
                    {
                        if (currentCell.Tool != relativeCell.Tool)
                        {
                            // changing tool
                            switch (relativeCell.Tool)
                            {
                                case Tools.ClimbingGear:
                                    edgeCost = new InfinityInt(7);
                                    break;
                                case Tools.Torch:
                                    edgeCost = new InfinityInt(7);
                                    break;
                                case Tools.Neither:
                                    edgeCost = new InfinityInt();
                                    break;
                                default:
                                    // should never be here
                                    throw new NotImplementedException();
                            }
                        }
                        else
                        {
                            // changing position
                            switch (RegionTypeMap[relativeCell.Y][relativeCell.X])
                            {
                                // Rocky to Rocky
                                case RegionType.Rocky:
                                    edgeCost = new InfinityInt(1);
                                    break;
                                // Rocky to Wet
                                case RegionType.Wet:
                                    if (currentCell.Tool == Tools.ClimbingGear)
                                        edgeCost = new InfinityInt(1);
                                    else
                                        edgeCost = new InfinityInt();
                                    break;
                                // Rocky to Narrow
                                case RegionType.Narrow:
                                    if (currentCell.Tool == Tools.Torch)
                                        edgeCost = new InfinityInt(1);
                                    else
                                        edgeCost = new InfinityInt();
                                    break;
                                default:
                                    // should never be here
                                    throw new NotImplementedException();
                            }
                        }
                    }
                    break;
                case RegionType.Wet:
                    {
                        if (currentCell.Tool != relativeCell.Tool)
                        {
                            // changing tool
                            switch (relativeCell.Tool)
                            {
                                case Tools.ClimbingGear:
                                    edgeCost = new InfinityInt(7);
                                    break;
                                case Tools.Torch:
                                    edgeCost = new InfinityInt();
                                    break;
                                case Tools.Neither:
                                    edgeCost = new InfinityInt(7);
                                    break;
                                default:
                                    // should never be here
                                    throw new NotImplementedException();
                            }
                        }
                        else
                        {
                            // changing position
                            switch (RegionTypeMap[relativeCell.Y][relativeCell.X])
                            {
                                // Wet to Rocky
                                case RegionType.Rocky:
                                    if (currentCell.Tool == Tools.ClimbingGear)
                                        edgeCost = new InfinityInt(1);
                                    else
                                        edgeCost = new InfinityInt();
                                    break;
                                // Wet to Wet
                                case RegionType.Wet:
                                    edgeCost = new InfinityInt(1);
                                    break;
                                // Wet to Narrow
                                case RegionType.Narrow:
                                    if (currentCell.Tool == Tools.Neither)
                                        edgeCost = new InfinityInt(1);
                                    else
                                        edgeCost = new InfinityInt();
                                    break;
                                default:
                                    // should never be here
                                    throw new NotImplementedException();
                            }
                        }
                    }
                    break;
                case RegionType.Narrow:
                    {
                        if (currentCell.Tool != relativeCell.Tool)
                        {
                            // changing tool
                            switch (relativeCell.Tool)
                            {
                                case Tools.ClimbingGear:
                                    edgeCost = new InfinityInt();
                                    break;
                                case Tools.Torch:
                                    edgeCost = new InfinityInt(7);
                                    break;
                                case Tools.Neither:
                                    edgeCost = new InfinityInt(7);
                                    break;
                                default:
                                    // should never be here
                                    throw new NotImplementedException();
                            }
                        }
                        else
                        {
                            // changing position
                            switch (RegionTypeMap[relativeCell.Y][relativeCell.X])
                            {
                                // Narrow to Rocky
                                case RegionType.Rocky:
                                    if (currentCell.Tool == Tools.Torch)
                                        edgeCost = new InfinityInt(1);
                                    else
                                        edgeCost = new InfinityInt();
                                    break;
                                // Narrow to Wet
                                case RegionType.Wet:
                                    if (currentCell.Tool == Tools.Neither)
                                        edgeCost = new InfinityInt(1);
                                    else
                                        edgeCost = new InfinityInt();
                                    break;
                                // Narrow to Narrow
                                case RegionType.Narrow:
                                    edgeCost = new InfinityInt(1);
                                    break;
                                default:
                                    // should never be here
                                    throw new NotImplementedException();
                            }
                        }
                    }
                    break;
                default:
                    // should never be here
                    throw new NotImplementedException();
            }

            InfinityInt trialTotalDistance = currentCell.Distance + edgeCost;
            if (trialTotalDistance < relativeCell.Distance)
            {
                relativeCell.Distance = trialTotalDistance;
            }
        }

        static void PrintMap()
        {
#if DEBUG
            for (int y = 0; y <= MaximumY; y++)
            {
                for (int x = 0; x <= MaximumX; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        AoCUtilities.DebugWrite("M");
                    }
                    else if (x == TargetX && y == TargetY)
                    {
                        AoCUtilities.DebugWrite("T");
                    }
                    else
                    {
                        AoCUtilities.DebugWrite("{0}", (char)RegionTypeMap[y][x]);
                    }
                }
                AoCUtilities.DebugWriteLine();
            }
            AoCUtilities.DebugWriteLine();
#endif
        }
    }

    public class DijkstraCell
    {
        public int X;
        public int Y;
        public Tools Tool;
        public InfinityInt Distance = new InfinityInt();

        public DijkstraCell(int x, int y, Tools tool)
        {
            X = x;
            Y = y;
            Tool = tool;
        }
    }

    public class InfinityInt
    {
        public bool Infinity;
        public int Value;

        public InfinityInt(int value)
        {
            Value = value;
            Infinity = false;
        }

        public InfinityInt()
        {
            Value = 0;
            Infinity = true;
        }

        public static bool operator ==(InfinityInt lhs, InfinityInt rhs)
        {
            if (lhs.Infinity && rhs.Infinity)
            {
                return true;
            }
            else if (lhs.Infinity || rhs.Infinity)
            {
                return false;
            }
            else
            {
                return lhs.Value == rhs.Value;
            }
        }

        public static bool operator !=(InfinityInt lhs, InfinityInt rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return this == (obj as InfinityInt);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static InfinityInt operator +(InfinityInt lhs, InfinityInt rhs)
        {
            if (lhs.Infinity || rhs.Infinity)
            {
                return new InfinityInt();
            }
            else
            {
                return new InfinityInt(lhs.Value + rhs.Value);
            }
        }

        public static bool operator <(InfinityInt lhs, InfinityInt rhs)
        {
            if (lhs.Infinity)
            {
                return false;
            }
            else
            {
                if (rhs.Infinity || lhs.Value < rhs.Value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool operator >(InfinityInt lhs, InfinityInt rhs)
        {
            return rhs < lhs;
        }

        public override string ToString()
        {
            if (Infinity)
            {
                return "I";
            }
            else
            {
                return Value.ToString();
            }
        }
    }
}
