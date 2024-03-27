using System.Collections.Generic;
using Board;

public class SudokuService : ISudokuService
{
	private readonly SudokuBoard _sudokuBoard = new();

	public IReadOnlyList<CellData> Initialize(SudokuType sudokuType)
	{
		DisplayGridConfig rules = SudokuGridRules.GetRules(sudokuType);
		_sudokuBoard.InitializeCells(rules.Rows, rules.Columns);

		GridSolver gridSolver = new(_sudokuBoard);
		gridSolver.Solve(_sudokuBoard);

		RemoveRandomCellsHandler.RemoveRandomCellsFromBoard(_sudokuBoard.Cells, 10);

		System.Random random = new System.Random();
		int selectedCellIndex = _sudokuBoard.Cells[random.Next(0, _sudokuBoard.Cells.Count)].Index;
		IReadOnlyList<CellData> cellDisplays = GetCellDisplays(selectedCellIndex);

		return cellDisplays;
	}

	public IReadOnlyList<CellData> GetCellDisplays(int selectedCellIndex)
	{
		CellData[] array = new CellData[_sudokuBoard.Cells.Count];

		Cell selectedCell = _sudokuBoard.Cells[selectedCellIndex];

		// Set current selected Cell
		array[selectedCellIndex] = new CellData(
			selectedCell.IsEmpty() ? string.Empty : selectedCell.ActualValue.ToString(),
			selectedCell.Row,
			selectedCell.Column,
			selectedCell.Index,
			CellState.SELECTED,
			GetUserPlacedValueState(selectedCell));

		//todo: omg refactor this

		foreach (Cell cell in _sudokuBoard.Cells)
		{
			// skip current selected cell
			if (cell.Index == selectedCell.Index) continue;

			CellState cellState = CellState.NONE;
			if (cell.Row == selectedCell.Row || cell.Column == selectedCell.Column)
			{
				if (cell.ActualValue == selectedCell.ActualValue && !cell.IsEmpty())
				{
					cellState = CellState.SAME_VALUE_IN_ROW_AND_COLUMN;
				}
				else
				{
					cellState = CellState.SAME_ROW_COLUMN;
				}
			}
			else if (cell.GroupBox == selectedCell.GroupBox)
			{
				if (cell.ActualValue == selectedCell.ActualValue && !cell.IsEmpty())
				{
					cellState = CellState.SAME_VALUE_IN_ROW_AND_COLUMN;
				}
				else
				{
					cellState = CellState.SAME_GROUP_BOX;
				}
			}
			else if (cell.ActualValue == selectedCell.ActualValue && !cell.IsEmpty())
			{
				cellState = CellState.SAME_VALUE;
			}

			array[cell.Index] = new CellData(
				cell.IsEmpty() ? string.Empty : cell.ActualValue.ToString(),
				cell.Row,
				cell.Column,
				cell.Index,
				cellState,
				GetUserPlacedValueState(cell));
		}

		return array;
	}

	public SudokuBoard.PlaceValueResult PlaceNumber(int number, int selectedCellIndex)
	{
		if (_sudokuBoard.Cells[selectedCellIndex].IsPlacedByGenerator)
			return SudokuBoard.PlaceValueResult.FILLED_BY_SOLVER;

		if (_sudokuBoard.Cells[selectedCellIndex].ActualValue == number)
		{
			_sudokuBoard.Cells[selectedCellIndex].SetValue(-1);
		}
		else
		{
			_sudokuBoard.Cells[selectedCellIndex].SetValue(number);
		}

		SudokuBoard.PlaceValueResult isValidValueForTheCell = _sudokuBoard.CanPlaceValue(number, _sudokuBoard.Cells[selectedCellIndex]); // todo pass row, column, value instead of "_sudokuBoard.Cells[selectedCell.Index]" ? 
		return isValidValueForTheCell;
	}

	private static UserPlacedValue GetUserPlacedValueState(Cell cell)
	{
		if (cell.IsPlacedByGenerator)
		{
			return UserPlacedValue.BY_GENERATOR;
		}
		return cell.IsPlacedGoodValue() ? UserPlacedValue.USER_PLACED_GOOD : UserPlacedValue.USER_PLACED_WRONG;
	}
}

public interface ISudokuService
{
	IReadOnlyList<CellData> Initialize(SudokuType sudokuType);
	IReadOnlyList<CellData> GetCellDisplays(int selectedCellIndex);
	SudokuBoard.PlaceValueResult PlaceNumber(int number, int selectedCellIndex);
}

public enum CellState // todo change name?
{
	NONE,
	SELECTED,
	SAME_ROW_COLUMN,
	SAME_VALUE_IN_ROW_AND_COLUMN,
	SAME_VALUE,
	SAME_GROUP_BOX,
}

public enum UserPlacedValue // todo change name?
{
	BY_GENERATOR,
	USER_PLACED_GOOD,
	USER_PLACED_WRONG,
}

public readonly struct CellData
{
	public readonly string Value;
	public readonly int Row;
	public readonly int Column;
	public readonly int Index;
	public readonly CellState CellState;
	public readonly UserPlacedValue UserPlacedValue;

	public CellData(string value,
		int row,
		int column,
		int index,
		CellState cellState,
		UserPlacedValue userPlacedValue)
	{
		Value = value;
		Row = row;
		Column = column;
		Index = index;
		CellState = cellState;
		UserPlacedValue = userPlacedValue;
	}
}
