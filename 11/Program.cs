using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _11
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            int gridSerialNumber = Int32.Parse(inputList[0]);
            
            int[,] grid = new int[300, 300];

            for (int y = 0; y < 300; y++)
            {
                for (int x = 0; x < 300; x++)
                {
                    int rackId = (x + 1) + 10;
                    int powerLevel = rackId * (y + 1);
                    powerLevel += gridSerialNumber;
                    powerLevel *= rackId;
                    powerLevel = (int)Math.Floor((double)((powerLevel % 1000) / 100));
                    powerLevel -= 5;
                    grid[y, x] = powerLevel;
                }
            }

            int bestX = 1;
            int bestY = 1;
            int bestSize = 1;
            int bestPower = 0;
            for (int size = 1; size <= 300; size++)
            {
                AoCUtilities.DebugWriteLine("Size: {0}", size);
                for (int y = 0; y <= 300 - size; y++)
                {
                    for (int x = 0; x <= 300 - size; x++)
                    {
                        int trialPower = 0;
                        for (int n = 0; n < size; n++)
                        {
                            for (int m = 0; m < size; m++)
                            {
                                trialPower += grid[y + n, x + m];
                            }
                        }

                        if (trialPower > bestPower)
                        {
                            bestPower = trialPower;
                            bestX = x + 1;
                            bestY = y + 1;
                            bestSize = size;
                        }
                    }
                }
            }
            Console.WriteLine("{0},{1},{2}", bestX, bestY, bestSize);
            Console.ReadLine();
        }
    }
}
