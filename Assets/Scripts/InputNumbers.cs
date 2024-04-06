using System.Collections.Generic;

public class InputNumbers
{
	public IReadOnlyList<int> AllNumbers => _allNumbers;
	public IEnumerable<int> AvailableNumbers => _availableNumbers;

	private readonly List<int> _availableNumbers;
	private readonly List<int> _allNumbers;

	public InputNumbers(int boardMaxColumns)
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
		if (_availableNumbers.Contains(number)) 
			_availableNumbers.Remove(number);
	}

	public void AddNumber(int number)
	{
		if (!_availableNumbers.Contains(number))
			_availableNumbers.Add(number);
	}
}