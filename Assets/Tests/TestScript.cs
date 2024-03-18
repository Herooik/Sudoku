using System.Collections.Generic;
using System.Linq;
using Board;
using GridSolver;
using NumberGenerator;
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
				_board.SetCellValue(cell.Index, 1);
			}

			Assert.That(_board.IsFullFilled(), Is.True);
		}

		[Test]
		public void InvalidValueInRow()
		{
			Cell cell = _board.Cells.First();
			_board.SetCellValue(cell.Index, 1);

			// Set a conflicting value in the same row
			Cell conflictingCell = _board.Cells.First(c => c.CellPosition.Row == cell.CellPosition.Row && c.Index != cell.Index);
			_board.SetCellValue(conflictingCell.Index, 1);

			Assert.That(_board.IsValidValueForTheCell(1, cell), Is.False);
		}

		[Test]
		public void InvalidValueInColumn()
		{
			Cell cell = _board.Cells.First();
			_board.SetCellValue(cell.Index, -1);

			// Set a conflicting value in the same column
			Cell conflictingCell = _board.Cells.First(c => c.CellPosition.Column == cell.CellPosition.Column && c.Index != cell.Index);
			_board.SetCellValue(conflictingCell.Index, 1);

			Assert.That(_board.IsValidValueForTheCell(1, cell), Is.False);
		}

		[Test]
		public void InvalidValueInGroupBox()
		{
			Cell cell = _board.Cells.First();
			_board.SetCellValue(cell.Index, -1);

			// Set a conflicting value in the same group box
			Cell conflictingCell = _board.Cells.First(c => c.GroupBox == cell.GroupBox && c.Index != cell.Index);
			_board.SetCellValue(conflictingCell.Index, 1);

			Assert.That(_board.IsValidValueForTheCell(1, cell), Is.False);
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
	}
}