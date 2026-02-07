using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000283 RID: 643
	public static class PointsOfInterests
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x0004CA1D File Offset: 0x0004AC1D
		public static ReadOnlyCollection<MonoBehaviour> Points
		{
			get
			{
				if (PointsOfInterests.points_ReadOnly == null)
				{
					PointsOfInterests.points_ReadOnly = new ReadOnlyCollection<MonoBehaviour>(PointsOfInterests.points);
				}
				return PointsOfInterests.points_ReadOnly;
			}
		}

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x0600147F RID: 5247 RVA: 0x0004CA3C File Offset: 0x0004AC3C
		// (remove) Token: 0x06001480 RID: 5248 RVA: 0x0004CA70 File Offset: 0x0004AC70
		public static event Action<MonoBehaviour> OnPointRegistered;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06001481 RID: 5249 RVA: 0x0004CAA4 File Offset: 0x0004ACA4
		// (remove) Token: 0x06001482 RID: 5250 RVA: 0x0004CAD8 File Offset: 0x0004ACD8
		public static event Action<MonoBehaviour> OnPointUnregistered;

		// Token: 0x06001483 RID: 5251 RVA: 0x0004CB0B File Offset: 0x0004AD0B
		public static void Register(MonoBehaviour point)
		{
			PointsOfInterests.points.Add(point);
			Action<MonoBehaviour> onPointRegistered = PointsOfInterests.OnPointRegistered;
			if (onPointRegistered != null)
			{
				onPointRegistered(point);
			}
			PointsOfInterests.CleanUp();
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0004CB2E File Offset: 0x0004AD2E
		public static void Unregister(MonoBehaviour point)
		{
			if (PointsOfInterests.points.Remove(point))
			{
				Action<MonoBehaviour> onPointUnregistered = PointsOfInterests.OnPointUnregistered;
				if (onPointUnregistered != null)
				{
					onPointUnregistered(point);
				}
			}
			PointsOfInterests.CleanUp();
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0004CB53 File Offset: 0x0004AD53
		private static void CleanUp()
		{
			PointsOfInterests.points.RemoveAll((MonoBehaviour e) => e == null);
		}

		// Token: 0x04000F2A RID: 3882
		private static List<MonoBehaviour> points = new List<MonoBehaviour>();

		// Token: 0x04000F2B RID: 3883
		private static ReadOnlyCollection<MonoBehaviour> points_ReadOnly;
	}
}
