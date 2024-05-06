using System.Collections.Generic;
using System.Linq;
using Configs;
using NUnit.Framework;
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
			SudokuBoard.Board.Board board = new SudokuBoard.Board.Board(sudokuGridConfig, boardSolver);

			board.GenerateNewBoard(10);
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

		[TestCaseSource(nameof(SudokuTypes))]
		public void Is_Duplicate_Value_In_Row(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard.Board.Board board = new SudokuBoard.Board.Board(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuGridConfig);

			boardGenerator.Generate(board);
		}

		[TestCaseSource(nameof(SudokuTypes))]
		public void Place_Valid_Value(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard.Board.Board board = new SudokuBoard.Board.Board(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuGridConfig);

			boardGenerator.Generate(board);

			board.SetCellAsUser(0, 0, 0, 0, 0, 5);

			UserCell userCell = (UserCell)board.GetCell(0, 0);
			board.PlaceValue(5, userCell.Row, userCell.Column);

			Assert.That(userCell.IsFilledGood, Is.True);
		}

		
		[TestCaseSource(nameof(SudokuTypes))]
		public void Place_Invalid_Value(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard.Board.Board board = new SudokuBoard.Board.Board(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuGridConfig);

			boardGenerator.Generate(board);

			board.SetCellAsUser(0, 0, 0, 0, 0, 7);

			UserCell userCell = (UserCell)board.GetCell(0, 0);
			board.PlaceValue(3, userCell.Row, userCell.Column);

			Assert.That(userCell.IsFilledGood, Is.False);
		}

		[TestCaseSource(nameof(SudokuTypes))]
		public void Are_Input_Numbers_Correct_On_Init(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard.Board.Board board = new SudokuBoard.Board.Board(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuGridConfig);
			InputNumbers inputNumbers = new InputNumbers(board.GetRowsLength());

			boardGenerator.Generate(board);

			Assert.That(inputNumbers.AvailableNumbers.Count, Is.EqualTo(board.GetRowsLength()));
		}

		[TestCaseSource(nameof(SudokuTypes))]
		public void Are_Input_Numbers_Correct_After_Clean_Cell(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard.Board.Board board = new SudokuBoard.Board.Board(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new RandomBoardGenerator(sudokuGridConfig, boardSolver, board.CanPlaceValue, board.IsFullFilled);
			InputNumbers inputNumbers = new InputNumbers(board.GetRowsLength());
			int numberToRemove = 1;

			boardGenerator.Generate(board);

			foreach (ICell cell in board.GetAllCellsWithNumber(numberToRemove))
			{
				board.SetCellAsEmpty(cell);
			}

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