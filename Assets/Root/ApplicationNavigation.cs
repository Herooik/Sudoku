using Gui.Gameplay.Models;
using Gui.Gameplay.Presenters;
using UnityEngine;

namespace Root
{
	public class ApplicationNavigation : MonoBehaviour
	{
		// Script for handling application navigation like open gameplay scene, menu etc.

		[SerializeField] private Transform _panelHolder;
		[SerializeField] private GameplayPanelPresenter _gameplayPanelPresenter;

		private void Start()
		{
			OpenGameplay();
		}

		public void OpenGameplay()
		{
			//todo make it generic using MVP pattern
			GameplayPanelModel model = new GameplayPanelModel();
			GameObject presenter = Instantiate(_gameplayPanelPresenter, _panelHolder).gameObject;
			presenter.GetComponent<GameplayPanelPresenter>().Bind(model);

			model.Show();
		}
	}
}
