using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = File.ReadAllLines("input.txt");
            var inputList = new List<string>(inputFile);

            List<InstructionGraphNode> graphNodes = new List<InstructionGraphNode>();

            for (int i = 0; i < inputList.Count; i++)
            {
                string requirement = inputList[i];
                char parentLetter = requirement.Substring(5, 1)[0];
                char childLetter = requirement.Substring(36, 1)[0];

                InstructionGraphNode parentNode = null;
                for (int n = 0; n < graphNodes.Count; n++)
                {
                    if (graphNodes[n].Letter == parentLetter)
                    {
                        parentNode = graphNodes[n];
                        break;
                    }
                }
                if (parentNode == null)
                {
                    parentNode = new InstructionGraphNode(parentLetter);
                    graphNodes.Add(parentNode);
                }

                InstructionGraphNode childNode = null;
                for (int n = 0; n < graphNodes.Count; n++)
                {
                    if (graphNodes[n].Letter == childLetter)
                    {
                        childNode = graphNodes[n];
                        break;
                    }
                }
                if (childNode == null)
                {
                    childNode = new InstructionGraphNode(childLetter);
                    graphNodes.Add(childNode);
                }

                parentNode.Children.Add(childNode);
                childNode.Parents.Add(parentNode);
            }

            int availableWorkers = 5;
            int seconds = 0;

            // populated graph
            List<InstructionGraphNode> freeNodes = CheckFreeNodes(graphNodes);
            List<InstructionGraphNode> workingNodes = new List<InstructionGraphNode>();
            while (graphNodes.Count > 0)
            {
                while (availableWorkers > 0 && freeNodes.Count > 0)
                {
                    freeNodes = freeNodes.OrderBy(freeNode => freeNode.Letter).ToList();

                    freeNodes[0].Working = true;
                    workingNodes.Add(freeNodes[0]);
                    freeNodes.Remove(freeNodes[0]);

                    availableWorkers--;
                }

                foreach (InstructionGraphNode workingNode in workingNodes)
                {
                    workingNode.TimeLeft--;
                }

                //Console.WriteLine("{0} | Working:", seconds);
                for (int i = 0; i < workingNodes.Count;)
                {
                    InstructionGraphNode workingNode = workingNodes[i];
                    //Console.WriteLine("{0} : {1}", workingNode.Letter, workingNode.TimeLeft);
                    if (workingNode.TimeLeft == 0)
                    {
                        Console.Write("{0}", workingNode.Letter);
                        graphNodes.Remove(workingNode);
                        foreach (InstructionGraphNode childNode in workingNode.Children)
                        {
                            childNode.Parents.Remove(workingNode);
                        }
                        workingNodes.Remove(workingNode);

                        availableWorkers++;
                    }
                    else
                    {
                        i++;
                    }
                }

                seconds++;

                freeNodes = CheckFreeNodes(graphNodes);
            }
            Console.WriteLine();
            Console.WriteLine(seconds);
            Console.ReadLine();
        }

        static List<InstructionGraphNode> CheckFreeNodes(List<InstructionGraphNode> graphNodes)
        {
            List<InstructionGraphNode> freeNodes = new List<InstructionGraphNode>();
            for (int i = 0; i < graphNodes.Count; i++)
            {
                if (graphNodes[i].Parents.Count == 0 && !graphNodes[i].Working)
                {
                    freeNodes.Add(graphNodes[i]);
                }
            }
            return freeNodes;

        }
    }

    public class InstructionGraphNode
    {
        // Technically only need one of these but helps code efficiency to keep track of both (think doubly linked list)
        public List<InstructionGraphNode> Parents = new List<InstructionGraphNode>();
        public List<InstructionGraphNode> Children = new List<InstructionGraphNode>();
        public int TimeLeft;
        public bool Working = false;
        public char Letter;

        public InstructionGraphNode(char letter)
        {
            Letter = letter;
            TimeLeft = letter - 64 + 60;
        }
    }
}
