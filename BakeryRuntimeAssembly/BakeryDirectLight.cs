using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class BakeryDirectLight : MonoBehaviour
{
	// Token: 0x04000001 RID: 1
	public Color color = Color.white;

	// Token: 0x04000002 RID: 2
	public float intensity = 1f;

	// Token: 0x04000003 RID: 3
	public float shadowSpread = 0.01f;

	// Token: 0x04000004 RID: 4
	public int samples = 16;

	// Token: 0x04000005 RID: 5
	public int bitmask = 1;

	// Token: 0x04000006 RID: 6
	public bool bakeToIndirect;

	// Token: 0x04000007 RID: 7
	public bool shadowmask;

	// Token: 0x04000008 RID: 8
	public bool shadowmaskDenoise;

	// Token: 0x04000009 RID: 9
	public float indirectIntensity = 1f;

	// Token: 0x0400000A RID: 10
	public Texture2D cloudShadow;

	// Token: 0x0400000B RID: 11
	public float cloudShadowTilingX = 0.01f;

	// Token: 0x0400000C RID: 12
	public float cloudShadowTilingY = 0.01f;

	// Token: 0x0400000D RID: 13
	public float cloudShadowOffsetX;

	// Token: 0x0400000E RID: 14
	public float cloudShadowOffsetY;

	// Token: 0x0400000F RID: 15
	public bool supersample;

	// Token: 0x04000010 RID: 16
	public int UID;

	// Token: 0x04000011 RID: 17
	public static int lightsChanged;

	// Token: 0x04000012 RID: 18
	private static GameObject objShownError;
}
