using System.Collections.Generic;
using Gui.Menu.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Menu.Presenters
{
	public class MainMenuPanelPresenter : MonoBehaviour
	{
		[SerializeField] private Button _startNewGameButton;
		[SerializeField] private TMP_Dropdown _difficultyDropdown;

		private MainMenuPanelModel _model;

		public void Bind(MainMenuPanelModel model)
		{
			_model = model;

			_difficultyDropdown.options = new List<TMP_Dropdown.OptionData>();
			_difficultyDropdown.AddOptions(model.Difficulties);
			_difficultyDropdown.onValueChanged.AddListener(model.ChooseDifficulty);

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
