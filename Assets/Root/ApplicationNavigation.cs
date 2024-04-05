using Gui.Gameplay.Models;
using Gui.Gameplay.Presenters;
using Gui.Menu.Models;
using Gui.Menu.Presenters;
using Gui.ScriptableObjects;
using UnityEngine;

namespace Root
{
	public class ApplicationNavigation : MonoBehaviour
	{
		// Script for handling application navigation like open gameplay scene, menu etc.

		[SerializeField] private Transform _panelHolder;
		[SerializeField] private GameplayPanelPresenter _gameplayPanelPresenter;
		[SerializeField] private MainMenuPanelPresenter _mainMenuPanelPresenter;

		[SerializeField] private DifficultyRulesSettings _difficultyRulesSettings;

		private void Start()
		{
			OpenMainMenu();
		}

		public void OpenGameplay(SudokuDifficulty selectedDifficulty)
		{
			//todo make it generic using MVP pattern
			GameplayPanelModel model = new GameplayPanelModel(this, _difficultyRulesSettings, SudokuType.NINE_BY_NINE, selectedDifficulty);
			GameObject presenter = Instantiate(_gameplayPanelPresenter, _panelHolder).gameObject;
			presenter.GetComponent<GameplayPanelPresenter>().Bind(model);
		}

		public void OpenMainMenu()
		{
			//todo make it generic using MVP pattern
			MainMenuPanelModel model = new MainMenuPanelModel(this);
			GameObject presenter = Instantiate(_mainMenuPanelPresenter, _panelHolder).gameObject;
			presenter.GetComponent<MainMenuPanelPresenter>().Bind(model);
		}
	}
}
