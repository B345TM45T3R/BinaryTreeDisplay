using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeDisplay
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader input = new StreamReader("Trees1.txt"); // "Trees.txt", "Trees1.txt" and "Trees2.txt" exist
            string trees = "";
            while (!input.EndOfStream)
            {
                trees += input.ReadLine() + ",";
            }
            trees = trees.Substring(0, trees.Length - 1);
            string[] TreeList = trees.Split(',');
            BTNode[] RootList = new BTNode[TreeList.Length];

            for (int x = 0; x < TreeList.Length; x++)
            {
                RootList[x] = BuildTree(TreeList[x]);
            }

            Console.WriteLine("Do you want to maximize the display before displaying the trees (matters in Win 10)? If so, do so now. Press any key to continue...");
            Console.ReadKey(true);
            for (int x = 0; x < TreeList.Length; x++)
            {
                Console.WriteLine("TREE {0}:", x + 1);
                Console.WriteLine();
                WriteHeading("My display method - Raw output:");
                displayTree(RootList[x], false, false);
                Console.WriteLine();
                WriteHeading("Existing display method:");
                displayTreeStructure(RootList[x]);
                Console.WriteLine();
                WriteHeading("My display method - Trimmed output:");
                displayTree(RootList[x]);
                Console.WriteLine();
                WriteHeading("Depth first order:");
                DisplayDepthFirst(RootList[x]);
                Console.WriteLine();
                Console.WriteLine("Press any key to continue to process next Tree.");
                Console.WriteLine("=======================================================================================");
                Console.ReadKey(true);
            }
            Console.WriteLine("=========================================");
            Console.WriteLine("END OF FILE REACHED - press Enter to exit");
            Console.WriteLine("=========================================");
            Console.ReadLine();
        }

        // Call my display method
        static void displayTree(BTNode startPoint, bool shrink = true, bool hide = true)
        {
            if (startPoint != null)
            {
                startPoint.displayTreeVisual(shrink, hide);
            }
        }

        // CURRENT DEFAULT DISPLAY
        static void displayTreeStructure(BTNode startPoint)
        // with thanks to Minnaar Fullard (WRA202 Class of 2014)
        // adapted from Will's answer at http://stackoverflow.com/questions/1649027/how-do-i-print-out-a-tree-structure
        {
            if (startPoint != null)
                PrintPretty(startPoint, "", false);
        }

        static void PrintPretty(BTNode node, String indent, bool rightChild)
        {
            String output = Convert.ToString(((string)node.value().ToString()));
            if (!(node.parent() == null))  // node is not root
            {
                if (node.parent().left() == node)  // node is left child
                    output += " ~l";
                else
                    output += " ~r";
            }
            Console.Write(indent);
            if (rightChild)
            {
                Console.Write(" /--");
                indent += " | ";
            }
            else
                if (!(node.parent() == null))
            {
                Console.Write(" \\--");
                indent += "   ";
            }
            Console.WriteLine("{0}", output);
            if (node.left() != null && node.right() != null)
            {
                PrintPretty(node.right(), indent, true);
                PrintPretty(node.left(), indent, false);
            }
            else if (node.left() != null)
            {
                PrintPretty(node.left(), indent, true);
            }
            else if (node.right() != null)
            {
                PrintPretty(node.right(), indent, true);
            }
        }


        static BTNode BuildTree(string notation)
        {
            if (notation.Length == 0 || notation == "()")
                return null;

            BTNode root = null;
            BTNode cur = null;
            bool addLeft = true;

            for (int x = 0; x < notation.Length; x++)
            {
                char curChar = notation[x];
                switch (curChar)
                {
                    case '(':
                        if (cur == null)
                        {
                            cur = new BTNode();
                            root = cur;
                        }
                        else
                        {
                            if (notation[x + 1] != ')')
                            {
                                BTNode newNode = new BTNode(null, null, null, cur);
                                if (cur.left() == null && addLeft)
                                {
                                    cur.setLeft(newNode);
                                }
                                else
                                {
                                    cur.setRight(newNode);
                                    addLeft = true;
                                }
                                cur = newNode;
                            }
                            else
                            {
                                addLeft = false;
                                x++;
                            }
                        }
                        break;

                    case ')':
                        cur = cur.parent();
                        break;

                    default:
                        if (cur.value() != null)
                            cur.setValue(cur.value().ToString() + notation[x].ToString());
                        else
                            cur.setValue(notation[x]);
                        break;
                }
            }
            return root;
        }

        static void DisplayDepthFirst(BTNode node)
        {
            Queue temp = new Queue();
            if (node != null)
                temp.Enqueue(node);
            while (temp.Count > 0)
            {
                BTNode cur = (BTNode)temp.Dequeue();
                Console.Write(cur.value() + " ");
                if (cur.left() != null)
                    temp.Enqueue(cur.left());
                if (cur.right() != null)
                    temp.Enqueue(cur.right());
            }
            Console.WriteLine();
        }

        static void WriteHeading(string text, bool allCaps = true, char underline = '-')
        {
            Console.WriteLine((allCaps) ? text.ToUpper() : text);
            for (int x = 0; x < text.Length; x++)
            {
                Console.Write(underline);
            }
            Console.WriteLine();
        }
    }
}
