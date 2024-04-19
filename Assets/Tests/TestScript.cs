using System.Collections.Generic;
using System.Linq;
using Board;
using BoardGenerator;
using NUnit.Framework;

namespace Tests
{
	public class TestScript
	{
		[TestCaseSource(nameof(SudokuTypes))]
		public void Is_Generated_Grid_Solvable(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard sudokuBoard = new SudokuBoard(sudokuGridConfig, boardSolver);

			sudokuBoard.GenerateNewBoard(10);
			bool solved = boardSolver.Solve(sudokuBoard.CellsArray, sudokuBoard.CanPlaceValue, sudokuBoard.IsFullFilled);

			Assert.That(solved, Is.True);
		}

		private static IEnumerable<TestCaseData> SudokuTypes()
		{
			yield return new TestCaseData(SudokuType.FOUR_BY_FOUR);
			yield return new TestCaseData(SudokuType.SIX_BY_SIX);
			yield return new TestCaseData(SudokuType.EIGHT_BY_EIGHT);
			yield return new TestCaseData(SudokuType.NINE_BY_NINE);
			// yield return new TestCaseData(SudokuType.TEN_BY_TEN);
			// yield return new TestCaseData(SudokuType.TWELVE_BY_TWELVE);
			// yield return new TestCaseData(SudokuType.SIXTEEN_BY_SIXTEEN);
		}

		[TestCaseSource(nameof(SudokuTypes))]
		public void Is_Duplicate_Value_In_Row(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard sudokuBoard = new SudokuBoard(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.CellsArray.GetRowsLength());

			boardGenerator.Generate(sudokuBoard.CellsArray);
		}

		[TestCaseSource(nameof(SudokuTypes))]
		public void Place_Valid_Value(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard sudokuBoard = new SudokuBoard(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.CellsArray.GetRowsLength());

			boardGenerator.Generate(sudokuBoard.CellsArray);

			sudokuBoard.CellsArray[0, 0] = new CellForUser(0, 0, 0, 0, 0, 5);
			CellForUser cell = (CellForUser)sudokuBoard.CellsArray[0, 0];
			sudokuBoard.PlaceValue(5, cell.Row, cell.Column);

			Assert.That(cell.IsFilledGood, Is.True);
		}

		
		[TestCaseSource(nameof(SudokuTypes))]
		public void Place_Invalid_Value(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard sudokuBoard = new SudokuBoard(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.CellsArray.GetRowsLength());

			boardGenerator.Generate(sudokuBoard.CellsArray);

			sudokuBoard.CellsArray[0, 0] = new CellForUser(0, 0, 0, 0, 0, 7);
			CellForUser cell = (CellForUser)sudokuBoard.CellsArray[0, 0];
			sudokuBoard.PlaceValue(3, cell.Row, cell.Column);

			Assert.That(cell.IsFilledGood, Is.False);
		}

		[TestCaseSource(nameof(SudokuTypes))]
		public void Are_Input_Numbers_Correct_On_Init(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard sudokuBoard = new SudokuBoard(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new EmptyBoardGenerator(sudokuBoard.CellsArray.GetRowsLength());
			InputNumbers inputNumbers = new InputNumbers(sudokuBoard.CellsArray.GetRowsLength());

			boardGenerator.Generate(sudokuBoard.CellsArray);

			Assert.That(inputNumbers.AvailableNumbers.Count, Is.EqualTo(sudokuBoard.CellsArray.GetRowsLength()));
		}

		[TestCaseSource(nameof(SudokuTypes))]
		public void Are_Input_Numbers_Correct_After_Clean_Cell(SudokuType sudokuType)
		{
			SudokuGridConfig sudokuGridConfig = SudokuConfig.GetConfig(sudokuType);
			IBoardSolver boardSolver = new BoardSolver(sudokuGridConfig);
			SudokuBoard sudokuBoard = new SudokuBoard(sudokuGridConfig, boardSolver);
			IBoardGenerator boardGenerator = new RandomBoardGenerator(sudokuGridConfig, boardSolver, sudokuBoard.CanPlaceValue, sudokuBoard.IsFullFilled);
			InputNumbers inputNumbers = new InputNumbers(sudokuBoard.CellsArray.GetRowsLength());
			int numberToRemove = 1;

			boardGenerator.Generate(sudokuBoard.CellsArray);

			foreach (ICell cell in sudokuBoard.CellsArray)
			{
				if (cell.Number == numberToRemove)
				{
					sudokuBoard.SetCellAsEmpty(cell);
				}
			}

			for (int i = 1; i <= sudokuBoard.CellsArray.GetRowsLength(); i++)
			{
				if (sudokuBoard.IsValueReachMaxOutUsed(i))
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