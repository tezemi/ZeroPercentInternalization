using UnityEngine;
using UnityEngine.UI;

namespace ZeroPercentInternalization
{
	[ExecuteAlways]
	[RequireComponent(typeof(Text))]
	public class LocalizedText : MonoBehaviour
	{
		[HideInInspector]
		public string SelectedKey;
		public InternalizedText InternalizedText;
		public Text Text { get; private set; }

		public int SelectedKeyIndex
		{
			get
			{
				int i = 0;
				foreach (var entry in InternalizedText.TextEntries)
				{
					if (entry.Key == SelectedKey)
					{
						return i;
					}

					i++;
				}

				return 0;
			}
		}

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

			if (InternalizedText != null && !string.IsNullOrEmpty(SelectedKey))
				Text.text = InternalizedText.GetValue(SelectedKey);
		}
	}
}
