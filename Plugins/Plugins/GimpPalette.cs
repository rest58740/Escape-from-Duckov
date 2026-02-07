using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class GimpPalette : ScriptableObject
{
	// Token: 0x04000001 RID: 1
	[SerializeField]
	public GimpPalette.Entry[] entries;

	// Token: 0x02000014 RID: 20
	[Serializable]
	public struct Entry
	{
		// Token: 0x04000024 RID: 36
		public string name;

		// Token: 0x04000025 RID: 37
		public Color color;
	}
}
