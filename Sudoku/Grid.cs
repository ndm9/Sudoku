using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sudoku
{
    internal class Grid
    {
        public int[,] grid = new int[9, 9];
        public int[,,] orderedMatrices = {
            {{0, 0}, {0, 1}, {0, 2}, {1, 0}, {1, 1}, {1, 2}, {2, 0}, {2, 1}, {2, 2}},
            {{0, 3}, {0, 4}, {0, 5}, {1, 3}, {1, 4}, {1, 5}, {2, 3}, {2, 4}, {2, 5}},
            {{0, 6}, {0, 7}, {0, 8}, {1, 6}, {1, 7}, {1, 8}, {2, 6}, {2, 7}, {2, 8}},

            {{3, 0}, {3, 1}, {3, 2}, {4, 0}, {4, 1}, {4, 2}, {5, 0}, {5, 1}, {5, 2}},
            {{3, 3}, {3, 4}, {3, 5}, {4, 3}, {4, 4}, {4, 5}, {5, 3}, {5, 4}, {5, 5}},
            {{3, 6}, {3, 7}, {3, 8}, {4, 6}, {4, 7}, {4, 8}, {5, 6}, {5, 7}, {5, 8}},

            {{6, 0}, {6, 1}, {6, 2}, {7, 0}, {7, 1}, {7, 2}, {8, 0}, {8, 1}, {8, 2}},
            {{6, 3}, {6, 4}, {6, 5}, {7, 3}, {7, 4}, {7, 5}, {8, 3}, {8, 4}, {8, 5}},
            {{6, 6}, {6, 7}, {6, 8}, {7, 6}, {7, 7}, {7, 8}, {8, 6}, {8, 7}, {8, 8}}
        };
        readonly IndexOutOfRangeException exceptionIndexOutOfRange0to9 = new("Index is invalid. Allowed 0-9.");
        readonly ArgumentOutOfRangeException exceptionArgumentOutOfRange0to9 = new ArgumentOutOfRangeException("Value is invalid. Allowed 0-9.");

        public Grid()
        {
            grid = new int[9, 9];
        }
        public Grid(int[,] values)
        {
            if (values != null && values.IsFixedSize && values.Length == 81)
                grid = values;
            else grid = new int[9, 9];
        }

        public override string ToString()
        {
            string retVal = string.Empty;

            if (grid != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        retVal += grid[i, j].ToString().PadLeft(2, ' ');
                    }
                    if (i < 8)
                        retVal += Environment.NewLine;
                }
            }
            return retVal;
        }

        public string ToStringStyled()
        {
            //Style[,] style1 = { 
            //    { new Style(PPlus.Color.Orange1),       new Style(PPlus.Color.White),   new Style(PPlus.Color.Red) },
            //    { new Style(PPlus.Color.RoyalBlue1),    new Style(PPlus.Color.Yellow),  new Style(PPlus.Color.Green) },
            //    { new Style(PPlus.Color.Grey),          new Style(PPlus.Color.Purple),  new Style(new PPlus.Color(0, 106, 80)) } };

            string retVal = ToString();

            for (int i = 0; i < 3; i++)
            {
                int back = i * 20;
                retVal = retVal.Insert(178 - back, "[/]").Insert(172 - back, "[/][#006760]")
                    .Insert(166 - back, "[/][#881CD5]")
                    .Insert(160 - back, "[#808080]");
            }

            for (int i = 0; i < 3; i++)
            {
                int back = i * 20;
                retVal = retVal.Insert(118 - back, "[/]").Insert(112 - back, "[/][#54AA01]")
                    .Insert(106 - back, "[/][#FFFF00]")
                    .Insert(100 - back, "[#4169E1]");
            }


            for (int i = 0; i < 3; i++)
            {
                int back = i * 20;
                retVal = retVal.Insert(58 - back, "[/]").Insert(52 - back, "[/][#FF0000]")
                    .Insert(46 - back, "[/][#FFFFFF]")
                    .Insert(40 - back, "[#FFA500]");
            }

            return retVal;
        }

        public int[] GetNumsInMatrix(int matrixIndex)
        {
            if (matrixIndex < 0 || matrixIndex >= 9) throw exceptionIndexOutOfRange0to9;

            int[] retVal = new int[9];

            for (int i = 0; i < 9; i++)
            {
                retVal[i] = grid[orderedMatrices[matrixIndex, i, 0], orderedMatrices[matrixIndex, i, 1]];
            }

            return retVal;
        }

        public int IndexOfNumInMatrix(int num, int matrixIndex)
        {
            if (matrixIndex < 0 || matrixIndex >= 9) throw exceptionIndexOutOfRange0to9;
            if (num < 0 || num > 9) throw exceptionArgumentOutOfRange0to9;

            for (int i = 0; i < 9; i++)
            {
                if (grid[orderedMatrices[matrixIndex, i, 0], orderedMatrices[matrixIndex, i, 1]] == num)
                    return i;
            }

            return -1;
        }
        public int IndexOfNumInRow(int num, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= 9) throw exceptionIndexOutOfRange0to9;
            if (num < 0 || num > 9) throw exceptionArgumentOutOfRange0to9;

            for (int i = 0; i < 9; i++)
            {
                if (grid[rowIndex, i] == num) return i;
            }

            return -1;
        }
        public int IndexOfNumInColumn(int num, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= 9) throw exceptionIndexOutOfRange0to9;
            if (num < 0 || num > 9) throw exceptionArgumentOutOfRange0to9;

            for (int i = 0; i < 9; i++)
            {
                if (grid[i, columnIndex] == num) return i;
            }

            return -1;
        }
    }
}
