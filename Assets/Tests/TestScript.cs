using System.Collections.Generic;
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
			_board.InitializeCells();
		}

		[Test]
		public void TestInitialization()
		{
			Assert.That(_board.Cells, Is.Not.Null);
			Assert.That(_board.Cells.Count, Is.EqualTo(81));
		}

		[Test]
		public void IsBoardFullFilled()
		{
			foreach (Cell cell in _board.Cells)
			{
				cell.SetValue(1);
			}

			Assert.That(_board.IsFullFilled(), Is.True);
		}

		[Test]
		public void GenerateSolvableGrid()
		{
			ISudokuService sudokuService = new SudokuService();
			BoardValidator.BoardValidator gridValidator = new BoardValidator.BoardValidator();
			SudokuBoard sudokuBoard = sudokuService.GetSolvableBoard();
			GridSolver.GridSolver gridSolver = new GridSolver.GridSolver(sudokuBoard);

			gridSolver.Solve(sudokuBoard);

			Assert.That(gridValidator.ValidateGrid(sudokuBoard.Cells), Is.True);
		}

		[Test]
		public void PlaceValidExpectedValue()
		{
			Cell cell = new Cell(0, 0, 0, 0, 0);
			cell.SetAsEmpty(1);

			Assert.That(cell.CanPlaceValue(1), Is.True);
		}

		[Test]
		public void PlaceInvalidExpectedValue()
		{
			Cell cell = new Cell(0, 0, 0, 0, 0);
			cell.SetAsEmpty(1);

			Assert.That(cell.CanPlaceValue(3), Is.False);
		}
	}
}