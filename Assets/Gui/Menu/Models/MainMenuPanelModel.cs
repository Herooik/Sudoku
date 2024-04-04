using System;
using System.Collections.Generic;
using System.Linq;
using Root;
using UnityEngine;

namespace Gui.Menu.Models
{
	public class MainMenuPanelModel
	{
		public event Action Refresh;

		public List<string> Difficulties => _difficulties.Select(sudokuDifficulty => sudokuDifficulty.ToString()).ToList();

		private readonly ApplicationNavigation _applicationNavigation;
		private readonly List<SudokuDifficulty> _difficulties;

		private SudokuDifficulty _selectedDifficulty;

		public MainMenuPanelModel(ApplicationNavigation applicationNavigation)
		{
			_applicationNavigation = applicationNavigation;

			_difficulties = new List<SudokuDifficulty>()
			{
				SudokuDifficulty.EASY,
				SudokuDifficulty.NORMAL,
				SudokuDifficulty.HARD,
				SudokuDifficulty.EXPERT,
				SudokuDifficulty.MASTER,
			};

			_selectedDifficulty = _difficulties[0];
		}

		public void StartNewGame()
		{
			_applicationNavigation.OpenGameplay(_selectedDifficulty);
		}

		public void ChooseDifficulty(int diffOption)
		{
			_selectedDifficulty = _difficulties[diffOption];
		}
	}
}
