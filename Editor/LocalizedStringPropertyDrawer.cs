using UnityEngine;
using UnityEditor;

namespace ZeroPercentInternalization.Editor
{
	[CustomPropertyDrawer(typeof(LocalizedString))]
	public class LocalizedStringPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var internalizedTextRect = new Rect(position.x, position.y, position.width / 2f, position.height);
			var keyRect = new Rect(position.x + position.width / 2f, position.y, position.width / 2f, position.height);

			var internalizedTextProperty = property.FindPropertyRelative("InternalizedText");

			EditorGUI.ObjectField(internalizedTextRect, internalizedTextProperty, GUIContent.none);

			if (internalizedTextProperty.objectReferenceValue != null)
			{
				var keyProperty = property.FindPropertyRelative("Key");

				var internalizedText = (InternalizedText)internalizedTextProperty.objectReferenceValue;

				var keys = internalizedText.GetKeys();

				var selectedIndex = 0;
				foreach (var key in keys)
				{
					if (key == keyProperty.stringValue)
					{
						break;
					}

					selectedIndex++;
				}

				if (selectedIndex >= keys.Length)
				{
					selectedIndex = 0;
				}

				GUIContent[] guiContents = new GUIContent[keys.Length];
				for (int i = 0; i < keys.Length; i++)
				{
					string key = keys[i];
					guiContents[i] = new GUIContent(key, internalizedText.GetValue(key));
				}

				keyProperty.stringValue = keys[EditorGUI.Popup(keyRect, selectedIndex, guiContents)];
			}

			EditorGUI.EndProperty();
		}
	}
}
