using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5
{
    class Program
    {
        static string ReactPolymer(string inputPolymer)
        {
            for (int i = 0; i < inputPolymer.Length - 1; i++)
            {
                char currentCharacter = inputPolymer[i];
                char nextCharacter = inputPolymer[i + 1];
                if (currentCharacter == nextCharacter + 32 || currentCharacter + 32 == nextCharacter)
                {
                    inputPolymer = inputPolymer.Substring(0, i) + inputPolymer.Substring(i + 2, inputPolymer.Length - (i + 2));
                    if (i != 0)
                    {
                        i -= 2;
                    }

                    /*
                    Console.WriteLine(polymer);
                    for (int n = 0; n < i; n++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("^");
                    Console.ReadLine();
                    */
                }
            }
            return inputPolymer;
        }

        static void Main(string[] args)
        {
            var inputFile = File.ReadAllLines("input.txt");
            var inputList = new List<string>(inputFile);

            string polymer = inputList[0];

            Console.WriteLine(polymer.Length);
            Console.WriteLine("---");

            string reactedPolymer = ReactPolymer(polymer);

            Console.WriteLine("---");
            Console.WriteLine(reactedPolymer.Length);
            Console.ReadLine();

            Dictionary<string,int> bestPolymerLength = new Dictionary<string, int>()
            {
                { "a" , 0 },
                { "b" , 0 },
                { "c" , 0 },
                { "d" , 0 },
                { "e" , 0 },
                { "f" , 0 },
                { "g" , 0 },
                { "h" , 0 },
                { "i" , 0 },
                { "j" , 0 },
                { "k" , 0 },
                { "l" , 0 },
                { "m" , 0 },
                { "n" , 0 },
                { "o" , 0 },
                { "p" , 0 },
                { "q" , 0 },
                { "r" , 0 },
                { "s" , 0 },
                { "t" , 0 },
                { "u" , 0 },
                { "v" , 0 },
                { "w" , 0 },
                { "x" , 0 },
                { "y" , 0 },
                { "z" , 0 },

            };

            Dictionary<string, int> bestPolymerLengthCopy = new Dictionary<string, int>(bestPolymerLength);

            foreach (KeyValuePair<string, int> letter in bestPolymerLengthCopy)
            {
                bestPolymerLength[letter.Key] = ReactPolymer(polymer.Replace(letter.Key, "").Replace(letter.Key.ToUpper(), "")).Length;
                //Console.WriteLine("{0} {1}", letter.Key, bestPolymerLength[letter.Key]);
            }

            List<KeyValuePair<string, int>> bestPolymerLengthList = bestPolymerLength.OrderBy(letter => letter.Value).ToList();
            Console.WriteLine(bestPolymerLengthList[0].Value);
            Console.ReadLine();
        }
    }
}
