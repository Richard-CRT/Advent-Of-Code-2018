using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _12
{
    public enum PotState { Plant, NoPlant };

    class Program
    {
        static int CurrentMinimum;
        static int CurrentMaximum;
        static int NextMinimum;
        static int NextMaximum;
        static List<Tuple<List<PotState>, PotState>> Rules = new List<Tuple<List<PotState>, PotState>>();

        static PotState GetPotStateAt(int i, List<PotState> potStates)
        {
            if (i < CurrentMinimum || i > CurrentMaximum)
            {
                return PotState.NoPlant;
            }
            else
            {
                return potStates[i - CurrentMinimum];
            }
        }

        static void SetPotStateAt(int i, PotState newPotState, List<PotState> potStates)
        {
            if (newPotState == PotState.Plant)
            {
                if (i < NextMinimum)
                {
                    for (int n = 0; n < NextMinimum - i; n++)
                    {
                        potStates.Insert(0, PotState.NoPlant);
                    }
                    NextMinimum = i;
                }
                else if (i > NextMaximum)
                {
                    for (int n = 0; n < i - NextMaximum; n++)
                    {
                        potStates.Add(PotState.NoPlant);
                    }
                    NextMaximum = i;
                }
            }

            if (i >= NextMinimum && i <= NextMaximum)
            {
                potStates[i - NextMinimum] = newPotState;
            }
        }

        static PotState GetNextPotStateAt(int i, List<PotState> potStates)
        {
            foreach (Tuple<List<PotState>, PotState> rule in Rules)
            {
                if (GetPotStateAt(i - 2, potStates) == rule.Item1[0]
                    && GetPotStateAt(i - 1, potStates) == rule.Item1[1]
                    && GetPotStateAt(i, potStates) == rule.Item1[2]
                    && GetPotStateAt(i + 1, potStates) == rule.Item1[3]
                    && GetPotStateAt(i + 2, potStates) == rule.Item1[4])
                {
                    return rule.Item2;
                }
            }
            return PotState.NoPlant;
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            string initialString = inputList[0].Substring(15, inputList[0].Length - 15);

            CurrentMinimum = 0;
            CurrentMaximum = initialString.Length - 1;

            List<PotState> potStates = new List<PotState>();

            for (int i = 0; i < initialString.Length; i++)
            {
                if (initialString[i] == '#')
                {
                    potStates.Add(PotState.Plant);
                }
                else
                {
                    potStates.Add(PotState.NoPlant);
                }
            }

            for (int i = 2; i < inputList.Count; i++)
            {
                string requirementString = inputList[(int)i].Substring(0, 5);
                string resultString = inputList[(int)i].Substring(9, 1);

                List<PotState> requirement = new List<PotState>();
                PotState result;

                foreach (char character in requirementString)
                {
                    if (character == '#')
                    {
                        requirement.Add(PotState.Plant);
                    }
                    else
                    {
                        requirement.Add(PotState.NoPlant);
                    }
                }

                if (resultString == "#")
                {
                    result = PotState.Plant;
                }
                else
                {
                    result = PotState.NoPlant;
                }

                Rules.Add(new Tuple<List<PotState>, PotState>(requirement, result));
            }

            int lastSum = 0;

            for (int generationNumber = 1; generationNumber <= 20; generationNumber++)
            {
                PrintPots(potStates);

                NextMinimum = CurrentMinimum;
                NextMaximum = CurrentMaximum;
                List<PotState> nextGenerationPotStates = new List<PotState>(potStates);
                // from (currentMinimum - 2) to (currentMaximum + 2)
                for (int i = CurrentMinimum - 2; i <= CurrentMaximum + 2; i++)
                {
                    PotState nextPotState = GetNextPotStateAt(i, potStates);
                    SetPotStateAt(i, nextPotState, nextGenerationPotStates);
                }
                CurrentMaximum = NextMaximum;
                CurrentMinimum = NextMinimum;
                potStates = nextGenerationPotStates;

                // trim from start of sequence
                int? firstPlant = null;
                for (int i = CurrentMinimum; i <= CurrentMaximum; i++)
                {
                    if (GetPotStateAt(i, potStates) == PotState.Plant)
                    {
                        firstPlant = i;
                        break;
                    }
                }
                for (int i = CurrentMinimum; i < firstPlant; i++)
                {
                    potStates.RemoveAt(0);
                    CurrentMinimum++;
                }

                // trim from end of sequence
                int? lastPlant = null;
                for (int i = CurrentMaximum; i >= CurrentMinimum; i--)
                {
                    if (GetPotStateAt(i, potStates) == PotState.Plant)
                    {
                        lastPlant = i;
                        break;
                    }
                }
                for (int i = CurrentMaximum; i > lastPlant; i--)
                {
                    potStates.RemoveAt(potStates.Count - 1);
                    CurrentMaximum--;
                }

                if (generationNumber % 1000 == 0 || (generationNumber - 1) % 1000 == 0)
                {
                    int sum = 0;
                    for (int i = CurrentMinimum; i <= CurrentMaximum; i++)
                    {
                        if (GetPotStateAt(i, potStates) == PotState.Plant)
                        {
                            sum += i;
                        }
                    }
                    Console.WriteLine("{0} {1} : {2}", generationNumber, sum, sum - lastSum);
                    lastSum = sum;
                }
            }

            PrintPots(potStates);

            Console.WriteLine(59454 + ((50000000000 - 1000) * 57));

            Console.ReadLine();
        }

        static void PrintPots(List<PotState> potStates)
        {

            for (int i = CurrentMinimum; i <= CurrentMaximum; i++)
            {
                AoCUtilities.DebugWrite("{0} ", i.ToString().PadLeft(2, ' '));
            }
            AoCUtilities.DebugWriteLine();

            for (int i = CurrentMinimum; i <= CurrentMaximum; i++)
            {
                if (GetPotStateAt(i, potStates) == PotState.Plant)
                {
                    AoCUtilities.DebugWrite(" # ");
                }
                else
                {
                    AoCUtilities.DebugWrite(" . ");
                }
            }
            AoCUtilities.DebugWriteLine();
        }
    }
}
