using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3
{
    class Program
    {
        public enum ClaimState { NotClaimed, Claimed, Overlap };

        static void Main(string[] args)
        {
            var logFile = File.ReadAllLines("input.txt");
            var logList = new List<string>(logFile);

            ClaimState[,] fabric = new ClaimState[1000, 1000];

            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    fabric[y, x] = ClaimState.NotClaimed;
                }
            }

            int sqInchesContested = 0;

            for (int i = 0; i < logList.Count; i++)
            {
                string claim = logList[i];
                string[] claimParts = claim.Split(' ');
                string claimCoordinates = claimParts[2];
                string claimSize = claimParts[3];

                string[] claimCoordinatesParts = claimCoordinates.Split(',');
                int claimX = Int32.Parse(claimCoordinatesParts[0]);
                int claimY = Int32.Parse(claimCoordinatesParts[1].Substring(0, claimCoordinatesParts[1].Length - 1));

                string[] claimSizeParts = claimSize.Split('x');
                int claimWidth = Int32.Parse(claimSizeParts[0]);
                int claimHeight = Int32.Parse(claimSizeParts[1]);

                for (int x = claimX; x < claimX + claimWidth; x++)
                {
                    for (int y = claimY; y < claimY + claimHeight; y++)
                    {
                        if (fabric[y, x] == ClaimState.NotClaimed)
                        {
                            fabric[y, x] = ClaimState.Claimed;
                        }
                        else if (fabric[y, x] == ClaimState.Claimed)
                        {
                            fabric[y, x] = ClaimState.Overlap;
                            sqInchesContested++;
                        }
                    }
                }
            }

            Console.WriteLine(sqInchesContested);


            int uniqueClaimIndex = -1;

            for (int i = 0; i < logList.Count; i++)
            {
                string claim = logList[i];
                string[] claimParts = claim.Split(' ');
                string claimCoordinates = claimParts[2];
                string claimSize = claimParts[3];

                string[] claimCoordinatesParts = claimCoordinates.Split(',');
                int claimX = Int32.Parse(claimCoordinatesParts[0]);
                int claimY = Int32.Parse(claimCoordinatesParts[1].Substring(0, claimCoordinatesParts[1].Length - 1));

                string[] claimSizeParts = claimSize.Split('x');
                int claimWidth = Int32.Parse(claimSizeParts[0]);
                int claimHeight = Int32.Parse(claimSizeParts[1]);

                bool hasOverlap = false;

                for (int x = claimX; x < claimX + claimWidth; x++)
                {
                    for (int y = claimY; y < claimY + claimHeight; y++)
                    {
                        if (fabric[y, x] == ClaimState.Overlap)
                        {
                            hasOverlap = true;
                        }
                    }
                }

                if (!hasOverlap)
                {
                    uniqueClaimIndex = i;
                }
            }

            Console.WriteLine(logList[uniqueClaimIndex]);
            Console.ReadLine();
        }
    }
}
