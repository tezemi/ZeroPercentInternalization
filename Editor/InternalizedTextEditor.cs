using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

namespace ZeroPercentInternalization.Editor
{
	[CustomEditor(typeof(InternalizedText))]
	public class InternalizedTextEditor : UnityEditor.Editor
	{
		private readonly List<int> _invalidKeyIndices = new List<int>();
		private bool _isInvalid => _invalidKeyIndices.Any();

		protected virtual void OnEnable()
		{
			TestIsInvalid((InternalizedText)target);
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.EndDisabledGroup();

			var text = (InternalizedText)target;

			Color initialTextColor = GUI.color;

			if (_isInvalid)
			{
				GUI.color = Color.red;
				EditorGUILayout.LabelField("Duplicate key names are not allowed.");
				GUI.color = initialTextColor;
			}

			if (text.Language == Language.NONE)
			{
				GUI.color = Color.yellow;
				EditorGUILayout.LabelField("A language should be specified.");
				GUI.color = initialTextColor;
			}

			if (text.Language == Language.NONE)
				GUI.backgroundColor = Color.yellow;
			text.Language = (Language)EditorGUILayout.EnumPopup("Language", text.Language);
			GUI.backgroundColor = initialTextColor;

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Keys", EditorStyles.boldLabel, GUILayout.Width(128f));
			EditorGUILayout.LabelField("Values", EditorStyles.boldLabel, GUILayout.Width(128f));
			GUILayout.EndHorizontal();

			for (var i = 0; i < text.TextEntries.Count; i++)
			{
				GUILayout.BeginHorizontal();

				// Simple text box for key
				Color initialBackgroundColor = GUI.backgroundColor;
				if (_invalidKeyIndices.Contains(i))
					GUI.backgroundColor = Color.red;
				text.TextEntries[i].Key = GUILayout.TextField(text.TextEntries[i].Key, GUILayout.Width(128f));
				GUI.backgroundColor = initialBackgroundColor;

				// More complex text area for values
				var valueGUIContent = new GUIContent(text.TextEntries[i].Value);
				var textAreaGUIStyle = new GUIStyle(EditorStyles.textArea);
				textAreaGUIStyle.CalcHeight(valueGUIContent, 0f);	// width is irrelevant
				textAreaGUIStyle.wordWrap = true;
				text.TextEntries[i].Value = GUILayout.TextArea(text.TextEntries[i].Value, textAreaGUIStyle);

				if (GUILayout.Button("-", GUILayout.Width(20f)))
				{
					text.TextEntries.RemoveAt(i);
				}

				GUILayout.EndHorizontal();
			}

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("+", GUILayout.Width(20f)))
			{
				text.TextEntries.Add(new TextEntry());
			}

			GUILayout.EndHorizontal();

			if (GUI.changed)
			{
				TestIsInvalid(text);

				var json = JsonConvert.SerializeObject(text, Formatting.Indented);
				File.WriteAllText(AssetDatabase.GetAssetPath(text), json);

				if (!_isInvalid)
				{
					foreach (var localizedText in FindObjectsOfType<LocalizedText>())
					{
						localizedText.SetTextToSelectedValue();
					}
				}
			}
		}

		private void TestIsInvalid(InternalizedText text)
		{
			_invalidKeyIndices.Clear();
			for (var i = 0; i < text.TextEntries.Count; i++)
			{
				for (var j = i + 1; j < text.TextEntries.Count; j++)
				{
					if (text.TextEntries[i].Key == text.TextEntries[j].Key)
					{
						_invalidKeyIndices.Add(i);
						_invalidKeyIndices.Add(j);
					}
				}
			}
		}
	}
}
