using System;
using UnityEngine;

namespace ECM2.Examples.Ladders
{
	// Token: 0x02000088 RID: 136
	public sealed class Ladder : MonoBehaviour
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x000119BD File Offset: 0x0000FBBD
		public Vector3 bottomAnchorPoint
		{
			get
			{
				return base.transform.position + base.transform.TransformVector(this.PathOffset);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x000119E0 File Offset: 0x0000FBE0
		public Vector3 topAnchorPoint
		{
			get
			{
				return this.bottomAnchorPoint + base.transform.up * this.PathLength;
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00011A04 File Offset: 0x0000FC04
		public Vector3 ClosestPointOnPath(Vector3 position, out float pathPosition)
		{
			Vector3 vector = this.topAnchorPoint - this.bottomAnchorPoint;
			float num = Vector3.Dot(position - this.bottomAnchorPoint, vector.normalized);
			if (num <= 0f)
			{
				pathPosition = num;
				return this.bottomAnchorPoint;
			}
			if (num <= vector.magnitude)
			{
				pathPosition = 0f;
				return this.bottomAnchorPoint + vector.normalized * num;
			}
			pathPosition = num - vector.magnitude;
			return this.topAnchorPoint;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00011A8C File Offset: 0x0000FC8C
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(this.bottomAnchorPoint, this.topAnchorPoint);
			if (this.BottomPoint == null || this.TopPoint == null)
			{
				return;
			}
			Gizmos.DrawWireCube(this.BottomPoint.position, Vector3.one * 0.25f);
			Gizmos.DrawWireCube(this.TopPoint.position, Vector3.one * 0.25f);
		}

		// Token: 0x040002AC RID: 684
		[Header("Ladder Path")]
		public float PathLength = 10f;

		// Token: 0x040002AD RID: 685
		public Vector3 PathOffset = new Vector3(0f, 0f, -0.5f);

		// Token: 0x040002AE RID: 686
		[Header("Anchor Points")]
		public Transform TopPoint;

		// Token: 0x040002AF RID: 687
		public Transform BottomPoint;
	}
}
