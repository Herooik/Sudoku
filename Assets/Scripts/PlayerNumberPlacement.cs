using System.Collections.Generic;

public class PlayerNumberPlacement
{
	public IEnumerable<int> AvailableNumbers => _availableNumbers;

	private readonly List<int> _availableNumbers;
	private readonly List<int> _allNumbers;

	public PlayerNumberPlacement(int boardMaxColumns)
	{
		List<int> allNumbers = new();
		for (int i = 1; i <= boardMaxColumns; i++)
		{
			allNumbers.Add(i);
		}
		_allNumbers = allNumbers;
		_availableNumbers = new List<int>(allNumbers);
	}

	public void RemoveNumber(int number)
	{
		_availableNumbers.Remove(number);
	}
}