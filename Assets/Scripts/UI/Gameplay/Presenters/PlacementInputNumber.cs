using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Presenters
{
	public class PlacementInputNumber : MonoBehaviour
	{
		public int Number { get; private set; }

		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField] private Button _button;

		public void Setup(int number, Action action)
		{
			Number = number;

			_button.onClick.AddListener(action.Invoke); // todo add IDisposable
			_text.SetText(number.ToString());
		}
	}
}
