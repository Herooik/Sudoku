/*
using System.Collections.Generic;
using Board;

public class SudokuService : ISudokuService
{
	private SudokuBoard _sudokuBoard;

	public IReadOnlyList<CellData> Initialize(SudokuType sudokuType)
	{
		DisplayGridConfig rules = SudokuGridRules.GetRules(sudokuType);
		GridSolver gridSolver = new GridSolver();
		_sudokuBoard = new SudokuBoard(rules.Rows, rules.Columns, gridSolver);

		// GridSolver gridSolver = new(_sudokuBoard);
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

		// zaznacz wszystko domyslne
		foreach (Cell cell in _sudokuBoard.Cells)
		{
			array[cell.Index] = new CellData(
					cell.IsEmpty() ? string.Empty : cell.ActualValue.ToString(),
					cell.Row,
					cell.Column,
					cell.Index,
					CellState.NONE,
					GetUserPlacedValueState(cell));
		}

		// zaznacz wszystkie w tej samym rzędzie
		foreach (Cell cell in _sudokuBoard._rows[selectedCell.Row])
		{
			array[cell.Index] = new CellData(
				cell.IsEmpty() ? string.Empty : cell.ActualValue.ToString(),
				cell.Row,
				cell.Column,
				cell.Index,
				CellState.SAME_ROW_COLUMN,
				GetUserPlacedValueState(cell));
		}
		// zaznacz wszystkie w tej samej kolumnie
		foreach (Cell cell in _sudokuBoard._columns[selectedCell.Column])
		{
			array[cell.Index] = new CellData(
				cell.IsEmpty() ? string.Empty : cell.ActualValue.ToString(),
				cell.Row,
				cell.Column,
				cell.Index,
				CellState.SAME_ROW_COLUMN,
				GetUserPlacedValueState(cell));
		}
		// zaznacz wszystkie w tym samym group box
		foreach (Cell cell in _sudokuBoard._groupBoxes[selectedCell.GroupBox])
		{
			array[cell.Index] = new CellData(
					cell.IsEmpty() ? string.Empty : cell.ActualValue.ToString(),
					cell.Row,
					cell.Column,
					cell.Index,
					CellState.SAME_GROUP_BOX,
					GetUserPlacedValueState(cell));
		}

		// zaznacz wszystkie te same liczby
		foreach (Cell cell in _sudokuBoard.Cells)
		{
			if (cell.IsEmpty()) continue;

			if (cell.ActualValue == selectedCell.ActualValue)
			{
				array[cell.Index] = new CellData(
					cell.IsEmpty() ? string.Empty : cell.ActualValue.ToString(),
					cell.Row,
					cell.Column,
					cell.Index,
					CellState.SAME_VALUE,
					GetUserPlacedValueState(cell));
			}
		}

		// zaznacz wszystkie duplikaty w tym samym group box, rzedzie, kolumnie
		foreach (Cell cell in _sudokuBoard.Cells)
		{
			if (cell.IsEmpty()) continue;

			if (cell.ActualValue == selectedCell.ActualValue)
			{
				if (cell.Row == selectedCell.Row || cell.Column == selectedCell.Column ||
				    cell.GroupBox == selectedCell.GroupBox)
				{
					array[cell.Index] = new CellData(
						cell.IsEmpty() ? string.Empty : cell.ActualValue.ToString(),
						cell.Row,
						cell.Column,
						cell.Index,
						CellState.WRONG_VALUE,
						GetUserPlacedValueState(cell));
				}
			}
		}

		// Set current selected Cell
		array[selectedCellIndex] = new CellData(
			selectedCell.IsEmpty() ? string.Empty : selectedCell.ActualValue.ToString(),
			selectedCell.Row,
			selectedCell.Column,
			selectedCell.Index,
			CellState.SELECTED,
			GetUserPlacedValueState(selectedCell));

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
	WRONG_VALUE,
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
*/
