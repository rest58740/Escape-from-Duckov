using System;

namespace Pathfinding
{
	// Token: 0x02000028 RID: 40
	public class NNConstraint
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00009CD7 File Offset: 0x00007ED7
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00009CFA File Offset: 0x00007EFA
		[Obsolete("Use distanceMetric = DistanceMetric.ClosestAsSeenFromAbove() instead")]
		public bool distanceXZ
		{
			get
			{
				return this.distanceMetric.isProjectedDistance && this.distanceMetric.distanceScaleAlongProjectionDirection == 0f;
			}
			set
			{
				if (value)
				{
					this.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
					return;
				}
				this.distanceMetric = DistanceMetric.Euclidean;
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009D16 File Offset: 0x00007F16
		public virtual bool SuitableGraph(int graphIndex, NavGraph graph)
		{
			return this.graphMask.Contains(graphIndex);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009D24 File Offset: 0x00007F24
		public virtual bool Suitable(GraphNode node)
		{
			return (!this.constrainWalkability || node.Walkable == this.walkable) && (!this.constrainArea || this.area < 0 || (ulong)node.Area == (ulong)((long)this.area)) && (!this.constrainTags || (this.tags >> (int)node.Tag & 1) != 0);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009D8B File Offset: 0x00007F8B
		public void UseSettings(PathRequestSettings settings)
		{
			this.graphMask = settings.graphMask;
			this.constrainTags = true;
			this.tags = settings.traversableTags;
			this.constrainWalkability = true;
			this.walkable = true;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00009DBA File Offset: 0x00007FBA
		[Obsolete("Use NNConstraint.Walkable instead. It is equivalent, but the name is more descriptive")]
		public static NNConstraint Default
		{
			get
			{
				return new NNConstraint();
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00009DBA File Offset: 0x00007FBA
		public static NNConstraint Walkable
		{
			get
			{
				return new NNConstraint();
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00009DC1 File Offset: 0x00007FC1
		public static NNConstraint None
		{
			get
			{
				return new NNConstraint
				{
					constrainWalkability = false,
					constrainArea = false,
					constrainTags = false,
					constrainDistance = false,
					graphMask = -1
				};
			}
		}

		// Token: 0x04000137 RID: 311
		public GraphMask graphMask = -1;

		// Token: 0x04000138 RID: 312
		public bool constrainArea;

		// Token: 0x04000139 RID: 313
		public int area = -1;

		// Token: 0x0400013A RID: 314
		public DistanceMetric distanceMetric;

		// Token: 0x0400013B RID: 315
		public bool constrainWalkability = true;

		// Token: 0x0400013C RID: 316
		public bool walkable = true;

		// Token: 0x0400013D RID: 317
		public bool constrainTags = true;

		// Token: 0x0400013E RID: 318
		public int tags = -1;

		// Token: 0x0400013F RID: 319
		public bool constrainDistance = true;
	}
}
