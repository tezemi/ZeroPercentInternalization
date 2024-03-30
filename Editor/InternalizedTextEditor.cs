using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

namespace ZeroPercentInternalization.Editor
{
	[CustomEditor(typeof(InternalizedText))]
	public class InternalizedTextEditor : UnityEditor.Editor
	{
		private readonly List<int> _invalidKeyIndices = new List<int>();
		private int _selectedLanguageIndex;
		private bool _hasInvalidKeys => _invalidKeyIndices.Any();

		protected virtual void OnEnable()
		{
			TestIsInvalid();
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.EndDisabledGroup();

			var text = (InternalizedText)target;

			// Begin errors and warnings
			Color initialTextColor = GUI.color;
			if (_hasInvalidKeys)
			{
				GUI.color = Color.red;
				EditorGUILayout.LabelField("Duplicate key names are not allowed.");
				GUI.color = initialTextColor;
			}

			// The language header
			// Shows the selected language, and buttons for adding and removing languages
			GUILayout.BeginHorizontal();
			var languageNames = text.Languages.Select(l => l.ToString()).ToArray();
			_selectedLanguageIndex = EditorGUILayout.Popup("Language", _selectedLanguageIndex, languageNames);

			var newLanguage = (Language)EditorGUILayout.EnumPopup(Language.NONE, GUILayout.Width(20f));
			if (newLanguage != Language.NONE && !text.Languages.Contains(newLanguage))
			{
				text.AddLanguageMap(newLanguage);

				_selectedLanguageIndex = text.Languages.Count - 1;

				SaveToDisk();

				return;
			}

			if (GUILayout.Button("-", GUILayout.Width(20f)))
			{
				if (EditorUtility.DisplayDialog("Delete Language",
					    "Do you want to delete this language? It will delete every text entry associated with it.",
					    "Yes", "Cancel")) 
				{
					text.RemoveLanguageMap(text.Languages[_selectedLanguageIndex]);
					_selectedLanguageIndex--;

					if (_selectedLanguageIndex < 0)
						_selectedLanguageIndex = 0;

					SaveToDisk();

					return;
				}
			}

			GUILayout.EndHorizontal();

			// If there are no languages, don't render anything
			if (!text.Languages.Any())
				return;

			// Text entry list starts here
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Keys", EditorStyles.boldLabel, GUILayout.Width(128f));
			EditorGUILayout.LabelField("Values", EditorStyles.boldLabel, GUILayout.Width(128f));
			GUILayout.EndHorizontal();

			Language selectedLanguage = text.Languages[_selectedLanguageIndex];
			List<TextEntry> textEntries = text.GetTextEntriesForLanguage(selectedLanguage);

			for (var i = 0; i < textEntries.Count; i++)
			{
				GUILayout.BeginHorizontal();

				// Simple text box for key
				Color initialBackgroundColor = GUI.backgroundColor;

				if (_invalidKeyIndices.Contains(i))
					GUI.backgroundColor = Color.red;

				var newKey = GUILayout.TextField(textEntries[i].Key, GUILayout.Width(128f));
				
				if (textEntries[i].Key != newKey)
					text.UpdateKeyForAllLanguages(i, newKey);

				GUI.backgroundColor = initialBackgroundColor;

				// More complex text area for values
				var value = textEntries[i].Value;
				var valueGUIContent = new GUIContent(value);
				var textAreaGUIStyle = new GUIStyle(EditorStyles.textArea);
				textAreaGUIStyle.CalcHeight(valueGUIContent, 0f);	// width is irrelevant
				textAreaGUIStyle.wordWrap = true;
				textEntries[i].Value = GUILayout.TextArea(value, textAreaGUIStyle);

				if (GUILayout.Button("-", GUILayout.Width(20f)))
				{
					text.RemoveKeyForAllLanguages(i);
				}

				GUILayout.EndHorizontal();
			}

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("+", GUILayout.Width(20f)))
			{
				text.AddKeyForAllLanguages();
			}

			GUILayout.EndHorizontal();

			// If something has changed, make sure it's invalid, then save to the disk, and update scene
			if (GUI.changed)
			{
				TestIsInvalid();

				SaveToDisk();

				if (!_hasInvalidKeys)
				{
					foreach (var localizedText in FindObjectsOfType<LocalizedText>())
					{
						localizedText.SetTextToSelectedValue();
					}
				}
			}
		}

		private void TestIsInvalid()
		{
			var text = (InternalizedText)target;
			_invalidKeyIndices.Clear();

			if (!text.Languages.Any())
				return;

			var keys = text.GetKeys();
			for (var i = 0; i < keys.Length; i++)
			{
				for (var j = i + 1; j < keys.Length; j++)
				{
					if (keys[i] == keys[j])
					{
						_invalidKeyIndices.Add(i);
						_invalidKeyIndices.Add(j);
					}
				}
			}
		}

		private void SaveToDisk()
		{
			var text = (InternalizedText)target;
			var json = JsonConvert.SerializeObject(text, Formatting.Indented);
			File.WriteAllText(AssetDatabase.GetAssetPath(text), json);
		}
	}
}
