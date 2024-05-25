using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Presenters
{
	public class MainMenuPanelPresenter : MonoBehaviour
	{
		[SerializeField] private Button _continueButton;
		[SerializeField] private Button _startNewGameButton;
		[SerializeField] private TMP_Dropdown _difficultyDropdown;
		[SerializeField] private TMP_Dropdown _typeDropdown;

		private MainMenuPanelModel _model;

		public void Bind(MainMenuPanelModel model)
		{
			_model = model;

			_continueButton.onClick.AddListener(() => _model.ContinueGame());
			_startNewGameButton.onClick.AddListener(() => _model.StartNewGame());

			_difficultyDropdown.options = new List<TMP_Dropdown.OptionData>();
			_difficultyDropdown.AddOptions(model.Difficulties);
			_difficultyDropdown.onValueChanged.AddListener(model.ChooseDifficulty);
			_typeDropdown.SetValueWithoutNotify(_model.SelectedDifficulty);

			_typeDropdown.options = new List<TMP_Dropdown.OptionData>();
			_typeDropdown.AddOptions(model.Types);
			_typeDropdown.onValueChanged.AddListener(model.ChooseType);
			_typeDropdown.SetValueWithoutNotify(_model.SelectedType);
		}
	}
}
