using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeDisplay
{
    public class BTNode   // NO MODIFICATIONS TO BE MADE TO THIS CLASS (PRAC INSTRUCTIONS)
    {
        private Object Value;
        private BTNode Left;
        private BTNode Right;
        private BTNode Parent;

        public BTNode()
        {
            Value = null;
            Left = null;
            Right = null;
            Parent = null;
        }

        public BTNode(Object Data)
        {
            Value = Data;
            Left = null;
            Right = null;
            Parent = null;
        }

        public BTNode(Object Data, BTNode L, BTNode R, BTNode P)
        {
            Value = Data;
            setLeft(L);
            setRight(R);
            Parent = P;
        }

        public BTNode left()
        {
            return Left;
        }

        public BTNode right()
        {
            return Right;
        }

        public BTNode parent()
        {
            return Parent;
        }

        public void setLeft(BTNode newLeft)
        {
            Left = newLeft;
            if (newLeft != null)
                newLeft.setParent(this);
        }

        public void setRight(BTNode newRight)
        {
            Right = newRight;
            if (newRight != null)
                newRight.setParent(this);
        }

        public void setParent(BTNode newParent)
        {
            Parent = newParent;
        }

        public Object value()
        {
            return Value;
        }

        public void setValue(Object Data)
        {
            Value = Data;
        }

        public BTNode clear()
        {
            return null;
        }
        public bool isLeft()
        {
            return Parent.left() == this;
        }

        // Additional Methods


        public bool isRoot()
        {
            return Parent == null;
        }


        public bool leafNode()
        /* post: Return true if node is a leaf node, otherwise return false. */
        {
            return (left() == null && right() == null);
        }

        /*====================================*
         * DISPLAYING THE TREE AS A STRUCTURE *
         *====================================*/

        public void displayTreeVisual(bool AutoShrink = true, bool hideNull = true)
        {
            //SETTING UP
            int height = FindTreeHeight(this, 0); //Height of the Tree
            int placeHolderSize = FindLongestValue(this); //Length of the longest "label" in the node

            if (height > 20) //2^20 nodes
            {
                Console.WriteLine("This tree is very large to process (Depth: {0}). OutOfMemory Exception is very likely to occur (on a machine with 6GB RAM). It is recommended to abort the display now.", height);
                if (!stillContinue(true))
                {
                    return;
                }
            }

            if (Math.Pow(2, height) * (2 + 2 * placeHolderSize) - placeHolderSize > short.MaxValue)
            {
                Console.WriteLine("This tree will most likely fail to display - based on the following assumptions:");
                Console.WriteLine("      1. The tree is either a full or perfect tree.");
                Console.WriteLine("      2. All the node labels are {0} characters long.", placeHolderSize);
                if (!stillContinue(true))
                {
                    return;
                }
            }



            //Building up useful place holder strings for use later
            string placeholder = "";
            for (int x = 0; x < placeHolderSize; x++)
                placeholder += " ";

            string leftBranch = placeholder + " /";
            string rightBranch = "\\ " + placeholder;

            string nullPlaceHolder = placeholder.Replace(' ', '*') + "**";
            string nullLeftBranch = placeholder + " *";
            string nullRightBranch = "* " + placeholder;


            //Initialize and populate "canvas" array
            StringBuilder[] treeToPrint = new StringBuilder[2 * height + 1]; //The "canvas" for the tree
            for (int x = 0; x < treeToPrint.Length; x++)
            {
                treeToPrint[x] = new StringBuilder("");
            }

            //CALCULATING FORMATTING REQUIREMENTS
            //Initialize and populate "Format Template" arrays
            string[] leftMargin = new string[height + 1];
            StringBuilder[] spacers = new StringBuilder[height + 1];
            string[] rightMargin = new string[height + 1];
            for (int x = 0; x < leftMargin.Length; x++)
            {
                leftMargin[x] = "";
                spacers[x] = new StringBuilder("");
                rightMargin[x] = "";
            }

            //Set up formatting template
            int bottomLevel = height;
            leftMargin[bottomLevel] = "";
            spacers[bottomLevel].Append(placeholder);
            rightMargin[bottomLevel] = "";

            int coeff = 1;
            int[] UnderscoreCounts = new int[height + 1];
            UnderscoreCounts[0] = placeHolderSize;

            for (int level = height - 1; level >= 0; level--)
            {
                coeff += (int)Math.Pow(2, height - level);
                int spaceCount = (coeff * placeHolderSize) + coeff - 1;
                UnderscoreCounts[height - level] = spaceCount;

                int nrUnderscores = 0;
                if (height - level >= 2)
                    nrUnderscores = UnderscoreCounts[height - level - 2];

                StringBuilder spaceInsert = new StringBuilder();
                for (int x = 0; x < spaceCount; x++)
                {
                    if (spaceInsert.Length < nrUnderscores || spaceInsert.Length >= spaceCount - nrUnderscores && nrUnderscores != 0)
                        spaceInsert.Append("_");
                    else
                        spaceInsert.Append(" ");
                }
                spacers[level] = spaceInsert;
                int trimmedLength = (spaceInsert.Length - placeHolderSize) / 2;
                leftMargin[level] = spaceInsert.ToString().Substring(spaceInsert.Length - trimmedLength);
                rightMargin[level] = spaceInsert.ToString().Remove(trimmedLength);
            }

            //CREATING THE TREE
            int curLevel = 0;
            int nodeSpaceCounter = 0;
            int maxNodeLevelSpaces = (int)Math.Pow(2, 0) - 1;
            int branchSpaceCounter = 0;
            int maxBranchLevelSpaces = (int)Math.Pow(2, 1) - 1;
            Queue temp = new Queue();
            temp.Enqueue(this);
            temp.Enqueue(curLevel);
            treeToPrint[2 * curLevel].Append(leftMargin[curLevel]);
            if (curLevel < height)
                treeToPrint[2 * curLevel + 1].Append(leftMargin[curLevel + 1].Replace('_', ' '));
            while (temp.Count > 0 && curLevel <= height)
            {
                if (temp.Peek() is int)
                {
                    treeToPrint[2 * curLevel].Append(rightMargin[curLevel]);
                    if (curLevel < height)
                        treeToPrint[2 * curLevel + 1].Append(rightMargin[curLevel + 1].Replace('_', ' '));
                    curLevel = (int)temp.Dequeue() + 1;
                    if (curLevel > height) break;
                    treeToPrint[2 * curLevel].Append(leftMargin[curLevel]);
                    if (curLevel < height)
                        treeToPrint[2 * curLevel + 1].Append(leftMargin[curLevel + 1].Replace('_', ' '));
                    temp.Enqueue(curLevel);

                    nodeSpaceCounter = 0;
                    maxNodeLevelSpaces = (int)Math.Pow(2, curLevel) - 1;
                    branchSpaceCounter = 0;
                    maxBranchLevelSpaces = (int)Math.Pow(2, curLevel + 1) - 1;
                }
                else
                {
                    BTNode curNode = (BTNode)temp.Dequeue();

                    if (curNode == null)
                    {
                        treeToPrint[2 * curLevel].Append(nullPlaceHolder);
                        if (nodeSpaceCounter < maxNodeLevelSpaces)
                        {
                            treeToPrint[2 * curLevel].Append(spacers[curLevel]);
                            nodeSpaceCounter++;
                        }

                        if (curLevel < height)
                        {
                            treeToPrint[2 * curLevel + 1].Append(nullLeftBranch);
                            treeToPrint[2 * curLevel + 1].Append(spacers[curLevel + 1].ToString().Replace('_', ' '));
                            branchSpaceCounter++;
                            temp.Enqueue(null);

                            treeToPrint[2 * curLevel + 1].Append(nullRightBranch);
                            if (branchSpaceCounter < maxBranchLevelSpaces)
                            {
                                treeToPrint[2 * curLevel + 1].Append(spacers[curLevel + 1].ToString().Replace('_', ' '));
                                branchSpaceCounter++;
                            }
                            temp.Enqueue(null);
                        }
                    }
                    else
                    {
                        string cargo = (curNode.value()).ToString(); //CHANGE TO SUIT APPLICATION
                        if (cargo.Length < placeHolderSize)
                            cargo = BalanceNodeCargo(cargo, placeHolderSize);
                        treeToPrint[2 * curLevel].Append("(" + cargo + ")");
                        if (nodeSpaceCounter < maxNodeLevelSpaces)
                        {
                            treeToPrint[2 * curLevel].Append(spacers[curLevel]);
                            nodeSpaceCounter++;
                        }
                        if (curLevel < height)
                        {
                            if (curNode.left() != null)
                            {
                                treeToPrint[2 * curLevel + 1].Append(leftBranch);
                                temp.Enqueue(curNode.left());
                            }
                            else
                            {
                                treeToPrint[2 * curLevel + 1].Append(nullLeftBranch);
                                temp.Enqueue(null);
                            }
                            treeToPrint[2 * curLevel + 1].Append(spacers[curLevel + 1].ToString().Replace('_', ' '));
                            branchSpaceCounter++;

                            if (curNode.right() != null)
                            {
                                treeToPrint[2 * curLevel + 1].Append(rightBranch);
                                temp.Enqueue(curNode.right());
                            }
                            else
                            {
                                treeToPrint[2 * curLevel + 1].Append(nullRightBranch);
                                temp.Enqueue(null);
                            }

                            if (branchSpaceCounter < maxBranchLevelSpaces)
                            {
                                treeToPrint[2 * curLevel + 1].Append(spacers[curLevel + 1].ToString().Replace('_', ' '));
                                branchSpaceCounter++;
                            }
                        }
                    }
                }
            }

            //MINIMIZE WIDTH
            if (treeToPrint[0].Length + 1 >= short.MaxValue && !AutoShrink)
            {
                Console.WriteLine("This tree is too large to display it's expanded form... Depending on the structure of the tree, applying AutoShrink might enable it to be displayed...");
                Console.WriteLine("Be warned, AutoShrink (v1.0) may take a long time!");
                if (stillContinue(false))
                {
                    Console.WriteLine("Applying AutoShrink...");
                    Console.WriteLine("Processing... Please wait.");
                    AutoShrink = true;
                }
                else
                {
                    return;
                }
            }

            if (AutoShrink)
            {
                StringBuilder[] ColsToDelete = new StringBuilder[treeToPrint[0].Length];
                for (int x = 0; x < ColsToDelete.Length; x++)
                {
                    ColsToDelete[x] = new StringBuilder("");
                }

                bool showExactProgress = false; // Slows the process down MUCH further
                bool showSteppedProgress = true;
                int steps = treeToPrint.Length * treeToPrint[0].Length;
                if (showExactProgress || showSteppedProgress) 
                {
                    Console.Write("    ");
                    Console.CursorVisible = false;
                }

                for (int x = 0; x < treeToPrint.Length; x++)
                {
                    if (showSteppedProgress)
                    {
                        Console.Write("\b\b\b\b");
                        Console.Write((int)((x / (treeToPrint.Length * 1.0)) * 100) + "%");
                    }

                    for (int y = 0; y < treeToPrint[x].Length; y++)
                    {
                        if (showExactProgress)
                        {
                            Console.Write("\b\b\b\b");
                            Console.Write((int)(((x * treeToPrint[x].Length + y) / (steps * 1.0)) * 100) + "%");
                        }
                        
                        switch (treeToPrint[x][y])
                        {
                            case '_':
                            case ' ':
                            case '*':
                                break;
                            default:
                                {
                                    ColsToDelete[y].Append(treeToPrint[x][y]);
                                    break;
                                }
                        }
                    }
                }

                if (showExactProgress || showSteppedProgress)
                {
                    Console.Write("\b\b\b\b");
                    Console.CursorVisible = true;
                }

                //Initialize and populate "canvas" array
                StringBuilder[] rebuildToPrint = new StringBuilder[2 * height + 1]; //The "canvas" for the shrunken tree
                for (int x = 0; x < rebuildToPrint.Length; x++)
                {
                    rebuildToPrint[x] = new StringBuilder("");
                }

                for (int x = 0; x < ColsToDelete.Length; x++)
                {
                    if (ColsToDelete[x].ToString() != "")
                    {
                        for (int y = 0; y < treeToPrint.Length; y++)
                        {
                            rebuildToPrint[y].Append(treeToPrint[y][x]);
                        }
                    }
                }

                for (int x = 0; x < rebuildToPrint.Length; x++)
                {
                    treeToPrint[x] = rebuildToPrint[x];
                }
            }

            //DISPLAY FINAL TREE DRAWING
            if (Console.BufferWidth < treeToPrint[0].Length + 1)
            {
                if (treeToPrint[0].Length + 1 < short.MaxValue)
                    Console.BufferWidth = treeToPrint[0].Length + 1;
                else
                {
                    Console.WriteLine("Sorry, this tree is too large to display visually.");
                    return;
                }
            }

            for (int x = 0; x < treeToPrint.Length; x++)
            {
                if (hideNull)
                    Console.WriteLine(treeToPrint[x].Replace("*", " "));
                else
                    Console.WriteLine(treeToPrint[x]);
            }
        }

        //OTHER RELATED METHODS   
        //Find tree height
        private int FindTreeHeight(BTNode curNode, int height)
        {
            if (curNode == null)
                return height - 1;

            int restLeft = Math.Max(height, FindTreeHeight(curNode.left(), height + 1));
            return Math.Max(restLeft, FindTreeHeight(curNode.right(), height + 1));
        }

        //Find longest cargo value
        private int FindLongestValue(BTNode curNode)
        {
            if (curNode == null)
                return 0;

            int maxLeft = Math.Max((curNode.value()).ToString().Length, FindLongestValue(curNode.left()));
            return Math.Max(maxLeft, FindLongestValue(curNode.right()));
        }

        //Standardize cargo label length (while keeping alignment roughly centred)
        public string BalanceNodeCargo(string cargo, int finalLength)
        {
            int startLength = cargo.Length;
            for (int x = 0; x < finalLength - startLength; x++)
            {
                if (x % 2 == 0)
                    cargo = " " + cargo;
                else
                    cargo += " ";
            }
            return cargo;
        }

        private bool stillContinue(bool youSure)
        {
            if (youSure)
                Console.WriteLine("Do you want to try anyway? [Y/N]");
            else
                Console.WriteLine("Do you want to turn AutoShrink on and continue? [Y/N]");
            while (true)
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                        Console.WriteLine("Yes");
                        Console.WriteLine();
                        return true;
                    case ConsoleKey.N:
                        Console.WriteLine("No  -  Display Aborted.");
                        Console.WriteLine();
                        return false;
                    default:
                        break;
                }
        }
    }
}
