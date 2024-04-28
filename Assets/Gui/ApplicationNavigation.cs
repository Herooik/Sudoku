using Gui.Gameplay.Models;
using Gui.Gameplay.Presenters;
using Gui.Menu.Models;
using Gui.Menu.Presenters;
using Gui.ScriptableObjects;
using UnityEngine;

namespace Gui
{
	public class ApplicationNavigation : MonoBehaviour
	{
		// Script for handling application navigation like open gameplay scene, menu etc.

		[SerializeField] private Transform _panelHolder;
		[SerializeField] private GameplayPanelPresenter _gameplayPanelPresenter;
		[SerializeField] private MainMenuPanelPresenter _mainMenuPanelPresenter;

		[SerializeField] private DifficultyRulesSettings _difficultyRulesSettings;

		private readonly SelectedGameSettings _selectedGameSettings = new();

		private GameObject _currentPanel;

		private void Start()
		{
			OpenMainMenu();
		}

		public void OpenMainMenu()
		{
			if (_currentPanel != null)
				Destroy(_currentPanel);

			MainMenuPanelModel model = new MainMenuPanelModel(this, _selectedGameSettings);
			MainMenuPanelPresenter panel = Instantiate(_mainMenuPanelPresenter, _panelHolder);
			panel.GetComponent<MainMenuPanelPresenter>().Bind(model);

			_currentPanel = panel.gameObject;
		}

		public void OpenGameplay()
		{
			if (_currentPanel != null)
				Destroy(_currentPanel);

			GameplayPanelModel model = new GameplayPanelModel(this, _difficultyRulesSettings, _selectedGameSettings);
			GameplayPanelPresenter panel = Instantiate(_gameplayPanelPresenter, _panelHolder);
			panel.GetComponent<GameplayPanelPresenter>().Bind(model);

			_currentPanel = panel.gameObject;
		}
	}
}