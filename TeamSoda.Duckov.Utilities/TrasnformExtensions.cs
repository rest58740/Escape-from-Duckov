using System;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000013 RID: 19
	public static class TrasnformExtensions
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00003F10 File Offset: 0x00002110
		public static void DestroyAllChildren(this Transform transform)
		{
			while (transform.childCount > 0)
			{
				Transform child = transform.GetChild(0);
				child.SetParent(null);
				if (Application.isPlaying)
				{
					Object.Destroy(child.gameObject);
				}
				else
				{
					Object.DestroyImmediate(child.gameObject);
				}
			}
		}
	}
}
