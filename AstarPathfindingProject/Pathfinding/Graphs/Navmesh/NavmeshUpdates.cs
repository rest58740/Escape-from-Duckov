using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Sync;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001C3 RID: 451
	[Serializable]
	public class NavmeshUpdates
	{
		// Token: 0x06000BF5 RID: 3061 RVA: 0x000463EC File Offset: 0x000445EC
		private static Rect ExpandedBounds(Rect rect)
		{
			rect.xMin -= 0.020000001f;
			rect.yMin -= 0.020000001f;
			rect.xMax += 0.020000001f;
			rect.yMax += 0.020000001f;
			return rect;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00046446 File Offset: 0x00044646
		internal void OnEnable()
		{
			this.lastUpdateTime = float.NegativeInfinity;
			NavmeshClipper.RefreshEnabledList();
			NavmeshClipper.AddEnableCallback(new Action<NavmeshClipper>(this.HandleOnEnableCallback), new Action<NavmeshClipper>(this.HandleOnDisableCallback));
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00046475 File Offset: 0x00044675
		internal void OnDisable()
		{
			NavmeshClipper.RemoveEnableCallback(new Action<NavmeshClipper>(this.HandleOnEnableCallback), new Action<NavmeshClipper>(this.HandleOnDisableCallback));
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00046494 File Offset: 0x00044694
		public void ForceUpdateAround(NavmeshClipper clipper)
		{
			for (int i = 0; i < this.listeners.Count; i++)
			{
				this.listeners[i].Dirty(clipper);
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000464CC File Offset: 0x000446CC
		public void DiscardPending()
		{
			for (int i = 0; i < this.listeners.Count; i++)
			{
				this.listeners[i].DiscardPending();
			}
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00046500 File Offset: 0x00044700
		private void HandleOnEnableCallback(NavmeshClipper obj)
		{
			for (int i = 0; i < this.listeners.Count; i++)
			{
				this.listeners[i].AddClipper(obj);
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00046538 File Offset: 0x00044738
		private void HandleOnDisableCallback(NavmeshClipper obj)
		{
			for (int i = 0; i < this.listeners.Count; i++)
			{
				this.listeners[i].RemoveClipper(obj);
			}
			this.lastUpdateTime = float.NegativeInfinity;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00046578 File Offset: 0x00044778
		private void AddListener(NavmeshUpdates.NavmeshUpdateSettings listener)
		{
			this.listeners.Add(listener);
			for (int i = 0; i < NavmeshClipper.allEnabled.Count; i++)
			{
				listener.AddClipper(NavmeshClipper.allEnabled[i]);
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000465B7 File Offset: 0x000447B7
		private void RemoveListener(NavmeshUpdates.NavmeshUpdateSettings listener)
		{
			this.listeners.Remove(listener);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000465C8 File Offset: 0x000447C8
		internal void Update()
		{
			if (this.astar.isScanning)
			{
				return;
			}
			bool flag = false;
			this.RefreshEnabledState();
			for (int i = 0; i < this.listeners.Count; i++)
			{
				flag |= this.listeners[i].anyTilesDirty;
			}
			if ((this.updateInterval >= 0f && Time.realtimeSinceStartup - this.lastUpdateTime > this.updateInterval) || flag)
			{
				this.ScheduleTileUpdates();
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00046643 File Offset: 0x00044843
		public void ForceUpdate()
		{
			this.RefreshEnabledState();
			this.ScheduleTileUpdates();
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00046654 File Offset: 0x00044854
		private void RefreshEnabledState()
		{
			NavGraph[] graphs = this.astar.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				NavmeshBase navmeshBase = graphs[i] as NavmeshBase;
				if (navmeshBase != null)
				{
					bool flag = navmeshBase.enableNavmeshCutting && navmeshBase.isScanned;
					if (navmeshBase.navmeshUpdateData.enabled != flag)
					{
						if (flag)
						{
							navmeshBase.navmeshUpdateData.Enable();
						}
						else
						{
							navmeshBase.navmeshUpdateData.Disable();
						}
					}
				}
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000466C4 File Offset: 0x000448C4
		private void ScheduleTileUpdates()
		{
			this.lastUpdateTime = Time.realtimeSinceStartup;
			foreach (NavmeshUpdates.NavmeshUpdateSettings navmeshUpdateSettings in this.listeners)
			{
				if (navmeshUpdateSettings.attachedToGraph)
				{
					GridLookup<NavmeshClipper>.Root allItems = navmeshUpdateSettings.clipperLookup.AllItems;
					if (!navmeshUpdateSettings.anyTilesDirty)
					{
						bool flag = false;
						for (GridLookup<NavmeshClipper>.Root root = allItems; root != null; root = root.next)
						{
							if (root.obj.RequiresUpdate(root))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							continue;
						}
					}
					float navmeshCuttingCharacterRadius = navmeshUpdateSettings.graph.NavmeshCuttingCharacterRadius;
					for (GridLookup<NavmeshClipper>.Root root2 = allItems; root2 != null; root2 = root2.next)
					{
						if (root2.obj.RequiresUpdate(root2))
						{
							navmeshUpdateSettings.MarkTilesDirty(root2.previousBounds);
							Rect rect = NavmeshUpdates.ExpandedBounds(root2.obj.GetBounds(navmeshUpdateSettings.tileLayout.transform, navmeshCuttingCharacterRadius));
							IntRect touchingTilesInGraphSpace = navmeshUpdateSettings.tileLayout.GetTouchingTilesInGraphSpace(rect);
							navmeshUpdateSettings.clipperLookup.Move(root2.obj, touchingTilesInGraphSpace);
							navmeshUpdateSettings.MarkTilesDirty(touchingTilesInGraphSpace);
							root2.obj.NotifyUpdated(root2);
						}
					}
					navmeshUpdateSettings.ScheduleDirtyTilesReload();
				}
			}
		}

		// Token: 0x04000840 RID: 2112
		public float updateInterval;

		// Token: 0x04000841 RID: 2113
		internal AstarPath astar;

		// Token: 0x04000842 RID: 2114
		private List<NavmeshUpdates.NavmeshUpdateSettings> listeners = new List<NavmeshUpdates.NavmeshUpdateSettings>();

		// Token: 0x04000843 RID: 2115
		private float lastUpdateTime = float.NegativeInfinity;

		// Token: 0x020001C4 RID: 452
		public class NavmeshUpdateSettings : IDisposable
		{
			// Token: 0x170001B7 RID: 439
			// (get) Token: 0x06000C03 RID: 3075 RVA: 0x0004683A File Offset: 0x00044A3A
			// (set) Token: 0x06000C04 RID: 3076 RVA: 0x00046842 File Offset: 0x00044A42
			public bool attachedToGraph { get; private set; }

			// Token: 0x170001B8 RID: 440
			// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0004684B File Offset: 0x00044A4B
			public bool enabled
			{
				get
				{
					return this.clipperLookup != null;
				}
			}

			// Token: 0x170001B9 RID: 441
			// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00046856 File Offset: 0x00044A56
			public bool anyTilesDirty
			{
				get
				{
					return this.dirtyTileCoordinates.Count > 0;
				}
			}

			// Token: 0x06000C07 RID: 3079 RVA: 0x00046866 File Offset: 0x00044A66
			private void AssertEnabled()
			{
				if (!this.enabled)
				{
					throw new InvalidOperationException("This method cannot be called when the NavmeshUpdateSettings is disabled");
				}
			}

			// Token: 0x06000C08 RID: 3080 RVA: 0x0004687B File Offset: 0x00044A7B
			public NavmeshUpdateSettings(NavmeshBase graph)
			{
				this.graph = graph;
				this.dirtyTiles = default(UnsafeBitArray);
			}

			// Token: 0x06000C09 RID: 3081 RVA: 0x000468A1 File Offset: 0x00044AA1
			public NavmeshUpdateSettings(NavmeshBase graph, TileLayout tileLayout)
			{
				this.graph = graph;
				if (graph.enableNavmeshCutting)
				{
					this.SetLayout(tileLayout);
				}
			}

			// Token: 0x06000C0A RID: 3082 RVA: 0x000468CA File Offset: 0x00044ACA
			public void UpdateLayoutFromGraph()
			{
				if (this.enabled)
				{
					this.ForceUpdateLayoutFromGraph();
				}
			}

			// Token: 0x06000C0B RID: 3083 RVA: 0x000468DC File Offset: 0x00044ADC
			private void ForceUpdateLayoutFromGraph()
			{
				NavMeshGraph navMeshGraph = this.graph as NavMeshGraph;
				if (navMeshGraph != null)
				{
					this.SetLayout(new TileLayout(navMeshGraph));
					return;
				}
				RecastGraph recastGraph = this.graph as RecastGraph;
				if (recastGraph != null)
				{
					this.SetLayout(new TileLayout(recastGraph));
				}
			}

			// Token: 0x06000C0C RID: 3084 RVA: 0x00046920 File Offset: 0x00044B20
			private void SetLayout(TileLayout tileLayout)
			{
				this.Dispose();
				this.tileLayout = tileLayout;
				this.clipperLookup = new GridLookup<NavmeshClipper>(tileLayout.tileCount);
				this.dirtyTiles = new UnsafeBitArray(tileLayout.tileCount.x * tileLayout.tileCount.y, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.graph.active.navmeshUpdates.AddListener(this);
			}

			// Token: 0x06000C0D RID: 3085 RVA: 0x0004698C File Offset: 0x00044B8C
			internal void MarkTilesDirty(IntRect rect)
			{
				if (!this.enabled)
				{
					return;
				}
				rect = IntRect.Intersection(rect, new IntRect(0, 0, this.tileLayout.tileCount.x - 1, this.tileLayout.tileCount.y - 1));
				for (int i = rect.ymin; i <= rect.ymax; i++)
				{
					for (int j = rect.xmin; j <= rect.xmax; j++)
					{
						int pos = j + i * this.tileLayout.tileCount.x;
						if (!this.dirtyTiles.IsSet(pos))
						{
							this.dirtyTiles.Set(pos, true);
							this.dirtyTileCoordinates.Add(new Vector2Int(j, i));
						}
					}
				}
			}

			// Token: 0x06000C0E RID: 3086 RVA: 0x00046A43 File Offset: 0x00044C43
			public void ReloadAllTiles()
			{
				if (!this.enabled)
				{
					return;
				}
				this.MarkTilesDirty(new IntRect(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue));
				this.ScheduleDirtyTilesReload();
			}

			// Token: 0x06000C0F RID: 3087 RVA: 0x00046A74 File Offset: 0x00044C74
			public void AttachToGraph()
			{
				if (this.graph.navmeshUpdateData != null)
				{
					this.graph.navmeshUpdateData.Dispose();
					this.graph.navmeshUpdateData.attachedToGraph = false;
				}
				this.graph.navmeshUpdateData = this;
				this.attachedToGraph = true;
			}

			// Token: 0x06000C10 RID: 3088 RVA: 0x00046AC2 File Offset: 0x00044CC2
			public void Enable()
			{
				if (this.enabled)
				{
					throw new InvalidOperationException("Already enabled");
				}
				this.ForceUpdateLayoutFromGraph();
				this.ReloadAllTiles();
			}

			// Token: 0x06000C11 RID: 3089 RVA: 0x00046AE3 File Offset: 0x00044CE3
			public void Disable()
			{
				if (!this.enabled)
				{
					return;
				}
				this.clipperLookup.Clear();
				this.ReloadAllTiles();
				this.graph.active.FlushWorkItems();
				this.Dispose();
			}

			// Token: 0x06000C12 RID: 3090 RVA: 0x00046B18 File Offset: 0x00044D18
			public void Dispose()
			{
				this.clipperLookup = null;
				if (this.dirtyTiles.IsCreated)
				{
					this.dirtyTiles.Dispose();
				}
				this.dirtyTiles = default(UnsafeBitArray);
				if (this.graph.active != null)
				{
					this.graph.active.navmeshUpdates.RemoveListener(this);
				}
			}

			// Token: 0x06000C13 RID: 3091 RVA: 0x00046B7C File Offset: 0x00044D7C
			public void DiscardPending()
			{
				if (!this.enabled)
				{
					return;
				}
				for (int i = 0; i < NavmeshClipper.allEnabled.Count; i++)
				{
					NavmeshClipper navmeshClipper = NavmeshClipper.allEnabled[i];
					GridLookup<NavmeshClipper>.Root root = this.clipperLookup.GetRoot(navmeshClipper);
					if (root != null)
					{
						navmeshClipper.NotifyUpdated(root);
					}
				}
				this.dirtyTileCoordinates.Clear();
				this.dirtyTiles.Clear();
			}

			// Token: 0x06000C14 RID: 3092 RVA: 0x00046BE0 File Offset: 0x00044DE0
			public void OnResized(IntRect newTileBounds, TileLayout tileLayout)
			{
				if (!this.enabled)
				{
					return;
				}
				this.clipperLookup.Resize(newTileBounds);
				this.tileLayout = tileLayout;
				float navmeshCuttingCharacterRadius = this.graph.NavmeshCuttingCharacterRadius;
				for (GridLookup<NavmeshClipper>.Root root = this.clipperLookup.AllItems; root != null; root = root.next)
				{
					Rect rect = NavmeshUpdates.ExpandedBounds(root.obj.GetBounds(tileLayout.transform, navmeshCuttingCharacterRadius));
					IntRect touchingTilesInGraphSpace = tileLayout.GetTouchingTilesInGraphSpace(rect);
					if (root.previousBounds != touchingTilesInGraphSpace)
					{
						this.clipperLookup.Dirty(root.obj);
						this.clipperLookup.Move(root.obj, touchingTilesInGraphSpace);
					}
				}
				for (int i = 0; i < this.dirtyTileCoordinates.Count; i++)
				{
					Vector2Int vector2Int = this.dirtyTileCoordinates[i];
					if (newTileBounds.Contains(vector2Int.x, vector2Int.y))
					{
						this.dirtyTileCoordinates[i] = new Vector2Int(vector2Int.x - newTileBounds.xmin, vector2Int.y - newTileBounds.ymin);
					}
					else
					{
						this.dirtyTileCoordinates.RemoveAtSwapBack(i);
						i--;
					}
				}
				this.dirtyTiles.Resize(newTileBounds.Width * newTileBounds.Height, NativeArrayOptions.UninitializedMemory);
				this.dirtyTiles.Clear();
				for (int j = 0; j < this.dirtyTileCoordinates.Count; j++)
				{
					this.dirtyTiles.Set(this.dirtyTileCoordinates[j].x + this.dirtyTileCoordinates[j].y * newTileBounds.Width, true);
				}
			}

			// Token: 0x06000C15 RID: 3093 RVA: 0x00046D83 File Offset: 0x00044F83
			public void Dirty(NavmeshClipper obj)
			{
				if (this.enabled)
				{
					this.clipperLookup.Dirty(obj);
				}
			}

			// Token: 0x06000C16 RID: 3094 RVA: 0x00046D9C File Offset: 0x00044F9C
			public void AddClipper(NavmeshClipper obj)
			{
				this.AssertEnabled();
				if (!obj.graphMask.Contains((int)this.graph.graphIndex))
				{
					return;
				}
				float navmeshCuttingCharacterRadius = this.graph.NavmeshCuttingCharacterRadius;
				Rect rect = NavmeshUpdates.ExpandedBounds(obj.GetBounds(this.tileLayout.transform, navmeshCuttingCharacterRadius));
				IntRect touchingTilesInGraphSpace = this.tileLayout.GetTouchingTilesInGraphSpace(rect);
				this.clipperLookup.Add(obj, touchingTilesInGraphSpace);
			}

			// Token: 0x06000C17 RID: 3095 RVA: 0x00046E08 File Offset: 0x00045008
			public void RemoveClipper(NavmeshClipper obj)
			{
				this.AssertEnabled();
				GridLookup<NavmeshClipper>.Root root = this.clipperLookup.GetRoot(obj);
				if (root != null)
				{
					this.MarkTilesDirty(root.previousBounds);
					this.clipperLookup.Remove(obj);
				}
			}

			// Token: 0x06000C18 RID: 3096 RVA: 0x00046E43 File Offset: 0x00045043
			public void ScheduleDirtyTilesReload()
			{
				this.AssertEnabled();
				if (this.dirtyTileCoordinates.Count == 0)
				{
					return;
				}
				TileLayout tileLayout = this.tileLayout;
				this.graph.active.AddWorkItem(delegate(IWorkItemContext ctx)
				{
					ctx.PreUpdate();
					this.ReloadDirtyTilesImmediately();
				});
			}

			// Token: 0x06000C19 RID: 3097 RVA: 0x00046E7C File Offset: 0x0004507C
			public void ReloadDirtyTilesImmediately()
			{
				if (!this.enabled || this.dirtyTileCoordinates.Count == 0)
				{
					return;
				}
				Promise<TileCutter.TileCutterOutput> promise = RecastBuilder.CutTiles(this.graph, this.clipperLookup, this.tileLayout).Schedule(this.dirtyTileCoordinates);
				promise.Complete();
				TileCutter.TileCutterOutput value = promise.GetValue();
				this.graph.StartBatchTileUpdate(false);
				if (!value.tileMeshes.tileMeshes.IsCreated)
				{
					for (int i = 0; i < this.dirtyTileCoordinates.Count; i++)
					{
						NavmeshTile tile = this.graph.GetTile(this.dirtyTileCoordinates[i].x, this.dirtyTileCoordinates[i].y);
						if (tile.isCut)
						{
							this.graph.ReplaceTilePostCut(tile.x, tile.z, tile.preCutVertsInTileSpace, tile.preCutTris, tile.preCutTags, true, true);
						}
					}
				}
				else
				{
					for (int j = 0; j < value.tileMeshes.tileMeshes.Length; j++)
					{
						TileMesh.TileMeshUnsafe tileMeshUnsafe = value.tileMeshes.tileMeshes[j];
						this.graph.ReplaceTilePostCut(this.dirtyTileCoordinates[j].x, this.dirtyTileCoordinates[j].y, tileMeshUnsafe.verticesInTileSpace, tileMeshUnsafe.triangles, tileMeshUnsafe.tags, true, true);
					}
				}
				value.Dispose();
				this.graph.EndBatchTileUpdate();
				this.dirtyTileCoordinates.Clear();
				this.dirtyTiles.Clear();
			}

			// Token: 0x04000844 RID: 2116
			internal readonly NavmeshBase graph;

			// Token: 0x04000845 RID: 2117
			public GridLookup<NavmeshClipper> clipperLookup;

			// Token: 0x04000846 RID: 2118
			public TileLayout tileLayout;

			// Token: 0x04000847 RID: 2119
			private UnsafeBitArray dirtyTiles;

			// Token: 0x04000848 RID: 2120
			private List<Vector2Int> dirtyTileCoordinates = new List<Vector2Int>();
		}
	}
}
