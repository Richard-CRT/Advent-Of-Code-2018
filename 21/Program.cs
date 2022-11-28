using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _21
{
    public delegate int[] OpCodeDelegate(int[] registers, int A, int B, int C);

    class Program
    {

        public static Dictionary<string, OpCodeDelegate> opCodeLink = new Dictionary<string, OpCodeDelegate>()
        {
            { "addr", OpCodes.addr },
            { "addi", OpCodes.addi },
            { "mulr", OpCodes.mulr },
            { "muli", OpCodes.muli },
            { "banr", OpCodes.banr },
            { "bani", OpCodes.bani },
            { "borr", OpCodes.borr },
            { "bori", OpCodes.bori },
            { "setr", OpCodes.setr },
            { "seti", OpCodes.seti },
            { "gtir", OpCodes.gtir },
            { "gtri", OpCodes.gtri },
            { "gtrr", OpCodes.gtrr },
            { "eqir", OpCodes.eqir },
            { "eqri", OpCodes.eqri },
            { "eqrr", OpCodes.eqrr },
        };

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            int[] r = new int[6] { 0, 0, 0, 0, 0, 0 };


            // Simulating the program on the highest level, no optimisation (incredibly inefficient,
            // hence why it's not feasible to run the program this way for r[0] = 1 initially


            /*
            int instructionPointer;
            int instructionPointerRegister;
            instructionPointerRegister = Int32.Parse(inputList[0].Substring(4));
            instructionPointer = 0;

            List<Command> Commands = new List<Command>();

            for (int i = 1; i < inputList.Count; i++)
            {
                string[] commandStringParts = inputList[i].Split(' ');

                OpCodeDelegate OpCode = opCodeLink[commandStringParts[0]];
                int A = Int32.Parse(commandStringParts[1]);
                int B = Int32.Parse(commandStringParts[2]);
                int C = Int32.Parse(commandStringParts[3]);

                Commands.Add(new Command(commandStringParts[0], OpCode, A, B, C));
            }

            int commandsExecuted = 0;
            int[] newR;
            while (instructionPointer >= 0 && instructionPointer < Commands.Count)
            {
                r[instructionPointerRegister] = instructionPointer;
                //Console.WriteLine(registers[InstructionPointerRegister]);


                Command command = Commands[r[instructionPointerRegister]];

                newR = command.OpCode(r, command.A, command.B, command.C);

                commandsExecuted++;

                if (instructionPointer > 25)
                {
                    AoCUtilities.DebugWrite("commands executed = {17} ip = {0} [{1}, {2}, {3}, {4}, {5}, {6}] {7} {8} {9} {10} [{11}, {12}, {13}, {14}, {15}, {16}]", r[instructionPointerRegister].ToString().PadLeft(2, ' '), r[0].ToString().PadLeft(4, ' '), r[1].ToString().PadLeft(4, ' '), r[2].ToString().PadLeft(4, ' '), r[3].ToString().PadLeft(4, ' '), r[4].ToString().PadLeft(4, ' '), r[5].ToString().PadLeft(4, ' '),
                                                                                                                                                   command.OpCodeString, command.A.ToString().PadLeft(2, ' '), command.B.ToString().PadLeft(2, ' '), command.C.ToString().PadLeft(2, ' '),
                                                                                                                                                    newR[0].ToString().PadLeft(4, ' '), newR[1].ToString().PadLeft(4, ' '), newR[2].ToString().PadLeft(4, ' '), newR[3].ToString().PadLeft(4, ' '), newR[4].ToString().PadLeft(4, ' '), newR[5].ToString().PadLeft(4, ' '), commandsExecuted.ToString().PadLeft(5, ' '));
                    AoCUtilities.DebugReadLine();
                }

                r = newR;
                instructionPointer = r[instructionPointerRegister];
                instructionPointer++;
            }

            //Console.WriteLine("Commands executed: {0}", commandsExecuted);
            Console.WriteLine("[{0}, {1}, {2}, {3}, {4}, {5}]", r[0], r[1], r[2], r[3], r[4], r[5]);
            Console.ReadLine();
            */

            /*
            instructionPointer = 0;
            r = new int[6] { 0, 0, 0, 0, 0, 0 };

            Action[] assembly = new Action[]
            {
                new Action(() => { r[4] = 123; }),
                new Action(() => { r[4] = r[4] & 456; }),
                new Action(() => { r[4] = r[4] == 72 ? 1 : 0; }),
                new Action(() => { r[5] = r[4] + 3; }),
                new Action(() => { r[5] = 0; }),
                new Action(() => { r[4] = 0; }),
                new Action(() => { r[1] = r[4] | 65536; }),
                new Action(() => { r[4] = 2024736; }),
                new Action(() => { r[2] = r[1] & 255; }),
                new Action(() => { r[4] = r[4] + r[2]; }),
                new Action(() => { r[4] = r[4] & 16777215; }),
                new Action(() => { r[4] = r[4] * 65899; }),
                new Action(() => { r[4] = r[4] & 16777215; }),
                new Action(() => { r[2] = 256 > r[1] ? 1 : 0; }),
                new Action(() => { r[5] = r[2] + 14; }),
                new Action(() => { r[5] = 15 + 1; }),
                new Action(() => { r[5] = 27; }),
                new Action(() => { r[2] = 0; }),
                new Action(() => { r[3] = r[2] + 1; }),
                new Action(() => { r[3] = r[3] * 256; }),
                new Action(() => { r[3] = r[3] > r[1] ? 1 : 0; }),
                new Action(() => { r[5] = r[3] + 21; }),
                new Action(() => { r[5] = 22 + 1; }),
                new Action(() => { r[5] = 25; }),
                new Action(() => { r[2] = r[2] + 1; }),
                new Action(() => { r[5] = 17; }),
                new Action(() => { r[1] = r[2]; }),
                new Action(() => { r[5] = 7; }),
                new Action(() => { r[2] = r[4] == r[0] ? 1 : 0; }),
                new Action(() => { r[5] = r[2] + 29; }),
                new Action(() => { r[5] = 5; }),
            };

            while (instructionPointer >= 0 && instructionPointer < 36)
            {
                r[5] = instructionPointer;
                assembly[instructionPointer]();
                instructionPointer = r[5];
                if (instructionPointer == 28)
                {
                    AoCUtilities.DebugWrite("[ {0} {1} {2} {3} {4} {5}]", r[0].ToString().PadLeft(4, ' '), r[1].ToString().PadLeft(4, ' '), r[2].ToString().PadLeft(4, ' '), r[3].ToString().PadLeft(4, ' '), r[4].ToString().PadLeft(4, ' '), r[5].ToString().PadLeft(4, ' '));
                    AoCUtilities.DebugReadLine();
                }
                instructionPointer++;
            }
            Console.WriteLine("[ {0}, {1}, {2}, {3}, {4}, {5}]", r[0], r[1], r[2], r[3], r[4], r[5]);
            Console.ReadLine();
            */

            /*
            r[4] = 123;
            do
            {
                r[4] = r[4] & 456;
            }
            while (r[4] != 72);
            r[4] = 0;
            do
            {
                r[1] = r[4] | 65536;
                r[4] = 2024736;
                do
                {
                    r[2] = r[1] & 255;
                    r[4] = r[4] + r[2];
                    r[4] = r[4] & 16777215;
                    r[4] = r[4] * 65899;
                    r[4] = r[4] & 16777215;
                    if (256 > r[1])
                    {
                        break;
                    }
                    r[2] = 0;
                    do
                    {
                        r[3] = r[2] + 1;
                        r[3] = r[3] * 256;
                        if (r[3] > r[1])
                        {
                            break;
                        }
                        r[2] = r[2] + 1;
                    }
                    while (true);
                    r[1] = r[2];
                }
                while (true);
                Console.WriteLine("[{0}, {1}, {2}, {3}, {4}, {5}]", r[0], r[1], r[2], r[3], r[4], r[5]);
                Console.ReadLine();
            }
            while (r[4] != r[0]);
            */

            List<int> valuesOfr4 = new List<int>();
            bool firstRun = true;

            r[4] = 123;
            do
            {
                r[4] = r[4] & 456;
            }
            while (r[4] != 72);
            r[4] = 0;
            do
            {
                r[1] = r[4] | 65536;
                r[4] = 2024736;
                do
                {
                    r[2] = r[1] & 255;
                    r[4] = r[4] + r[2];
                    r[4] = r[4] & 16777215;
                    r[4] = r[4] * 65899;
                    r[4] = r[4] & 16777215;
                    if (256 > r[1])
                    {
                        break;
                    }
                    r[1] = r[1] / 256;
                }
                while (true);
                if (firstRun)
                {
                    firstRun = false;
                    Console.WriteLine("Part 1:");
                    Console.WriteLine("{0}", r[4]);
                }
                if (valuesOfr4.IndexOf(r[4]) == -1)
                {
                    valuesOfr4.Add(r[4]);
                }
                else
                {
                    Console.WriteLine("Part 2:");
                    Console.WriteLine("{0}", valuesOfr4[valuesOfr4.Count - 1]);
                    break;
                }
            }
            while (r[4] != r[0]);

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }

    public class Command
    {
        public string OpCodeString;
        public OpCodeDelegate OpCode;
        public int A;
        public int B;
        public int C;

        public Command(string opCodeString, OpCodeDelegate opCode, int a, int b, int c)
        {
            OpCodeString = opCodeString;
            OpCode = opCode;
            A = a;
            B = b;
            C = c;
        }
    }

    public static class OpCodes
    {
        public static int[] addr(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A] + registers[B];
            return newRegisters;
        }

        public static int[] addi(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A] + B;
            return newRegisters;
        }

        public static int[] mulr(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A] * registers[B];
            return newRegisters;
        }

        public static int[] muli(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A] * B;
            return newRegisters;
        }

        public static int[] banr(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A] & registers[B];
            return newRegisters;
        }

        public static int[] bani(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A] & B;
            return newRegisters;
        }

        public static int[] borr(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A] | registers[B];
            return newRegisters;
        }

        public static int[] bori(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A] | B;
            return newRegisters;
        }

        public static int[] setr(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = registers[A];
            return newRegisters;
        }

        public static int[] seti(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            newRegisters[C] = A;
            return newRegisters;
        }

        public static int[] gtir(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            if (A > registers[B])
            {
                newRegisters[C] = 1;
            }
            else
            {
                newRegisters[C] = 0;
            }
            return newRegisters;
        }

        public static int[] gtri(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            if (registers[A] > B)
            {
                newRegisters[C] = 1;
            }
            else
            {
                newRegisters[C] = 0;
            }
            return newRegisters;
        }

        public static int[] gtrr(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            if (registers[A] > registers[B])
            {
                newRegisters[C] = 1;
            }
            else
            {
                newRegisters[C] = 0;
            }
            return newRegisters;
        }

        public static int[] eqir(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            if (A == registers[B])
            {
                newRegisters[C] = 1;
            }
            else
            {
                newRegisters[C] = 0;
            }
            return newRegisters;
        }

        public static int[] eqri(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            if (registers[A] == B)
            {
                newRegisters[C] = 1;
            }
            else
            {
                newRegisters[C] = 0;
            }
            return newRegisters;
        }

        public static int[] eqrr(int[] registers, int A, int B, int C)
        {
            int[] newRegisters = registers.Clone() as int[];
            if (registers[A] == registers[B])
            {
                newRegisters[C] = 1;
            }
            else
            {
                newRegisters[C] = 0;
            }
            return newRegisters;
        }
    }
}
