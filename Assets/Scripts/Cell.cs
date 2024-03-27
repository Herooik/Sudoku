public class Cell
{
	public int ActualValue { get; private set; }
	public bool IsPlacedByGenerator { get; private set; } = true;

	public readonly int Index;
	public readonly int GroupBox;
	public readonly int Row;
	public readonly int Column;

	private int _expectedValue;

	public Cell(int index, int row, int column, int groupBox, int value)
	{
		Index = index;
		GroupBox = groupBox;
		ActualValue = value;
		Row = row;
		Column = column;
	}

	public bool IsEmpty()
	{
		return ActualValue <= 0;
	}

	public void SetValue(int value)
	{
		ActualValue = value;
	}

	public void RemoveValue()
	{
		IsPlacedByGenerator = false;
		_expectedValue = ActualValue;
		ActualValue = -1;
	}

	public bool IsPlacedGoodValue()
	{
		return ActualValue == _expectedValue;
	}
}