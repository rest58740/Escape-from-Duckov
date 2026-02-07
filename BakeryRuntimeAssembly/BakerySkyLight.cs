using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class BakerySkyLight : MonoBehaviour
{
	// Token: 0x04000082 RID: 130
	public string texName = "sky.dds";

	// Token: 0x04000083 RID: 131
	public Color color = Color.white;

	// Token: 0x04000084 RID: 132
	public float intensity = 1f;

	// Token: 0x04000085 RID: 133
	public int samples = 32;

	// Token: 0x04000086 RID: 134
	public bool hemispherical;

	// Token: 0x04000087 RID: 135
	public int bitmask = 1;

	// Token: 0x04000088 RID: 136
	public bool bakeToIndirect = true;

	// Token: 0x04000089 RID: 137
	public float indirectIntensity = 1f;

	// Token: 0x0400008A RID: 138
	public bool tangentSH;

	// Token: 0x0400008B RID: 139
	public bool correctRotation;

	// Token: 0x0400008C RID: 140
	public Cubemap cubemap;

	// Token: 0x0400008D RID: 141
	public int UID;

	// Token: 0x0400008E RID: 142
	public static int lightsChanged;

	// Token: 0x0400008F RID: 143
	private static GameObject objShownError;
}
