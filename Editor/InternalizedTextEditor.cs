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

			if (_isInvalid)
			{
				Color initialTextColor = GUI.color;
				GUI.color = Color.red;
				EditorGUILayout.LabelField("Duplicate key names are not allowed.");
				GUI.color = initialTextColor;
			}

			text.Language = (Language)EditorGUILayout.EnumPopup("Language", text.Language);

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Keys", EditorStyles.boldLabel, GUILayout.Width(128f));
			EditorGUILayout.LabelField("Values", EditorStyles.boldLabel, GUILayout.Width(128f));
			GUILayout.EndHorizontal();

			for (var i = 0; i < text.Keys.Count; i++)
			{
				GUILayout.BeginHorizontal();

				// Simple text box for key
				Color initialBackgroundColor = GUI.backgroundColor;
				if (_invalidKeyIndices.Contains(i))
					GUI.backgroundColor = Color.red;
				text.Keys[i] = GUILayout.TextField(text.Keys[i], GUILayout.Width(128f));
				GUI.backgroundColor = initialBackgroundColor;

				// More complex text area for values
				var valueGUIContent = new GUIContent(text.Values[i]);
				var textAreaGUIStyle = new GUIStyle(EditorStyles.textArea);
				textAreaGUIStyle.CalcHeight(valueGUIContent, 0f);	// width is irrelevant
				textAreaGUIStyle.wordWrap = true;
				text.Values[i] = GUILayout.TextArea(text.Values[i], textAreaGUIStyle);

				if (GUILayout.Button("-", GUILayout.Width(20f)))
				{
					text.Keys.RemoveAt(i);
					text.Values.RemoveAt(i);
				}

				GUILayout.EndHorizontal();
			}

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("+", GUILayout.Width(20f)))
			{
				text.Keys.Add("NewKey");
				text.Values.Add(string.Empty);
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
			for (var i = 0; i < text.Keys.Count; i++)
			{
				for (var j = i + 1; j < text.Keys.Count; j++)
				{
					if (text.Keys[i] == text.Keys[j])
					{
						_invalidKeyIndices.Add(i);
						_invalidKeyIndices.Add(j);
					}
				}
			}
		}
	}
}
