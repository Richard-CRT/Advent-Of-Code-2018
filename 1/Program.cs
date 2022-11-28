using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class Program
    {
        static void Main(string[] args)
        {
            var logFile = File.ReadAllLines("input.txt");
            var logList = new List<string>(logFile);
            List<int> frequencies = new List<int> { 0 };
            int val = 0;

            while (true)
            {
                for (int i = 0; i < logList.Count; i++)
                {
                    int change = Int32.Parse(logList[i]);
                    val += change;
                    if (frequencies.IndexOf(val) != -1)
                    {
                        Console.WriteLine(val);
                        Console.ReadLine();
                    }
                    frequencies.Add(val);
                }
            }
        }
    }
}
