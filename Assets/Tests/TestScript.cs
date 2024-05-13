using System.Collections.Generic;
using System.Linq;
using Configs;
using NUnit.Framework;
using SudokuBoard;
using SudokuBoard.BoardGenerator;
using SudokuBoard.MistakeHandler;
using SudokuBoard.PlayerInputNumbers;
using SudokuBoard.Solver;

namespace Tests
{
	public class TestScript
	{
		[TestCaseSource(nameof(SudokuTypes))]
		public void Is_Generated_Grid_Solvable(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard.Board.Board board = new SudokuBoard.Board.Board(sudokuGridConfig);
			IBoardGenerator boardGenerator = new RandomBoardGenerator(sudokuGridConfig, boardSolver, board.CanPlaceValue, board.IsFullFilled);
			boardGenerator.Generate(board);

			RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(board, 10);
			bool solved = boardSolver.Solve(board, board.CanPlaceValue, board.IsFullFilled);

			Assert.That(solved, Is.True);
		}

		private static IEnumerable<TestCaseData> SudokuTypes()
		{
			yield return new TestCaseData(SudokuType.FOUR_BY_FOUR);
			yield return new TestCaseData(SudokuType.SIX_BY_SIX);
			yield return new TestCaseData(SudokuType.EIGHT_BY_EIGHT);
			yield return new TestCaseData(SudokuType.NINE_BY_NINE);
			yield return new TestCaseData(SudokuType.TEN_BY_TEN);
			yield return new TestCaseData(SudokuType.TWELVE_BY_TWELVE);
			yield return new TestCaseData(SudokuType.SIXTEEN_BY_SIXTEEN);
		}

		[TestCase]
		public void Remove_Cells_From_Board()
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(SudokuType.FOUR_BY_FOUR);
			SudokuBoard.Board.Board board = new(sudokuGridConfig);

			int[,] grid = new int[,]
			{
				{ 1, 2, 3, 4 },
				{ 4, 3, 2, 1 },
				{ 2, 1, 4, 3 },
				{ 3, 4, 1, 2 },
			};
			BoardHelper.BuildFromInt(board, grid, sudokuGridConfig);

			int cellsToRemove = 5;
			RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(board, cellsToRemove);

			int emptyCells = 0;
			for (int row = 0; row < board.GetRowsLength(); row++)
			{
				for (int col = 0; col < board.GetRowsLength(); col++)
				{
					ICell cell = board.GetCell(row, col);
					if (cell.IsEmpty)
						emptyCells++;
				}
			}

			Assert.That(emptyCells, Is.EqualTo(cellsToRemove));
		}

		[TestCase]
		public void Duplicate_Value_In_Row()
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(SudokuType.FOUR_BY_FOUR);
			SudokuBoard.Board.Board board = new(sudokuGridConfig);

			int[,] grid = new int[,]
			{
				{ 1, 2, 1, 1 },
				{ 4, 3, 2, 1 },
				{ 2, 1, 4, 3 },
				{ 3, 4, 1, 2 },
			};
			BoardHelper.BuildFromInt(board, grid, sudokuGridConfig);

			Assert.That(board.Validate(), Is.False);
		}

		[TestCase]
		public void Duplicate_Value_In_Column()
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(SudokuType.FOUR_BY_FOUR);
			SudokuBoard.Board.Board board = new(sudokuGridConfig);

			int[,] grid = new int[,]
			{
				{ 1, 2, 3, 4 },
				{ 1, 3, 2, 1 },
				{ 2, 1, 4, 3 },
				{ 3, 4, 1, 2 },
			};
			BoardHelper.BuildFromInt(board, grid, sudokuGridConfig);

			Assert.That(board.Validate(), Is.False);
		}
		
		[TestCase]
		public void Duplicate_Value_In_GroupBox()
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(SudokuType.FOUR_BY_FOUR);
			SudokuBoard.Board.Board board = new(sudokuGridConfig);

			int[,] grid = new int[,]
			{
				{ 1, 2, 3, 4 },
				{ 4, 1, 2, 1 },
				{ 2, 1, 4, 3 },
				{ 3, 4, 1, 2 },
			};
			BoardHelper.BuildFromInt(board, grid, sudokuGridConfig);

			Assert.That(board.Validate(), Is.False);
		}

		[TestCase]
		public void Place_Valid_Value()
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(SudokuType.FOUR_BY_FOUR);
			SudokuBoard.Board.Board board = new(sudokuGridConfig);

			int[,] grid = new int[,]
			{
				{ 1, 2, 3, 0 },
				{ 4, 3, 2, 1 },
				{ 2, 1, 4, 3 },
				{ 3, 4, 1, 2 },
			};
			BoardHelper.BuildFromInt(board, grid, sudokuGridConfig);

			ICell userCell = board.GetCell(0, 3);
			board.PlaceValue(4, userCell.Row, userCell.Column);

			Assert.That(board.Validate(), Is.True);
		}


		[TestCase]
		public void Place_Invalid_Value()
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(SudokuType.FOUR_BY_FOUR);
			SudokuBoard.Board.Board board = new(sudokuGridConfig);

			int[,] grid = new int[,]
			{
				{ 1, 2, 3, 0 },
				{ 4, 3, 2, 1 },
				{ 2, 1, 4, 3 },
				{ 3, 4, 1, 2 },
			};
			BoardHelper.BuildFromInt(board, grid, sudokuGridConfig);

			ICell userCell = board.GetCell(0, 3);
			board.PlaceValue(1, userCell.Row, userCell.Column);

			Assert.That(board.Validate(), Is.False);
		}

		[TestCase]
		public void Input_Numbers_Correct_On_Init()
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(SudokuType.FOUR_BY_FOUR);
			SudokuBoard.Board.Board board = new(sudokuGridConfig);

			int[,] grid = new int[,]
			{
				{ 1, 2, 3, 4 },
				{ 4, 3, 2, 1 },
				{ 2, 1, 4, 3 },
				{ 3, 4, 1, 2 },
			};
			BoardHelper.BuildFromInt(board, grid, sudokuGridConfig);
			InputNumbers inputNumbers = new InputNumbers(board.GetRowsLength());

			Assert.That(inputNumbers.AvailableNumbers.Count, Is.EqualTo(board.GetRowsLength()));
		}

		[TestCase]
		public void Are_Input_Numbers_Correct_After_Clean_Cell()
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(SudokuType.FOUR_BY_FOUR);
			SudokuBoard.Board.Board board = new(sudokuGridConfig);

			int[,] grid = new int[,]
			{
				{ 1, 2, 3, 4 },
				{ 4, 3, 2, 0 },
				{ 2, 0, 4, 3 },
				{ 3, 4, 1, 2 },
			};
			BoardHelper.BuildFromInt(board, grid, sudokuGridConfig);
			InputNumbers inputNumbers = new InputNumbers(board.GetRowsLength());

			for (int i = 1; i <= board.GetRowsLength(); i++)
			{
				if (board.IsValueReachMaxOutUsed(i))
				{
					inputNumbers.RemoveNumber(i);
				}
			}

			Assert.That(inputNumbers.AvailableNumbers.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Increase_Mistake()
		{
			MistakeHandler mistakeHandler = new MistakeHandler(0, 3);

			mistakeHandler.Increase();

			Assert.That(mistakeHandler.Current, Is.EqualTo(1));
		}
		
		[Test]
		public void Increase_Mistake_End_Game()
		{
			MistakeHandler mistakeHandler = new MistakeHandler(0, 3);

			mistakeHandler.Increase();
			mistakeHandler.Increase();
			mistakeHandler.Increase();

			Assert.That(mistakeHandler.MaxedOut, Is.True);
		}
	}
}