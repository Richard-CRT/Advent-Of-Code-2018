using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _9
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            string[] inputParts = inputList[0].Split(' ');
            int playerCount = Int32.Parse(inputParts[0]);
            int marbleMax = Int32.Parse(inputParts[6]);

            Marble currentMarble = new Marble(0);
            currentMarble.ClockwiseMarble = currentMarble;
            currentMarble.CounterClockwiseMarble = currentMarble;

            long[] elfScores = new long[playerCount];
            Array.Clear(elfScores, 0, elfScores.Length);

            for (int marbleCounter = 1; marbleCounter <= marbleMax * 100; marbleCounter++)
            {
                if (marbleCounter % 23 != 0)
                {
                    Marble clockwiseMarble = currentMarble.ClockwiseMarble;
                    Marble doubleClockwiseMarble = clockwiseMarble.ClockwiseMarble;

                    // create new marble
                    currentMarble = new Marble(marbleCounter);

                    // break current links
                    clockwiseMarble.ClockwiseMarble = currentMarble;
                    doubleClockwiseMarble.CounterClockwiseMarble = currentMarble;

                    currentMarble.CounterClockwiseMarble = clockwiseMarble;
                    currentMarble.ClockwiseMarble = doubleClockwiseMarble;
                }
                else
                {
                    int playerId = marbleCounter % playerCount;

                    Marble sevenCounterClockwiseMarble = currentMarble.RelativeCounterClockwiseMarble(7);
                    // add marbleCounter to score
                    // add sevenCounterClockwiseMarble.Value to score

                    elfScores[playerId] += marbleCounter + sevenCounterClockwiseMarble.Value;

                    // break current links and remove 7 from circle
                    Marble eightCounterClockwiseMarble = sevenCounterClockwiseMarble.CounterClockwiseMarble;
                    Marble sixCounterClockwiseMarble = sevenCounterClockwiseMarble.ClockwiseMarble;

                    eightCounterClockwiseMarble.ClockwiseMarble = sixCounterClockwiseMarble;
                    sixCounterClockwiseMarble.CounterClockwiseMarble = eightCounterClockwiseMarble;

                    currentMarble = sixCounterClockwiseMarble;
                }

#if DEBUG
                Marble iteratingMarble = currentMarble;
                do
                {
                    AoCUtilities.DebugWrite("{0} ", iteratingMarble.Value.ToString().PadLeft(2, ' '));
                    iteratingMarble = iteratingMarble.ClockwiseMarble;
                }
                while (iteratingMarble != currentMarble);
                AoCUtilities.DebugWriteLine();
#endif
            }

            Console.WriteLine(elfScores.Max());
            Console.ReadLine();
        }
    }

    public class Marble
    {
        public int Value;
        public Marble ClockwiseMarble = null;
        public Marble CounterClockwiseMarble = null;

        public Marble(int value)
        {
            Value = value;
        }

        public Marble RelativeCounterClockwiseMarble(int offset)
        {
            Marble returnMarble = this;
            for (int i = 0; i < offset; i++)
            {
                returnMarble = returnMarble.CounterClockwiseMarble;
            }
            return returnMarble;
        }

        public Marble RelativeClockwiseMarble(int offset)
        {
            Marble returnMarble = this;
            for (int i = 0; i < offset; i++)
            {
                returnMarble = returnMarble.ClockwiseMarble;
            }
            return returnMarble;
        }
    }
}
