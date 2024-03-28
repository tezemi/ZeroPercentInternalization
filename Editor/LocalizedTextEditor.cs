using System;
using UnityEngine;
using UnityEditor;

namespace ZeroPercentInternalization.Editor
{
	[CustomEditor(typeof(LocalizedText))]
	public class LocalizedTextEditor : UnityEditor.Editor
	{
		private string[] _keys;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var localizedText = (LocalizedText)target;

			if (localizedText.InternalizedText != null)
			{
				if (_keys == null || _keys.Length == 0)
					PopulateKeys();

				if (_keys != null && _keys.Length > 0)
				{
					var selectedIndex = EditorGUILayout.Popup("Key", localizedText.SelectedKeyIndex, _keys);
					try
					{
						localizedText.SelectedKey = _keys[selectedIndex];
					}
					catch (IndexOutOfRangeException)
					{
						Debug.LogWarning($"Internalized text key was not found on '{localizedText.gameObject.name}'. It may have been moved.", localizedText);
						localizedText.SelectedKey = _keys[0];
						localizedText.SetTextToSelectedValue();
					}
				}

				if (GUI.changed)
				{
					localizedText.SetTextToSelectedValue();
					PopulateKeys();
				}
			}
		}

		private void PopulateKeys()
		{
			_keys = ((LocalizedText)target).InternalizedText.GetKeys();
		}
	}
}
