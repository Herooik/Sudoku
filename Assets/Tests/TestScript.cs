using System.Linq;
using Board;
using BoardGenerator;
using NUnit.Framework;

namespace Tests
{
	public class TestScript
	{
		// private SudokuBoard _board;

		[SetUp]
		public void Setup()
		{
			// GridSolver gridSolver = new();
			// DisplayGridConfig rules = SudokuGridRules.GetRules(SudokuType.NINE_BY_NINE);
			// _board = new SudokuBoard(rules.Rows, rules.Columns, gridSolver);
		}

		[Test]
		public void GenerateSolvableGrid()
		{
			int rows = 9;
			int columns = 9;
			GridSolver gridSolver = new GridSolver();
			SudokuBoard sudokuBoard = new SudokuBoard(rows, columns, gridSolver);

			sudokuBoard.GenerateNewBoard();
			bool solved = gridSolver.Solve(sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength(), sudokuBoard.CellsArray, sudokuBoard.CanPlaceValue, sudokuBoard.IsFullFilled);
	
			Assert.That(solved, Is.True);
		}

		[Test]
		public void Is_Duplicate_Value_In_Row()
		{
			int rows = 9;
			int columns = 9;
			GridSolver gridSolver = new GridSolver();
			SudokuBoard sudokuBoard = new SudokuBoard(rows, columns, gridSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength());

			boardGenerator.Generate(sudokuBoard.CellsArray);
		}

		[Test]
		public void Place_Valid_Value()
		{
			int rows = 9;
			int columns = 9;
			GridSolver gridSolver = new GridSolver();
			SudokuBoard sudokuBoard = new SudokuBoard(rows, columns, gridSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength());

			boardGenerator.Generate(sudokuBoard.CellsArray);

			Assert.That(sudokuBoard.CanPlaceValue(5, sudokuBoard.CellsArray[0, 0]), Is.True);
		}

		
		[Test]
		public void Place_Invalid_Value()
		{
			int rows = 9;
			int columns = 9;
			GridSolver gridSolver = new GridSolver();
			SudokuBoard sudokuBoard = new SudokuBoard(rows, columns, gridSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength());

			boardGenerator.Generate(sudokuBoard.CellsArray);
			sudokuBoard.PlaceValue(5, sudokuBoard.CellsArray[0,0]);

			Assert.That(sudokuBoard.CanPlaceValue(5, sudokuBoard.CellsArray[0, 1]), Is.False);
		}

		[Test]
		public void ValuesToPlace()
		{
			int rows = 9;
			int columns = 9;
			GridSolver gridSolver = new GridSolver();
			SudokuBoard sudokuBoard = new SudokuBoard(rows, columns, gridSolver);
			// IBoardGenerator boardGenerator = new RandomBoardGenerator(sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength(), gridSolver, sudokuBoard.CanPlaceValue, sudokuBoard.IsFullFilled);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength());
			PlayerNumberPlacement playerNumberPlacement = new PlayerNumberPlacement(9);

			boardGenerator.Generate(sudokuBoard.CellsArray);

			var temp = sudokuBoard.CellsArray[0, 0];
			// sudokuBoard.CellsArray[0, 0] = new CellFilledByUserInput(temp.Index, temp.GroupBox, temp.Row, temp.Column, 5, false);

			// _board.Cells[0].RemoveValue();
			//
			// for (int i = 1; i <= _board.GetColumnsLength(); i++)
			// {
			// 	if (_board.IsValueReachMaxOutUsed(i))
			// 	{
			// 		playerNumberPlacement.RemoveNumber(i);
			// 	}
			// }

			Assert.That(playerNumberPlacement.AvailableNumbers.Count, Is.EqualTo(1));
		}
	}
}