using Gui.Gameplay.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gui.Gameplay.Presenters
{
	public class GameplayPanelPresenter : MonoBehaviour
	{
		[SerializeField] private BoardPanelPresenter _boardPanelPresenter;
		[FormerlySerializedAs("_numbersToPlacePresenter")] [SerializeField] private PlayerNumberPlacementPresenter _playerNumberPlacementPresenter;

		private GameplayPanelModel _model;

		public void Bind(GameplayPanelModel model)
		{
			_model = model;

			_model.Setup += OnSetup;
			_model.Refresh += OnRefresh;
		}

		private void OnDisable()
		{
			_model.Setup -= OnSetup;
			_model.Refresh -= OnRefresh;
		}

		private void OnSetup()
		{
			_boardPanelPresenter.Initialize(9, _model._cellDisplays, _model.SelectCell);
			_playerNumberPlacementPresenter.Initialize(_model.Numbers, _model.PlaceNumber);

			OnRefresh();
		}

		private void OnRefresh()
		{
			_boardPanelPresenter.Refresh(_model._cellDisplays);
			_playerNumberPlacementPresenter.Refresh();
		}
	}
}