using UnityEngine;

namespace ZeroPercentInternalization
{
	public static class ZeroPercentInternalizationConfiguration
	{
		public static Language Language
		{
			get
			{
				return (Language)PlayerPrefs.GetInt(GetKey(nameof(Language)), (int)Language.EN);
			}
			set
			{
				PlayerPrefs.SetInt(GetKey(nameof(Language)), (int)value);
			}
		}

		private static string GetKey(string key)
		{
			return $"{nameof(ZeroPercentInternalizationConfiguration)}.{key}";
		}
	}
}
