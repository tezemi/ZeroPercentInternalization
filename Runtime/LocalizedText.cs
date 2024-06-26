﻿using UnityEngine;
using UnityEngine.UI;

namespace ZeroPercentInternalization
{
	[ExecuteAlways]
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("Zero Percent Localization/Localized Text", 1)]
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
