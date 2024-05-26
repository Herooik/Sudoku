namespace Cells
{
	public interface ICell
	{
		int Index { get; }
		int GroupBox { get; }
		int Row { get; }
		int Column { get; }
		int Number { get; }

		bool IsEmpty => Number <= 0;
		bool IsPlacedGood { get; }
		bool IsSolverCell { get; }
	}
}

public enum CellType
{
	EMPTY,
	SOLVER,
	USER,
}