using UnityEditor;

namespace ZeroPercentInternalization
{
	public static class ZeroPercentInternalizationConfiguration
	{
		public static Language Language
		{
			get
			{
				return (Language)EditorPrefs.GetInt(GetKey(nameof(Language)), (int)Language.EN);
			}
			set
			{
				EditorPrefs.SetInt(GetKey(nameof(Language)), (int)value);
			}
		}

		public static string RelativeTextPath
		{
			get
			{
				return EditorPrefs.GetString(GetKey(nameof(RelativeTextPath)), "Text");
			}
			set
			{
				EditorPrefs.SetString(GetKey(nameof(RelativeTextPath)), value);
			}
		}

		private static string GetKey(string key)
		{
			return $"{nameof(ZeroPercentInternalizationConfiguration)}.{key}";
		}
	}
}
