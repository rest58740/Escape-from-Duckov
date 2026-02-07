using System;

namespace Pathfinding
{
	// Token: 0x020000AF RID: 175
	public class NNConstraintWithTraversalProvider : NNConstraint
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0001B055 File Offset: 0x00019255
		public void Reset()
		{
			this.traversalProvider = null;
			this.baseConstraint = null;
			this.path = null;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001B06C File Offset: 0x0001926C
		public bool isSet
		{
			get
			{
				return this.traversalProvider != null;
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001B078 File Offset: 0x00019278
		public void Set(Path path, NNConstraint constraint, ITraversalProvider traversalProvider)
		{
			this.path = path;
			this.traversalProvider = traversalProvider;
			this.baseConstraint = constraint;
			this.graphMask = constraint.graphMask;
			this.constrainArea = constraint.constrainArea;
			this.area = constraint.area;
			this.distanceMetric = constraint.distanceMetric;
			this.constrainWalkability = constraint.constrainWalkability;
			this.walkable = constraint.walkable;
			this.constrainTags = constraint.constrainTags;
			this.tags = constraint.tags;
			this.constrainDistance = constraint.constrainDistance;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001B106 File Offset: 0x00019306
		public override bool SuitableGraph(int graphIndex, NavGraph graph)
		{
			return this.baseConstraint.SuitableGraph(graphIndex, graph);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001B115 File Offset: 0x00019315
		public override bool Suitable(GraphNode node)
		{
			return this.baseConstraint.Suitable(node) && this.traversalProvider.CanTraverse(this.path, node);
		}

		// Token: 0x040003A4 RID: 932
		public ITraversalProvider traversalProvider;

		// Token: 0x040003A5 RID: 933
		public NNConstraint baseConstraint;

		// Token: 0x040003A6 RID: 934
		public Path path;
	}
}
