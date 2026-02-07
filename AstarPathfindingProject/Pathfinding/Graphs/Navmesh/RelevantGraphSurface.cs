using System;
using Pathfinding.Drawing;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001C5 RID: 453
	[AddComponentMenu("Pathfinding/Navmesh/RelevantGraphSurface")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/relevantgraphsurface.html")]
	public class RelevantGraphSurface : VersionedMonoBehaviour
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0004703E File Offset: 0x0004523E
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00047046 File Offset: 0x00045246
		public RelevantGraphSurface Next
		{
			get
			{
				return this.next;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x0004704E File Offset: 0x0004524E
		public RelevantGraphSurface Prev
		{
			get
			{
				return this.prev;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00047056 File Offset: 0x00045256
		public static RelevantGraphSurface Root
		{
			get
			{
				return RelevantGraphSurface.root;
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0004705D File Offset: 0x0004525D
		public void UpdatePosition()
		{
			this.position = base.transform.position;
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00047070 File Offset: 0x00045270
		private void OnEnable()
		{
			this.UpdatePosition();
			if (RelevantGraphSurface.root == null)
			{
				RelevantGraphSurface.root = this;
				return;
			}
			this.next = RelevantGraphSurface.root;
			RelevantGraphSurface.root.prev = this;
			RelevantGraphSurface.root = this;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x000470A8 File Offset: 0x000452A8
		private void OnDisable()
		{
			if (RelevantGraphSurface.root == this)
			{
				RelevantGraphSurface.root = this.next;
				if (RelevantGraphSurface.root != null)
				{
					RelevantGraphSurface.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00047134 File Offset: 0x00045334
		public static void UpdateAllPositions()
		{
			RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.root;
			while (relevantGraphSurface != null)
			{
				relevantGraphSurface.UpdatePosition();
				relevantGraphSurface = relevantGraphSurface.Next;
			}
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00047160 File Offset: 0x00045360
		public static void FindAllGraphSurfaces()
		{
			RelevantGraphSurface[] array = UnityCompatibility.FindObjectsByTypeUnsorted<RelevantGraphSurface>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnDisable();
				array[i].OnEnable();
			}
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00047194 File Offset: 0x00045394
		public override void DrawGizmos()
		{
			Color color = new Color(0.22352941f, 0.827451f, 0.18039216f);
			if (!GizmoContext.InActiveSelection(this))
			{
				color.a *= 0.4f;
			}
			Draw.Line(base.transform.position - Vector3.up * this.maxRange, base.transform.position + Vector3.up * this.maxRange, color);
		}

		// Token: 0x0400084A RID: 2122
		private static RelevantGraphSurface root;

		// Token: 0x0400084B RID: 2123
		public float maxRange = 1f;

		// Token: 0x0400084C RID: 2124
		private RelevantGraphSurface prev;

		// Token: 0x0400084D RID: 2125
		private RelevantGraphSurface next;

		// Token: 0x0400084E RID: 2126
		private Vector3 position;
	}
}
