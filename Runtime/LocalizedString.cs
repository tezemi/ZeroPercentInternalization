using System;

namespace ZeroPercentInternalization
{
	[Serializable]
	public class LocalizedString
	{
		public InternalizedText InternalizedText;
		public string Key;
		public bool IsValid => InternalizedText != null && !string.IsNullOrEmpty(Key);

		public override string ToString()
		{
			return InternalizedText.GetValue(Key);
		}

		public static implicit operator string (LocalizedString l) => l.ToString();
	}
}
