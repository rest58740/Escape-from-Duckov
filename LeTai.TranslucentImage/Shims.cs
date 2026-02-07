using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x0200000F RID: 15
	public static class Shims
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00003484 File Offset: 0x00001684
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T FindObjectOfType<T>(bool includeInactive = false, bool sorted = true) where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectOfType<T>(includeInactive);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000348C File Offset: 0x0000168C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] FindObjectsOfType<T>(bool includeInactive = false) where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsOfType<T>(includeInactive);
		}
	}
}
