using System.Linq;
using UnityEngine;
using UnityEditor;

namespace ZeroPercentInternalization.Editor
{
	[CustomEditor(typeof(LocalizedText))]
	public class LocalizedTextEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var localizedText = (LocalizedText)target;

			if (localizedText.InternalizedText is not null)
				localizedText.SelectedKeyIndex = EditorGUILayout.Popup("Key", localizedText.SelectedKeyIndex, localizedText.InternalizedText.TextEntries.Select(t => t.Key).ToArray());

			if (GUI.changed)
				localizedText.SetTextToSelectedValue();
		}
	}
}
