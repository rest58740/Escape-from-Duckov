using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000122 RID: 290
	[ExecuteAlways]
	public abstract class NavmeshClipper : VersionedMonoBehaviour
	{
		// Token: 0x06000906 RID: 2310 RVA: 0x00030B1C File Offset: 0x0002ED1C
		internal static void RefreshEnabledList()
		{
			NavmeshClipper[] array = UnityCompatibility.FindObjectsByTypeUnsorted<NavmeshClipper>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].enabled && array[i].listIndex == -1)
				{
					array[i].enabled = false;
					array[i].enabled = true;
				}
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00030B64 File Offset: 0x0002ED64
		public static void AddEnableCallback(Action<NavmeshClipper> onEnable, Action<NavmeshClipper> onDisable)
		{
			NavmeshClipper.OnEnableCallback = (Action<NavmeshClipper>)Delegate.Combine(NavmeshClipper.OnEnableCallback, onEnable);
			NavmeshClipper.OnDisableCallback = (Action<NavmeshClipper>)Delegate.Combine(NavmeshClipper.OnDisableCallback, onDisable);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00030B90 File Offset: 0x0002ED90
		public static void RemoveEnableCallback(Action<NavmeshClipper> onEnable, Action<NavmeshClipper> onDisable)
		{
			NavmeshClipper.OnEnableCallback = (Action<NavmeshClipper>)Delegate.Remove(NavmeshClipper.OnEnableCallback, onEnable);
			NavmeshClipper.OnDisableCallback = (Action<NavmeshClipper>)Delegate.Remove(NavmeshClipper.OnDisableCallback, onDisable);
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00030BBC File Offset: 0x0002EDBC
		public static List<NavmeshClipper> allEnabled
		{
			get
			{
				return NavmeshClipper.all;
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00030BC3 File Offset: 0x0002EDC3
		protected virtual void OnEnable()
		{
			if (this.listIndex != -1)
			{
				return;
			}
			if (NavmeshClipper.OnEnableCallback != null)
			{
				NavmeshClipper.OnEnableCallback(this);
			}
			this.listIndex = NavmeshClipper.all.Count;
			NavmeshClipper.all.Add(this);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00030BFC File Offset: 0x0002EDFC
		protected virtual void OnDisable()
		{
			if (this.listIndex == -1)
			{
				return;
			}
			NavmeshClipper.all[this.listIndex] = NavmeshClipper.all[NavmeshClipper.all.Count - 1];
			NavmeshClipper.all[this.listIndex].listIndex = this.listIndex;
			NavmeshClipper.all.RemoveAt(NavmeshClipper.all.Count - 1);
			this.listIndex = -1;
			if (NavmeshClipper.OnDisableCallback != null)
			{
				NavmeshClipper.OnDisableCallback(this);
			}
		}

		// Token: 0x0600090C RID: 2316
		public abstract void NotifyUpdated(GridLookup<NavmeshClipper>.Root previousState);

		// Token: 0x0600090D RID: 2317
		public abstract Rect GetBounds(GraphTransform transform, float radiusMargin);

		// Token: 0x0600090E RID: 2318
		public abstract bool RequiresUpdate(GridLookup<NavmeshClipper>.Root previousState);

		// Token: 0x0600090F RID: 2319
		public abstract void ForceUpdate();

		// Token: 0x0400061C RID: 1564
		private static Action<NavmeshClipper> OnEnableCallback;

		// Token: 0x0400061D RID: 1565
		private static Action<NavmeshClipper> OnDisableCallback;

		// Token: 0x0400061E RID: 1566
		private static readonly List<NavmeshClipper> all = new List<NavmeshClipper>();

		// Token: 0x0400061F RID: 1567
		private int listIndex = -1;

		// Token: 0x04000620 RID: 1568
		public GraphMask graphMask = GraphMask.everything;
	}
}
