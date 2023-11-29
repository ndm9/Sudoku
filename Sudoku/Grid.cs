using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sudoku
{
    public enum SudokuRegion
    {
        Matrix,
        Row,
        Column
    }

    public class Grid
    {
        public int[,] grid = new int[9, 9];

        readonly IndexOutOfRangeException exceptionIndexOutOfRange0to9 = new("Index is invalid. Allowed 0-9.");
        readonly ArgumentOutOfRangeException exceptionArgumentOutOfRange0to9 = new("Value is invalid. Allowed 0-9.");

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

        public static (int rowIndex, int columnIndex) MatrixToGridLocation(int matrixIndex, int positionIndex)
        {
            return (matrixIndex / 3 * 3 + (positionIndex / 3), matrixIndex % 3 * 3 + (positionIndex % 3));
        }
        public static (int matrixIndex, int positionIndex) GridToMatrixLocation(int rowIndex, int columnIndex)
        {
            return (rowIndex / 3 * 3 + (columnIndex / 3), rowIndex % 3 * 3 + (columnIndex % 3));
        }

        public int[] GetNumsInMatrix(int matrixIndex)
        {
            if (matrixIndex < 0 || matrixIndex >= 9) throw exceptionIndexOutOfRange0to9;

            int[] retVal = new int[9];

            for (int i = 0; i < 9; i++)
            {
                var (rowIndex, columnIndex) = MatrixToGridLocation(matrixIndex, i);
                retVal[i] = grid[rowIndex, columnIndex];
            }

            return retVal;
        }

        public int IndexOfNumInMatrix(int num, int matrixIndex)
        {
            return IndexOfNumInRegion(num, SudokuRegion.Matrix, matrixIndex);
        }
        public int IndexOfNumInRow(int num, int rowIndex)
        {
            return IndexOfNumInRegion(num, SudokuRegion.Row, rowIndex);
        }
        public int IndexOfNumInColumn(int num, int columnIndex)
        {
            return IndexOfNumInRegion(num, SudokuRegion.Column, columnIndex);
        }
        public int IndexOfNumInRegion(int num, SudokuRegion region, int regionIndex, int skipPositionIndex = -1)
        {
            if (regionIndex < 0 || regionIndex >= 9) throw exceptionIndexOutOfRange0to9;

            switch (region)
            {
                case SudokuRegion.Matrix:
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != skipPositionIndex)
                        {
                            var (rowIndex, columnIndex) = MatrixToGridLocation(regionIndex, i);
                            if (grid[rowIndex, columnIndex] == num)
                                return i;
                        }
                    }
                    break;
                case SudokuRegion.Row:
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != skipPositionIndex && grid[regionIndex, i] == num) return i;
                    }
                    break;
                case SudokuRegion.Column:
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != skipPositionIndex && grid[i, regionIndex] == num) return i;
                    }
                    break;
                default:
                    break;
            }

            return -1;
        }

        public bool SetNumInMatrix(int num, int matrixIndex, int positionIndex, bool validate = false)
        {
            return SetNumInRegion(num, SudokuRegion.Matrix, matrixIndex, positionIndex, validate);
        }
        public bool SetNumInRow(int num, int rowIndex, int positionIndex, bool validate = false)
        {
            return SetNumInRegion(num, SudokuRegion.Row, rowIndex, positionIndex, validate);
        }
        public bool SetNumInColumn(int num, int columnIndex, int positionIndex, bool validate = false)
        {
            return SetNumInRegion(num, SudokuRegion.Column, columnIndex, positionIndex, validate);
        }
        public bool SetNumInRegion(int num, SudokuRegion region, int regionIndex, int positionIndex, bool validate = false)
        {
            if (regionIndex < 0 || regionIndex >= 9 || positionIndex < 0 || positionIndex >= 9) throw exceptionIndexOutOfRange0to9;
            if (validate && (num < 0 || num > 9)) throw exceptionArgumentOutOfRange0to9;

            switch (region)
            {
                case SudokuRegion.Matrix:
                    var (rowIndex, columnIndex) = MatrixToGridLocation(regionIndex, positionIndex);
                    if(validate && !ConformsToRules(num, region, regionIndex, positionIndex)) return false;
                    grid[rowIndex, columnIndex] = num;
                    break;
                case SudokuRegion.Row:
                    if(validate && !ConformsToRules(num, region, regionIndex, positionIndex)) return false;
                    grid[regionIndex, positionIndex] = num;
                    break;
                case SudokuRegion.Column:
                    if(validate && !ConformsToRules(num, region, regionIndex, positionIndex)) return false;
                    grid[positionIndex, regionIndex] = num;
                    break;
            }
            return true;
        }
        public bool ConformsToRules(int newNum, SudokuRegion region, int regionIndex, int positionIndex)
        {
            if (regionIndex < 0 || regionIndex >= 9 || positionIndex < 0 || positionIndex >= 9) throw exceptionIndexOutOfRange0to9;
            if (newNum < 0 || newNum > 9) return false;
            return IndexOfNumInRegion(newNum, region, regionIndex, positionIndex) < 0;
        }
    }
}
