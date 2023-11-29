using Xunit;
using Sudoku;

namespace Sudoku.Tests
{
    public class GridTests
    {
        private static readonly int[,,] orderedMatrices = {
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
        private Grid myGrid = new();

        //Setup
        public GridTests()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    myGrid.SetNumInRow(i * 10 + j, i, j);
                }
            }
            /*	    r0	1	2	3	4	5	6	7	8
                c0	0	1	2	3	4	5	6	7	8
                1	10	11	12	13	14	15	16	17	18
                2	20	21	22	23	24	25	26	27	28
                3	30	31	32	33	34	35	36	37	38
                4	40	41	42	43	44	45	46	47	48
                5	50	51	52	53	54	55	56	57	58
                6	60	61	62	63	64	65	66	67	68
                7	70	71	72	73	74	75	76	77	78
                8	80	81	82	83	84	85	86	87	88
            */
        }

        [Fact()]
        public void Grid_WhenCorrectSizedGridProvided_SetsAsInitialGrid()
        {
            int[,] values = new int[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    values[i, j] = myGrid.grid[i, j];

            Grid grid = new(values);
            Assert.Equal(myGrid.grid, grid.grid);
        }
        [Fact()]
        public void Grid_WhenSmallerSizedGridProvided_UsesDefaultInitialGrid()
        {
            int[,] values = new int[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    values[i, j] = myGrid.grid[i, j];

            Grid grid = new(values);
            Assert.Equal(new int[9, 9], grid.grid);
        }
        [Fact]
        public void MatrixToGridLocation_ForAllMatrices_ReturnsValidGridCoordinate()
        {
            //Arrange
            //Act
            for (int matrixIndex = 0; matrixIndex < 9; matrixIndex++)
            {
                for (int positionIndex = 0; positionIndex < 9; positionIndex++)
                {
                    var (rowIndex, columnIndex) = Grid.MatrixToGridLocation(matrixIndex, positionIndex);

                    //Assert
                    Assert.Equal((orderedMatrices[matrixIndex, positionIndex, 0], orderedMatrices[matrixIndex, positionIndex, 1]), (rowIndex, columnIndex));
                }
            }
        }
        [Fact]
        public void GridToMatrixLocation_ForWholeGrid_ReturnsValidMatrixCoordinate()
        {
            //Arrange
            //Act
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                {
                    var (matrixIndex, positionIndex) = Grid.GridToMatrixLocation(rowIndex, columnIndex);

                    //Assert
                    Assert.Equal((rowIndex, columnIndex), (orderedMatrices[matrixIndex, positionIndex, 0], orderedMatrices[matrixIndex, positionIndex, 1]));
                }
            }
        }
        [Fact()]
        public void ToString_ForPrintout_ReturnsCorrectString()
        {
            Assert.Equal(" 0 1 2 3 4 5 6 7 8\r\n101112131415161718\r\n202122232425262728\r\n303132333435363738\r\n404142434445464748\r\n505152535455565758\r\n606162636465666768\r\n707172737475767778\r\n808182838485868788"
                , myGrid.ToString());
        }
        [Fact()]
        public void ToStringStyled_ForPrintout_ReturnsCorrectString()
        {
            Assert.Equal("[#FFA500] 0 1 2[/][#FFFFFF] 3 4 5[/][#FF0000] 6 7 8[/]\r\n[#FFA500]101112[/][#FFFFFF]131415[/][#FF0000]161718[/]\r\n[#FFA500]202122[/][#FFFFFF]232425[/][#FF0000]262728[/]\r\n[#4169E1]303132[/][#FFFF00]333435[/][#54AA01]363738[/]\r\n[#4169E1]404142[/][#FFFF00]434445[/][#54AA01]464748[/]\r\n[#4169E1]505152[/][#FFFF00]535455[/][#54AA01]565758[/]\r\n[#808080]606162[/][#881CD5]636465[/][#006760]666768[/]\r\n[#808080]707172[/][#881CD5]737475[/][#006760]767778[/]\r\n[#808080]808182[/][#881CD5]838485[/][#006760]868788[/]"
                , myGrid.ToStringStyled());
        }

        [Fact()]
        public void GetNumsInMatrix_ForEachMatrix_ReturnsCorrectValues()
        {
            var firstExpected = new int[] { 0, 1, 2, 10, 11, 12, 20, 21, 22 };
            int[] expected = new int[9];
            firstExpected.CopyTo(expected, 0);
            for (int matrixIndex = 0; matrixIndex < 9; matrixIndex++)
            {
                if (matrixIndex > 0)
                {
                    int add = (matrixIndex / 3 * 30) + (matrixIndex % 3 * 3);
                    for (int i = 0; i < firstExpected.Length; i++)
                    {
                        expected[i] = firstExpected[i] + add;
                    }
                }
                Assert.Equal(expected, myGrid.GetNumsInMatrix(matrixIndex));
            }
        }
        [Fact()]
        public void IndexOfNumInMatrix_ForFilledGrid_ReturnsCorrectValues()
        {
            Assert.Equal(4, myGrid.IndexOfNumInMatrix(11, 0));
            Assert.Equal(2, myGrid.IndexOfNumInMatrix(38, 5));
            Assert.Equal(0, myGrid.IndexOfNumInMatrix(63, 7));
            Assert.Equal(-1, myGrid.IndexOfNumInMatrix(9, 0));
        }
        [Fact()]
        public void IndexOfNumInRow_ForFilledGrid_ReturnsCorrectValues()
        {
            Assert.Equal(1, myGrid.IndexOfNumInRow(11, 1));
            Assert.Equal(8, myGrid.IndexOfNumInRow(38, 3));
            Assert.Equal(3, myGrid.IndexOfNumInRow(63, 6));
            Assert.Equal(-1, myGrid.IndexOfNumInRow(9, 0));
        }
        [Fact()]
        public void IndexOfNumInColumn_ForFilledGrid_ReturnsCorrectValues()
        {
            Assert.Equal(1, myGrid.IndexOfNumInColumn(11, 1));
            Assert.Equal(3, myGrid.IndexOfNumInColumn(38, 8));
            Assert.Equal(6, myGrid.IndexOfNumInColumn(63, 3));
            Assert.Equal(-1, myGrid.IndexOfNumInColumn(9, 0));
        }

        [Theory]
        [InlineData(SudokuRegion.Matrix, 0, 12, 5, -1)]
        [InlineData(SudokuRegion.Matrix, 0, 20, 6, -1)]
        [InlineData(SudokuRegion.Matrix, 0, 12, 3, 5)]
        [InlineData(SudokuRegion.Matrix, 7, 72, 3, -1)]
        [InlineData(SudokuRegion.Matrix, 7, 74, 4, -1)]
        [InlineData(SudokuRegion.Matrix, 7, 85, 4, 8)]

        [InlineData(SudokuRegion.Row, 1, 12, 2, -1)]
        [InlineData(SudokuRegion.Row, 2, 20, 0, -1)]
        [InlineData(SudokuRegion.Row, 1, 12, 3, 2)]
        [InlineData(SudokuRegion.Row, 7, 72, 2, -1)]
        [InlineData(SudokuRegion.Row, 7, 74, 4, -1)]
        [InlineData(SudokuRegion.Row, 8, 85, 4, 5)]

        [InlineData(SudokuRegion.Column, 2, 12, 1, -1)]
        [InlineData(SudokuRegion.Column, 0, 20, 2, -1)]
        [InlineData(SudokuRegion.Column, 2, 12, 3, 1)]
        [InlineData(SudokuRegion.Column, 2, 72, 7, -1)]
        [InlineData(SudokuRegion.Column, 4, 74, 7, -1)]
        [InlineData(SudokuRegion.Column, 5, 85, 4, 8)]
        public void IndexOfNumInRegion_WhenSkipNumberIsUsed_ReturnIndexOrNotFound(SudokuRegion region, int regionIndex, int numToFind, int skipIndex, int expected)
        {
            var result = myGrid.IndexOfNumInRegion(numToFind, region, regionIndex, skipIndex);
            Assert.Equal(expected, result);
        }
    }
}