using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
[Serializable]
public struct CharacterRandomPresetInfo
{
	// Token: 0x040004CD RID: 1229
	public CharacterRandomPreset randomPreset;

	// Token: 0x040004CE RID: 1230
	[Range(0f, 1f)]
	public float weight;
}
