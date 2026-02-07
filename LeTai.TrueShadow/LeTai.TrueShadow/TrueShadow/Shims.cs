using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LeTai.TrueShadow
{
	// Token: 0x02000012 RID: 18
	public static class Shims
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00004DC3 File Offset: 0x00002FC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T FindObjectOfType<T>(bool includeInactive = false, bool sorted = true) where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectOfType<T>(includeInactive);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004DCB File Offset: 0x00002FCB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] FindObjectsOfType<T>(bool includeInactive = false) where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsOfType<T>(includeInactive);
		}
	}
}
