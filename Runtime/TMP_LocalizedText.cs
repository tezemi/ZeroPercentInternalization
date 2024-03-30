using UnityEngine;
using TMPro;

namespace ZeroPercentInternalization
{
	[ExecuteAlways]
	[RequireComponent(typeof(TMP_Text))]
	[AddComponentMenu("Zero Percent Localization/Localized Text (TextMeshPro)", 2)]
	public class TMP_LocalizedText : MonoBehaviour
	{
		[HideInInspector]
		public string SelectedKey;
		public InternalizedText InternalizedText;
		public TMP_Text Text { get; private set; }

		public int SelectedKeyIndex
		{
			get
			{
				int i = 0;
				foreach (var key in InternalizedText.GetKeys())
				{
					if (key == SelectedKey)
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
			Text = GetComponent<TMP_Text>();
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
				Text = GetComponent<TMP_Text>();

			if (InternalizedText != null && !string.IsNullOrEmpty(SelectedKey))
				Text.text = InternalizedText.GetValue(SelectedKey);
		}

		public static void UpdateAll()
		{
			foreach (var localizedText in FindObjectsOfType<LocalizedText>())
			{
				localizedText.SetTextToSelectedValue();
			}

			foreach (var localizedText in FindObjectsOfType<TMP_LocalizedText>())
			{
				localizedText.SetTextToSelectedValue();
			}
		}
	}
}