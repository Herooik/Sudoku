using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gui.Menu.Models
{
	public class MainMenuPanelModel
	{
		public event Action Refresh;

		public List<string> Difficulties => _difficulties.Select(sudokuDifficulty => sudokuDifficulty.ToString()).ToList();

		private readonly ApplicationNavigation _applicationNavigation;
		private readonly SelectedGameSettings _selectedGameSettings;

		private readonly List<SudokuDifficulty> _difficulties;

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

			_selectedGameSettings.Difficulty = _difficulties[0]; 
		}

		public void StartNewGame()
		{
			_applicationNavigation.OpenGameplay();
		}

		public void ChooseDifficulty(int diffOption)
		{
			_selectedGameSettings.Difficulty = _difficulties[diffOption];
		}
	}
}
