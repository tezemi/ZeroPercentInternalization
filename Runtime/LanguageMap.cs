using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZeroPercentInternalization
{
	[Serializable]
	public class LanguageMap
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public Language Language;
		public List<TextEntry> TextEntries = new List<TextEntry>();

		public LanguageMap(Language language)
		{
			Language = language;
		}
	}
}
