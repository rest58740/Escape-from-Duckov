using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000284 RID: 644
	public static class UnityCompatibility
	{
		// Token: 0x06000F54 RID: 3924 RVA: 0x0005EDF6 File Offset: 0x0005CFF6
		public static T[] FindObjectsByTypeSorted<T>() where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.InstanceID);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0005EDFE File Offset: 0x0005CFFE
		public static T[] FindObjectsByTypeUnsorted<T>() where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0005EE06 File Offset: 0x0005D006
		public static T[] FindObjectsByTypeUnsortedWithInactive<T>() where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0005EE0F File Offset: 0x0005D00F
		public static T FindAnyObjectByType<T>() where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindAnyObjectByType<T>();
		}
	}
}
