using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Pathfinding.WindowsStore;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public class AstarData
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000A997 File Offset: 0x00008B97
		[Obsolete("Use navmeshGraph instead")]
		public NavMeshGraph navmesh
		{
			get
			{
				return this.navmeshGraph;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000A99F File Offset: 0x00008B9F
		// (set) Token: 0x0600023C RID: 572 RVA: 0x0000A9A7 File Offset: 0x00008BA7
		public NavMeshGraph navmeshGraph { get; private set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000A9B0 File Offset: 0x00008BB0
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		public GridGraph gridGraph { get; private set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000A9C1 File Offset: 0x00008BC1
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000A9C9 File Offset: 0x00008BC9
		public LayerGridGraph layerGridGraph { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000A9D2 File Offset: 0x00008BD2
		// (set) Token: 0x06000242 RID: 578 RVA: 0x0000A9DA File Offset: 0x00008BDA
		public PointGraph pointGraph { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000A9E3 File Offset: 0x00008BE3
		// (set) Token: 0x06000244 RID: 580 RVA: 0x0000A9EB File Offset: 0x00008BEB
		public RecastGraph recastGraph { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000A9F4 File Offset: 0x00008BF4
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000A9FC File Offset: 0x00008BFC
		public LinkGraph linkGraph { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000AA05 File Offset: 0x00008C05
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000AA0C File Offset: 0x00008C0C
		public static Type[] graphTypes { get; private set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000AA14 File Offset: 0x00008C14
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000AA42 File Offset: 0x00008C42
		private byte[] data
		{
			get
			{
				byte[] array = (this.dataString != null) ? Convert.FromBase64String(this.dataString) : null;
				if (array != null && array.Length == 0)
				{
					return null;
				}
				return array;
			}
			set
			{
				this.dataString = ((value != null) ? Convert.ToBase64String(value) : null);
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000AA56 File Offset: 0x00008C56
		internal AstarData(AstarPath active)
		{
			this.active = active;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000AA7C File Offset: 0x00008C7C
		public byte[] GetData()
		{
			return this.data;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000AA84 File Offset: 0x00008C84
		public void SetData(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000AA90 File Offset: 0x00008C90
		public void OnEnable()
		{
			this.FindGraphTypes();
			if (this.graphs == null)
			{
				this.graphs = new NavGraph[0];
			}
			if (this.cacheStartup && this.file_cachedStartup != null && Application.isPlaying)
			{
				this.LoadFromCache();
				return;
			}
			this.DeserializeGraphs();
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000AAE1 File Offset: 0x00008CE1
		internal void LockGraphStructure(bool allowAddingGraphs = false)
		{
			this.graphStructureLocked.Add(allowAddingGraphs);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000AAEF File Offset: 0x00008CEF
		internal void UnlockGraphStructure()
		{
			if (this.graphStructureLocked.Count == 0)
			{
				throw new InvalidOperationException();
			}
			this.graphStructureLocked.RemoveAt(this.graphStructureLocked.Count - 1);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000AB1C File Offset: 0x00008D1C
		private PathProcessor.GraphUpdateLock AssertSafe(bool onlyAddingGraph = false)
		{
			if (this.graphStructureLocked.Count > 0)
			{
				bool flag = true;
				for (int i = 0; i < this.graphStructureLocked.Count; i++)
				{
					flag &= this.graphStructureLocked[i];
				}
				if (!onlyAddingGraph || !flag)
				{
					throw new InvalidOperationException("Graphs cannot be added, removed or serialized while the graph structure is locked. This is the case when a graph is currently being scanned and when executing graph updates and work items.\nHowever as a special case, graphs can be added inside work items.");
				}
			}
			PathProcessor.GraphUpdateLock result = this.active.PausePathfinding();
			if (!this.active.IsInsideWorkItem)
			{
				this.active.FlushWorkItems();
				this.active.pathReturnQueue.ReturnPaths(false);
			}
			return result;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000ABA4 File Offset: 0x00008DA4
		public void GetNodes(Action<GraphNode> callback)
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					this.graphs[i].GetNodes(callback);
				}
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000ABDC File Offset: 0x00008DDC
		public void UpdateShortcuts()
		{
			this.navmeshGraph = (NavMeshGraph)this.FindGraphOfType(typeof(NavMeshGraph));
			this.gridGraph = (GridGraph)this.FindGraphOfType(typeof(GridGraph));
			this.layerGridGraph = (LayerGridGraph)this.FindGraphOfType(typeof(LayerGridGraph));
			this.pointGraph = (PointGraph)this.FindGraphOfType(typeof(PointGraph));
			this.recastGraph = (RecastGraph)this.FindGraphOfType(typeof(RecastGraph));
			this.linkGraph = (LinkGraph)this.FindGraphOfType(typeof(LinkGraph));
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000AC8C File Offset: 0x00008E8C
		public void LoadFromCache()
		{
			using (AstarData.MarkerLoadFromCache.Auto())
			{
				using (this.AssertSafe(false))
				{
					if (this.file_cachedStartup != null)
					{
						byte[] bytes = this.file_cachedStartup.bytes;
						this.DeserializeGraphs(bytes);
						GraphModifier.TriggerEvent(GraphModifier.EventType.PostCacheLoad);
					}
					else
					{
						Debug.LogError("Can't load from cache since the cache is empty");
					}
				}
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000AD20 File Offset: 0x00008F20
		public byte[] SerializeGraphs()
		{
			return this.SerializeGraphs(SerializeSettings.Settings);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000AD30 File Offset: 0x00008F30
		public byte[] SerializeGraphs(SerializeSettings settings)
		{
			uint num;
			return this.SerializeGraphs(settings, out num);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000AD46 File Offset: 0x00008F46
		public byte[] SerializeGraphs(SerializeSettings settings, out uint checksum)
		{
			return this.SerializeGraphs(settings, out checksum, this.graphs);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000AD58 File Offset: 0x00008F58
		private byte[] SerializeGraphs(SerializeSettings settings, out uint checksum, NavGraph[] graphs)
		{
			byte[] result;
			using (this.AssertSafe(false))
			{
				AstarSerializer astarSerializer = new AstarSerializer(this, settings, this.active.gameObject);
				astarSerializer.OpenSerialize();
				astarSerializer.SerializeGraphs(graphs);
				astarSerializer.SerializeExtraInfo();
				byte[] array = astarSerializer.CloseSerialize();
				checksum = astarSerializer.GetChecksum();
				result = array;
			}
			return result;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000ADC4 File Offset: 0x00008FC4
		public void DeserializeGraphs()
		{
			byte[] data = this.data;
			if (data != null)
			{
				this.DeserializeGraphs(data);
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000ADE4 File Offset: 0x00008FE4
		public void ClearGraphs()
		{
			using (this.AssertSafe(false))
			{
				this.ClearGraphsInternal();
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000AE20 File Offset: 0x00009020
		private void ClearGraphsInternal()
		{
			if (this.graphs == null)
			{
				return;
			}
			using (this.AssertSafe(false))
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null)
					{
						this.active.DirtyBounds(this.graphs[i].bounds);
						((IGraphInternals)this.graphs[i]).OnDestroy();
						this.graphs[i].active = null;
					}
				}
				this.graphs = new NavGraph[0];
				this.UpdateShortcuts();
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000AEC0 File Offset: 0x000090C0
		public void DisposeUnmanagedData()
		{
			if (this.graphs == null)
			{
				return;
			}
			using (this.AssertSafe(false))
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null)
					{
						((IGraphInternals)this.graphs[i]).DisposeUnmanagedData();
					}
				}
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000AF28 File Offset: 0x00009128
		internal void DestroyAllNodes()
		{
			if (this.graphs == null)
			{
				return;
			}
			using (this.AssertSafe(false))
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null)
					{
						((IGraphInternals)this.graphs[i]).DestroyAllNodes();
					}
				}
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000AF90 File Offset: 0x00009190
		public void OnDestroy()
		{
			this.ClearGraphsInternal();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000AF98 File Offset: 0x00009198
		public NavGraph[] DeserializeGraphs(byte[] bytes)
		{
			NavGraph[] result;
			using (this.AssertSafe(false))
			{
				this.ClearGraphs();
				result = this.DeserializeGraphsAdditive(bytes);
			}
			return result;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000AFDC File Offset: 0x000091DC
		public NavGraph[] DeserializeGraphsAdditive(byte[] bytes)
		{
			return this.DeserializeGraphsAdditive(bytes, true);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000AFE8 File Offset: 0x000091E8
		private NavGraph[] DeserializeGraphsAdditive(byte[] bytes, bool warnIfDuplicateGuids)
		{
			NavGraph[] result;
			using (this.AssertSafe(false))
			{
				try
				{
					if (bytes == null)
					{
						throw new ArgumentNullException("bytes");
					}
					AstarSerializer astarSerializer = new AstarSerializer(this, this.active.gameObject);
					if (!astarSerializer.OpenDeserialize(bytes))
					{
						throw new ArgumentException("Invalid data file (cannot read zip).\nThe data is either corrupt or it was saved using a 3.0.x or earlier version of the system");
					}
					NavGraph[] array = this.DeserializeGraphsPartAdditive(astarSerializer, warnIfDuplicateGuids);
					astarSerializer.CloseDeserialize();
					this.UpdateShortcuts();
					GraphModifier.TriggerEvent(GraphModifier.EventType.PostGraphLoad);
					result = array;
				}
				catch (Exception innerException)
				{
					Debug.LogException(new Exception("Caught exception while deserializing data.", innerException));
					this.graphs = new NavGraph[0];
					this.UpdateShortcuts();
					throw;
				}
				finally
				{
				}
			}
			return result;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B0B8 File Offset: 0x000092B8
		private NavGraph[] DeserializeGraphsPartAdditive(AstarSerializer sr, bool warnIfDuplicateGuids)
		{
			if (this.graphs == null)
			{
				this.graphs = new NavGraph[0];
			}
			List<NavGraph> gr = new List<NavGraph>(this.graphs);
			while (gr.Count > 0 && gr[gr.Count - 1] == null)
			{
				gr.RemoveAt(gr.Count - 1);
			}
			this.FindGraphTypes();
			bool allowLoadingNodes = this.active == AstarPath.active;
			int lastUsedGraphIndex = -1;
			NavGraph[] newGraphs = sr.DeserializeGraphs(AstarData.graphTypes, allowLoadingNodes, delegate
			{
				int lastUsedGraphIndex = lastUsedGraphIndex;
				lastUsedGraphIndex++;
				while (lastUsedGraphIndex < gr.Count && gr[lastUsedGraphIndex] != null)
				{
					lastUsedGraphIndex = lastUsedGraphIndex;
					lastUsedGraphIndex++;
				}
				return lastUsedGraphIndex;
			});
			for (int m = 0; m < newGraphs.Length; m++)
			{
				while (gr.Count < (int)(newGraphs[m].graphIndex + 1U))
				{
					gr.Add(null);
				}
				gr[(int)newGraphs[m].graphIndex] = newGraphs[m];
			}
			if ((long)gr.Count > 255L)
			{
				throw new InvalidOperationException("Graph Count Limit Reached. You cannot have more than " + 254U.ToString() + " graphs.");
			}
			this.graphs = gr.ToArray();
			bool flag = false;
			int i;
			int i2;
			for (i = 0; i < this.graphs.Length; i = i2 + 1)
			{
				if (this.graphs[i] != null)
				{
					this.graphs[i].GetNodes(delegate(GraphNode node)
					{
						node.GraphIndex = (uint)i;
					});
					flag |= this.graphs[i].isScanned;
				}
				i2 = i;
			}
			for (int j = 0; j < this.graphs.Length; j++)
			{
				for (int k = j + 1; k < this.graphs.Length; k++)
				{
					if (this.graphs[j] != null && this.graphs[k] != null && this.graphs[j].guid == this.graphs[k].guid)
					{
						if (warnIfDuplicateGuids)
						{
							Debug.LogWarning("Guid Conflict when importing graphs additively. Imported graph will get a new Guid.\nThis message is (relatively) harmless.");
						}
						this.graphs[j].guid = Pathfinding.Util.Guid.NewGuid();
						break;
					}
				}
			}
			sr.PostDeserialization();
			if (flag)
			{
				this.active.AddWorkItem(delegate(IWorkItemContext ctx)
				{
					for (int l = 0; l < newGraphs.Length; l++)
					{
						if (newGraphs[l].isScanned)
						{
							ctx.DirtyBounds(newGraphs[l].bounds);
						}
					}
				});
				this.active.FlushWorkItems();
			}
			return newGraphs;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B34F File Offset: 0x0000954F
		public void FindGraphTypes()
		{
			if (AstarData.graphTypes != null)
			{
				return;
			}
			AstarData.graphTypes = AssemblySearcher.FindTypesInheritingFrom<NavGraph>().ToArray();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B368 File Offset: 0x00009568
		internal NavGraph CreateGraph(Type type)
		{
			NavGraph navGraph = Activator.CreateInstance(type) as NavGraph;
			navGraph.active = this.active;
			return navGraph;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B381 File Offset: 0x00009581
		public T AddGraph<T>() where T : NavGraph
		{
			return this.AddGraph(typeof(T)) as T;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public NavGraph AddGraph(Type type)
		{
			NavGraph navGraph = null;
			for (int i = 0; i < AstarData.graphTypes.Length; i++)
			{
				if (object.Equals(AstarData.graphTypes[i], type))
				{
					navGraph = this.CreateGraph(AstarData.graphTypes[i]);
				}
			}
			if (navGraph == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"No NavGraph of type '",
					(type != null) ? type.ToString() : null,
					"' could be found, ",
					AstarData.graphTypes.Length.ToString(),
					" graph types are avaliable"
				}));
				return null;
			}
			this.AddGraph(navGraph);
			return navGraph;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B438 File Offset: 0x00009638
		private void AddGraph(NavGraph graph)
		{
			using (this.AssertSafe(true))
			{
				int num = Array.IndexOf<NavGraph>(this.graphs, null);
				if (num == -1)
				{
					if ((long)this.graphs.Length >= 254L)
					{
						throw new Exception(string.Format("Graph Count Limit Reached. You cannot have more than {0} graphs.", 254U));
					}
					Memory.Realloc<NavGraph>(ref this.graphs, this.graphs.Length + 1);
					num = this.graphs.Length - 1;
				}
				this.graphs[num] = graph;
				graph.graphIndex = (uint)num;
				graph.active = this.active;
				this.UpdateShortcuts();
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000B4EC File Offset: 0x000096EC
		public bool RemoveGraph(NavGraph graph)
		{
			bool result;
			using (this.AssertSafe(false))
			{
				this.active.DirtyBounds(graph.bounds);
				((IGraphInternals)graph).OnDestroy();
				graph.active = null;
				int num = Array.IndexOf<NavGraph>(this.graphs, graph);
				if (num != -1)
				{
					this.graphs[num] = null;
				}
				this.UpdateShortcuts();
				if (AstarPath.active == this.active)
				{
					this.active.AddWorkItem(delegate()
					{
						this.active.offMeshLinks.Refresh();
					});
					this.active.FlushWorkItems();
				}
				result = (num != -1);
			}
			return result;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000B59C File Offset: 0x0000979C
		public NavGraph DuplicateGraph(NavGraph graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (Array.IndexOf<NavGraph>(this.graphs, graph) == -1)
			{
				throw new ArgumentException("Graph doesn't exist");
			}
			uint num;
			byte[] bytes = this.SerializeGraphs(SerializeSettings.Settings, out num, new NavGraph[]
			{
				graph
			});
			return this.DeserializeGraphsAdditive(bytes, false)[0];
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000B5F4 File Offset: 0x000097F4
		public static NavGraph GetGraph(GraphNode node)
		{
			if (node == null || node.Destroyed)
			{
				return null;
			}
			AstarPath astarPath = AstarPath.active;
			if (astarPath == null)
			{
				return null;
			}
			AstarData data = astarPath.data;
			if (data == null || data.graphs == null)
			{
				return null;
			}
			uint graphIndex = node.GraphIndex;
			return data.graphs[(int)graphIndex];
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000B63C File Offset: 0x0000983C
		public NavGraph FindGraph(Func<NavGraph, bool> predicate)
		{
			if (this.graphs != null)
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null && predicate(this.graphs[i]))
					{
						return this.graphs[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000B688 File Offset: 0x00009888
		public NavGraph FindGraphOfType(Type type)
		{
			return this.FindGraph((NavGraph graph) => object.Equals(graph.GetType(), type));
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000B6B4 File Offset: 0x000098B4
		public NavGraph FindGraphWhichInheritsFrom(Type type)
		{
			return this.FindGraph((NavGraph graph) => WindowsStoreCompatibility.GetTypeInfo(type).IsAssignableFrom(WindowsStoreCompatibility.GetTypeInfo(graph.GetType())));
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000B6E0 File Offset: 0x000098E0
		public IEnumerable FindGraphsOfType(Type type)
		{
			if (this.graphs == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.graphs.Length; i = num + 1)
			{
				if (this.graphs[i] != null && object.Equals(this.graphs[i].GetType(), type))
				{
					yield return this.graphs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000B6F7 File Offset: 0x000098F7
		public IEnumerable GetUpdateableGraphs()
		{
			if (this.graphs == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.graphs.Length; i = num + 1)
			{
				if (this.graphs[i] is IUpdatableGraph)
				{
					yield return this.graphs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000B707 File Offset: 0x00009907
		public int GetGraphIndex(NavGraph graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (this.graphs == null)
			{
				throw new ArgumentException("No graphs exist");
			}
			int num = Array.IndexOf<NavGraph>(this.graphs, graph);
			if (num == -1)
			{
				throw new ArgumentException("Graph doesn't exist");
			}
			return num;
		}

		// Token: 0x040001BC RID: 444
		private AstarPath active;

		// Token: 0x040001C4 RID: 452
		[NonSerialized]
		public NavGraph[] graphs = new NavGraph[0];

		// Token: 0x040001C5 RID: 453
		[SerializeField]
		private string dataString;

		// Token: 0x040001C6 RID: 454
		public TextAsset file_cachedStartup;

		// Token: 0x040001C7 RID: 455
		[SerializeField]
		public bool cacheStartup;

		// Token: 0x040001C8 RID: 456
		private List<bool> graphStructureLocked = new List<bool>();

		// Token: 0x040001C9 RID: 457
		private static readonly ProfilerMarker MarkerLoadFromCache = new ProfilerMarker("LoadFromCache");

		// Token: 0x040001CA RID: 458
		private static readonly ProfilerMarker MarkerDeserializeGraphs = new ProfilerMarker("DeserializeGraphs");

		// Token: 0x040001CB RID: 459
		private static readonly ProfilerMarker MarkerSerializeGraphs = new ProfilerMarker("SerializeGraphs");

		// Token: 0x040001CC RID: 460
		private static readonly ProfilerMarker MarkerFindGraphTypes = new ProfilerMarker("FindGraphTypes");
	}
}
