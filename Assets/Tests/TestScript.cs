using System;
using System.Linq;
using Board;
using NUnit.Framework;

namespace Tests
{
	public class TestScript
	{
		private SudokuBoard _board;

		[SetUp]
		public void Setup()
		{
			_board = new SudokuBoard();
			DisplayGridConfig rules = SudokuGridRules.GetRules(SudokuType.NINE_BY_NINE);
			_board.InitializeCells(rules.Rows, rules.Columns);
		}

		[Test]
		public void TestInitialization()
		{
			Assert.That(_board.Cells, Is.Not.Null);
			Assert.That(_board.Cells.Count, Is.EqualTo(81));
		}

		[Test]
		public void GenerateSolvableGrid()
		{
			ISudokuService sudokuService = new SudokuService();
			SudokuBoard sudokuBoard = sudokuService.CreateSolvableBoard(SudokuType.NINE_BY_NINE);
			GridSolver gridSolver = new GridSolver(sudokuBoard);

			gridSolver.Solve(sudokuBoard);

			Assert.That(_board.IsFullFilled(), Is.True);
		}

		[Test]
		public void PlaceValidExpectedValue()
		{
			GridSolver gridSolver = new(_board);

			gridSolver.Solve(_board);
			int value = _board.Cells[0].ActualValue;
			_board.Cells[0].SetAsEmpty();

			Assert.That(_board.CanPlaceValue(value, _board.Cells[0]), Is.True);
		}

		[Test]
		public void PlaceInvalidExpectedValue()
		{
			Random random = new Random();
			GridSolver gridSolver = new(_board);

			gridSolver.Solve(_board);
			int value = _board.Cells[0].ActualValue;
			// Generate the opposite value within the range [1, 9] excluding the original value
			int oppositeValue;
			do
			{
				oppositeValue = random.Next(1, 10);
			} while (oppositeValue == value);
			_board.Cells[0].SetAsEmpty();

			Assert.That(_board.CanPlaceValue(oppositeValue, _board.Cells[0]), Is.False);
		}

		[Test]
		public void ValuesToPlace()
		{
			PlayerNumberPlacement playerNumberPlacement = new PlayerNumberPlacement(_board.Columns);
			GridSolver gridSolver = new(_board);

			gridSolver.Solve(_board);
			_board.Cells[0].SetAsEmpty();

			for (int i = 1; i <= _board.Columns; i++)
			{
				if (_board.IsValueReachMaxOutUsed(i))
				{
					playerNumberPlacement.RemoveNumber(i);
				}
			}

			Assert.That(playerNumberPlacement.AvailableNumbers.Count, Is.EqualTo(1));
		}
	}
}