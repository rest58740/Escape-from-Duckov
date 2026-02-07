using System;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000004 RID: 4
	public static class DOTweenAnimationExtensions
	{
		// Token: 0x06000030 RID: 48 RVA: 0x000035A7 File Offset: 0x000017A7
		public static bool IsSameOrSubclassOf<T>(this Component t)
		{
			return t is T;
		}
	}
}
