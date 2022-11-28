using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _14
{
    class Program
    {
        static List<int> Recipes;
        static int Elf1Current = 0;
        static int Elf2Current = 1;

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            List<int> recipesToFind = new List<int>();
            foreach (char character in inputList[0])
            {
                recipesToFind.Add(Int32.Parse(character.ToString()));
            }

            //recipesToMake = 2018;

            Recipes = new List<int> { 3, 7 };

            int foundKey = -1;
            PrintRecipes();
            while (foundKey == -1)
            {
                int sum = Recipes[Elf1Current] + Recipes[Elf2Current];
                if (sum >= 10)
                {
                    Recipes.Add(1);
                    Recipes.Add(sum - 10);
                }
                else
                {
                    Recipes.Add(sum);
                }
                PrintRecipes();

                if (Recipes.Count > recipesToFind.Count)
                {
                    int match = Recipes.Count - 1 - recipesToFind.Count;
                    for (int i = 0; i < recipesToFind.Count; i++)
                    {
                        if (Recipes[Recipes.Count-1-recipesToFind.Count + i] != recipesToFind[i])
                        {
                            match = -1;
                            break;
                        }
                    }
                    if (match == -1)
                    {
                        match = Recipes.Count - recipesToFind.Count;
                        for (int i = 0; i < recipesToFind.Count; i++)
                        {
                            if (Recipes[Recipes.Count - recipesToFind.Count + i] != recipesToFind[i])
                            {
                                match = -1;
                                break;
                            }
                        }
                    }
                    foundKey = match;
                }

                Elf1Current = (Elf1Current + (1 + Recipes[Elf1Current])) % Recipes.Count;
                Elf2Current = (Elf2Current + (1 + Recipes[Elf2Current])) % Recipes.Count;
            }

            Console.WriteLine("Result: {0}",foundKey);
            Console.ReadLine();
        }

        static void PrintRecipes()
        {
#if DEBUG
            for (int n = 0; n < Recipes.Count; n++)
            {
                if (n == Elf1Current && n == Elf2Current)
                {

                }
                else if (n == Elf1Current)
                {
                    AoCUtilities.DebugWrite("({0})", Recipes[n]);
                }
                else if (n == Elf2Current)
                {
                    AoCUtilities.DebugWrite("[{0}]", Recipes[n]);
                }
                else
                {
                    AoCUtilities.DebugWrite(" {0} ", Recipes[n]);
                }
            }
            AoCUtilities.DebugWriteLine();
#endif
        }
    }
}
