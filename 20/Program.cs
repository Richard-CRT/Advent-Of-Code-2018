using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _20
{
    class Program
    {
        public static int MinimumX = int.MaxValue;
        public static int MinimumY = int.MaxValue;
        public static int MaximumX = int.MinValue;
        public static int MaximumY = int.MinValue;

        public static Room FurthestRoom = null;
        public static int HowMany1000Rooms = 0;

        public static Dictionary<string, Room> AllRooms = new Dictionary<string, Room>();

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            string pattern = inputList[0].Substring(1, inputList[0].Length - 2);

            Room startRoom = new Room(0, 0);
            AllRooms["0,0"] = startRoom;

            ExplorePatternSegment(pattern, startRoom);

            CalculateDistances(startRoom);
            CalculateStats();
            PrintMap();
            Console.ReadLine();
        }

        static void CalculateStats()
        {
            HowMany1000Rooms = 0;
            foreach (KeyValuePair<string, Room> keyValuePair in AllRooms)
            {
                if (keyValuePair.Value.X < MinimumX)
                {
                    MinimumX = keyValuePair.Value.X;
                }
                if (keyValuePair.Value.Y < MinimumY)
                {
                    MinimumY = keyValuePair.Value.Y;
                }
                if (keyValuePair.Value.X > MaximumX)
                {
                    MaximumX = keyValuePair.Value.X;
                }
                if (keyValuePair.Value.Y > MaximumY)
                {
                    MaximumY = keyValuePair.Value.Y;
                }
                if (FurthestRoom == null || keyValuePair.Value.Distance > FurthestRoom.Distance)
                {
                    FurthestRoom = keyValuePair.Value;
                }
                if (keyValuePair.Value.Distance >= 1000)
                {
                    HowMany1000Rooms++;
                }
            }
        }

        static void PrintMap(Room currentRoom = null)
        {
            Console.WriteLine();
            string line = "   ╬";
            for (int x = MinimumX; x <= MaximumX; x++)
            {
                line += "══╬";
            }
            Console.WriteLine(line);
            for (int y = MinimumY; y <= MaximumY; y++)
            {
                string line1 = "   ";
                string line2 = "   ";
                for (int x = MinimumX; x <= MaximumX; x++)
                {
                    if (x == MinimumX)
                    {
                        line1 += "║";
                        line2 += "╬";
                    }

                    if (x == 0 && y == 0)
                    {
                        line1 += "<>";
                    }
                    else if (currentRoom != null && currentRoom.X == x && currentRoom.Y == y)
                    {
                        line1 += "██";
                    }
                    else
                    {
                        //line1 += AllRooms[x+","+y].Distance.ToString().PadLeft(2, ' ');
                        line1 += "  ";
                    }

                    Room room;
                    if (AllRooms.TryGetValue(x + "," + y, out room))
                    {
                        if (room.EastRoom != null)
                        {
                            line1 += " ";
                        }
                        else
                        {
                            line1 += "║";
                        }
                        if (room.SouthRoom != null)
                        {
                            line2 += "  ";
                        }
                        else
                        {
                            line2 += "══";
                        }
                        line2 += "╬";
                    }
                    else
                    {
                        line1 += "║";
                        line2 += "══╬";
                    }
                }
                Console.WriteLine(line1);
                Console.WriteLine(line2);
            }

            Console.WriteLine("   Furthest room is {0} doors away", FurthestRoom.Distance);
            Console.WriteLine("   There are {0} rooms at least 1000 rooms from the current room", HowMany1000Rooms);
        }

        static void CalculateDistances(Room startRoom)
        {
            List<Room> roomsToTraverse = new List<Room> { startRoom };
            startRoom.Distance = 0;

            while (roomsToTraverse.Count > 0)
            {
                Room currentRoom = roomsToTraverse[0];
                if (currentRoom.NorthRoom != null && currentRoom.NorthRoom.Distance == -1)
                {
                    currentRoom.NorthRoom.Distance = currentRoom.Distance + 1;
                    roomsToTraverse.Add(currentRoom.NorthRoom);
                }
                if (currentRoom.EastRoom != null && currentRoom.EastRoom.Distance == -1)
                {
                    currentRoom.EastRoom.Distance = currentRoom.Distance + 1;
                    roomsToTraverse.Add(currentRoom.EastRoom);
                }
                if (currentRoom.SouthRoom != null && currentRoom.SouthRoom.Distance == -1)
                {
                    currentRoom.SouthRoom.Distance = currentRoom.Distance + 1;
                    roomsToTraverse.Add(currentRoom.SouthRoom);
                }
                if (currentRoom.WestRoom != null && currentRoom.WestRoom.Distance == -1)
                {
                    currentRoom.WestRoom.Distance = currentRoom.Distance + 1;
                    roomsToTraverse.Add(currentRoom.WestRoom);
                }
                roomsToTraverse.RemoveAt(0);
            }
        }

        static Room AlreadyExplored(int x, int y)
        {
            Room room = null;
            if (AllRooms.TryGetValue(x + "," + y, out room))
            {
                return room;
            }

            return room;
        }

        static void ExplorePatternSegment(string patternSegment, Room beginRoom)
        {
            Room currentRoom = beginRoom;

            //Console.WriteLine(patternSegment);

            for (int i = 0; i < patternSegment.Length; i++)
            {
                char character = patternSegment[i];
                switch (character)
                {
                    case 'N':
                        {
                            Room alreadyExplored = AlreadyExplored(currentRoom.X, currentRoom.Y - 1);
                            if (alreadyExplored == null)
                            {
                                Room newRoom = new Room(currentRoom.X, currentRoom.Y - 1);
                                AllRooms[newRoom.X + "," + newRoom.Y] = newRoom;
                                currentRoom.NorthRoom = newRoom;
                            }
                            else
                            {
                                currentRoom.NorthRoom = alreadyExplored;
                            }
                            currentRoom.NorthRoom.SouthRoom = currentRoom;
                            currentRoom = currentRoom.NorthRoom;

                        }
                        break;
                    case 'E':
                        {

                            Room alreadyExplored = AlreadyExplored(currentRoom.X + 1, currentRoom.Y);
                            if (alreadyExplored == null)
                            {
                                Room newRoom = new Room(currentRoom.X + 1, currentRoom.Y);
                                AllRooms[newRoom.X + "," + newRoom.Y] = newRoom;
                                currentRoom.EastRoom = newRoom;
                            }
                            else
                            {
                                currentRoom.EastRoom = alreadyExplored;
                            }
                            currentRoom.EastRoom.WestRoom = currentRoom;
                            currentRoom = currentRoom.EastRoom;

                        }
                        break;
                    case 'S':
                        {

                            Room alreadyExplored = AlreadyExplored(currentRoom.X, currentRoom.Y + 1);
                            if (alreadyExplored == null)
                            {
                                Room newRoom = new Room(currentRoom.X, currentRoom.Y + 1);
                                AllRooms[newRoom.X + "," + newRoom.Y] = newRoom;
                                currentRoom.SouthRoom = newRoom;
                            }
                            else
                            {
                                currentRoom.SouthRoom = alreadyExplored;
                            }
                            currentRoom.SouthRoom.NorthRoom = currentRoom;
                            currentRoom = currentRoom.SouthRoom;

                        }
                        break;
                    case 'W':
                        {
                            Room alreadyExplored = AlreadyExplored(currentRoom.X - 1, currentRoom.Y);
                            if (alreadyExplored == null)
                            {
                                Room newRoom = new Room(currentRoom.X - 1, currentRoom.Y);
                                AllRooms[newRoom.X + "," + newRoom.Y] = newRoom;
                                currentRoom.WestRoom = newRoom;
                            }
                            else
                            {
                                currentRoom.WestRoom = alreadyExplored;
                            }
                            currentRoom.WestRoom.EastRoom = currentRoom;
                            currentRoom = currentRoom.WestRoom;

                        }
                        break;
                    case '(':
                        {
                            // need to find the corresponding )
                            int indentation = 0;
                            int n = i;
                            do
                            {
                                if ((patternSegment[n] == '|' || patternSegment[n] == ')') && indentation == 1)
                                {
                                    string subPatternSegment = patternSegment.Substring(i + 1, n - i - 1);
                                    ExplorePatternSegment(subPatternSegment, currentRoom);
                                    i = n;
                                }

                                if (patternSegment[n] == '(')
                                {
                                    indentation++;
                                }
                                else if (patternSegment[n] == ')')
                                {
                                    indentation--;
                                }

                                n++;
                            }
                            while (n < patternSegment.Length && indentation > 0);
                        }
                        break;
                }
            }
        }
    }

    public class Room
    {
        public Room NorthRoom = null;
        public Room EastRoom = null;
        public Room SouthRoom = null;
        public Room WestRoom = null;

        public int Distance = -1;

        public int X;
        public int Y;

        public Room(int x, int y)
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
