using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public static class ColorExtensions
{
	// Token: 0x06000555 RID: 1365 RVA: 0x00018334 File Offset: 0x00016534
	public static string ToHexString(this Color color)
	{
		return ((byte)(color.r * 255f)).ToString("X2") + ((byte)(color.g * 255f)).ToString("X2") + ((byte)(color.b * 255f)).ToString("X2") + ((byte)(color.a * 255f)).ToString("X2");
	}
}
