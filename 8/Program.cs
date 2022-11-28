using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _8
{
    class Program
    {
        static int Indentations = -1;

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            string[] numbersString = inputList[0].Split(' ');
            List<int> numbers = new List<int>();
            foreach (string numberString in numbersString)
            {
                numbers.Add(Int32.Parse(numberString));
            }

            ProcessNodeReturn processNodeReturn = ProcessNode(numbers, 0);
            TreeNode treeRoot = processNodeReturn.TreeNode;
            Console.WriteLine("Sum: {0}",treeRoot.TotalSumOfMetaData);
            Console.WriteLine("Value: {0}",treeRoot.Value);
            Console.ReadLine();
        }

        static ProcessNodeReturn ProcessNode(List<int> numbers, int startIndex)
        {
            TreeNode newNode = new TreeNode();

            Indentations++;
            string indentationString = "";
            for (int i = 0; i < Indentations; i++)
            {
                indentationString += "    ";
            }

            AoCUtilities.DebugWriteLine(indentationString + "New Node");
            int childCount = numbers[startIndex];
            int metadataCount = numbers[startIndex + 1];
            AoCUtilities.DebugWrite(indentationString);
            foreach (int number in numbers)
            {
                AoCUtilities.DebugWrite("{0} ", number.ToString().PadLeft(2, ' '));
            }
            AoCUtilities.DebugWriteLine();

            AoCUtilities.DebugWrite(indentationString);
            for (int i = 0; i < startIndex; i++)
            {
                AoCUtilities.DebugWrite("   ");
            }
            AoCUtilities.DebugWriteLine(" ^");
            AoCUtilities.DebugWriteLine(indentationString+"Child Count: {0}", childCount);
            AoCUtilities.DebugWriteLine(indentationString+"Metadata Count: {0}", metadataCount);

            AoCUtilities.DebugReadLine();

            int endOfChildrenIndex = startIndex + 1;
            for (int i = 0; i < childCount; i++)
            {
                ProcessNodeReturn processNodeReturn = ProcessNode(numbers, endOfChildrenIndex + 1);
                endOfChildrenIndex = processNodeReturn.EndIndex;
                newNode.Children.Add(processNodeReturn.TreeNode);
                processNodeReturn.TreeNode.Parent = newNode;
            }

            AoCUtilities.DebugWrite(indentationString + "Saving metadata: ");
            for (int i = 0; i < metadataCount; i++)
            {
                newNode.Metadata.Add(numbers[endOfChildrenIndex + 1 + i]);
                AoCUtilities.DebugWrite("{0}, ", numbers[endOfChildrenIndex + 1 + i]);
            }
            AoCUtilities.DebugWriteLine();
            AoCUtilities.DebugWriteLine();

            endOfChildrenIndex += metadataCount;

            AoCUtilities.DebugWriteLine(indentationString+"Returning to Parent Node");
            AoCUtilities.DebugWriteLine(indentationString+"EndOfChildrenIndex: {0}", endOfChildrenIndex);
            AoCUtilities.DebugWriteLine();
            Indentations--;

            return new ProcessNodeReturn(endOfChildrenIndex, newNode);
        }
    }

    public class ProcessNodeReturn
    {
        public int EndIndex;
        public TreeNode TreeNode;

        public ProcessNodeReturn(int endIndex, TreeNode treeNode)
        {
            EndIndex = endIndex;
            TreeNode = treeNode;  
        }
    }

    public class TreeNode
    {
        public TreeNode Parent = null;
        public List<TreeNode> Children = new List<TreeNode>();

        public List<int> Metadata = new List<int>();

        public int TotalSumOfMetaData
        {
            get
            {
                int sum = Metadata.Sum();
                foreach (TreeNode childNode in Children)
                {
                    sum += childNode.TotalSumOfMetaData;
                }
                return sum;
            }
        }

        public int Value
        {
            get
            {
                if (Children.Count == 0)
                {
                    return Metadata.Sum();
                }
                else
                {
                    int value = 0;
                    foreach (int metadataNumber in Metadata)
                    {
                        int metadataIndex = metadataNumber - 1;
                        if (metadataIndex >= 0 && metadataIndex < Children.Count)
                        {
                            value += Children[metadataIndex].Value;
                        }
                    }
                    return value;
                }
            }
        }
    }
}
