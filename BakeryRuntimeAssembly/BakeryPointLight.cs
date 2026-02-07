using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class BakeryPointLight : MonoBehaviour
{
	// Token: 0x04000060 RID: 96
	public int UID;

	// Token: 0x04000061 RID: 97
	public Color color = Color.white;

	// Token: 0x04000062 RID: 98
	public float intensity = 1f;

	// Token: 0x04000063 RID: 99
	public float shadowSpread = 0.05f;

	// Token: 0x04000064 RID: 100
	public float cutoff = 10f;

	// Token: 0x04000065 RID: 101
	public bool realisticFalloff;

	// Token: 0x04000066 RID: 102
	public bool legacySampling = true;

	// Token: 0x04000067 RID: 103
	public int samples = 8;

	// Token: 0x04000068 RID: 104
	public BakeryPointLight.ftLightProjectionMode projMode;

	// Token: 0x04000069 RID: 105
	public Texture2D cookie;

	// Token: 0x0400006A RID: 106
	public float angle = 30f;

	// Token: 0x0400006B RID: 107
	public float innerAngle;

	// Token: 0x0400006C RID: 108
	public Cubemap cubemap;

	// Token: 0x0400006D RID: 109
	public UnityEngine.Object iesFile;

	// Token: 0x0400006E RID: 110
	public int bitmask = 1;

	// Token: 0x0400006F RID: 111
	public bool bakeToIndirect;

	// Token: 0x04000070 RID: 112
	public bool shadowmask;

	// Token: 0x04000071 RID: 113
	public bool shadowmaskFalloff;

	// Token: 0x04000072 RID: 114
	public float indirectIntensity = 1f;

	// Token: 0x04000073 RID: 115
	public float falloffMinRadius = 1f;

	// Token: 0x04000074 RID: 116
	public int shadowmaskGroupID;

	// Token: 0x04000075 RID: 117
	public BakeryPointLight.Direction directionMode;

	// Token: 0x04000076 RID: 118
	public int maskChannel;

	// Token: 0x04000077 RID: 119
	private const float GIZMO_MAXSIZE = 0.1f;

	// Token: 0x04000078 RID: 120
	private const float GIZMO_SCALE = 0.01f;

	// Token: 0x04000079 RID: 121
	private float screenRadius = 0.1f;

	// Token: 0x0400007A RID: 122
	public static int lightsChanged;

	// Token: 0x0400007B RID: 123
	private static GameObject objShownError;

	// Token: 0x0200001A RID: 26
	public enum ftLightProjectionMode
	{
		// Token: 0x040000E0 RID: 224
		Omni,
		// Token: 0x040000E1 RID: 225
		Cookie,
		// Token: 0x040000E2 RID: 226
		Cubemap,
		// Token: 0x040000E3 RID: 227
		IES,
		// Token: 0x040000E4 RID: 228
		Cone
	}

	// Token: 0x0200001B RID: 27
	public enum Direction
	{
		// Token: 0x040000E6 RID: 230
		NegativeY,
		// Token: 0x040000E7 RID: 231
		PositiveZ
	}
}
