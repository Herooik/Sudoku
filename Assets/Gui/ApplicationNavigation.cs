using Gui.Gameplay.Models;
using Gui.Gameplay.Presenters;
using Gui.Menu.Models;
using Gui.Menu.Presenters;
using Gui.ScriptableObjects;
using UnityEngine;

public class SelectedGameSettings
{
	public SudokuType SudokuType = SudokuType.NINE_BY_NINE;
	public SudokuDifficulty Difficulty = SudokuDifficulty.NORMAL;
}

namespace Gui
{
	public class ApplicationNavigation : MonoBehaviour
	{
		// Script for handling application navigation like open gameplay scene, menu etc.

		[SerializeField] private Transform _panelHolder;
		[SerializeField] private GameplayPanelPresenter _gameplayPanelPresenter;
		[SerializeField] private MainMenuPanelPresenter _mainMenuPanelPresenter;

		[SerializeField] private DifficultyRulesSettings _difficultyRulesSettings;

		private SelectedGameSettings _selectedGameSettings = new();

		private MainMenuPanelModel _menuPanelModel;
		private GameplayPanelModel _gameplayModel;

		private void Start()
		{
			_menuPanelModel = new MainMenuPanelModel(this, _selectedGameSettings);
			_gameplayModel = new GameplayPanelModel(this, _difficultyRulesSettings, _selectedGameSettings);

			OpenMainMenu();
		}

		public void OpenMainMenu()
		{
			MainMenuPanelPresenter panel = Instantiate(_mainMenuPanelPresenter, _panelHolder);
			panel.GetComponent<MainMenuPanelPresenter>().Bind(_menuPanelModel);
		}

		public void OpenGameplay()
		{
			GameplayPanelPresenter panel = Instantiate(_gameplayPanelPresenter, _panelHolder);
			panel.GetComponent<GameplayPanelPresenter>().Bind(_gameplayModel);
		}
	}
}
