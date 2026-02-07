using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200002A RID: 42
	public readonly struct NNInfo
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00009E5D File Offset: 0x0000805D
		[Obsolete("This field has been renamed to 'position'", true)]
		public Vector3 clampedPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00009E65 File Offset: 0x00008065
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NNInfo(GraphNode node, Vector3 position, float distanceCostSqr)
		{
			this.node = node;
			if (node == null)
			{
				this.position = Vector3.positiveInfinity;
				this.distanceCostSqr = float.PositiveInfinity;
				return;
			}
			this.position = position;
			this.distanceCostSqr = distanceCostSqr;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009E5D File Offset: 0x0000805D
		public static explicit operator Vector3(NNInfo ob)
		{
			return ob.position;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009E96 File Offset: 0x00008096
		public static explicit operator GraphNode(NNInfo ob)
		{
			return ob.node;
		}

		// Token: 0x04000140 RID: 320
		public readonly GraphNode node;

		// Token: 0x04000141 RID: 321
		public readonly Vector3 position;

		// Token: 0x04000142 RID: 322
		public readonly float distanceCostSqr;

		// Token: 0x04000143 RID: 323
		public static readonly NNInfo Empty = new NNInfo(null, Vector3.positiveInfinity, float.PositiveInfinity);
	}
}
