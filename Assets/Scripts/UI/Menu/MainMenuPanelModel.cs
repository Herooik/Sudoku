using System.Collections.Generic;
using System.Linq;
using Configs;

namespace UI.Menu
{
	public class MainMenuPanelModel
	{
		public List<string> Difficulties => _difficulties.Select(sudokuDifficulty => sudokuDifficulty.ToString()).ToList();
		public int SelectedDifficulty { get; private set; }
		public List<string> Types => _types.Select(sudokuType => sudokuType.ToString()).ToList();
		public int SelectedType { get; private set; }

		private readonly ApplicationNavigation _applicationNavigation;
		private readonly SelectedGameSettings _selectedGameSettings;

		private readonly List<SudokuDifficulty> _difficulties;
		private readonly List<SudokuType> _types;

		public MainMenuPanelModel(
			ApplicationNavigation applicationNavigation,
			SelectedGameSettings selectedGameSettings)
		{
			_applicationNavigation = applicationNavigation;
			_selectedGameSettings = selectedGameSettings;

			_difficulties = new List<SudokuDifficulty>()
			{
				SudokuDifficulty.EASY,
				SudokuDifficulty.NORMAL,
				SudokuDifficulty.HARD,
				SudokuDifficulty.EXPERT,
				SudokuDifficulty.MASTER,
			};

			_types = new List<SudokuType>()
			{
				SudokuType.FOUR_BY_FOUR,
				SudokuType.SIX_BY_SIX,
				SudokuType.EIGHT_BY_EIGHT,
				SudokuType.NINE_BY_NINE,
				SudokuType.TEN_BY_TEN,
				SudokuType.TWELVE_BY_TWELVE,
				SudokuType.SIXTEEN_BY_SIXTEEN,
			};

			SelectedDifficulty = 0;
			foreach (SudokuDifficulty difficulty in _difficulties.Where(d => d == _selectedGameSettings.Difficulty))
			{
				SelectedDifficulty = _difficulties.IndexOf(difficulty);
				break;
			}

			SelectedType = 0;
			foreach (SudokuType sudokuType in _types.Where(t => t == _selectedGameSettings.SudokuType))
			{
				SelectedType = _types.IndexOf(sudokuType);
				break;
			}
		}

		public void StartNewGame()
		{
			_applicationNavigation.OpenGameplay();
		}

		public void ChooseDifficulty(int diffOption)
		{
			_selectedGameSettings.Difficulty = _difficulties[diffOption];
		}
		
		public void ChooseType(int diffOption)
		{
			_selectedGameSettings.SudokuType = _types[diffOption];
		}
	}
}
