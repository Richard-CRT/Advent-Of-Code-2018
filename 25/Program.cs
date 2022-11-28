using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _25
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            List<Coordinate> Coordinates = new List<Coordinate>();
            foreach (string inputLine in inputList)
            {
                string[] inputLineParts = inputLine.Split(',');
                int x = Int32.Parse(inputLineParts[0]);
                int y = Int32.Parse(inputLineParts[1]);
                int z = Int32.Parse(inputLineParts[2]);
                int t = Int32.Parse(inputLineParts[3]);

                Coordinates.Add(new Coordinate(x, y, z, t));
            }

            List<Constellation> Constellations = new List<Constellation>();
            foreach (Coordinate coordinate in Coordinates)
            {
                List<Constellation> matchingConstellations = new List<Constellation>();
                foreach (Constellation constellation in Constellations)
                {
                    foreach (Coordinate coordinateInConstellation in constellation.Coordinates)
                    {
                        if (coordinate.DistanceFrom(coordinateInConstellation) <= 3)
                        {
                            matchingConstellations.Add(constellation);
                            break;
                        }
                    }
                }

                if (matchingConstellations.Count == 0)
                {
                    Constellation newConstellation = new Constellation();
                    newConstellation.Coordinates.Add(coordinate);
                    Constellations.Add(newConstellation);
                }
                else if (matchingConstellations.Count == 1)
                {
                    matchingConstellations[0].Coordinates.Add(coordinate);
                }
                else
                {
                    // need to join multiple constellations into 1
                    Constellation masterConstellation = matchingConstellations[0];
                    for (int i = 1; i < matchingConstellations.Count; i++)
                    {
                        masterConstellation.Coordinates.AddRange(matchingConstellations[i].Coordinates);
                        Constellations.Remove(matchingConstellations[i]);
                    }
                    masterConstellation.Coordinates.Add(coordinate);
                }
            }

            Console.WriteLine("There are {0} constellation(s)", Constellations.Count);
            Console.ReadLine();
        }
    }

    public class Coordinate
    {
        public int X;
        public int Y;
        public int Z;
        public int T;

        public Coordinate(int x, int y, int z, int t)
        {
            X = x;
            Y = y;
            Z = z;
            T = t;
        }

        public int DistanceFrom(Coordinate otherCoordinate)
        {
            return Math.Abs(this.X - otherCoordinate.X) + Math.Abs(this.Y - otherCoordinate.Y) + Math.Abs(this.Z - otherCoordinate.Z) + Math.Abs(this.T - otherCoordinate.T);
        }
    }

    public class Constellation
    {
        public List<Coordinate> Coordinates = new List<Coordinate>();
    }
}
