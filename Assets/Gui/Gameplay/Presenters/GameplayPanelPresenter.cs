using Gui.Gameplay.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gui.Gameplay.Presenters
{
	public class GameplayPanelPresenter : MonoBehaviour
	{
		[FormerlySerializedAs("_gridPanelComponent")] [SerializeField] private BoardPanelComponent _boardPanelComponent;

		private GameplayPanelModel _model;

		public void Init(GameplayPanelModel model)
		{
			_model = model;
			_model.Setup += Setup;
		}

		private void Setup()
		{
		}
	}
}