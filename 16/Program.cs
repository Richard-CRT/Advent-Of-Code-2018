using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _16
{
    public delegate int[] OpCodeDelegate(int[] registers, int A, int B, int C);

    class Program
    {
        public static List<OpCodeDelegate>[] OpCodeLinks = new List<OpCodeDelegate>[16];

        static void Main(string[] args)
        {
            for (int n = 0; n < 16; n++)
            {
                OpCodeLinks[n] = new List<OpCodeDelegate> { OpCodes.addr, OpCodes.addi, OpCodes.mulr, OpCodes.muli,
                                                            OpCodes.banr, OpCodes.bani, OpCodes.borr, OpCodes.bori,
                                                            OpCodes.setr, OpCodes.seti, OpCodes.gtir, OpCodes.gtri,
                                                            OpCodes.gtrr, OpCodes.eqir, OpCodes.eqri, OpCodes.eqrr };
                }

            List<string> inputList = AoCUtilities.GetInput();

            List<Sample> samples = new List<Sample>();

            int i = 0;
            while (i < inputList.Count && inputList[i].Length > 0 && inputList[i][0] == 'B')
            {
                string beforeString = inputList[i].Substring(9, 10).Replace(",", "");
                string commandString = inputList[i + 1];
                string afterString = inputList[i + 2].Substring(9, 10).Replace(",", ""); ;

                string[] beforeStringParts = beforeString.Split(' ');
                string[] commandStringParts = commandString.Split(' ');
                string[] afterStringParts = afterString.Split(' ');

                int[] registersBefore = new int[4];
                int OpCode, A, B, C;
                int[] registersAfter = new int[4];

                registersBefore[0] = Int32.Parse(beforeStringParts[0]);
                registersBefore[1] = Int32.Parse(beforeStringParts[1]);
                registersBefore[2] = Int32.Parse(beforeStringParts[2]);
                registersBefore[3] = Int32.Parse(beforeStringParts[3]);

                OpCode = Int32.Parse(commandStringParts[0]);
                A = Int32.Parse(commandStringParts[1]);
                B = Int32.Parse(commandStringParts[2]);
                C = Int32.Parse(commandStringParts[3]);

                registersAfter[0] = Int32.Parse(afterStringParts[0]);
                registersAfter[1] = Int32.Parse(afterStringParts[1]);
                registersAfter[2] = Int32.Parse(afterStringParts[2]);
                registersAfter[3] = Int32.Parse(afterStringParts[3]);

                samples.Add(new Sample(registersBefore, registersAfter, OpCode, A, B, C));

                i += 4;
            }

            int samplesWhichCouldBe3OrMore = 0;

            foreach (Sample sample in samples)
            {
                //Console.WriteLine();

                if (sample.CheckOpcodes() >= 3)
                {
                    samplesWhichCouldBe3OrMore++;
                }
            }

            Console.WriteLine("There are {0} samples that behave like 3 or more opcodes", samplesWhichCouldBe3OrMore);

            int numberOfUnknowns = 1;
            while (numberOfUnknowns != 0)
            {
                numberOfUnknowns = 0;
                for (int n = 0; n < 16; n++)
                {
                    if (OpCodeLinks[n].Count == 1)
                    {
                        for (int m = 0; m < 16; m++)
                        {
                            if (n != m)
                            {
                                OpCodeLinks[m].Remove(OpCodeLinks[n][0]);
                            }
                        }
                    }
                    else
                    {
                        numberOfUnknowns++;
                    }
                }
            }

            Console.WriteLine("Successfully derived OpCode association");

            int[] registers = new int[] { 0, 0, 0, 0 };
            for (int n = i; n < inputList.Count; n++)
            {
                if (inputList[n] != "")
                {
                    string commandString = inputList[n];
                    string[] commandStringParts = commandString.Split(' ');
                    int OpCode, A, B, C;
                    OpCode = Int32.Parse(commandStringParts[0]);
                    A = Int32.Parse(commandStringParts[1]);
                    B = Int32.Parse(commandStringParts[2]);
                    C = Int32.Parse(commandStringParts[3]);

                    registers = OpCodeLinks[OpCode][0](registers, A, B, C);
                }
            }

            Console.WriteLine("Registers:");
            Console.WriteLine("[ {0}, {1}, {2}, {3} ]", registers[0], registers[1], registers[2], registers[3]);
            Console.ReadLine();
        }
    }

    public class Sample
    {
        public int[] RegistersBefore;
        public int OpCode;
        public int A;
        public int B;
        public int C;
        public int[] RegistersAfter;

        public Sample(int[] registersBefore, int[] registersAfter, int opcode, int a, int b, int c)
        {
            RegistersBefore = registersBefore;
            OpCode = opcode;
            A = a;
            B = b;
            C = c;
            RegistersAfter = registersAfter;
        }

        public int CheckOpcodes()
        {
            //Console.WriteLine("New Sample:");

            int opCodeMatches = 0;

            int[] opcodeRegistersAfter;
            OpCodeDelegate testAction;

            List<OpCodeDelegate> possibleOpCodes = new List<OpCodeDelegate>();

            //Console.WriteLine("Checking: addr");
            testAction = OpCodes.addr;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: addr");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: addi");
            testAction = OpCodes.addi;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: addi");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: mulr");
            testAction = OpCodes.mulr;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: mulr");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: muli");
            testAction = OpCodes.muli;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: muli");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: banr");
            testAction = OpCodes.banr;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: banr");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: bani");
            testAction = OpCodes.bani;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: bani");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: borr");
            testAction = OpCodes.borr;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: borr");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: bori");
            testAction = OpCodes.bori;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: bori");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: setr");
            testAction = OpCodes.setr;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: setr");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: seti");
            testAction = OpCodes.seti;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: seti");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: gtir");
            testAction = OpCodes.gtir;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: gtir");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: gtri");
            testAction = OpCodes.gtri;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: gtri");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: gtrr");
            testAction = OpCodes.gtrr;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: gtrr");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: eqir");
            testAction = OpCodes.eqir;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: eqir");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: eqri");
            testAction = OpCodes.eqri;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: eqri");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            //Console.WriteLine("Checking: eqrr");
            testAction = OpCodes.eqrr;
            opcodeRegistersAfter = testAction(this.RegistersBefore, this.A, this.B, this.C);
            if (CompareRegisters(this.RegistersAfter, opcodeRegistersAfter))
            {
                //Console.WriteLine("Could be: eqrr");
                opCodeMatches++;
                possibleOpCodes.Add(testAction);
            }

            for (int i = 0; i < Program.OpCodeLinks[this.OpCode].Count;)
            {
                if (!possibleOpCodes.Contains(Program.OpCodeLinks[this.OpCode][i]))
                {
                    Program.OpCodeLinks[this.OpCode].RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            return opCodeMatches;
        }

        bool CompareRegisters(int[] registersA, int[] registersB)
        {
            return (registersA[0] == registersB[0]
                && registersA[1] == registersB[1]
                && registersA[2] == registersB[2]
                && registersA[3] == registersB[3]);
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
