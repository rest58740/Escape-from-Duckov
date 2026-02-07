using System;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Jobs;
using Pathfinding.Serialization;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pathfinding
{
	// Token: 0x02000078 RID: 120
	[AddComponentMenu("Pathfinding/Navmesh Prefab")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/navmeshprefab.html")]
	public class NavmeshPrefab : VersionedMonoBehaviour
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x0001453C File Offset: 0x0001273C
		protected override void Reset()
		{
			base.Reset();
			AstarPath.FindAstarPath();
			if (AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				RecastGraph recastGraph = AstarPath.active.data.recastGraph;
				this.bounds = new Bounds(Vector3.zero, new Vector3(recastGraph.TileWorldSizeX, recastGraph.forcedBoundsSize.y, recastGraph.TileWorldSizeZ));
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000145AE File Offset: 0x000127AE
		[ContextMenu("Snap to closest tile alignment")]
		public void SnapToClosestTileAlignment()
		{
			AstarPath.FindAstarPath();
			if (AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				this.SnapToClosestTileAlignment(AstarPath.active.data.recastGraph);
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000145E8 File Offset: 0x000127E8
		[ContextMenu("Apply here")]
		public void Apply()
		{
			AstarPath.FindAstarPath();
			if (AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				RecastGraph recastGraph = AstarPath.active.data.recastGraph;
				this.Apply(recastGraph);
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00014630 File Offset: 0x00012830
		public void SnapToClosestTileAlignment(RecastGraph graph)
		{
			TileLayout tileLayout = new TileLayout(graph);
			IntRect intRect;
			int num;
			float y;
			NavmeshPrefab.SnapToGraph(tileLayout, base.transform.position, base.transform.rotation, this.bounds, out intRect, out num, out y);
			Bounds tileBoundsInGraphSpace = tileLayout.GetTileBoundsInGraphSpace(intRect.xmin, intRect.ymin, intRect.Width, intRect.Height);
			Vector3 point = new Vector3(tileBoundsInGraphSpace.center.x, y, tileBoundsInGraphSpace.center.z);
			base.transform.rotation = Quaternion.Euler(graph.rotation) * Quaternion.Euler(0f, (float)(num * 90), 0f);
			base.transform.position = tileLayout.transform.Transform(point) + base.transform.rotation * (-this.bounds.center + new Vector3(0f, this.bounds.extents.y, 0f));
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00014740 File Offset: 0x00012940
		public void SnapSizeToClosestTileMultiple(RecastGraph graph)
		{
			this.bounds = NavmeshPrefab.SnapSizeToClosestTileMultiple(graph, this.bounds);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00014754 File Offset: 0x00012954
		private void Start()
		{
			this.startHasRun = true;
			if (this.applyOnStart && this.serializedNavmesh != null && AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				this.Apply(AstarPath.active.data.recastGraph);
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000147B4 File Offset: 0x000129B4
		private void OnEnable()
		{
			if (this.startHasRun && this.applyOnStart && this.serializedNavmesh != null && AstarPath.active != null && AstarPath.active.data.recastGraph != null)
			{
				this.Apply(AstarPath.active.data.recastGraph);
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00014814 File Offset: 0x00012A14
		private void OnDisable()
		{
			if (this.removeTilesWhenDisabled && this.serializedNavmesh != null && AstarPath.active != null)
			{
				Vector3 pos = base.transform.position;
				Quaternion rot = base.transform.rotation;
				AstarPath.active.AddWorkItem(delegate(IWorkItemContext ctx)
				{
					RecastGraph recastGraph = AstarPath.active.data.recastGraph;
					if (recastGraph != null)
					{
						IntRect tileRect;
						int num;
						float num2;
						NavmeshPrefab.SnapToGraph(new TileLayout(recastGraph), pos, rot, this.bounds, out tileRect, out num, out num2);
						recastGraph.ClearTiles(tileRect);
					}
				});
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001488C File Offset: 0x00012A8C
		public static Bounds SnapSizeToClosestTileMultiple(RecastGraph graph, Bounds bounds)
		{
			float num = Mathf.Max((float)graph.editorTileSize * graph.cellSize, 0.001f);
			Vector2 vector = new Vector2(bounds.size.x / num, bounds.size.z / num);
			Vector2Int vector2Int = new Vector2Int(Mathf.Max(1, Mathf.RoundToInt(vector.x)), Mathf.Max(1, Mathf.RoundToInt(vector.y)));
			return new Bounds(bounds.center, new Vector3((float)vector2Int.x * num, bounds.size.y, (float)vector2Int.y * num));
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00014930 File Offset: 0x00012B30
		public static void SnapToGraph(TileLayout tileLayout, Vector3 position, Quaternion rotation, Bounds bounds, out IntRect tileRect, out int snappedRotation, out float yOffset)
		{
			Vector3 vector = tileLayout.transform.InverseTransformVector(rotation * Vector3.right);
			snappedRotation = -Mathf.RoundToInt(Mathf.Atan2(vector.z, vector.x) / 1.5707964f);
			Quaternion quaternion = Quaternion.Euler(0f, (float)(snappedRotation * 90), 0f);
			Matrix4x4 matrix4x = tileLayout.transform.inverseMatrix * Matrix4x4.TRS(position + quaternion * bounds.center, quaternion, Vector3.one);
			Vector3 lhs = matrix4x.MultiplyPoint3x4(-bounds.extents);
			Vector3 rhs = matrix4x.MultiplyPoint3x4(bounds.extents);
			Vector3 vector2 = Vector3.Min(lhs, rhs);
			Vector3 vector3 = Vector3.Scale(vector2, new Vector3(1f / tileLayout.TileWorldSizeX, 1f, 1f / tileLayout.TileWorldSizeZ));
			Vector2Int vector2Int = new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.z));
			Vector2 vector4 = new Vector2(bounds.size.x, bounds.size.z);
			if ((snappedRotation % 2 + 2) % 2 == 1)
			{
				Memory.Swap<float>(ref vector4.x, ref vector4.y);
			}
			int num = Mathf.Max(1, Mathf.RoundToInt(vector4.x / tileLayout.TileWorldSizeX));
			int num2 = Mathf.Max(1, Mathf.RoundToInt(vector4.y / tileLayout.TileWorldSizeZ));
			tileRect = new IntRect(vector2Int.x, vector2Int.y, vector2Int.x + num - 1, vector2Int.y + num2 - 1);
			yOffset = vector2.y;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00014AE4 File Offset: 0x00012CE4
		public void Apply(RecastGraph graph)
		{
			if (this.serializedNavmesh == null)
			{
				throw new InvalidOperationException("Cannot Apply NavmeshPrefab because no serialized data has been set");
			}
			AstarPath.active.AddWorkItem(delegate()
			{
				IntRect tileRect;
				int rotation;
				float yOffset;
				NavmeshPrefab.SnapToGraph(new TileLayout(graph), this.transform.position, this.transform.rotation, this.bounds, out tileRect, out rotation, out yOffset);
				TileMeshes tileMeshes = TileMeshes.Deserialize(this.serializedNavmesh.bytes);
				tileMeshes.Rotate(rotation);
				if (tileMeshes.tileRect.Width != tileRect.Width || tileMeshes.tileRect.Height != tileRect.Height)
				{
					throw new Exception(string.Concat(new string[]
					{
						"NavmeshPrefab has been scanned with a different size than it is right now (or with a different graph). Expected to find ",
						tileRect.Width.ToString(),
						"x",
						tileRect.Height.ToString(),
						" tiles, but found ",
						tileMeshes.tileRect.Width.ToString(),
						"x",
						tileMeshes.tileRect.Height.ToString()
					}));
				}
				tileMeshes.tileRect = tileRect;
				graph.ReplaceTiles(tileMeshes, yOffset);
			});
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00014B34 File Offset: 0x00012D34
		public byte[] Scan()
		{
			AstarPath.FindAstarPath();
			if (AstarPath.active == null || AstarPath.active.data.recastGraph == null)
			{
				throw new InvalidOperationException("There's no recast graph in the scene. Add one if you want to scan this navmesh prefab.");
			}
			return this.Scan(AstarPath.active.data.recastGraph);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00014B84 File Offset: 0x00012D84
		public byte[] Scan(RecastGraph graph)
		{
			NavmeshPrefab.SerializedOutput serializedOutput = this.ScanAsync(graph).Complete();
			byte[] data = serializedOutput.data;
			serializedOutput.Dispose();
			return data;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00014BB0 File Offset: 0x00012DB0
		public Promise<NavmeshPrefab.SerializedOutput> ScanAsync(RecastGraph graph)
		{
			DisposeArena arena = new DisposeArena();
			TileLayout tileLayout = new TileLayout(new Bounds(base.transform.position + base.transform.rotation * this.bounds.center, this.bounds.size), base.transform.rotation, graph.cellSize, graph.editorTileSize, graph.useTiles);
			tileLayout.graphSpaceSize.x = float.PositiveInfinity;
			tileLayout.graphSpaceSize.z = float.PositiveInfinity;
			TileBuilder tileBuilder = RecastBuilder.BuildTileMeshes(graph, tileLayout, new IntRect(0, 0, tileLayout.tileCount.x - 1, tileLayout.tileCount.y - 1));
			Scene scene = base.gameObject.scene;
			tileBuilder.collectionSettings.physicsScene = new PhysicsScene?(scene.GetPhysicsScene());
			tileBuilder.collectionSettings.physicsScene2D = new PhysicsScene2D?(scene.GetPhysicsScene2D());
			Promise<TileBuilder.TileBuilderOutput> promise = tileBuilder.Schedule(arena);
			NavmeshPrefab.SerializedOutput serializedOutput = new NavmeshPrefab.SerializedOutput
			{
				promise = promise,
				arena = arena
			};
			return new Promise<NavmeshPrefab.SerializedOutput>(new NavmeshPrefab.SerializeJob
			{
				tileMeshesPromise = promise,
				output = serializedOutput
			}.ScheduleManaged(promise.handle), serializedOutput);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00014CF8 File Offset: 0x00012EF8
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			migrations.TryMigrateFromLegacyFormat(out num);
			if (migrations.AddAndMaybeRunMigration(1, true))
			{
				this.removeTilesWhenDisabled = false;
			}
			base.OnUpgradeSerializedData(ref migrations, unityThread);
		}

		// Token: 0x040002A2 RID: 674
		public TextAsset serializedNavmesh;

		// Token: 0x040002A3 RID: 675
		public bool applyOnStart = true;

		// Token: 0x040002A4 RID: 676
		public bool removeTilesWhenDisabled = true;

		// Token: 0x040002A5 RID: 677
		public Bounds bounds = new Bounds(Vector3.zero, new Vector3(10f, 10f, 10f));

		// Token: 0x040002A6 RID: 678
		private bool startHasRun;

		// Token: 0x02000079 RID: 121
		public class SerializedOutput : IProgress, IDisposable
		{
			// Token: 0x170000AC RID: 172
			// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00014D61 File Offset: 0x00012F61
			public float Progress
			{
				get
				{
					return this.promise.Progress;
				}
			}

			// Token: 0x060003F3 RID: 1011 RVA: 0x00014D6E File Offset: 0x00012F6E
			public void Dispose()
			{
				this.promise.Dispose();
				this.arena.DisposeAll();
			}

			// Token: 0x040002A7 RID: 679
			public Promise<TileBuilder.TileBuilderOutput> promise;

			// Token: 0x040002A8 RID: 680
			public byte[] data;

			// Token: 0x040002A9 RID: 681
			public DisposeArena arena;
		}

		// Token: 0x0200007A RID: 122
		private struct SerializeJob : IJob
		{
			// Token: 0x060003F5 RID: 1013 RVA: 0x00014D88 File Offset: 0x00012F88
			public void Execute()
			{
				TileBuilder.TileBuilderOutput value = this.tileMeshesPromise.GetValue();
				this.output.data = value.tileMeshes.ToManaged().Serialize();
			}

			// Token: 0x040002AA RID: 682
			public Promise<TileBuilder.TileBuilderOutput> tileMeshesPromise;

			// Token: 0x040002AB RID: 683
			public NavmeshPrefab.SerializedOutput output;
		}
	}
}
