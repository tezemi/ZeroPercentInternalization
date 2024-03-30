﻿using System;

namespace ZeroPercentInternalization
{
	[Serializable]
	public class LocalizedString
	{
		public InternalizedText InternalizedText;
		public string Key;

		public override string ToString()
		{
			return InternalizedText.GetValue(Key);
		}
	}
}