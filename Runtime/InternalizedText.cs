using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroPercentInternalization
{
	[Serializable]
	public class InternalizedText : ScriptableObject
	{
		public Language Language;
		public List<TextEntry> TextEntries;
		private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

		public string GetValue(string key)
		{
			if (string.IsNullOrEmpty(key))
				return string.Empty;

			if (Application.isPlaying)
			{
				if (!_cache.ContainsKey(key))
				{
					foreach (var entry in TextEntries)
					{
						_cache.Add(entry.Key, entry.Value);
					}
				}

				return _cache[key];
			}

			foreach (var entry in TextEntries)
			{
				if (entry.Key == key)
				{
					return entry.Value;
				}
			}

			return string.Empty;
		}

		public string[] GetKeys()
		{
			var keys = new string[TextEntries.Count];
			for (var i = 0; i < TextEntries.Count; i++)
			{
				var entry = TextEntries[i];
				keys[i] = entry.Key;
			}

			return keys;
		}
	}
}

