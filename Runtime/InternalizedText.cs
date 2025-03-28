using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Newtonsoft.Json;

namespace ZeroPercentInternalization
{
	[Serializable]
	public class InternalizedText : ScriptableObject
	{
		[HideInInspector]
		public List<Language> Languages = new List<Language>();
		[JsonProperty]
		[SerializeField]
		private List<LanguageMap> _languageMaps = new List<LanguageMap>();
		private readonly Dictionary<string, string> _keyToValueCache = new Dictionary<string, string>();
		private readonly Dictionary<Language, List<TextEntry>> _languageToTextCache = new Dictionary<Language, List<TextEntry>>();

		protected virtual void OnValidate()
		{
			if (!Languages.Any())
				return;

			var keyCount = _languageMaps[0].TextEntries.Count;

			foreach (var languageMap in _languageMaps)
			{
				if (languageMap.TextEntries.Count != keyCount)
				{
					Debug.LogError($"Language '{languageMap.Language}' on '{name}' has an incorrect number of keys. " +
					               $"If you edited the JSON directly, make sure that every language has the same number of keys.", this);
				}
			}
		}

		public string GetValue(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogWarning("Can't get text value for empty or null key.", this);

				return string.Empty;
			}
			
			Language language = ZeroPercentInternalizationConfiguration.Language;

			// If the game is running, use cached values, if not, use only recent values 
			if (Application.isPlaying)
			{
				// If the language map cache doesn't have this key, it hasn't been populated yet
				if (!_languageToTextCache.ContainsKey(language))
				{
					foreach (var languageMap in _languageMaps)
					{
						_languageToTextCache.Add(languageMap.Language, languageMap.TextEntries);
					}
				}

				if (_languageToTextCache.ContainsKey(language))
				{
					List<TextEntry> textEntries = _languageToTextCache[language];

					// If the text cache doesn't have this key, it also hasn't been populated yet
					if (!_keyToValueCache.ContainsKey($"{language}{key}"))
					{
						foreach (var entry in textEntries)
						{
							_keyToValueCache.Add($"{language}{entry.Key}", entry.Value);
						}
					}

					if (_keyToValueCache.ContainsKey($"{language}{key}"))
					{
						return _keyToValueCache[$"{language}{key}"];
					}
					else
					{
						Debug.LogWarning($"Could not find the key '{key}' in {name}'s '{language}' language map.", this);

						return string.Empty;
					}
				}
				else
				{
					Debug.LogWarning($"Could not find the '{language}' language map in {name}.", this);

					return string.Empty;
				}
			}
			else
			{
				foreach (var languageMap in _languageMaps)
				{
					if (languageMap.Language == language)
					{
						List<TextEntry> textEntries = languageMap.TextEntries;

						foreach (var entry in textEntries)
						{
							if (entry.Key == key)
							{
								return entry.Value;
							}
						}

						Debug.LogWarning($"Could not find the key '{key}' in {name}'s '{language}' language map.", this);

						return string.Empty;
					}
				}

				Debug.LogWarning($"Could not find the '{language}' language map in {name}.", this);

				return string.Empty;
			}
		}

		public string[] GetKeys()
		{
			if (!Languages.Any())
			{
				Debug.LogWarning($"Can't get keys for '{name}'. There are no languages.", this);
			}

			List<TextEntry> textEntries = _languageMaps[0].TextEntries;

			var keys = new string[textEntries.Count];
			for (var i = 0; i < textEntries.Count; i++)
			{
				var entry = textEntries[i];
				keys[i] = entry.Key;
			}

			return keys;
		}
		
		public void Initialize(List<LanguageMap> languageMaps)
		{
			Languages.Clear();
			_languageMaps = languageMaps;

			foreach (var languageMap in _languageMaps)
			{
				Languages.Add(languageMap.Language);
			}
		}

		public void UpdateKeyForAllLanguages(int index, string newKey)
		{
			foreach (var languageMap in _languageMaps)
			{
				languageMap.TextEntries[index].Key = newKey;
			}
		}

		public void AddKeyForAllLanguages()
		{
			foreach (var languageMap in _languageMaps)
			{
				string keyName = $"NewKey{languageMap.TextEntries.Count:D2}";
				if (languageMap.TextEntries.Count > 0)
					keyName = Regex.Replace
					(
						languageMap.TextEntries.Last().Key, 
						"[0-9]+$", 
						n => (Convert.ToInt32(n.Value) + 1).ToString("D2")
					);

				languageMap.TextEntries.Add(new TextEntry { Key = keyName });
			}
		}

		public void RemoveKeyForAllLanguages(int index)
		{
			foreach (var languageMap in _languageMaps)
			{
				try
				{
					languageMap.TextEntries.RemoveAt(index);
				}
				catch (ArgumentOutOfRangeException)
				{
					Debug.LogError($"Couldn't remove a key on language '{languageMap.Language}' for '{name}'. The data may be malformed.", this);
				}
			}
		}

		public void SwapKeyIndicesForAllLanguages(int index1, int index2)
		{
			foreach (var languageMap in _languageMaps)
			{
				try
				{
					TextEntry entry1 = languageMap.TextEntries[index1];
					TextEntry entry2 = languageMap.TextEntries[index2];

					languageMap.TextEntries[index1] = entry2;
					languageMap.TextEntries[index2] = entry1;
				}
				catch (ArgumentOutOfRangeException)
				{
					Debug.LogError($"Couldn't switch key on language '{languageMap.Language}' for '{name}'. The data may be malformed.", this);
				}
			}
		}

		public void AddLanguageMap(Language language)
		{
			if (Languages.Contains(language))
			{
				throw new ArgumentException("Can't add a new language map, one already exists for that language.", nameof(Languages));
			}

			var newLanguageMap = new LanguageMap(language);
			_languageMaps.Add(newLanguageMap);
			Languages.Add(language);

			foreach (var key in GetKeys())
			{
				newLanguageMap.TextEntries.Add(new TextEntry { Key = key });
			}
		}

		public void RemoveLanguageMap(Language language)
		{
			if (!Languages.Contains(language))
			{
				throw new ArgumentException("Can't remove the language map, one does not exist for that language.", nameof(Languages));
			}

			LanguageMap languageMapToRemove = null;
			foreach (var languageMap in _languageMaps)
			{
				if (languageMap.Language == language)
				{
					languageMapToRemove = languageMap;
					break;
				}
			}

			_languageMaps.Remove(languageMapToRemove);
			Languages.Remove(language);
		}

		public List<TextEntry> GetTextEntriesForLanguage(Language language)
		{
			foreach (var languageMap in _languageMaps)
			{
				if (languageMap.Language == language)
					return languageMap.TextEntries;
			}

			return null;
		}
	}
}

