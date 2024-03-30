using System;
using System.Collections.Generic;
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
			var language = ZeroPercentInternalizationConfiguration.Language;

			foreach (var languageMap in _languageMaps)
			{
				if (languageMap.Language == language)
				{
					List<TextEntry> textEntries = languageMap.TextEntries;

					var keys = new string[textEntries.Count];
					for (var i = 0; i < textEntries.Count; i++)
					{
						var entry = textEntries[i];
						keys[i] = entry.Key;
					}

					return keys;
				}
			}

			Debug.LogWarning($"Could not get keys for language '{language}' on {name}.", this);

			return Array.Empty<string>();
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
				languageMap.TextEntries.Add(new TextEntry { Key = $"NewKey{languageMap.TextEntries.Count}" });
			}
		}

		public void RemoveKeyForAllLanguages(int index)
		{
			foreach (var languageMap in _languageMaps)
			{
				languageMap.TextEntries.RemoveAt(index);
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

