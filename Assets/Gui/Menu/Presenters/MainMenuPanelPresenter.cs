using Gui.Menu.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Menu.Presenters
{
	public class MainMenuPanelPresenter : MonoBehaviour
	{
		[SerializeField] private Button _startNewGameButton;

		private MainMenuPanelModel _model;

		public void Bind(MainMenuPanelModel model)
		{
			_model = model;

			_startNewGameButton.onClick.AddListener(() => _model.StartNewGame());

			_model.Refresh += OnRefresh;
			OnRefresh();
		}

		private void OnDisable()
		{
			_model.Refresh -= OnRefresh;
		}

		private void OnRefresh()
		{
			
		}
	}
}
