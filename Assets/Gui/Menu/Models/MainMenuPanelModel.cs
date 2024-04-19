using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using UnityEngine;

namespace Gui.Menu.Models
{
	public class MainMenuPanelModel
	{
		public event Action Refresh;

		public List<string> Difficulties => _difficulties.Select(sudokuDifficulty => sudokuDifficulty.ToString()).ToList();
		public List<string> Types => _types.Select(sudokuType => sudokuType.ToString()).ToList();

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

			_selectedGameSettings.Difficulty = _difficulties[0]; 
			_selectedGameSettings.SudokuType = _types[0]; 
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
