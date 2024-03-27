using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroPercentInternalization
{
	[Serializable]
	public class InternalizedText : ScriptableObject
	{
		public Language Language;
		public List<string> Keys;
		public List<string> Values;
	}
}

