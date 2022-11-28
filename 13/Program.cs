using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _13
{
    public enum TrackType { Space = ' ', EastWest = '═', NorthSouth = '║', NorthEastSouthWest = '╬', EastSouth = '╔', NorthWest = '╝', SouthWest = '╗', NorthEast = '╚' };
    public enum CartDirection { North = '^', East = '>', South = 'v', West = '<' };
    public enum CartTurnOrder { Left = 0, Straight = 1, Right = 2 };

    class Program
    {
        static List<Cart> AllCarts = new List<Cart>();
        static int Ticks = 0;

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            /* 
             * First step is to process the track into memory,
             * will convert / and \ into more specific track type
             * at same time so I don't have to do it on the spot
             * later
             */
            int xSize = 0;
            foreach (string line in inputList)
            {
                if (line.Length > xSize)
                {
                    xSize = line.Length;
                }
            }
            int ySize = inputList.Count;

            TrackCell[,] grid = new TrackCell[ySize, xSize];

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    grid[y, x] = new TrackCell();
                    char character;
                    if (x < inputList[y].Length)
                    {
                        character = inputList[y][x];
                    }
                    else
                    {
                        character = ' ';
                    }
                    switch (character)
                    {
                        case '-':
                            {
                                grid[y, x].Type = TrackType.EastWest;
                                break;
                            }
                        case '|':
                            {
                                grid[y, x].Type = TrackType.NorthSouth;
                                break;
                            }
                        case '+':
                            {
                                grid[y, x].Type = TrackType.NorthEastSouthWest;
                                break;
                            }
                        case '/':
                            {
                                if (x > 0 && (grid[y, x - 1].Type == TrackType.EastWest || grid[y, x - 1].Type == TrackType.NorthEastSouthWest
                                               || grid[y, x - 1].Type == TrackType.NorthEast || grid[y, x - 1].Type == TrackType.EastSouth))
                                {
                                    grid[y, x].Type = TrackType.NorthWest;
                                }
                                else
                                {
                                    grid[y, x].Type = TrackType.EastSouth;
                                }
                                break;
                            }
                        case '\\':
                            {
                                if (x > 0 && (grid[y, x - 1].Type == TrackType.EastWest || grid[y, x - 1].Type == TrackType.NorthEastSouthWest
                                               || grid[y, x - 1].Type == TrackType.NorthEast || grid[y, x - 1].Type == TrackType.EastSouth))
                                {
                                    grid[y, x].Type = TrackType.SouthWest;
                                }
                                else
                                {
                                    grid[y, x].Type = TrackType.NorthEast;
                                }
                                break;
                            }
                        case 'v':
                            {
                                grid[y, x].Type = TrackType.NorthSouth;
                                Cart newCart = new Cart(CartDirection.South, x, y);
                                newCart.ID = (char)(97 + AllCarts.Count);
                                grid[y, x].Carts.Add(newCart);
                                AllCarts.Add(newCart);
                                break;
                            }
                        case '^':
                            {
                                grid[y, x].Type = TrackType.NorthSouth;
                                Cart newCart = new Cart(CartDirection.North, x, y);
                                newCart.ID = (char)(97 + AllCarts.Count);
                                grid[y, x].Carts.Add(newCart);
                                AllCarts.Add(newCart);
                                break;
                            }
                        case '<':
                            {
                                grid[y, x].Type = TrackType.EastWest;
                                Cart newCart = new Cart(CartDirection.West, x, y);
                                newCart.ID = (char)(97 + AllCarts.Count);
                                grid[y, x].Carts.Add(newCart);
                                AllCarts.Add(newCart);
                                break;
                            }
                        case '>':
                            {
                                grid[y, x].Type = TrackType.EastWest;
                                Cart newCart = new Cart(CartDirection.East, x, y);
                                newCart.ID = (char)(97 + AllCarts.Count);
                                grid[y, x].Carts.Add(newCart);
                                AllCarts.Add(newCart);
                                break;
                            }
                    }
                }
            }

            PrintGrid(grid);
            AoCUtilities.DebugReadLine();
            Location lastCartLocation = null;
            while (lastCartLocation == null)
            {
                lastCartLocation = Tick(grid);
                PrintGrid(grid);
                AoCUtilities.DebugWriteLine("Tick: {0}", Ticks);
                AoCUtilities.DebugReadLine();
            }

            Console.WriteLine("Tick: {0}", Ticks);
            Console.WriteLine("{0},{1}", lastCartLocation.X, lastCartLocation.Y);
            Console.ReadLine();
        }

        static private Location Tick(TrackCell[,] grid)
        {
            Location lastCartLocation = null;

            AllCarts = AllCarts.OrderBy(cart => cart.Y).ThenBy(cart => cart.X).ToList();
            List<Cart> CopyAllCarts = new List<Cart>(AllCarts);
            foreach (Cart cart in CopyAllCarts)
            {
                if (!AllCarts.Contains(cart))
                    continue;
                //AoCUtilities.DebugWriteLine("{0},{1}", cart.X, cart.Y);

                int nx = cart.X;
                int ny = cart.Y;

                switch (cart.Direction)
                {
                    case CartDirection.North:
                        ny = cart.Y - 1;
                        nx = cart.X;
                        break;
                    case CartDirection.East:
                        ny = cart.Y;
                        nx = cart.X + 1;
                        break;
                    case CartDirection.South:
                        ny = cart.Y + 1;
                        nx = cart.X;
                        break;
                    case CartDirection.West:
                        ny = cart.Y;
                        nx = cart.X - 1;
                        break;
                }

                TrackCell currentTrackCell = grid[cart.Y, cart.X];
                TrackCell nextTrackCell = grid[ny, nx];
                CartDirection nextCartDirection = cart.Direction;

                switch (nextTrackCell.Type)
                {
                    case TrackType.NorthEast:
                        if (cart.Direction == CartDirection.West)
                        {
                            nextCartDirection = CartDirection.North;
                        }
                        else
                        {
                            nextCartDirection = CartDirection.East;
                        }
                        break;
                    case TrackType.EastSouth:
                        if (cart.Direction == CartDirection.West)
                        {
                            nextCartDirection = CartDirection.South;
                        }
                        else
                        {
                            nextCartDirection = CartDirection.East;
                        }
                        break;
                    case TrackType.SouthWest:
                        if (cart.Direction == CartDirection.East)
                        {
                            nextCartDirection = CartDirection.South;
                        }
                        else
                        {
                            nextCartDirection = CartDirection.West;
                        }
                        break;
                    case TrackType.NorthWest:
                        if (cart.Direction == CartDirection.East)
                        {
                            nextCartDirection = CartDirection.North;
                        }
                        else
                        {
                            nextCartDirection = CartDirection.West;
                        }
                        break;
                    case TrackType.NorthEastSouthWest:
                        switch (cart.Direction)
                        {
                            case CartDirection.North:
                                switch (cart.TurnOrder)
                                {
                                    case CartTurnOrder.Left:
                                        nextCartDirection = CartDirection.West;
                                        break;
                                    case CartTurnOrder.Right:
                                        nextCartDirection = CartDirection.East;
                                        break;
                                }
                                break;
                            case CartDirection.East:
                                switch (cart.TurnOrder)
                                {
                                    case CartTurnOrder.Left:
                                        nextCartDirection = CartDirection.North;
                                        break;
                                    case CartTurnOrder.Right:
                                        nextCartDirection = CartDirection.South;
                                        break;
                                }
                                break;
                            case CartDirection.South:
                                switch (cart.TurnOrder)
                                {
                                    case CartTurnOrder.Left:
                                        nextCartDirection = CartDirection.East;
                                        break;
                                    case CartTurnOrder.Right:
                                        nextCartDirection = CartDirection.West;
                                        break;
                                }
                                break;
                            case CartDirection.West:
                                switch (cart.TurnOrder)
                                {
                                    case CartTurnOrder.Left:
                                        nextCartDirection = CartDirection.South;
                                        break;
                                    case CartTurnOrder.Right:
                                        nextCartDirection = CartDirection.North;
                                        break;
                                }
                                break;
                        }

                        cart.TurnOrder = (CartTurnOrder)(((int)cart.TurnOrder + 1) % 3);
                        break;
                }

                cart.Direction = nextCartDirection;
                cart.X = nx;
                cart.Y = ny;
                currentTrackCell.Carts.Remove(cart);
                nextTrackCell.Carts.Add(cart);
                if (nextTrackCell.Carts.Count > 1)
                {
                    // crash
                    foreach (Cart cartToRemove in nextTrackCell.Carts)
                    {
                        AllCarts.Remove(cartToRemove);
                    }
                    nextTrackCell.Carts.Clear();
                }
            }

            if (AllCarts.Count <= 1)
            {
                Cart lastCart = AllCarts[0];
                lastCartLocation = new Location(lastCart.X, lastCart.Y);
            }

            Ticks++;
            return lastCartLocation;
        }

        static private void PrintGrid(TrackCell[,] grid)
        {
#if DEBUG
            string printString = "";

            AoCUtilities.DebugClear();
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x].Carts.Count > 1)
                    {
                        printString += "X";
                    }
                    else if (grid[y, x].Carts.Count == 1)
                    {
                        printString += string.Format("{0}", (char)grid[y, x].Carts[0].Direction);
                    }
                    else
                    {
                        printString += string.Format("{0}", (char)grid[y, x].Type);
                    }
                }
                printString += Environment.NewLine;
            }
            AoCUtilities.DebugWrite(printString);
#endif
        }
    }

    public class Location
    {
        public int X;
        public int Y;

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class TrackCell
    {
        public TrackType Type;
        public List<Cart> Carts = new List<Cart>();
    }

    public class Cart
    {
        public char ID;
        public CartDirection Direction;
        public CartTurnOrder TurnOrder = CartTurnOrder.Left;
        public int X;
        public int Y;

        public Cart(CartDirection direction, int x, int y)
        {
            Direction = direction;
            X = x;
            Y = y;
        }
    }
}
