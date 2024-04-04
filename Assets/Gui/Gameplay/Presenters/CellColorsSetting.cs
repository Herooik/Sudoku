using UnityEngine;

namespace Gui.Gameplay.Presenters
{
	[CreateAssetMenu(fileName = nameof(CellColorsSetting), menuName = "Game/" + nameof(CellColorsSetting))]
	public class CellColorsSetting : ScriptableObject
	{
		public Color _default = Color.white;
		public Color _selected = Color.cyan;
		public Color _sameNumber = new Color(.5f, .5f, 0, 1);
		public Color _sameWrongNumberInColumnRowGroupBox = new Color(1f,.5f, .5f, 1);
		public Color _sameColumnRowGroupBox = Color.yellow;
		
		[Header("Text colors")]
		public Color _goodNumberText = Color.blue;
		public Color _wrongNumberText = Color.red;
	}
}