using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = File.ReadAllLines("input.txt");
            var inputList = new List<string>(inputFile);

            List<Coordinate> coordinates = new List<Coordinate>();

            for (int i = 0; i < inputList.Count; i++)
            {
                coordinates.Add(new Coordinate(inputList[i]));
            }

            int dim = 400;

            int[,] grid = new int[dim, dim];

            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim; x++)
                {
                    int shortestDistanceIndex = -2;
                    int shortestDistance = -2;
                    for (int i = 0; i < coordinates.Count; i++)
                    {
                        int distance = Math.Abs(coordinates[i].X - x) + Math.Abs(coordinates[i].Y - y);
                        if (shortestDistanceIndex == -2 || distance < shortestDistance)
                        {
                            shortestDistanceIndex = i;
                            shortestDistance = distance;
                        }
                        else if (distance == shortestDistance)
                        {
                            shortestDistanceIndex = -1;
                        }
                    }
                    grid[y, x] = shortestDistanceIndex;
                }
            }

            List<int> coordinateExceptions = new List<int>();
            for (int y = 0; y < dim; y++)
            {
                int nearestCoordinate = grid[y, 0];
                if (coordinateExceptions.IndexOf(nearestCoordinate) == -1)
                {
                    coordinateExceptions.Add(nearestCoordinate);
                }
            }
            for (int y = 0; y < dim; y++)
            {
                int nearestCoordinate = grid[y, dim-1];
                if (coordinateExceptions.IndexOf(nearestCoordinate) == -1)
                {
                    coordinateExceptions.Add(nearestCoordinate);
                }
            }
            for (int x = 0; x < dim; x++)
            {
                int nearestCoordinate = grid[0, x];
                if (coordinateExceptions.IndexOf(nearestCoordinate) == -1)
                {
                    coordinateExceptions.Add(nearestCoordinate);
                }
            }
            for (int x = 0; x < dim; x++)
            {
                int nearestCoordinate = grid[dim-1, x];
                if (coordinateExceptions.IndexOf(nearestCoordinate) == -1)
                {
                    coordinateExceptions.Add(nearestCoordinate);
                }
            }

            int[] coordinateArea = new int[50];
            Array.Clear(coordinateArea, 0, coordinateArea.Length);

            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim; x++)
                {
                    if (grid[y, x] >= 0 && coordinateExceptions.IndexOf(grid[y,x]) == -1)
                    {
                        coordinateArea[grid[y, x]]++;
                    }
                }
            }

            for (int i = 0; i < coordinateArea.Length; i++)
            {
                Console.WriteLine("{0} {1}", i, coordinateArea[i]);
            }
            Console.ReadLine();

            // part 2

            int p2RegionSize = 0;

            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim; x++)
                {
                    int totalDistance = 0;
                    for (int i = 0; i < coordinates.Count; i++)
                    {
                        int distance = Math.Abs(coordinates[i].X - x) + Math.Abs(coordinates[i].Y - y);
                        totalDistance += distance;
                    }
                    if (totalDistance < 10000)
                    {
                        p2RegionSize++;
                    }
                }
            }
            Console.WriteLine("{0}", p2RegionSize);
            Console.ReadLine();
        }
    }

    public class Coordinate
    {
        public int X;
        public int Y;

        public Coordinate(string coordinate)
        {
            string[] coordinateParts = coordinate.Split(',');
            int x = Int32.Parse(coordinateParts[0]);
            int y = Int32.Parse(coordinateParts[1]);
            X = x;
            Y = y;
        }
    }
}
