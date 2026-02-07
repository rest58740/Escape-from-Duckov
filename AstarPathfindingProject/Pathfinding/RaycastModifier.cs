using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000119 RID: 281
	[AddComponentMenu("Pathfinding/Modifiers/Raycast Modifier")]
	[RequireComponent(typeof(Seeker))]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/raycastmodifier.html")]
	[Serializable]
	public class RaycastModifier : MonoModifier
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0002D2C5 File Offset: 0x0002B4C5
		public override int Order
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002F104 File Offset: 0x0002D304
		public override void Apply(Path p)
		{
			if (!this.useRaycasting && !this.useGraphRaycasting)
			{
				return;
			}
			List<Vector3> list = p.vectorPath;
			this.cachedFilter.path = p;
			this.cachedNNConstraint.graphMask = p.nnConstraint.graphMask;
			if (this.ValidateLine(null, null, p.vectorPath[0], p.vectorPath[p.vectorPath.Count - 1], this.cachedFilter.cachedDelegate, this.cachedNNConstraint))
			{
				Vector3 item = p.vectorPath[0];
				Vector3 item2 = p.vectorPath[p.vectorPath.Count - 1];
				list.ClearFast<Vector3>();
				list.Add(item);
				list.Add(item2);
			}
			else
			{
				int num = RaycastModifier.iterationsByQuality[(int)this.quality];
				for (int i = 0; i < num; i++)
				{
					if (i != 0)
					{
						Polygon.Subdivide(list, RaycastModifier.buffer, 3);
						Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref list);
						RaycastModifier.buffer.ClearFast<Vector3>();
						list.Reverse();
					}
					list = ((this.quality >= RaycastModifier.Quality.High) ? this.ApplyDP(p, list, this.cachedFilter.cachedDelegate, this.cachedNNConstraint) : this.ApplyGreedy(p, list, this.cachedFilter.cachedDelegate, this.cachedNNConstraint));
				}
				if (num % 2 == 0)
				{
					list.Reverse();
				}
			}
			p.vectorPath = list;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002F264 File Offset: 0x0002D464
		private List<Vector3> ApplyGreedy(Path p, List<Vector3> points, Func<GraphNode, bool> filter, NNConstraint nnConstraint)
		{
			bool flag = points.Count == p.path.Count;
			int i = 0;
			while (i < points.Count)
			{
				Vector3 vector = points[i];
				GraphNode n = (flag && points[i] == (Vector3)p.path[i].position) ? p.path[i] : null;
				RaycastModifier.buffer.Add(vector);
				int num = 1;
				int num2 = 2;
				for (;;)
				{
					int num3 = i + num2;
					if (num3 >= points.Count)
					{
						goto Block_2;
					}
					Vector3 vector2 = points[num3];
					GraphNode n2 = (flag && vector2 == (Vector3)p.path[num3].position) ? p.path[num3] : null;
					if (!this.ValidateLine(n, n2, vector, vector2, filter, nnConstraint))
					{
						break;
					}
					num = num2;
					num2 *= 2;
				}
				IL_14F:
				while (num + 1 < num2)
				{
					int num4 = (num + num2) / 2;
					int index = i + num4;
					Vector3 vector3 = points[index];
					GraphNode n3 = (flag && vector3 == (Vector3)p.path[index].position) ? p.path[index] : null;
					if (this.ValidateLine(n, n3, vector, vector3, filter, nnConstraint))
					{
						num = num4;
					}
					else
					{
						num2 = num4;
					}
				}
				i += num;
				continue;
				Block_2:
				num2 = points.Count - i;
				goto IL_14F;
			}
			Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref points);
			RaycastModifier.buffer.ClearFast<Vector3>();
			return points;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002F3F0 File Offset: 0x0002D5F0
		private List<Vector3> ApplyDP(Path p, List<Vector3> points, Func<GraphNode, bool> filter, NNConstraint nnConstraint)
		{
			if (RaycastModifier.DPCosts.Length < points.Count)
			{
				RaycastModifier.DPCosts = new float[points.Count];
				RaycastModifier.DPParents = new int[points.Count];
			}
			for (int i = 0; i < RaycastModifier.DPParents.Length; i++)
			{
				RaycastModifier.DPCosts[i] = (float)(RaycastModifier.DPParents[i] = -1);
			}
			bool flag = points.Count == p.path.Count;
			for (int j = 0; j < points.Count; j++)
			{
				float num = RaycastModifier.DPCosts[j];
				Vector3 vector = points[j];
				bool flag2 = flag && vector == (Vector3)p.path[j].position;
				for (int k = j + 1; k < points.Count; k++)
				{
					float num2 = num + (points[k] - vector).magnitude + 0.0001f;
					if (RaycastModifier.DPParents[k] == -1 || num2 < RaycastModifier.DPCosts[k])
					{
						bool flag3 = flag && points[k] == (Vector3)p.path[k].position;
						if (k != j + 1 && !this.ValidateLine(flag2 ? p.path[j] : null, flag3 ? p.path[k] : null, vector, points[k], filter, nnConstraint))
						{
							break;
						}
						RaycastModifier.DPCosts[k] = num2;
						RaycastModifier.DPParents[k] = j;
					}
				}
			}
			for (int num3 = points.Count - 1; num3 != -1; num3 = RaycastModifier.DPParents[num3])
			{
				RaycastModifier.buffer.Add(points[num3]);
			}
			RaycastModifier.buffer.Reverse();
			Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref points);
			RaycastModifier.buffer.ClearFast<Vector3>();
			return points;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002F5E4 File Offset: 0x0002D7E4
		protected bool ValidateLine(GraphNode n1, GraphNode n2, Vector3 v1, Vector3 v2, Func<GraphNode, bool> filter, NNConstraint nnConstraint)
		{
			if (this.useRaycasting)
			{
				if (this.use2DPhysics)
				{
					if (this.thickRaycast && this.thickRaycastRadius > 0f && Physics2D.CircleCast(v1 + this.raycastOffset, this.thickRaycastRadius, v2 - v1, (v2 - v1).magnitude, this.mask))
					{
						return false;
					}
					if (Physics2D.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, this.mask))
					{
						return false;
					}
				}
				else
				{
					if (Physics.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, this.mask))
					{
						return false;
					}
					if (this.thickRaycast && this.thickRaycastRadius > 0f)
					{
						if (Physics.CheckSphere(v1 + this.raycastOffset + (v2 - v1).normalized * this.thickRaycastRadius, this.thickRaycastRadius, this.mask))
						{
							return false;
						}
						if (Physics.SphereCast(new Ray(v1 + this.raycastOffset, v2 - v1), this.thickRaycastRadius, (v2 - v1).magnitude, this.mask))
						{
							return false;
						}
					}
				}
			}
			if (this.useGraphRaycasting)
			{
				bool flag = n1 != null && n2 != null;
				if (n1 == null)
				{
					n1 = AstarPath.active.GetNearest(v1, nnConstraint).node;
				}
				if (n2 == null)
				{
					n2 = AstarPath.active.GetNearest(v2, nnConstraint).node;
				}
				if (n1 != null && n2 != null)
				{
					NavGraph graph = n1.Graph;
					NavGraph graph2 = n2.Graph;
					if (graph != graph2)
					{
						return false;
					}
					IRaycastableGraph raycastableGraph = graph as IRaycastableGraph;
					GridGraph gridGraph = graph as GridGraph;
					if (flag && gridGraph != null)
					{
						return !gridGraph.Linecast(n1 as GridNodeBase, n2 as GridNodeBase, filter);
					}
					if (raycastableGraph != null)
					{
						GraphHitInfo graphHitInfo;
						return !raycastableGraph.Linecast(v1, v2, out graphHitInfo, null, filter);
					}
				}
			}
			return true;
		}

		// Token: 0x040005DB RID: 1499
		public bool useRaycasting;

		// Token: 0x040005DC RID: 1500
		public LayerMask mask = -1;

		// Token: 0x040005DD RID: 1501
		[Tooltip("Checks around the line between two points, not just the exact line.\nMake sure the ground is either too far below or is not inside the mask since otherwise the raycast might always hit the ground.")]
		public bool thickRaycast;

		// Token: 0x040005DE RID: 1502
		[Tooltip("Distance from the ray which will be checked for colliders")]
		public float thickRaycastRadius;

		// Token: 0x040005DF RID: 1503
		[Tooltip("Check for intersections with 2D colliders instead of 3D colliders.")]
		public bool use2DPhysics;

		// Token: 0x040005E0 RID: 1504
		[Tooltip("Offset from the original positions to perform the raycast.\nCan be useful to avoid the raycast intersecting the ground or similar things you do not want to it intersect")]
		public Vector3 raycastOffset = Vector3.zero;

		// Token: 0x040005E1 RID: 1505
		[Tooltip("Use raycasting on the graphs. Only currently works with GridGraph and NavmeshGraph and RecastGraph. This is a pro version feature.")]
		public bool useGraphRaycasting = true;

		// Token: 0x040005E2 RID: 1506
		[Tooltip("When using the high quality mode the script will try harder to find a shorter path. This is significantly slower than the greedy low quality approach.")]
		public RaycastModifier.Quality quality = RaycastModifier.Quality.Medium;

		// Token: 0x040005E3 RID: 1507
		private static readonly int[] iterationsByQuality = new int[]
		{
			1,
			2,
			1,
			3
		};

		// Token: 0x040005E4 RID: 1508
		private static List<Vector3> buffer = new List<Vector3>();

		// Token: 0x040005E5 RID: 1509
		private static float[] DPCosts = new float[16];

		// Token: 0x040005E6 RID: 1510
		private static int[] DPParents = new int[16];

		// Token: 0x040005E7 RID: 1511
		private RaycastModifier.Filter cachedFilter = new RaycastModifier.Filter();

		// Token: 0x040005E8 RID: 1512
		private NNConstraint cachedNNConstraint = NNConstraint.None;

		// Token: 0x0200011A RID: 282
		public enum Quality
		{
			// Token: 0x040005EA RID: 1514
			Low,
			// Token: 0x040005EB RID: 1515
			Medium,
			// Token: 0x040005EC RID: 1516
			High,
			// Token: 0x040005ED RID: 1517
			Highest
		}

		// Token: 0x0200011B RID: 283
		private class Filter
		{
			// Token: 0x060008ED RID: 2285 RVA: 0x0002F8A8 File Offset: 0x0002DAA8
			public Filter()
			{
				this.cachedDelegate = new Func<GraphNode, bool>(this.CanTraverse);
			}

			// Token: 0x060008EE RID: 2286 RVA: 0x0002F8C2 File Offset: 0x0002DAC2
			private bool CanTraverse(GraphNode node)
			{
				return this.path.CanTraverse(node);
			}

			// Token: 0x040005EE RID: 1518
			public Path path;

			// Token: 0x040005EF RID: 1519
			public readonly Func<GraphNode, bool> cachedDelegate;
		}
	}
}
