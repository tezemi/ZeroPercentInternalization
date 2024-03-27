using UnityEngine;
using UnityEngine.UI;

namespace ZeroPercentInternalization
{
	[ExecuteAlways]
	[RequireComponent(typeof(Text))]
	public class LocalizedText : MonoBehaviour
	{
		[HideInInspector]
		public int SelectedKeyIndex;
		public InternalizedText InternalizedText;
		public Text Text { get; private set; }

		protected virtual void Awake()
		{
			Text = GetComponent<Text>();
		}

		protected virtual void OnEnable()
		{
			SetTextToSelectedValue();
		}

		public virtual void OnValidate()
		{
			SetTextToSelectedValue();
		}

		public void SetTextToSelectedValue()
		{
			if (Text == null)
				Text = GetComponent<Text>();

			if (InternalizedText != null)
				Text.text = InternalizedText.Values[SelectedKeyIndex];
		}
	}
}
