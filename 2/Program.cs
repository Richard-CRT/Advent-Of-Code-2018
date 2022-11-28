using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2
{
    class Program
    {
        static void Main(string[] args)
        {
            var logFile = File.ReadAllLines("input.txt");
            var logList = new List<string>(logFile);

            int twos = 0;
            int threes = 0;

            for (int i = 0; i < logList.Count; i++)
            {
                List<char> characters = new List<char>();
                for (int n = 0; n < logList[i].Length; n++)
                {
                    characters.Add(logList[i][n]);
                }
                List<char> checkedCharacters = new List<char>();
                bool got2 = false;
                bool got3 = false;
                foreach (char character in characters)
                {
                    if (checkedCharacters.IndexOf(character) == -1)
                    {
                        int appearances = 0;
                        foreach (char iterationCharacter in characters)
                        {
                            if (character == iterationCharacter)
                            {
                                appearances++;
                            }
                        }
                        checkedCharacters.Add(character);
                        Console.WriteLine("{0} {1}", character, appearances);
                        if (appearances == 2 && !got2)
                        {
                            got2 = true;
                            twos++;
                            Console.WriteLine("Two");
                        }
                        else if (appearances == 3 && !got3)
                        {
                            got3 = true;
                            threes++;
                            Console.WriteLine("Three");
                        }
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine(twos * threes);
            Console.ReadLine();

            for (int m = 0; m < logList.Count; m++)
            {
                string line = logList[m];
                List<int> matches = new List<int>();
                for (int n = 0; n < logList.Count; n++)
                {
                    string lineCompare = logList[n];
                    int differences = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] != lineCompare[i])
                        {
                            differences++;
                        }
                        if (differences > 1)
                        {
                            break;
                        }
                    }
                    if (differences == 1)
                    {
                        matches.Add(n);
                    }
                }

                if (matches.Count != 0)
                {
                    Console.WriteLine("{0} {1}", m, matches.Count);
                    for (int i = 0; i < matches.Count; i++)
                    {
                        int index = matches[i];
                        Console.WriteLine("{0} {1}", i, logList[index]);
                    }
                    Console.WriteLine();
                }
            }
            Console.ReadLine();
        }
    }
}
