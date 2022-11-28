using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _19
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
            int instructionPointer;
            int[] r = new int[6] { 0, 0, 0, 0, 0, 0 };

            
            // Simulating the program on the highest level, no optimisation (incredibly inefficient,
            // hence why it's not feasible to run the program this way for r[0] = 1 initially
            
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
                //AoCUtilities.DebugWrite("commands executed = {17} ip = {0} [{1}, {2}, {3}, {4}, {5}, {6}] {7} {8} {9} {10} [{11}, {12}, {13}, {14}, {15}, {16}]", r[instructionPointerRegister].ToString().PadLeft(2, ' '), r[0].ToString().PadLeft(4, ' '), r[1].ToString().PadLeft(4, ' '), r[2].ToString().PadLeft(4, ' '), r[3].ToString().PadLeft(4, ' '), r[4].ToString().PadLeft(4, ' '), r[5].ToString().PadLeft(4, ' '),
                //                                                                                                                                command.OpCodeString, command.A.ToString().PadLeft(2, ' '), command.B.ToString().PadLeft(2, ' '), command.C.ToString().PadLeft(2, ' '),
                //                                                                                                                                newR[0].ToString().PadLeft(4, ' '), newR[1].ToString().PadLeft(4, ' '), newR[2].ToString().PadLeft(4, ' '), newR[3].ToString().PadLeft(4, ' '), newR[4].ToString().PadLeft(4, ' '), newR[5].ToString().PadLeft(4, ' '), commandsExecuted.ToString().PadLeft(5, ' '));
                //AoCUtilities.DebugReadLine();

                r = newR;
                instructionPointer = r[instructionPointerRegister];
                instructionPointer++;
            }

            //Console.WriteLine("Commands executed: {0}", commandsExecuted);
            Console.WriteLine("[{0}, {1}, {2}, {3}, {4}, {5}]", r[0], r[1], r[2], r[3], r[4], r[5]);
            Console.ReadLine();
            

            
            // Converted assembly/opcodes into register transfer notation (note replacement of [3] with the line number as line number === program counter)
            // Slightly more efficient as can do away with the opcodes entirely, however this is hardcoded with the instruction pointer register == 3
            
            instructionPointer = 0;
            r = new int[6] { 0, 0, 0, 0, 0, 0 };

            Action[] assembly = new Action[]
            {
                new Action(() => { r[3] = 16; }),
                new Action(() => { r[2] = 1; }),
                new Action(() => { r[4] = 1; }),
                new Action(() => { r[1] = r[2] * r[4]; }),
                new Action(() => { r[1] = r[1] == r[5] ? 1 : 0; }),
                new Action(() => { r[3] = r[1] + 5; }),
                new Action(() => { r[3] = 7; }),
                new Action(() => { r[0] = r[2] + r[0]; }),
                new Action(() => { r[4] = r[4] + 1; }),
                new Action(() => { r[1] = r[4] > r[5] ? 1 : 0; }),
                new Action(() => { r[3] = r[1] + 10; }),
                new Action(() => { r[3] = 2; }),
                new Action(() => { r[2] = r[2] + 1; }),
                new Action(() => { r[1] = r[2] > r[5] ? 1 : 0; }),
                new Action(() => { r[3] = r[1] + 14; }),
                new Action(() => { r[3] = 1; }),
                new Action(() => { r[3] = 256; }),
                new Action(() => { r[5] = r[5] + 2; }),
                new Action(() => { r[5] = r[5] * r[5]; }),
                new Action(() => { r[5] = r[5] * 19; }),
                new Action(() => { r[5] = r[5] * 11; }),
                new Action(() => { r[1] = r[1] + 3; }),
                new Action(() => { r[1] = r[1] * 22; }),
                new Action(() => { r[1] = r[1] + 12; }),
                new Action(() => { r[5] = r[5] + r[1]; }),
                new Action(() => { r[3] = r[0] + 25; }),
                new Action(() => { r[3] = 0; }),
                new Action(() => { r[1] = 27; }),
                new Action(() => { r[1] = r[1] * 28; }),
                new Action(() => { r[1] = r[1] + 29; }),
                new Action(() => { r[1] = r[1] * 30; }),
                new Action(() => { r[1] = r[1] * 14; }),
                new Action(() => { r[1] = r[1] * 32; }),
                new Action(() => { r[5] = r[5] + r[1]; }),
                new Action(() => { r[0] = 0; }),
                new Action(() => { r[3] = 0; }),
            };

            while (instructionPointer >= 0 && instructionPointer < 36)
            {
                r[3] = instructionPointer;
                assembly[instructionPointer]();
                instructionPointer = r[3];
                instructionPointer++;
                //AoCUtilities.DebugWrite("[ {0} {1} {2} {3} {4} {5}]", r[0].ToString().PadLeft(4, ' '), r[1].ToString().PadLeft(4, ' '), r[2].ToString().PadLeft(4, ' '), r[3].ToString().PadLeft(4, ' '), r[4].ToString().PadLeft(4, ' '), r[5].ToString().PadLeft(4, ' '));
                //AoCUtilities.DebugReadLine();
            }
            Console.WriteLine("[ {0}, {1}, {2}, {3}, {4}, {5}]", r[0], r[1], r[2], r[3], r[4], r[5]);
            Console.ReadLine();
            

            
            // Register notation reverse engineered to basic C#, definitely not the most efficient algorithm,
            // but clear enough to see what the program does (r[0] <- sum of factors of r[5]) and what to do next.
            // It is clear from the setup code that r[0] = 1 initially is going to be the longest running program

            r = new int[6] { 0, 0, 0, 0, 0, 0 };

            void nestedloops()
            {
                r[2] = 1;

                do
                {
                    r[4] = 1;

                    do
                    {
                        if (r[2] * r[4] == r[5])
                        {
                            r[0] += r[2];

                        }
                        r[4] += 1;

                    }
                    while (r[4] <= r[5]);


                    r[2] += 1;

                }
                while (r[2] <= r[5]);
            }

            r[5] = ((int)Math.Pow(r[5] + 2, 2) * 209) + (22 * r[1]) + 78;
            if (r[0] == 0)
            {
                nestedloops();
            }
            else
            {
                if (r[0] <= 1)
                {
                    r[1] = 27;

                }
                if (r[0] <= 2)
                {
                    r[1] = r[1] * 28;

                }
                if (r[0] <= 3)
                {
                    r[1] += 29;

                }
                if (r[0] <= 4)
                {
                    r[1] = r[1] * 30;

                }
                if (r[0] <= 5)
                {
                    r[1] = r[1] * 14;

                }
                if (r[0] <= 6)
                {
                    r[1] = r[1] * 32;

                }
                if (r[0] <= 7)
                {
                    r[5] += r[1];

                }
                if (r[0] <= 8)
                {
                    r[0] = 0;

                }
                if (r[0] <= 9)
                {
                    nestedloops();
                }
            }
            
            Console.WriteLine("r[2] and r[4] are local iteration variables, r[3] is unused");
            Console.WriteLine("[ {0}, {1}, {2}, {3}, {4}, {5}]", r[0], r[1], r[2], r[3], r[4], r[5]);
            Console.ReadLine();
            


            // Much more efficient algorithm to achieve the same purpose, no longer shows any similarity to the original
            // assembly, since I've used the modulo operator (note the setup part of the code is the same, although with
            // r[0] = 0 or 1 initially it does not need to be this complex)

            r = new int[6] { 0, 0, 0, 0, 0, 0 };

            void factors()
            {
                for (int i = 1; i <= r[5]; i++)
                {
                    if (r[5] % i == 0)
                    {
                        r[0] += i;
                    }
                }
            }

            r[5] = ((int)Math.Pow(r[5] + 2, 2) * 209) + (22 * r[1]) + 78;
            if (r[0] == 0)
            {
                factors();
            }
            else
            {
                if (r[0] <= 1)
                {
                    r[1] = 27;

                }
                if (r[0] <= 2)
                {
                    r[1] = r[1] * 28;
                }
                if (r[0] <= 3)
                {
                    r[1] += 29;

                }
                if (r[0] <= 4)
                {
                    r[1] = r[1] * 30;

                }
                if (r[0] <= 5)
                {
                    r[1] = r[1] * 14;

                }
                if (r[0] <= 6)
                {
                    r[1] = r[1] * 32;

                }
                if (r[0] <= 7)
                {
                    r[5] += r[1];

                }
                if (r[0] <= 8)
                {
                    r[0] = 0;

                }
                if (r[0] <= 9)
                {
                    factors();
                }
            }

            Console.WriteLine("r[2], r[3] and r[4] are unused");
            Console.WriteLine("[ {0}, {1}, {2}, {3}, {4}, {5}]", r[0], r[1], r[2], r[3], r[4], r[5]);
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
