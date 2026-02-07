using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002CB RID: 715
	[AddComponentMenu("")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/rvosquareobstacle.html")]
	public class RVOSquareObstacle : RVOObstacle
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x000185BF File Offset: 0x000167BF
		protected override bool StaticObstacle
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0001797A File Offset: 0x00015B7A
		protected override bool ExecuteInEditor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x0001797A File Offset: 0x00015B7A
		protected override bool LocalCoordinates
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x0006AC7F File Offset: 0x00068E7F
		protected override float Height
		{
			get
			{
				return this.height;
			}
		}

		// Token: 0x04000CD8 RID: 3288
		public float height = 1f;

		// Token: 0x04000CD9 RID: 3289
		public Vector2 size = Vector3.one;

		// Token: 0x04000CDA RID: 3290
		public Vector2 center = Vector3.zero;
	}
}
