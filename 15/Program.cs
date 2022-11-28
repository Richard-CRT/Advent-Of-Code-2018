#define OVERRIDE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _15
{

    class Program
    {
        public static GridCell[,] Map;
        public static int MapXSize;
        public static int MapYSize;
        public static int Rounds = 0;
        public static int ElfAttackPower = 4;
        public static int ElfDeaths = 1;

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            MapYSize = inputList.Count;
            MapXSize = inputList[0].Length;

            while (ElfDeaths > 0)
            {
                ElfDeaths = 0;
                Rounds = 0;

                Console.WriteLine("Elf Attack Power: {0}", ElfAttackPower);

                Map = new GridCell[MapYSize, MapXSize];

                int startElves = 0;

                for (int y = 0; y < MapYSize; y++)
                {
                    for (int x = 0; x < MapXSize; x++)
                    {
                        switch (inputList[y][x])
                        {
                            case '#':
                                Map[y, x] = new Wall(x, y);
                                break;
                            case '.':
                                Map[y, x] = new Space(x, y);
                                break;
                            case 'E':
                                Map[y, x] = new Elf(x, y);
                                startElves++;
                                break;
                            case 'G':
                                Map[y, x] = new Goblin(x, y);
                                break;
                        }
                    }
                }

                PrintMap();

                bool combatOver = false;
                while (!combatOver)
                {
                    List<Unit> TurnOrder = new List<Unit>();
                    for (int y = 0; y < MapYSize; y++)
                    {
                        for (int x = 0; x < MapXSize; x++)
                        {
                            if (Map[y, x] is Unit)
                            {
                                Unit unit = Map[y, x] as Unit;
                                TurnOrder.Add(unit);
                            }
                        }
                    }

                    int turnIndex;
                    for (turnIndex = 0; turnIndex < TurnOrder.Count; turnIndex++)
                    {
                        Unit unit = TurnOrder[turnIndex];
                        if (!unit.Dead)
                        {
                            //AoCUtilities.DebugWriteLine("{0} - {1},{2}", unit.ToString(), unit.X, unit.Y);
                            unit.Turn();
                            if (ElfDeaths > 0)
                            {
                                Console.WriteLine("Elf Death!");
                                combatOver = true;
                                break;
                            }

                            int elfCount = 0;
                            int goblinCount = 0;
                            for (int y = 0; y < MapYSize; y++)
                            {
                                for (int x = 0; x < MapXSize; x++)
                                {
                                    if (Map[y, x] is Elf)
                                    {
                                        elfCount++;
                                    }
                                    if (Map[y, x] is Goblin)
                                    {
                                        goblinCount++;
                                    }
                                }
                            }

                            if (elfCount == 0 || goblinCount == 0)
                            {
                                combatOver = true;
                                break;
                            }
                        }
                    }

                    int numberOfTurnsLeftToBeHad = 0;
                    for (int i = turnIndex + 1; i < TurnOrder.Count; i++)
                    {
                        Unit unit = TurnOrder[i];
                        if (!unit.Dead)
                        {
                            numberOfTurnsLeftToBeHad++;
                        }
                    }
                    if (combatOver && numberOfTurnsLeftToBeHad > 0)
                    {
                        break;
                    }

                    Rounds++;
                    //Console.WriteLine(Rounds);
                    PrintMap();
                    //Console.ReadLine();
                }

                if (ElfDeaths == 0)
                {
                    PrintMap();
                    Console.WriteLine("Elf Attack Power was {0}", ElfAttackPower);
                    Console.WriteLine("Combat ends after {0}", Rounds);

                    int sum = 0;
                    for (int y = 0; y < MapYSize; y++)
                    {
                        for (int x = 0; x < MapXSize; x++)
                        {
                            if (Map[y, x] is Unit)
                            {
                                Unit unit = Map[y, x] as Unit;
                                sum += unit.HitPoints;
                            }
                        }
                    }
                    Console.WriteLine("Total remaining hitpoints is {0}", sum);
                    Console.WriteLine("Outcome is {0}", sum * Rounds);
                }
                

                int endElves = 0;
                for (int y = 0; y < MapYSize; y++)
                {
                    for (int x = 0; x < MapXSize; x++)
                    {
                        if (Map[y, x] is Elf)
                        {
                            endElves++;
                        }
                    }
                }
                ElfAttackPower++;
            }

            Console.ReadLine();
        }

        public static void PrintMap()
        {
#if DEBUG || OVERRIDE
            AoCUtilities.DebugClear();

            string toPrint = "";
            toPrint += string.Format("Round {0}\n", Rounds);
            toPrint += string.Format("   ", Rounds);
            for (int x = 0; x < MapXSize; x++)
            {
                toPrint += string.Format("{0}", x / 10);
            }
            toPrint += "\n";
            toPrint += "   ";
            for (int x = 0; x < MapXSize; x++)
            {
                toPrint += string.Format("{0}", x % 10);
            }
            toPrint += "\n";
            for (int y = 0; y < MapYSize; y++)
            {
                toPrint += string.Format("{0} ", y.ToString().PadLeft(2, ' '));
                for (int x = 0; x < MapXSize; x++)
                {
                    toPrint += string.Format("{0}", Map[y, x].ToString());
                }
                toPrint += "  ";
                for (int x = 0; x < MapXSize; x++)
                {
                    if (Map[y, x] is Unit)
                    {
                        Unit unit = Map[y, x] as Unit;
                        toPrint += string.Format("{0}({1}) ", unit, unit.HitPoints);
                    }
                }
                toPrint += "\n";
            }
            toPrint += "\n";
            AoCUtilities.DebugWrite(toPrint);

            /*
            AoCUtilities.DebugWriteLine("Round {0}", Rounds);
            AoCUtilities.DebugWrite("   ");
            for (int x = 0; x < MapXSize; x++)
            {
                AoCUtilities.DebugWrite("{0}", x / 10);
            }
            AoCUtilities.DebugWriteLine();
            AoCUtilities.DebugWrite("   ");
            for (int x = 0; x < MapXSize; x++)
            {
                AoCUtilities.DebugWrite("{0}", x % 10);
            }
            AoCUtilities.DebugWriteLine();
            for (int y = 0; y < MapYSize; y++)
            {
                AoCUtilities.DebugWrite("{0} ", y.ToString().PadLeft(2, ' '));
                for (int x = 0; x < MapXSize; x++)
                {
                    AoCUtilities.DebugWrite("{0}", Map[y, x].ToString());
                }
                AoCUtilities.DebugWrite("  ");
                for (int x = 0; x < MapXSize; x++)
                {
                    if (Map[y, x] is Unit)
                    {
                        Unit unit = Map[y, x] as Unit;
                        AoCUtilities.DebugWrite("{0}({1}) ", unit, unit.HitPoints);
                    }
                }
                AoCUtilities.DebugWriteLine();
            }
            AoCUtilities.DebugWriteLine();
            */
#endif
        }
    }

    public abstract class GridCell
    {
        public int X;
        public int Y;

        public GridCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Wall : GridCell
    {
        public Wall(int x, int y) : base(x, y)
        {

        }

        public override string ToString()
        {
            return "█";
        }
    }

    public class Space : GridCell
    {
        public int Distance;

        public Space(int x, int y) : base(x, y)
        {

        }

        public override string ToString()
        {
            return " ";
        }
    }

    public abstract class Unit : GridCell
    {
        private int hitPoints = 0;
        public int HitPoints
        {
            get
            {
                return hitPoints;
            }
            set
            {
                hitPoints = value;
                if (hitPoints <= 0)
                {
                    if (this is Elf)
                    {
                        Program.ElfDeaths++;
                    }
                    Dead = true;
                    Program.Map[this.Y, this.X] = new Space(this.X, this.Y);
                }
            }
        }
        public bool Dead = false;
        public int AttackPower;

        public Unit(int x, int y) : base(x, y)
        {

        }

        public List<Unit> GetAdjacentTargets()
        {
            List<Unit> adjacentTargets = new List<Unit>();

            if (this.Y > 0)
            {
                if (IsEnemy(this.X, this.Y - 1))
                {
                    adjacentTargets.Add(Program.Map[this.Y - 1, this.X] as Unit);
                }
            }
            if (this.Y < Program.MapYSize - 1)
            {
                if (IsEnemy(this.X, this.Y + 1))
                {
                    adjacentTargets.Add(Program.Map[this.Y + 1, this.X] as Unit);
                }
            }
            if (this.X > 0)
            {
                if (IsEnemy(this.X - 1, this.Y))
                {
                    adjacentTargets.Add(Program.Map[this.Y, this.X - 1] as Unit);
                }
            }
            if (this.X < Program.MapXSize - 1)
            {
                if (IsEnemy(this.X + 1, this.Y))
                {
                    adjacentTargets.Add(Program.Map[this.Y, this.X + 1] as Unit);
                }
            }

            return adjacentTargets;
        }

        public void Turn()
        {
            List<Unit> adjacentTargets = GetAdjacentTargets();
            if (adjacentTargets.Count == 0)
            {
                Move();
                adjacentTargets = GetAdjacentTargets();
            }

            if (adjacentTargets.Count > 0)
            {
                // Try attack
                //AoCUtilities.DebugWriteLine("Attack");
                adjacentTargets = adjacentTargets.OrderBy(target => target.HitPoints).ThenBy(target => target.Y).ThenBy(target => target.X).ToList();
                Unit adjacentTarget = adjacentTargets[0];
                adjacentTarget.HitPoints -= this.AttackPower;
            }
        }

        public void Move()
        {
            //AoCUtilities.DebugWriteLine("Start Move");

            List<Unit> targets = this.GetTargets();

            /*
            AoCUtilities.DebugWriteLine("Targets:");
            foreach (Unit target in targets)
            {
                AoCUtilities.DebugWriteLine("{0},{1}",target.X, target.Y);
            }
            */

            List<Space> freeSpaces = new List<Space>();
            foreach (Unit target in targets)
            {
                if (target.Y > 0 && Program.Map[target.Y - 1, target.X] is Space)
                {
                    Space space = Program.Map[target.Y - 1, target.X] as Space;
                    if (!freeSpaces.Contains(space))
                    {
                        freeSpaces.Add(space);
                    }
                }

                if (target.X > 0 && Program.Map[target.Y, target.X - 1] is Space)
                {
                    Space space = Program.Map[target.Y, target.X - 1] as Space;
                    if (!freeSpaces.Contains(space))
                    {
                        freeSpaces.Add(space);
                    }
                }

                if (target.X < Program.MapXSize - 1 && Program.Map[target.Y, target.X + 1] is Space)
                {
                    Space space = Program.Map[target.Y, target.X + 1] as Space;
                    if (!freeSpaces.Contains(space))
                    {
                        freeSpaces.Add(space);
                    }
                }

                if (target.Y < Program.MapYSize - 1 && Program.Map[target.Y + 1, target.X] is Space)
                {
                    Space space = Program.Map[target.Y + 1, target.X] as Space;
                    if (!freeSpaces.Contains(space))
                    {
                        freeSpaces.Add(space);
                    }
                }
            }

            /*
            AoCUtilities.DebugWriteLine("In-Range Squares:");
            foreach (Space freeSpace in freeSpaces)
            {
                AoCUtilities.DebugWriteLine("{0},{1}", freeSpace.X, freeSpace.Y);
            }
            */

            DijkstraCell[,] distanceMapFromUnit = Dijkstra(this.X, this.Y);

            /*
            AoCUtilities.DebugWriteLine("Dijkstra's from Unit:");
            for (int y = 0; y < Program.MapYSize; y++)
            {
                for (int x = 0; x < Program.MapXSize; x++)
                {
                    AoCUtilities.DebugWrite(distanceMapFromUnit[y, x].Distance.ToString().PadLeft(3, ' '));
                }
                AoCUtilities.DebugWriteLine();
            }
            */

            List<Space> reachableFreeSpaces = new List<Space>();

            for (int y = 0; y < Program.MapYSize; y++)
            {
                for (int x = 0; x < Program.MapXSize; x++)
                {
                    if (!distanceMapFromUnit[y, x].Distance.Infinity)
                    {
                        foreach (Space freeSpace in freeSpaces)
                        {
                            if (freeSpace.Y == y && freeSpace.X == x)
                            {
                                freeSpace.Distance = distanceMapFromUnit[y, x].Distance.Value;
                                reachableFreeSpaces.Add(freeSpace);
                                break;
                            }
                        }
                    }
                }
            }

            reachableFreeSpaces = reachableFreeSpaces.OrderBy(reachableFreeSpace => reachableFreeSpace.Distance).ThenBy(reachableFreeSpace => reachableFreeSpace.Y).ThenBy(reachableFreeSpace => reachableFreeSpace.X).ToList();

            /*
            AoCUtilities.DebugWriteLine("Reachable In-Range Squares:");
            foreach (Space reachableFreeSpace in reachableFreeSpaces)
            {
                AoCUtilities.DebugWriteLine("{0},{1} - Distance: {2}", reachableFreeSpace.X, reachableFreeSpace.Y, reachableFreeSpace.Distance);
            }
            */

            if (reachableFreeSpaces.Count > 0)
            {
                /*
                AoCUtilities.DebugWriteLine("Chosen In-Range Square");
                AoCUtilities.DebugWriteLine("{0},{1} - Distance: {2}", reachableFreeSpaces[0].X, reachableFreeSpaces[0].Y, reachableFreeSpaces[0].Distance);
                */

                DijkstraCell[,] distanceMapFromChosenInRangeSquare = Dijkstra(reachableFreeSpaces[0].X, reachableFreeSpaces[0].Y);

                /*
                AoCUtilities.DebugWriteLine("Dijkstra's from Chosen In-Range Square:");
                for (int y = 0; y < Program.MapYSize; y++)
                {
                    for (int x = 0; x < Program.MapXSize; x++)
                    {
                        AoCUtilities.DebugWrite(distanceMapFromChosenInRangeSquare[y, x].Distance.ToString().PadLeft(3, ' '));
                    }
                    AoCUtilities.DebugWriteLine();
                }
                */

                // order of these checks is important
                DijkstraCell bestMove = null;
                if (this.Y > 0)
                {
                    if (bestMove == null || distanceMapFromChosenInRangeSquare[this.Y - 1, this.X].Distance < bestMove.Distance)
                    {
                        bestMove = distanceMapFromChosenInRangeSquare[this.Y - 1, this.X];
                    }
                }
                if (this.X > 0)
                {
                    if (bestMove == null || distanceMapFromChosenInRangeSquare[this.Y, this.X - 1].Distance < bestMove.Distance)
                    {
                        bestMove = distanceMapFromChosenInRangeSquare[this.Y, this.X - 1];
                    }
                }
                if (this.X < Program.MapXSize - 1)
                {
                    if (bestMove == null || distanceMapFromChosenInRangeSquare[this.Y, this.X + 1].Distance < bestMove.Distance)
                    {
                        bestMove = distanceMapFromChosenInRangeSquare[this.Y, this.X + 1];
                    }
                }
                if (this.Y < Program.MapYSize - 1)
                {
                    if (bestMove == null || distanceMapFromChosenInRangeSquare[this.Y + 1, this.X].Distance < bestMove.Distance)
                    {
                        bestMove = distanceMapFromChosenInRangeSquare[this.Y + 1, this.X];
                    }
                }
                // bestMove should never be null

                /*
                AoCUtilities.DebugWriteLine("Cell to move to:");
                AoCUtilities.DebugWriteLine("{0},{1}", bestMove.X, bestMove.Y);
                */

                Program.Map[this.Y, this.X] = new Space(this.X, this.Y);
                this.X = bestMove.X;
                this.Y = bestMove.Y;
                Program.Map[this.Y, this.X] = this;
                //Program.PrintMap();
            }

            //AoCUtilities.DebugWriteLine("End Move");
        }

        public DijkstraCell[,] Dijkstra(int startX, int startY)
        {
            // Dijkstra's

            List<DijkstraCell> unvisitedPoints = new List<DijkstraCell>();
            DijkstraCell[,] distanceMap = new DijkstraCell[Program.MapYSize, Program.MapXSize];
            for (int y = 0; y < Program.MapYSize; y++)
            {
                for (int x = 0; x < Program.MapXSize; x++)
                {
                    distanceMap[y, x] = new DijkstraCell(x, y);
                    unvisitedPoints.Add(distanceMap[y, x]);
                }
            }

            distanceMap[startY, startX].Distance = new InfinityInt(0);

            while (unvisitedPoints.Count > 0)
            {
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
                    DijkstraCell dijkstraCellAbove = distanceMap[nearestVertex.Y - 1, nearestVertex.X];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellAbove);
                }
                if (nearestVertex.Y < Program.MapYSize - 1)
                {
                    DijkstraCell dijkstraCellBelow = distanceMap[nearestVertex.Y + 1, nearestVertex.X];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellBelow);
                }
                if (nearestVertex.X > 0)
                {
                    DijkstraCell dijkstraCellLeft = distanceMap[nearestVertex.Y, nearestVertex.X - 1];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellLeft);
                }
                if (nearestVertex.X < Program.MapXSize - 1)
                {
                    DijkstraCell dijkstraCellRight = distanceMap[nearestVertex.Y, nearestVertex.X + 1];
                    UpdateCell(distanceMap, nearestVertex, dijkstraCellRight);
                }
            }

            return distanceMap;
        }

        public void UpdateCell(DijkstraCell[,] distanceMap, DijkstraCell currentCell, DijkstraCell relativeCell)
        {
            InfinityInt edgeDistance;
            if (Program.Map[relativeCell.Y, relativeCell.X] is Space)
            {
                edgeDistance = new InfinityInt(1); // 1 as 1 step to move adjacently
            }
            else
            {
                edgeDistance = new InfinityInt(); // infinity as can only move into free space
            }

            InfinityInt trialTotalDistance = currentCell.Distance + edgeDistance;
            if (trialTotalDistance < relativeCell.Distance)
            {
                relativeCell.Distance = trialTotalDistance;
            }
        }

        public List<Unit> GetTargets()
        {
            List<Unit> targets = new List<Unit>();
            for (int y = 0; y < Program.MapYSize; y++)
            {
                for (int x = 0; x < Program.MapXSize; x++)
                {
                    if (IsEnemy(x, y))
                    {
                        targets.Add(Program.Map[y, x] as Unit);
                    }
                }
            }
            return targets;
        }

        public abstract bool IsEnemy(int x, int y);
    }

    public class Elf : Unit
    {
        public Elf(int x, int y) : base(x, y)
        {
            HitPoints = 200;
            AttackPower = Program.ElfAttackPower;
        }

        public override string ToString()
        {
            return "░";
        }

        public override bool IsEnemy(int x, int y)
        {
            return Program.Map[y, x] is Goblin;
        }
    }

    public class Goblin : Unit
    {
        public Goblin(int x, int y) : base(x, y)
        {
            HitPoints = 200;
            AttackPower = 3;
        }

        public override string ToString()
        {
            return "▓";
        }

        public override bool IsEnemy(int x, int y)
        {
            return Program.Map[y, x] is Elf;
        }
    }

    public class DijkstraCell
    {
        public int X;
        public int Y;
        public InfinityInt Distance = new InfinityInt();

        public DijkstraCell(int x, int y)
        {
            X = x;
            Y = y;
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

        public override bool Equals(object obj)
        {
            return this == (obj as InfinityInt);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator !=(InfinityInt lhs, InfinityInt rhs)
        {
            return !(lhs == rhs);
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
