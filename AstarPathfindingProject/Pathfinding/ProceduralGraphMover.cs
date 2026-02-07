using System;
using System.Collections.Generic;
using Pathfinding.Jobs;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000166 RID: 358
	[AddComponentMenu("Pathfinding/Procedural Graph Mover")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/proceduralgraphmover.html")]
	public class ProceduralGraphMover : VersionedMonoBehaviour
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0003C2DE File Offset: 0x0003A4DE
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x0003C2E6 File Offset: 0x0003A4E6
		public bool updatingGraph { get; private set; }

		// Token: 0x06000A9B RID: 2715 RVA: 0x0003C2F0 File Offset: 0x0003A4F0
		private void Start()
		{
			if (AstarPath.active == null)
			{
				throw new Exception("There is no AstarPath object in the scene");
			}
			if (this.graph == null)
			{
				if (this.graphIndex < 0)
				{
					throw new Exception("Graph index should not be negative");
				}
				if (this.graphIndex >= AstarPath.active.data.graphs.Length)
				{
					throw new Exception(string.Concat(new string[]
					{
						"The ProceduralGraphMover was configured to use graph index ",
						this.graphIndex.ToString(),
						", but only ",
						AstarPath.active.data.graphs.Length.ToString(),
						" graphs exist"
					}));
				}
				this.graph = AstarPath.active.data.graphs[this.graphIndex];
				if (!(this.graph is GridGraph) && !(this.graph is RecastGraph))
				{
					throw new Exception("The ProceduralGraphMover was configured to use graph index " + this.graphIndex.ToString() + " but that graph either does not exist or is not a GridGraph, LayerGridGraph or RecastGraph");
				}
				RecastGraph recastGraph = this.graph as RecastGraph;
				if (recastGraph != null && !recastGraph.useTiles)
				{
					Debug.LogWarning("The ProceduralGraphMover component only works with tiled recast graphs. Enable tiling in the recast graph inspector.", this);
				}
			}
			this.UpdateGraph(true);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0003C41F File Offset: 0x0003A61F
		private void OnDisable()
		{
			if (AstarPath.active != null)
			{
				AstarPath.active.FlushWorkItems();
			}
			this.updatingGraph = false;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0003C440 File Offset: 0x0003A640
		private void Update()
		{
			if (AstarPath.active == null || this.graph == null || !this.graph.isScanned)
			{
				return;
			}
			GridGraph gridGraph = this.graph as GridGraph;
			if (gridGraph != null)
			{
				Vector3 a = gridGraph.transform.InverseTransform(gridGraph.center);
				Vector3 b = gridGraph.transform.InverseTransform(this.target.position);
				if (VectorMath.SqrDistanceXZ(a, b) > this.updateDistance * this.updateDistance)
				{
					this.UpdateGraph(true);
					return;
				}
				return;
			}
			else
			{
				if (this.graph is RecastGraph)
				{
					this.UpdateGraph(true);
					return;
				}
				throw new Exception("ProceduralGraphMover cannot be used with graphs of type " + this.graph.GetType().Name);
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0003C4FC File Offset: 0x0003A6FC
		public void UpdateGraph(bool async = true)
		{
			if (!base.enabled)
			{
				throw new InvalidOperationException("This component has been disabled");
			}
			if (this.updatingGraph)
			{
				return;
			}
			GridGraph gridGraph = this.graph as GridGraph;
			if (gridGraph != null)
			{
				this.UpdateGridGraph(gridGraph, async);
				return;
			}
			RecastGraph recastGraph = this.graph as RecastGraph;
			if (recastGraph != null)
			{
				Vector2Int delta = ProceduralGraphMover.RecastGraphTileShift(recastGraph, this.target.position);
				if (delta.x != 0 || delta.y != 0)
				{
					this.updatingGraph = true;
					this.UpdateRecastGraph(recastGraph, delta, async);
				}
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0003C580 File Offset: 0x0003A780
		private void UpdateGridGraph(GridGraph graph, bool async)
		{
			this.updatingGraph = true;
			List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises = new List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>>();
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(IWorkItemContext ctx)
			{
				Vector3 vector = graph.transform.InverseTransformVector(this.target.position - graph.center);
				int num = Mathf.RoundToInt(vector.x);
				int num2 = Mathf.RoundToInt(vector.z);
				if (num != 0 || num2 != 0)
				{
					IGraphUpdatePromise graphUpdatePromise = graph.TranslateInDirection(num, num2);
					promises.Add(new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(graphUpdatePromise, graphUpdatePromise.Prepare()));
				}
			}, delegate(IWorkItemContext ctx, bool force)
			{
				if (GraphUpdateProcessor.ProcessGraphUpdatePromises(promises, ctx, force ? TimeSlice.Infinite : TimeSlice.MillisFromNow(2f)) == -1)
				{
					this.updatingGraph = false;
					return true;
				}
				return false;
			}));
			if (!async)
			{
				AstarPath.active.FlushWorkItems();
			}
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0003C5E8 File Offset: 0x0003A7E8
		private static Vector2Int RecastGraphTileShift(RecastGraph graph, Vector3 targetCenter)
		{
			Vector3 vector = graph.transform.InverseTransform(targetCenter) - graph.transform.InverseTransform(graph.forcedBoundsCenter);
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
			{
				vector.z = 0f;
			}
			else
			{
				vector.x = 0f;
			}
			return new Vector2Int((int)(Mathf.Max(0f, Mathf.Abs(vector.x) / graph.TileWorldSizeX + 0.5f - 0.2f) * Mathf.Sign(vector.x)), (int)(Mathf.Max(0f, Mathf.Abs(vector.z) / graph.TileWorldSizeZ + 0.5f - 0.2f) * Mathf.Sign(vector.z)));
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0003C6BC File Offset: 0x0003A8BC
		private void UpdateRecastGraph(RecastGraph graph, Vector2Int delta, bool async)
		{
			this.updatingGraph = true;
			List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises = new List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>>();
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(IWorkItemContext ctx)
			{
				IGraphUpdatePromise graphUpdatePromise = graph.TranslateInDirection(delta.x, delta.y);
				promises.Add(new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(graphUpdatePromise, graphUpdatePromise.Prepare()));
			}, delegate(IWorkItemContext ctx, bool force)
			{
				if (GraphUpdateProcessor.ProcessGraphUpdatePromises(promises, ctx, force ? TimeSlice.Infinite : TimeSlice.MillisFromNow(2f)) == -1)
				{
					this.updatingGraph = false;
					return true;
				}
				return false;
			}));
			if (!async)
			{
				AstarPath.active.FlushWorkItems();
			}
		}

		// Token: 0x04000716 RID: 1814
		public float updateDistance = 10f;

		// Token: 0x04000717 RID: 1815
		public Transform target;

		// Token: 0x04000719 RID: 1817
		public NavGraph graph;

		// Token: 0x0400071A RID: 1818
		[HideInInspector]
		public int graphIndex;
	}
}
