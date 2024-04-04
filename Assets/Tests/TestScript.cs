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

			sudokuBoard.CellsArray[0, 0] = new CellForUser(0, 0, 0, 0, 0, 5);
			CellForUser cell = (CellForUser)sudokuBoard.CellsArray[0, 0];
			sudokuBoard.PlaceValue(5, cell);

			Assert.That(cell.IsFilledGood, Is.True);
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

			sudokuBoard.CellsArray[0, 0] = new CellForUser(0, 0, 0, 0, 0, 7);
			CellForUser cell = (CellForUser)sudokuBoard.CellsArray[0, 0];
			sudokuBoard.PlaceValue(3, cell);

			Assert.That(cell.IsFilledGood, Is.False);
		}

		[Test]
		public void ValuesToPlace1()
		{
			int rows = 9;
			int columns = 9;
			GridSolver gridSolver = new GridSolver();
			SudokuBoard sudokuBoard = new SudokuBoard(rows, columns, gridSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength());
			InputNumbers inputNumbers = new InputNumbers(9);

			boardGenerator.Generate(sudokuBoard.CellsArray);

			Assert.That(inputNumbers.AvailableNumbers.Count, Is.EqualTo(sudokuBoard.GetRowsLength()));
		}

		[Test]
		public void ValuesToPlace2()
		{
			int rows = 9;
			int columns = 9;
			GridSolver gridSolver = new GridSolver();
			SudokuBoard sudokuBoard = new SudokuBoard(rows, columns, gridSolver);
			IBoardGenerator boardGenerator = new RandomBoardGenerator(sudokuBoard.GetRowsLength(), sudokuBoard.GetColumnsLength(), gridSolver, sudokuBoard.CanPlaceValue, sudokuBoard.IsFullFilled);
			InputNumbers inputNumbers = new InputNumbers(9);
			int numberToRemove = 1;

			boardGenerator.Generate(sudokuBoard.CellsArray);

			foreach (ICell cell in sudokuBoard.CellsArray)
			{
				if (cell.Number == numberToRemove)
				{
					sudokuBoard.SetCellAsEmpty(cell);
				}
			}

			for (int i = 1; i <= sudokuBoard.GetRowsLength(); i++)
			{
				if (sudokuBoard.IsValueReachMaxOutUsed(i))
				{
					inputNumbers.RemoveNumber(i);
				}
			}

			Assert.That(inputNumbers.AvailableNumbers.Count(), Is.EqualTo(1));
		}
	}
}