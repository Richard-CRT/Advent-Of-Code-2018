using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _23
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            List<Nanobot> Nanobots = new List<Nanobot>();

            foreach (string inputLine in inputList)
            {
                string[] inputLineParts = inputLine.Split(' ');
                string[] coordinateParts = inputLineParts[0].Substring(5, inputLineParts[0].Length - 7).Split(',');

                int x = Int32.Parse(coordinateParts[0]);
                int y = Int32.Parse(coordinateParts[1]);
                int z = Int32.Parse(coordinateParts[2]);
                int radius = Int32.Parse(inputLineParts[1].Substring(2));

                Nanobot newBot = new Nanobot(x, y, z, radius);
                Nanobots.Add(newBot);
            }

            Nanobots = Nanobots.OrderByDescending(nanobot => nanobot.Radius).ToList();

            Nanobot strongestNanoBot = Nanobots[0];

            int nanobotsInRange = 0;
            foreach (Nanobot otherNanobot in Nanobots)
            {
                if (strongestNanoBot.DistanceFrom(otherNanobot) <= strongestNanoBot.Radius)
                {
                    nanobotsInRange++;
                }
            }

            Console.WriteLine("{0} nanobots in range of strongest nanobot", nanobotsInRange);

            

            int minimumX = int.MaxValue;
            int maximumX = int.MinValue;
            int minimumY = int.MaxValue;
            int maximumY = int.MinValue;
            int minimumZ = int.MaxValue;
            int maximumZ = int.MinValue;
            foreach (Nanobot otherNanobot in Nanobots)
            {
                if (otherNanobot.Position.X < minimumX)
                {
                    minimumX = otherNanobot.Position.X;
                }
                if (otherNanobot.Position.X > maximumX)
                {
                    maximumX = otherNanobot.Position.X;
                }
                if (otherNanobot.Position.Y < minimumY)
                {
                    minimumY = otherNanobot.Position.Y;
                }
                if (otherNanobot.Position.Y > maximumY)
                {
                    maximumY = otherNanobot.Position.Y;
                }
                if (otherNanobot.Position.Z < minimumZ)
                {
                    minimumZ = otherNanobot.Position.Z;
                }
                if (otherNanobot.Position.Z > maximumZ)
                {
                    maximumZ = otherNanobot.Position.Z;
                }
            }

            int sizeX = maximumX - minimumX + 1;
            int sizeY = maximumY - minimumY + 1;
            int sizeZ = maximumZ - minimumZ + 1;

            int deltaX = (int)Math.Pow(2, Math.Ceiling(Math.Log(sizeX, 2)));
            int deltaY = (int)Math.Pow(2, Math.Ceiling(Math.Log(sizeY, 2)));
            int deltaZ = (int)Math.Pow(2, Math.Ceiling(Math.Log(sizeZ, 2)));

            List<Nanobot> workingNanobots = new List<Nanobot>(Nanobots);

            //List<TrialTeleportPosition> trialTeleportPositions = new List<TrialTeleportPosition>();

            Position bestPosition = null;
            while (true)
            {
                int bestNanoRobotsInRange = 0;
                for (int z = minimumZ; z <= maximumZ; z += deltaZ)
                {
                    for (int y = minimumY; y <= maximumY; y += deltaY)
                    {
                        for (int x = minimumX; x <= maximumX; x += deltaX)
                        {
                            Position currentPosition = new Position(x, y, z);
                            int nanoRobotsInRange = 0;
                            foreach (Nanobot nanobot in workingNanobots)
                            {
                                if (currentPosition.DistanceFrom(nanobot.Position) <= nanobot.Radius)
                                {
                                    nanoRobotsInRange++;
                                }
                            }
                            if (nanoRobotsInRange > bestNanoRobotsInRange)
                            {
                                bestNanoRobotsInRange = nanoRobotsInRange;
                                bestPosition = currentPosition;
                            }
                            else if (nanoRobotsInRange == bestNanoRobotsInRange)
                            {
                                if (bestPosition == null || currentPosition.DistanceFromOrigin < bestPosition.DistanceFromOrigin)
                                {
                                    bestPosition = currentPosition;
                                }
                            }
                            //Position position nanoRobotsInRange new Position(x, y, z);

                            //int nanobotsInRangeOfPosition = 0;
                            //trialTeleportPositions.Add(new TrialTeleportPosition(position, nanobotsInRangeOfPosition));
                        }
                    }
                }
                if (deltaX == 1 && deltaY == 1 && deltaZ == 1)
                {
                    break;
                }
                maximumZ = bestPosition.Z + deltaZ;
                minimumZ = bestPosition.Z - deltaZ;
                maximumY = bestPosition.Y + deltaY;
                minimumY = bestPosition.Y - deltaY;
                maximumX = bestPosition.X + deltaX;
                minimumX = bestPosition.X - deltaX;
                if (deltaX > 1)
                    deltaX /= 2;
                if (deltaY > 1)
                    deltaY /= 2;
                if (deltaZ > 1)
                    deltaZ /= 2;
            }


            //trialTeleportPositions = trialTeleportPositions.OrderByDescending(trialTeleportPosition => trialTeleportPosition.NanobotsInRange).ThenBy(trialTeleportPosition => trialTeleportPosition.Position.DistanceFromOrigin).ToList();

            //Console.WriteLine("Best coordinate choice is {0} from you", trialTeleportPositions[0].Position.DistanceFromOrigin);

            Console.WriteLine(bestPosition.DistanceFromOrigin);
            Console.ReadLine();
        }
    }

    public class TrialTeleportPosition
    {
        public Position Position;
        public int NanobotsInRange = 0;

        public TrialTeleportPosition(Position position, int nanobotsInRange)
        {
            Position = position;
            NanobotsInRange = nanobotsInRange;
        }
    }

    public class Position
    {
        public int X;
        public int Y;
        public int Z;

        public readonly int DistanceFromOrigin;

        public Position(int x, int y, int z)
        {
            DistanceFromOrigin = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
            X = x;
            Y = y;
            Z = z;
        }

        public int DistanceFrom(Position otherPosition)
        {
            return Math.Abs(this.X - otherPosition.X) + Math.Abs(this.Y - otherPosition.Y) + Math.Abs(this.Z - otherPosition.Z);
        }
    }

    public class Nanobot
    {
        public Position Position;
        public int Radius;

        public Nanobot(int x, int y, int z, int radius)
        {
            Position = new Position(x, y, z);
            Radius = radius;
        }

        public int DistanceFrom(Nanobot otherNanobot)
        {
            return this.Position.DistanceFrom(otherNanobot.Position);
        }
    }
}
