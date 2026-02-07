using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Drawing.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Drawing
{
	// Token: 0x0200002B RID: 43
	public class DrawingData
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000A3E6 File Offset: 0x000085E6
		private int adjustedSceneModeVersion
		{
			get
			{
				return this.sceneModeVersion + (Application.isPlaying ? 1000 : 0);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000A3FE File Offset: 0x000085FE
		internal int GetNextDrawOrderIndex()
		{
			this.currentDrawOrderIndex++;
			return this.currentDrawOrderIndex;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000A414 File Offset: 0x00008614
		internal void PoolMesh(Mesh mesh)
		{
			this.stagingCachedMeshes.Add(mesh);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000A422 File Offset: 0x00008622
		private void SortPooledMeshes()
		{
			this.cachedMeshes.Sort((Mesh a, Mesh b) => b.vertexCount - a.vertexCount);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000A450 File Offset: 0x00008650
		internal Mesh GetMesh(int desiredVertexCount)
		{
			if (this.cachedMeshes.Count > 0)
			{
				int num = 0;
				int i = this.cachedMeshes.Count;
				while (i > num + 1)
				{
					int num2 = (num + i) / 2;
					if (this.cachedMeshes[num2].vertexCount < desiredVertexCount)
					{
						i = num2;
					}
					else
					{
						num = num2;
					}
				}
				Mesh result = this.cachedMeshes[num];
				if (num == 0)
				{
					this.lastTimeLargestCachedMeshWasUsed = this.version;
				}
				this.cachedMeshes.RemoveAt(num);
				return result;
			}
			Mesh mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
			mesh.MarkDynamic();
			return mesh;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000A4DC File Offset: 0x000086DC
		internal void LoadFontDataIfNecessary()
		{
			if (this.fontData.material == null)
			{
				SDFFont font = DefaultFonts.LoadDefaultFont();
				this.fontData.Dispose();
				this.fontData = new SDFLookupData(font);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000A519 File Offset: 0x00008719
		private static float CurrentTime
		{
			get
			{
				if (!Application.isPlaying)
				{
					return Time.realtimeSinceStartup;
				}
				return Time.time;
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000A52D File Offset: 0x0000872D
		private unsafe static void UpdateTime()
		{
			*SharedDrawingData.BurstTime.Data = DrawingData.CurrentTime;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000A540 File Offset: 0x00008740
		public CommandBuilder GetBuilder(bool renderInGame = false)
		{
			DrawingData.UpdateTime();
			return new CommandBuilder(this, DrawingData.Hasher.NotSupplied, this.frameRedrawScope, default(RedrawScope), !renderInGame, false, this.adjustedSceneModeVersion);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000A578 File Offset: 0x00008778
		internal CommandBuilder GetBuiltInBuilder(bool renderInGame = false)
		{
			DrawingData.UpdateTime();
			return new CommandBuilder(this, DrawingData.Hasher.NotSupplied, this.frameRedrawScope, default(RedrawScope), !renderInGame, true, this.adjustedSceneModeVersion);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000A5AF File Offset: 0x000087AF
		public CommandBuilder GetBuilder(RedrawScope redrawScope, bool renderInGame = false)
		{
			DrawingData.UpdateTime();
			return new CommandBuilder(this, DrawingData.Hasher.NotSupplied, this.frameRedrawScope, redrawScope, !renderInGame, false, this.adjustedSceneModeVersion);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000A5D3 File Offset: 0x000087D3
		public CommandBuilder GetBuilder(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope), bool renderInGame = false)
		{
			if (!hasher.Equals(DrawingData.Hasher.NotSupplied))
			{
				this.DiscardData(hasher);
			}
			DrawingData.UpdateTime();
			return new CommandBuilder(this, hasher, this.frameRedrawScope, redrawScope, !renderInGame, false, this.adjustedSceneModeVersion);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000A608 File Offset: 0x00008808
		public DrawingSettings.Settings settingsRef
		{
			get
			{
				if (this.settingsAsset == null)
				{
					this.settingsAsset = DrawingSettings.GetSettingsAsset();
					if (this.settingsAsset == null)
					{
						throw new InvalidOperationException("ALINE settings could not be found");
					}
				}
				return this.settingsAsset.settings;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000A647 File Offset: 0x00008847
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000A64F File Offset: 0x0000884F
		public int version { get; private set; } = 1;

		// Token: 0x060002E2 RID: 738 RVA: 0x0000A658 File Offset: 0x00008858
		private void DiscardData(DrawingData.Hasher hasher)
		{
			this.processedData.ReleaseAllWithHash(this, hasher);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000A667 File Offset: 0x00008867
		internal void OnChangingPlayMode()
		{
			this.sceneModeVersion++;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000A677 File Offset: 0x00008877
		public bool Draw(DrawingData.Hasher hasher)
		{
			if (hasher.Equals(DrawingData.Hasher.NotSupplied))
			{
				throw new ArgumentException("Invalid hash value");
			}
			return this.processedData.SetVersion(hasher, this.version);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000A6A4 File Offset: 0x000088A4
		public bool Draw(DrawingData.Hasher hasher, RedrawScope scope)
		{
			if (hasher.Equals(DrawingData.Hasher.NotSupplied))
			{
				throw new ArgumentException("Invalid hash value");
			}
			this.processedData.SetCustomScope(hasher, scope);
			return this.processedData.SetVersion(hasher, this.version);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000A6DF File Offset: 0x000088DF
		internal void Draw(RedrawScope scope)
		{
			if (scope.id != 0)
			{
				this.processedData.SetVersion(scope, this.version);
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000A6FC File Offset: 0x000088FC
		internal void DrawUntilDisposed(RedrawScope scope)
		{
			if (scope.id != 0)
			{
				this.Draw(scope);
				this.persistentRedrawScopes.Add(scope.id);
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000A71F File Offset: 0x0000891F
		internal void DisposeRedrawScope(RedrawScope scope)
		{
			if (scope.id != 0)
			{
				this.processedData.SetVersion(scope, -1);
				this.persistentRedrawScopes.Remove(scope.id);
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000A74C File Offset: 0x0000894C
		public void TickFramePreRender()
		{
			this.data.DisposeCommandBuildersWithJobDependencies(this);
			this.processedData.FilterOldPersistentCommands(this.version, this.lastTickVersion, DrawingData.CurrentTime, this.adjustedSceneModeVersion);
			foreach (int id in this.persistentRedrawScopes)
			{
				this.processedData.SetVersion(new RedrawScope(this, id), this.version);
			}
			this.processedData.ReleaseDataOlderThan(this, this.lastTickVersion2 + 1);
			this.lastTickVersion2 = this.lastTickVersion;
			this.lastTickVersion = this.version;
			this.currentDrawOrderIndex = 0;
			this.cachedMeshes.AddRange(this.stagingCachedMeshes);
			this.stagingCachedMeshes.Clear();
			this.SortPooledMeshes();
			if (this.version - this.lastTimeLargestCachedMeshWasUsed > 60 && this.cachedMeshes.Count > 0)
			{
				UnityEngine.Object.DestroyImmediate(this.cachedMeshes[0]);
				this.cachedMeshes.RemoveAt(0);
				this.lastTimeLargestCachedMeshWasUsed = this.version;
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000A87C File Offset: 0x00008A7C
		public void PostRenderCleanup()
		{
			this.data.ReleaseAllUnused();
			int version = this.version;
			this.version = version + 1;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000A8A4 File Offset: 0x00008AA4
		private int totalMemoryUsage
		{
			get
			{
				return this.data.memoryUsage + this.processedData.memoryUsage;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000A8C0 File Offset: 0x00008AC0
		private void LoadMaterials()
		{
			if (this.surfaceMaterial == null)
			{
				this.surfaceMaterial = Resources.Load<Material>("aline_surface_mat");
			}
			if (this.lineMaterial == null)
			{
				this.lineMaterial = Resources.Load<Material>("aline_outline_mat");
			}
			if (this.fontData.material == null)
			{
				SDFFont font = DefaultFonts.LoadDefaultFont();
				this.fontData.Dispose();
				this.fontData = new SDFLookupData(font);
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000A93C File Offset: 0x00008B3C
		public DrawingData()
		{
			this.gizmosHandle = GCHandle.Alloc(this, GCHandleType.Weak);
			this.LoadMaterials();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000A9B7 File Offset: 0x00008BB7
		private static int CeilLog2(int x)
		{
			return (int)math.ceil(math.log2((float)x));
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		public void Render(Camera cam, bool allowGizmos, DrawingData.CommandBufferWrapper commandBuffer, bool allowCameraDefault)
		{
			this.LoadMaterials();
			if (this.surfaceMaterial == null || this.lineMaterial == null)
			{
				return;
			}
			Plane[] planes = this.frustrumPlanes;
			GeometryUtility.CalculateFrustumPlanes(cam, planes);
			DrawingData.Range range;
			if (!this.cameraVersions.TryGetValue(cam, out range))
			{
				range = new DrawingData.Range
				{
					start = int.MinValue,
					end = int.MinValue
				};
			}
			if (range.end > this.lastTickVersion)
			{
				range.end = this.version + 1;
			}
			else
			{
				range = new DrawingData.Range
				{
					start = range.end,
					end = this.version + 1
				};
			}
			range.start = Mathf.Max(range.start, this.lastTickVersion2 + 1);
			DrawingSettings.Settings settingsRef = this.settingsRef;
			if (!GL.wireframe)
			{
				this.processedData.SubmitMeshes(this, cam, range.start, allowGizmos, allowCameraDefault);
				this.meshes.Clear();
				this.processedData.CollectMeshes(range.start, this.meshes, cam, allowGizmos, allowCameraDefault);
				this.processedData.PoolDynamicMeshes(this);
				this.meshes.Sort(DrawingData.meshSorter);
				int nameID = Shader.PropertyToID("_Color");
				int nameID2 = Shader.PropertyToID("_FadeColor");
				Color color = new Color(1f, 1f, 1f, settingsRef.solidOpacity);
				Color value = new Color(1f, 1f, 1f, settingsRef.solidOpacityBehindObjects);
				Color value2 = new Color(1f, 1f, 1f, settingsRef.lineOpacity);
				Color value3 = new Color(1f, 1f, 1f, settingsRef.lineOpacityBehindObjects);
				Color value4 = new Color(1f, 1f, 1f, settingsRef.textOpacity);
				Color value5 = new Color(1f, 1f, 1f, settingsRef.textOpacityBehindObjects);
				int i = 0;
				while (i < this.meshes.Count)
				{
					int num = i + 1;
					DrawingData.MeshType meshType = this.meshes[i].type & DrawingData.MeshType.BaseType;
					while (num < this.meshes.Count && (this.meshes[num].type & DrawingData.MeshType.BaseType) == meshType)
					{
						num++;
					}
					this.customMaterialProperties.Clear();
					Material material;
					switch (meshType)
					{
					case DrawingData.MeshType.Solid:
						material = this.surfaceMaterial;
						this.customMaterialProperties.SetColor(nameID, color);
						this.customMaterialProperties.SetColor(nameID2, value);
						break;
					case DrawingData.MeshType.Lines:
						material = this.lineMaterial;
						this.customMaterialProperties.SetColor(nameID, value2);
						this.customMaterialProperties.SetColor(nameID2, value3);
						break;
					case DrawingData.MeshType.Solid | DrawingData.MeshType.Lines:
						goto IL_2E1;
					case DrawingData.MeshType.Text:
						material = this.fontData.material;
						this.customMaterialProperties.SetColor(nameID, value4);
						this.customMaterialProperties.SetColor(nameID2, value5);
						break;
					default:
						goto IL_2E1;
					}
					for (int j = 0; j < material.passCount; j++)
					{
						for (int k = i; k < num; k++)
						{
							DrawingData.RenderedMeshWithType renderedMeshWithType = this.meshes[k];
							if ((renderedMeshWithType.type & DrawingData.MeshType.Custom) != (DrawingData.MeshType)0)
							{
								if (GeometryUtility.TestPlanesAABB(planes, DrawingData.TransformBoundingBox(renderedMeshWithType.matrix, renderedMeshWithType.mesh.bounds)))
								{
									this.customMaterialProperties.SetColor(nameID, color * renderedMeshWithType.color);
									commandBuffer.DrawMesh(renderedMeshWithType.mesh, renderedMeshWithType.matrix, material, 0, j, this.customMaterialProperties);
									this.customMaterialProperties.SetColor(nameID, color);
								}
							}
							else if (GeometryUtility.TestPlanesAABB(planes, renderedMeshWithType.mesh.bounds))
							{
								commandBuffer.DrawMesh(renderedMeshWithType.mesh, Matrix4x4.identity, material, 0, j, this.customMaterialProperties);
							}
						}
					}
					i = num;
					continue;
					IL_2E1:
					throw new InvalidOperationException("Invalid mesh type");
				}
				this.meshes.Clear();
			}
			this.cameraVersions[cam] = range;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000ADDC File Offset: 0x00008FDC
		private static Bounds TransformBoundingBox(Matrix4x4 matrix, Bounds bounds)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			Bounds result = new Bounds(matrix.MultiplyPoint(min), Vector3.zero);
			result.Encapsulate(matrix.MultiplyPoint(new Vector3(min.x, min.y, max.z)));
			result.Encapsulate(matrix.MultiplyPoint(new Vector3(min.x, max.y, min.z)));
			result.Encapsulate(matrix.MultiplyPoint(new Vector3(min.x, max.y, max.z)));
			result.Encapsulate(matrix.MultiplyPoint(new Vector3(max.x, min.y, min.z)));
			result.Encapsulate(matrix.MultiplyPoint(new Vector3(max.x, min.y, max.z)));
			result.Encapsulate(matrix.MultiplyPoint(new Vector3(max.x, max.y, min.z)));
			result.Encapsulate(matrix.MultiplyPoint(new Vector3(max.x, max.y, max.z)));
			return result;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000AF14 File Offset: 0x00009114
		public void ClearData()
		{
			this.gizmosHandle.Free();
			this.data.Dispose();
			this.processedData.Dispose(this);
			for (int i = 0; i < this.cachedMeshes.Count; i++)
			{
				UnityEngine.Object.DestroyImmediate(this.cachedMeshes[i]);
			}
			this.cachedMeshes.Clear();
			this.fontData.Dispose();
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000B02D File Offset: 0x0000922D
		public static void Initialize$BuilderData_AnyBuffersWrittenTo_000002F7$BurstDirectCall()
		{
			DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.Initialize();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000B034 File Offset: 0x00009234
		public static void Initialize$BuilderData_ResetAllBuffers_000002F8$BurstDirectCall()
		{
			DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.Initialize();
		}

		// Token: 0x04000080 RID: 128
		internal DrawingData.BuilderDataContainer data;

		// Token: 0x04000081 RID: 129
		internal DrawingData.ProcessedBuilderDataContainer processedData;

		// Token: 0x04000082 RID: 130
		private List<DrawingData.RenderedMeshWithType> meshes = new List<DrawingData.RenderedMeshWithType>();

		// Token: 0x04000083 RID: 131
		private List<Mesh> cachedMeshes = new List<Mesh>();

		// Token: 0x04000084 RID: 132
		private List<Mesh> stagingCachedMeshes = new List<Mesh>();

		// Token: 0x04000085 RID: 133
		private int lastTimeLargestCachedMeshWasUsed;

		// Token: 0x04000086 RID: 134
		internal SDFLookupData fontData;

		// Token: 0x04000087 RID: 135
		private int currentDrawOrderIndex;

		// Token: 0x04000088 RID: 136
		internal int sceneModeVersion;

		// Token: 0x04000089 RID: 137
		public Material surfaceMaterial;

		// Token: 0x0400008A RID: 138
		public Material lineMaterial;

		// Token: 0x0400008B RID: 139
		public Material textMaterial;

		// Token: 0x0400008C RID: 140
		public DrawingSettings settingsAsset;

		// Token: 0x0400008E RID: 142
		private int lastTickVersion;

		// Token: 0x0400008F RID: 143
		private int lastTickVersion2;

		// Token: 0x04000090 RID: 144
		private HashSet<int> persistentRedrawScopes = new HashSet<int>();

		// Token: 0x04000091 RID: 145
		internal GCHandle gizmosHandle;

		// Token: 0x04000092 RID: 146
		public RedrawScope frameRedrawScope;

		// Token: 0x04000093 RID: 147
		private Dictionary<Camera, DrawingData.Range> cameraVersions = new Dictionary<Camera, DrawingData.Range>();

		// Token: 0x04000094 RID: 148
		internal static readonly ProfilerMarker MarkerScheduleJobs = new ProfilerMarker("ScheduleJobs");

		// Token: 0x04000095 RID: 149
		internal static readonly ProfilerMarker MarkerAwaitUserDependencies = new ProfilerMarker("Await user dependencies");

		// Token: 0x04000096 RID: 150
		internal static readonly ProfilerMarker MarkerSchedule = new ProfilerMarker("Schedule");

		// Token: 0x04000097 RID: 151
		internal static readonly ProfilerMarker MarkerBuild = new ProfilerMarker("Build");

		// Token: 0x04000098 RID: 152
		internal static readonly ProfilerMarker MarkerPool = new ProfilerMarker("Pool");

		// Token: 0x04000099 RID: 153
		internal static readonly ProfilerMarker MarkerRelease = new ProfilerMarker("Release");

		// Token: 0x0400009A RID: 154
		internal static readonly ProfilerMarker MarkerBuildMeshes = new ProfilerMarker("Build Meshes");

		// Token: 0x0400009B RID: 155
		internal static readonly ProfilerMarker MarkerCollectMeshes = new ProfilerMarker("Collect Meshes");

		// Token: 0x0400009C RID: 156
		internal static readonly ProfilerMarker MarkerSortMeshes = new ProfilerMarker("Sort Meshes");

		// Token: 0x0400009D RID: 157
		internal static readonly ProfilerMarker LeakTracking = new ProfilerMarker("RedrawScope Leak Tracking");

		// Token: 0x0400009E RID: 158
		private static readonly DrawingData.MeshCompareByDrawingOrder meshSorter = new DrawingData.MeshCompareByDrawingOrder();

		// Token: 0x0400009F RID: 159
		private Plane[] frustrumPlanes = new Plane[6];

		// Token: 0x040000A0 RID: 160
		private MaterialPropertyBlock customMaterialProperties = new MaterialPropertyBlock();

		// Token: 0x0200002C RID: 44
		public struct Hasher : IEquatable<DrawingData.Hasher>
		{
			// Token: 0x1700000E RID: 14
			// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000B03C File Offset: 0x0000923C
			public static DrawingData.Hasher NotSupplied
			{
				get
				{
					return new DrawingData.Hasher
					{
						hash = ulong.MaxValue
					};
				}
			}

			// Token: 0x060002F6 RID: 758 RVA: 0x0000B05C File Offset: 0x0000925C
			public static DrawingData.Hasher Create<T>(T init)
			{
				DrawingData.Hasher result = default(DrawingData.Hasher);
				result.Add<T>(init);
				return result;
			}

			// Token: 0x060002F7 RID: 759 RVA: 0x0000B07A File Offset: 0x0000927A
			public void Add<T>(T hash)
			{
				this.hash = (1572869UL * this.hash ^ (ulong)((long)hash.GetHashCode() + 12289L));
			}

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000B0A5 File Offset: 0x000092A5
			public ulong Hash
			{
				get
				{
					return this.hash;
				}
			}

			// Token: 0x060002F9 RID: 761 RVA: 0x0000B0AD File Offset: 0x000092AD
			public override int GetHashCode()
			{
				return (int)this.hash;
			}

			// Token: 0x060002FA RID: 762 RVA: 0x0000B0B6 File Offset: 0x000092B6
			public bool Equals(DrawingData.Hasher other)
			{
				return this.hash == other.hash;
			}

			// Token: 0x040000A1 RID: 161
			private ulong hash;
		}

		// Token: 0x0200002D RID: 45
		internal struct ProcessedBuilderData
		{
			// Token: 0x17000010 RID: 16
			// (get) Token: 0x060002FB RID: 763 RVA: 0x0000B0C6 File Offset: 0x000092C6
			public bool isValid
			{
				get
				{
					return this.type > DrawingData.ProcessedBuilderData.Type.Invalid;
				}
			}

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x060002FC RID: 764 RVA: 0x0000B0D1 File Offset: 0x000092D1
			public unsafe UnsafeAppendBuffer* splitterOutputPtr
			{
				get
				{
					return &((DrawingData.ProcessedBuilderData.MeshBuffers*)this.temporaryMeshBuffers.GetUnsafePtr<DrawingData.ProcessedBuilderData.MeshBuffers>())->splitterOutput;
				}
			}

			// Token: 0x060002FD RID: 765 RVA: 0x0000B0E4 File Offset: 0x000092E4
			public void Init(DrawingData.ProcessedBuilderData.Type type, DrawingData.BuilderData.Meta meta)
			{
				this.submitted = false;
				this.type = type;
				this.meta = meta;
				if (this.meshes == null)
				{
					this.meshes = new List<DrawingData.MeshWithType>();
				}
				if (!this.temporaryMeshBuffers.IsCreated)
				{
					this.temporaryMeshBuffers = new NativeArray<DrawingData.ProcessedBuilderData.MeshBuffers>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					this.temporaryMeshBuffers[0] = new DrawingData.ProcessedBuilderData.MeshBuffers(Allocator.Persistent);
				}
			}

			// Token: 0x060002FE RID: 766 RVA: 0x0000B148 File Offset: 0x00009348
			public unsafe void SetSplitterJob(DrawingData gizmos, JobHandle splitterJob)
			{
				this.splitterJob = splitterJob;
				if (this.type == DrawingData.ProcessedBuilderData.Type.Static)
				{
					GeometryBuilder.CameraInfo cameraInfo = new GeometryBuilder.CameraInfo(null);
					this.buildJob = GeometryBuilder.Build(gizmos, (DrawingData.ProcessedBuilderData.MeshBuffers*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<DrawingData.ProcessedBuilderData.MeshBuffers>(this.temporaryMeshBuffers), ref cameraInfo, splitterJob);
					DrawingData.ProcessedBuilderData.SubmittedJobs++;
					if (DrawingData.ProcessedBuilderData.SubmittedJobs % 8 == 0)
					{
						JobHandle.ScheduleBatchedJobs();
					}
				}
			}

			// Token: 0x060002FF RID: 767 RVA: 0x0000B1A4 File Offset: 0x000093A4
			public unsafe void SchedulePersistFilter(int version, int lastTickVersion, float time, int sceneModeVersion)
			{
				if (this.type != DrawingData.ProcessedBuilderData.Type.Persistent)
				{
					throw new InvalidOperationException();
				}
				if (this.meta.sceneModeVersion != sceneModeVersion)
				{
					this.meta.version = -1;
					return;
				}
				if (this.meta.version < lastTickVersion || this.submitted)
				{
					this.splitterJob.Complete();
					this.meta.version = version;
					if (this.temporaryMeshBuffers[0].splitterOutput.Length == 0)
					{
						this.meta.version = -1;
						return;
					}
					this.buildJob.Complete();
					this.splitterJob = new PersistentFilterJob
					{
						buffer = &((DrawingData.ProcessedBuilderData.MeshBuffers*)this.temporaryMeshBuffers.GetUnsafePtr<DrawingData.ProcessedBuilderData.MeshBuffers>())->splitterOutput,
						time = time
					}.Schedule(this.splitterJob);
				}
			}

			// Token: 0x06000300 RID: 768 RVA: 0x0000B276 File Offset: 0x00009476
			public bool IsValidForCamera(Camera camera, bool allowGizmos, bool allowCameraDefault)
			{
				if (!allowGizmos && this.meta.isGizmos)
				{
					return false;
				}
				if (this.meta.cameraTargets != null)
				{
					return this.meta.cameraTargets.Contains(camera);
				}
				return allowCameraDefault;
			}

			// Token: 0x06000301 RID: 769 RVA: 0x0000B2AA File Offset: 0x000094AA
			public unsafe void Schedule(DrawingData gizmos, ref GeometryBuilder.CameraInfo cameraInfo)
			{
				if (this.type != DrawingData.ProcessedBuilderData.Type.Static)
				{
					this.buildJob = GeometryBuilder.Build(gizmos, (DrawingData.ProcessedBuilderData.MeshBuffers*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<DrawingData.ProcessedBuilderData.MeshBuffers>(this.temporaryMeshBuffers), ref cameraInfo, this.splitterJob);
				}
			}

			// Token: 0x06000302 RID: 770 RVA: 0x0000B2D3 File Offset: 0x000094D3
			public unsafe void BuildMeshes(DrawingData gizmos)
			{
				if (this.type == DrawingData.ProcessedBuilderData.Type.Static && this.submitted)
				{
					return;
				}
				this.buildJob.Complete();
				GeometryBuilder.BuildMesh(gizmos, this.meshes, (DrawingData.ProcessedBuilderData.MeshBuffers*)this.temporaryMeshBuffers.GetUnsafePtr<DrawingData.ProcessedBuilderData.MeshBuffers>());
				this.submitted = true;
			}

			// Token: 0x06000303 RID: 771 RVA: 0x0000B310 File Offset: 0x00009510
			public unsafe void CollectMeshes(List<DrawingData.RenderedMeshWithType> meshes)
			{
				List<DrawingData.MeshWithType> list = this.meshes;
				int num = 0;
				UnsafeAppendBuffer capturedState = this.temporaryMeshBuffers[0].capturedState;
				int num2 = capturedState.Length / UnsafeUtility.SizeOf<DrawingData.ProcessedBuilderData.CapturedState>();
				for (int i = 0; i < list.Count; i++)
				{
					Color color;
					Matrix4x4 matrix;
					int drawingOrderIndex;
					if ((list[i].type & DrawingData.MeshType.Custom) != (DrawingData.MeshType)0)
					{
						DrawingData.ProcessedBuilderData.CapturedState capturedState2 = *(DrawingData.ProcessedBuilderData.CapturedState*)(capturedState.Ptr + (IntPtr)num * (IntPtr)sizeof(DrawingData.ProcessedBuilderData.CapturedState));
						color = capturedState2.color;
						matrix = capturedState2.matrix;
						num++;
						drawingOrderIndex = this.meta.drawOrderIndex + 1;
					}
					else
					{
						color = Color.white;
						matrix = Matrix4x4.identity;
						drawingOrderIndex = this.meta.drawOrderIndex;
					}
					meshes.Add(new DrawingData.RenderedMeshWithType
					{
						mesh = list[i].mesh,
						type = list[i].type,
						drawingOrderIndex = drawingOrderIndex,
						color = color,
						matrix = matrix
					});
				}
			}

			// Token: 0x06000304 RID: 772 RVA: 0x0000B414 File Offset: 0x00009614
			private void PoolMeshes(DrawingData gizmos, bool includeCustom)
			{
				if (!this.isValid)
				{
					throw new InvalidOperationException();
				}
				int num = 0;
				for (int i = 0; i < this.meshes.Count; i++)
				{
					if ((this.meshes[i].type & DrawingData.MeshType.Custom) == (DrawingData.MeshType)0 || (includeCustom && (this.meshes[i].type & DrawingData.MeshType.Pool) != (DrawingData.MeshType)0))
					{
						gizmos.PoolMesh(this.meshes[i].mesh);
					}
					else
					{
						this.meshes[num] = this.meshes[i];
						num++;
					}
				}
				this.meshes.RemoveRange(num, this.meshes.Count - num);
			}

			// Token: 0x06000305 RID: 773 RVA: 0x0000B4C3 File Offset: 0x000096C3
			public void PoolDynamicMeshes(DrawingData gizmos)
			{
				if (this.type == DrawingData.ProcessedBuilderData.Type.Static && this.submitted)
				{
					return;
				}
				this.PoolMeshes(gizmos, false);
			}

			// Token: 0x06000306 RID: 774 RVA: 0x0000B4E0 File Offset: 0x000096E0
			public void Release(DrawingData gizmos)
			{
				if (!this.isValid)
				{
					throw new InvalidOperationException();
				}
				this.PoolMeshes(gizmos, true);
				this.meshes.Clear();
				this.type = DrawingData.ProcessedBuilderData.Type.Invalid;
				this.splitterJob.Complete();
				this.buildJob.Complete();
				DrawingData.ProcessedBuilderData.MeshBuffers value = this.temporaryMeshBuffers[0];
				value.DisposeIfLarge();
				this.temporaryMeshBuffers[0] = value;
			}

			// Token: 0x06000307 RID: 775 RVA: 0x0000B54C File Offset: 0x0000974C
			public void Dispose()
			{
				if (this.isValid)
				{
					throw new InvalidOperationException();
				}
				this.splitterJob.Complete();
				this.buildJob.Complete();
				if (this.temporaryMeshBuffers.IsCreated)
				{
					this.temporaryMeshBuffers[0].Dispose();
					this.temporaryMeshBuffers.Dispose();
				}
			}

			// Token: 0x040000A2 RID: 162
			public DrawingData.ProcessedBuilderData.Type type;

			// Token: 0x040000A3 RID: 163
			public DrawingData.BuilderData.Meta meta;

			// Token: 0x040000A4 RID: 164
			private bool submitted;

			// Token: 0x040000A5 RID: 165
			public NativeArray<DrawingData.ProcessedBuilderData.MeshBuffers> temporaryMeshBuffers;

			// Token: 0x040000A6 RID: 166
			private JobHandle buildJob;

			// Token: 0x040000A7 RID: 167
			private JobHandle splitterJob;

			// Token: 0x040000A8 RID: 168
			public List<DrawingData.MeshWithType> meshes;

			// Token: 0x040000A9 RID: 169
			private static int SubmittedJobs;

			// Token: 0x0200002E RID: 46
			public enum Type
			{
				// Token: 0x040000AB RID: 171
				Invalid,
				// Token: 0x040000AC RID: 172
				Static,
				// Token: 0x040000AD RID: 173
				Dynamic,
				// Token: 0x040000AE RID: 174
				Persistent
			}

			// Token: 0x0200002F RID: 47
			public struct CapturedState
			{
				// Token: 0x040000AF RID: 175
				public Matrix4x4 matrix;

				// Token: 0x040000B0 RID: 176
				public Color color;
			}

			// Token: 0x02000030 RID: 48
			public struct MeshBuffers
			{
				// Token: 0x06000308 RID: 776 RVA: 0x0000B5AC File Offset: 0x000097AC
				public MeshBuffers(Allocator allocator)
				{
					this.splitterOutput = new UnsafeAppendBuffer(0, 4, allocator);
					this.vertices = new UnsafeAppendBuffer(0, 4, allocator);
					this.triangles = new UnsafeAppendBuffer(0, 4, allocator);
					this.solidVertices = new UnsafeAppendBuffer(0, 4, allocator);
					this.solidTriangles = new UnsafeAppendBuffer(0, 4, allocator);
					this.textVertices = new UnsafeAppendBuffer(0, 4, allocator);
					this.textTriangles = new UnsafeAppendBuffer(0, 4, allocator);
					this.capturedState = new UnsafeAppendBuffer(0, 4, allocator);
					this.bounds = default(Bounds);
				}

				// Token: 0x06000309 RID: 777 RVA: 0x0000B660 File Offset: 0x00009860
				public void Dispose()
				{
					this.splitterOutput.Dispose();
					this.vertices.Dispose();
					this.triangles.Dispose();
					this.solidVertices.Dispose();
					this.solidTriangles.Dispose();
					this.textVertices.Dispose();
					this.textTriangles.Dispose();
					this.capturedState.Dispose();
				}

				// Token: 0x0600030A RID: 778 RVA: 0x0000B6C8 File Offset: 0x000098C8
				private static void DisposeIfLarge(ref UnsafeAppendBuffer ls)
				{
					if (ls.Length * 3 < ls.Capacity && ls.Capacity > 1024)
					{
						AllocatorManager.AllocatorHandle allocator = ls.Allocator;
						ls.Dispose();
						ls = new UnsafeAppendBuffer(0, 4, allocator);
					}
				}

				// Token: 0x0600030B RID: 779 RVA: 0x0000B710 File Offset: 0x00009910
				public void DisposeIfLarge()
				{
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.splitterOutput);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.vertices);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.triangles);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.solidVertices);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.solidTriangles);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.textVertices);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.textTriangles);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.capturedState);
				}

				// Token: 0x040000B1 RID: 177
				public UnsafeAppendBuffer splitterOutput;

				// Token: 0x040000B2 RID: 178
				public UnsafeAppendBuffer vertices;

				// Token: 0x040000B3 RID: 179
				public UnsafeAppendBuffer triangles;

				// Token: 0x040000B4 RID: 180
				public UnsafeAppendBuffer solidVertices;

				// Token: 0x040000B5 RID: 181
				public UnsafeAppendBuffer solidTriangles;

				// Token: 0x040000B6 RID: 182
				public UnsafeAppendBuffer textVertices;

				// Token: 0x040000B7 RID: 183
				public UnsafeAppendBuffer textTriangles;

				// Token: 0x040000B8 RID: 184
				public UnsafeAppendBuffer capturedState;

				// Token: 0x040000B9 RID: 185
				public Bounds bounds;
			}
		}

		// Token: 0x02000031 RID: 49
		internal struct SubmittedMesh
		{
			// Token: 0x040000BA RID: 186
			public Mesh mesh;

			// Token: 0x040000BB RID: 187
			public bool temporary;
		}

		// Token: 0x02000032 RID: 50
		[BurstCompile]
		internal struct BuilderData : IDisposable
		{
			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600030C RID: 780 RVA: 0x0000B775 File Offset: 0x00009975
			// (set) Token: 0x0600030D RID: 781 RVA: 0x0000B77D File Offset: 0x0000997D
			public DrawingData.BuilderData.State state { readonly get; private set; }

			// Token: 0x0600030E RID: 782 RVA: 0x0000B786 File Offset: 0x00009986
			public void Reserve(int dataIndex, bool isBuiltInCommandBuilder)
			{
				if (this.state != DrawingData.BuilderData.State.Free)
				{
					throw new InvalidOperationException();
				}
				this.state = DrawingData.BuilderData.State.Reserved;
				this.packedMeta = new DrawingData.BuilderData.BitPackedMeta(dataIndex, DrawingData.BuilderData.UniqueIDCounter++ & 32767, isBuiltInCommandBuilder);
			}

			// Token: 0x0600030F RID: 783 RVA: 0x0000B7C0 File Offset: 0x000099C0
			public void Init(DrawingData.Hasher hasher, RedrawScope frameRedrawScope, RedrawScope customRedrawScope, bool isGizmos, int drawOrderIndex, int sceneModeVersion)
			{
				if (this.state != DrawingData.BuilderData.State.Reserved)
				{
					throw new InvalidOperationException();
				}
				this.meta = new DrawingData.BuilderData.Meta
				{
					hasher = hasher,
					redrawScope1 = frameRedrawScope,
					redrawScope2 = customRedrawScope,
					isGizmos = isGizmos,
					version = 0,
					drawOrderIndex = drawOrderIndex,
					sceneModeVersion = sceneModeVersion,
					cameraTargets = null
				};
				if (this.meshes == null)
				{
					this.meshes = new List<DrawingData.SubmittedMesh>();
				}
				if (!this.commandBuffers.IsCreated)
				{
					this.commandBuffers = new NativeArray<UnsafeAppendBuffer>(JobsUtility.ThreadIndexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					for (int i = 0; i < this.commandBuffers.Length; i++)
					{
						this.commandBuffers[i] = new UnsafeAppendBuffer(0, 4, Allocator.Persistent);
					}
				}
				this.state = DrawingData.BuilderData.State.Initialized;
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000310 RID: 784 RVA: 0x0000B896 File Offset: 0x00009A96
			public unsafe UnsafeAppendBuffer* bufferPtr
			{
				get
				{
					return (UnsafeAppendBuffer*)this.commandBuffers.GetUnsafePtr<UnsafeAppendBuffer>();
				}
			}

			// Token: 0x06000311 RID: 785 RVA: 0x0000B8A3 File Offset: 0x00009AA3
			[BurstCompile]
			[MonoPInvokeCallback(typeof(DrawingData.BuilderData.AnyBuffersWrittenToDelegate))]
			private unsafe static bool AnyBuffersWrittenTo(UnsafeAppendBuffer* buffers, int numBuffers)
			{
				return DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.Invoke(buffers, numBuffers);
			}

			// Token: 0x06000312 RID: 786 RVA: 0x0000B8AC File Offset: 0x00009AAC
			[BurstCompile]
			[MonoPInvokeCallback(typeof(DrawingData.BuilderData.AnyBuffersWrittenToDelegate))]
			private unsafe static void ResetAllBuffers(UnsafeAppendBuffer* buffers, int numBuffers)
			{
				DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.Invoke(buffers, numBuffers);
			}

			// Token: 0x06000313 RID: 787 RVA: 0x0000B8B5 File Offset: 0x00009AB5
			public void SubmitWithDependency(GCHandle gcHandle, JobHandle dependency, AllowedDelay allowedDelay)
			{
				this.state = DrawingData.BuilderData.State.WaitingForUserDefinedJob;
				this.disposeDependency = dependency;
				this.disposeDependencyDelay = allowedDelay;
				this.disposeGCHandle = gcHandle;
			}

			// Token: 0x06000314 RID: 788 RVA: 0x0000B8D4 File Offset: 0x00009AD4
			public unsafe void Submit(DrawingData gizmos)
			{
				if (this.state != DrawingData.BuilderData.State.Initialized)
				{
					throw new InvalidOperationException();
				}
				if (this.meshes.Count == 0 && !DrawingData.BuilderData.AnyBuffersWrittenToInvoke((UnsafeAppendBuffer*)this.commandBuffers.GetUnsafeReadOnlyPtr<UnsafeAppendBuffer>(), this.commandBuffers.Length))
				{
					this.Release();
					return;
				}
				this.meta.version = gizmos.version;
				DrawingData.BuilderData.Meta meta = this.meta;
				meta.drawOrderIndex = this.meta.drawOrderIndex * 3;
				int index = gizmos.processedData.Reserve(DrawingData.ProcessedBuilderData.Type.Static, meta);
				meta.drawOrderIndex = this.meta.drawOrderIndex * 3 + 1;
				int index2 = gizmos.processedData.Reserve(DrawingData.ProcessedBuilderData.Type.Dynamic, meta);
				meta.drawOrderIndex = this.meta.drawOrderIndex + 1000000;
				int index3 = gizmos.processedData.Reserve(DrawingData.ProcessedBuilderData.Type.Persistent, meta);
				this.splitterJob = new StreamSplitter
				{
					inputBuffers = this.commandBuffers,
					staticBuffer = gizmos.processedData.Get(index).splitterOutputPtr,
					dynamicBuffer = gizmos.processedData.Get(index2).splitterOutputPtr,
					persistentBuffer = gizmos.processedData.Get(index3).splitterOutputPtr
				}.Schedule(default(JobHandle));
				gizmos.processedData.Get(index).SetSplitterJob(gizmos, this.splitterJob);
				gizmos.processedData.Get(index2).SetSplitterJob(gizmos, this.splitterJob);
				gizmos.processedData.Get(index3).SetSplitterJob(gizmos, this.splitterJob);
				if (this.meshes.Count > 0)
				{
					List<DrawingData.MeshWithType> list = gizmos.processedData.Get(index2).meshes;
					for (int i = 0; i < this.meshes.Count; i++)
					{
						list.Add(new DrawingData.MeshWithType
						{
							mesh = this.meshes[i].mesh,
							type = (DrawingData.MeshType.Solid | DrawingData.MeshType.Custom | (this.meshes[i].temporary ? DrawingData.MeshType.Pool : ((DrawingData.MeshType)0)))
						});
					}
					this.meshes.Clear();
				}
				this.state = DrawingData.BuilderData.State.WaitingForSplitter;
			}

			// Token: 0x06000315 RID: 789 RVA: 0x0000BB04 File Offset: 0x00009D04
			public void CheckJobDependency(DrawingData gizmos, bool allowBlocking)
			{
				if (this.state == DrawingData.BuilderData.State.WaitingForUserDefinedJob && (this.disposeDependency.IsCompleted || (allowBlocking && this.disposeDependencyDelay == AllowedDelay.EndOfFrame)))
				{
					this.disposeDependency.Complete();
					this.disposeDependency = default(JobHandle);
					this.disposeGCHandle.Free();
					this.state = DrawingData.BuilderData.State.Initialized;
					this.Submit(gizmos);
				}
			}

			// Token: 0x06000316 RID: 790 RVA: 0x0000BB62 File Offset: 0x00009D62
			public void Release()
			{
				if (this.state == DrawingData.BuilderData.State.Free)
				{
					throw new InvalidOperationException();
				}
				this.state = DrawingData.BuilderData.State.Free;
				this.ClearData();
			}

			// Token: 0x06000317 RID: 791 RVA: 0x0000BB80 File Offset: 0x00009D80
			private unsafe void ClearData()
			{
				this.disposeDependency.Complete();
				this.splitterJob.Complete();
				this.meta = default(DrawingData.BuilderData.Meta);
				this.disposeDependency = default(JobHandle);
				this.preventDispose = false;
				this.meshes.Clear();
				DrawingData.BuilderData.ResetAllBuffers((UnsafeAppendBuffer*)this.commandBuffers.GetUnsafePtr<UnsafeAppendBuffer>(), this.commandBuffers.Length);
			}

			// Token: 0x06000318 RID: 792 RVA: 0x0000BBE8 File Offset: 0x00009DE8
			public void Dispose()
			{
				if (this.state == DrawingData.BuilderData.State.WaitingForUserDefinedJob)
				{
					this.disposeDependency.Complete();
					this.disposeGCHandle.Free();
					this.state = DrawingData.BuilderData.State.WaitingForSplitter;
				}
				if (this.state == DrawingData.BuilderData.State.Reserved || this.state == DrawingData.BuilderData.State.Initialized || this.state == DrawingData.BuilderData.State.WaitingForUserDefinedJob)
				{
					Debug.LogError("Drawing data is being destroyed, but a drawing instance is still active. Are you sure you have called Dispose on all drawing instances? This will cause a memory leak!");
					return;
				}
				this.splitterJob.Complete();
				if (this.commandBuffers.IsCreated)
				{
					for (int i = 0; i < this.commandBuffers.Length; i++)
					{
						this.commandBuffers[i].Dispose();
					}
					this.commandBuffers.Dispose();
				}
			}

			// Token: 0x0600031A RID: 794 RVA: 0x0000BCE0 File Offset: 0x00009EE0
			[BurstCompile]
			[MonoPInvokeCallback(typeof(DrawingData.BuilderData.AnyBuffersWrittenToDelegate))]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static bool AnyBuffersWrittenTo$BurstManaged(UnsafeAppendBuffer* buffers, int numBuffers)
			{
				bool flag = false;
				for (int i = 0; i < numBuffers; i++)
				{
					flag |= (buffers[i].Length > 0);
				}
				return flag;
			}

			// Token: 0x0600031B RID: 795 RVA: 0x0000BD14 File Offset: 0x00009F14
			[BurstCompile]
			[MonoPInvokeCallback(typeof(DrawingData.BuilderData.AnyBuffersWrittenToDelegate))]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void ResetAllBuffers$BurstManaged(UnsafeAppendBuffer* buffers, int numBuffers)
			{
				for (int i = 0; i < numBuffers; i++)
				{
					buffers[i].Reset();
				}
			}

			// Token: 0x040000BC RID: 188
			public DrawingData.BuilderData.BitPackedMeta packedMeta;

			// Token: 0x040000BD RID: 189
			public List<DrawingData.SubmittedMesh> meshes;

			// Token: 0x040000BE RID: 190
			public NativeArray<UnsafeAppendBuffer> commandBuffers;

			// Token: 0x040000C0 RID: 192
			public bool preventDispose;

			// Token: 0x040000C1 RID: 193
			private JobHandle splitterJob;

			// Token: 0x040000C2 RID: 194
			private JobHandle disposeDependency;

			// Token: 0x040000C3 RID: 195
			private AllowedDelay disposeDependencyDelay;

			// Token: 0x040000C4 RID: 196
			private GCHandle disposeGCHandle;

			// Token: 0x040000C5 RID: 197
			public DrawingData.BuilderData.Meta meta;

			// Token: 0x040000C6 RID: 198
			private static int UniqueIDCounter = 0;

			// Token: 0x040000C7 RID: 199
			private static readonly DrawingData.BuilderData.AnyBuffersWrittenToDelegate AnyBuffersWrittenToInvoke = BurstCompiler.CompileFunctionPointer<DrawingData.BuilderData.AnyBuffersWrittenToDelegate>(new DrawingData.BuilderData.AnyBuffersWrittenToDelegate(DrawingData.BuilderData.AnyBuffersWrittenTo)).Invoke;

			// Token: 0x040000C8 RID: 200
			private static readonly DrawingData.BuilderData.ResetAllBuffersToDelegate ResetAllBuffersToInvoke = BurstCompiler.CompileFunctionPointer<DrawingData.BuilderData.ResetAllBuffersToDelegate>(new DrawingData.BuilderData.ResetAllBuffersToDelegate(DrawingData.BuilderData.ResetAllBuffers)).Invoke;

			// Token: 0x02000033 RID: 51
			public enum State
			{
				// Token: 0x040000CA RID: 202
				Free,
				// Token: 0x040000CB RID: 203
				Reserved,
				// Token: 0x040000CC RID: 204
				Initialized,
				// Token: 0x040000CD RID: 205
				WaitingForSplitter,
				// Token: 0x040000CE RID: 206
				WaitingForUserDefinedJob
			}

			// Token: 0x02000034 RID: 52
			public struct Meta
			{
				// Token: 0x040000CF RID: 207
				public DrawingData.Hasher hasher;

				// Token: 0x040000D0 RID: 208
				public RedrawScope redrawScope1;

				// Token: 0x040000D1 RID: 209
				public RedrawScope redrawScope2;

				// Token: 0x040000D2 RID: 210
				public int version;

				// Token: 0x040000D3 RID: 211
				public bool isGizmos;

				// Token: 0x040000D4 RID: 212
				public int sceneModeVersion;

				// Token: 0x040000D5 RID: 213
				public int drawOrderIndex;

				// Token: 0x040000D6 RID: 214
				public Camera[] cameraTargets;
			}

			// Token: 0x02000035 RID: 53
			public struct BitPackedMeta
			{
				// Token: 0x0600031C RID: 796 RVA: 0x0000BD3D File Offset: 0x00009F3D
				public BitPackedMeta(int dataIndex, int uniqueID, bool isBuiltInCommandBuilder)
				{
					if (dataIndex > 65535)
					{
						throw new Exception("Too many command builders active. Are some command builders not being disposed?");
					}
					this.flags = (uint)(dataIndex | uniqueID << 17 | (isBuiltInCommandBuilder ? 65536 : 0));
				}

				// Token: 0x17000014 RID: 20
				// (get) Token: 0x0600031D RID: 797 RVA: 0x0000BD6A File Offset: 0x00009F6A
				public int dataIndex
				{
					get
					{
						return (int)(this.flags & 65535U);
					}
				}

				// Token: 0x17000015 RID: 21
				// (get) Token: 0x0600031E RID: 798 RVA: 0x0000BD78 File Offset: 0x00009F78
				public int uniqueID
				{
					get
					{
						return (int)(this.flags >> 17);
					}
				}

				// Token: 0x17000016 RID: 22
				// (get) Token: 0x0600031F RID: 799 RVA: 0x0000BD83 File Offset: 0x00009F83
				public bool isBuiltInCommandBuilder
				{
					get
					{
						return (this.flags & 65536U) > 0U;
					}
				}

				// Token: 0x06000320 RID: 800 RVA: 0x0000BD94 File Offset: 0x00009F94
				public static bool operator ==(DrawingData.BuilderData.BitPackedMeta lhs, DrawingData.BuilderData.BitPackedMeta rhs)
				{
					return lhs.flags == rhs.flags;
				}

				// Token: 0x06000321 RID: 801 RVA: 0x0000BDA4 File Offset: 0x00009FA4
				public static bool operator !=(DrawingData.BuilderData.BitPackedMeta lhs, DrawingData.BuilderData.BitPackedMeta rhs)
				{
					return lhs.flags != rhs.flags;
				}

				// Token: 0x06000322 RID: 802 RVA: 0x0000BDB8 File Offset: 0x00009FB8
				public override bool Equals(object obj)
				{
					if (obj is DrawingData.BuilderData.BitPackedMeta)
					{
						DrawingData.BuilderData.BitPackedMeta bitPackedMeta = (DrawingData.BuilderData.BitPackedMeta)obj;
						return this.flags == bitPackedMeta.flags;
					}
					return false;
				}

				// Token: 0x06000323 RID: 803 RVA: 0x0000BDE4 File Offset: 0x00009FE4
				public override int GetHashCode()
				{
					return (int)this.flags;
				}

				// Token: 0x040000D7 RID: 215
				private uint flags;

				// Token: 0x040000D8 RID: 216
				private const int UniqueIDBitshift = 17;

				// Token: 0x040000D9 RID: 217
				private const int IsBuiltInFlagIndex = 16;

				// Token: 0x040000DA RID: 218
				private const int IndexMask = 65535;

				// Token: 0x040000DB RID: 219
				private const int MaxDataIndex = 65535;

				// Token: 0x040000DC RID: 220
				public const int UniqueIdMask = 32767;
			}

			// Token: 0x02000036 RID: 54
			// (Invoke) Token: 0x06000325 RID: 805
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private unsafe delegate bool AnyBuffersWrittenToDelegate(UnsafeAppendBuffer* buffers, int numBuffers);

			// Token: 0x02000037 RID: 55
			// (Invoke) Token: 0x06000329 RID: 809
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private unsafe delegate void ResetAllBuffersToDelegate(UnsafeAppendBuffer* buffers, int numBuffers);

			// Token: 0x02000038 RID: 56
			// (Invoke) Token: 0x0600032D RID: 813
			internal unsafe delegate bool AnyBuffersWrittenTo_000002F7$PostfixBurstDelegate(UnsafeAppendBuffer* buffers, int numBuffers);

			// Token: 0x02000039 RID: 57
			internal static class AnyBuffersWrittenTo_000002F7$BurstDirectCall
			{
				// Token: 0x06000330 RID: 816 RVA: 0x0000BDEC File Offset: 0x00009FEC
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.Pointer == 0)
					{
						DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.DeferredCompilation, methodof(DrawingData.BuilderData.AnyBuffersWrittenTo$BurstManaged(UnsafeAppendBuffer*, int)).MethodHandle, typeof(DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.Pointer;
				}

				// Token: 0x06000331 RID: 817 RVA: 0x0000BE18 File Offset: 0x0000A018
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x06000332 RID: 818 RVA: 0x0000BE30 File Offset: 0x0000A030
				public unsafe static void Constructor()
				{
					DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(DrawingData.BuilderData.AnyBuffersWrittenTo(UnsafeAppendBuffer*, int)).MethodHandle);
				}

				// Token: 0x06000333 RID: 819 RVA: 0x00002104 File Offset: 0x00000304
				public static void Initialize()
				{
				}

				// Token: 0x06000334 RID: 820 RVA: 0x0000BE41 File Offset: 0x0000A041
				// Note: this type is marked as 'beforefieldinit'.
				static AnyBuffersWrittenTo_000002F7$BurstDirectCall()
				{
					DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.Constructor();
				}

				// Token: 0x06000335 RID: 821 RVA: 0x0000BE48 File Offset: 0x0000A048
				public unsafe static bool Invoke(UnsafeAppendBuffer* buffers, int numBuffers)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = DrawingData.BuilderData.AnyBuffersWrittenTo_000002F7$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Boolean(Unity.Collections.LowLevel.Unsafe.UnsafeAppendBuffer*,System.Int32), buffers, numBuffers, functionPointer);
						}
					}
					return DrawingData.BuilderData.AnyBuffersWrittenTo$BurstManaged(buffers, numBuffers);
				}

				// Token: 0x040000DD RID: 221
				private static IntPtr Pointer;

				// Token: 0x040000DE RID: 222
				private static IntPtr DeferredCompilation;
			}

			// Token: 0x0200003A RID: 58
			// (Invoke) Token: 0x06000337 RID: 823
			internal unsafe delegate void ResetAllBuffers_000002F8$PostfixBurstDelegate(UnsafeAppendBuffer* buffers, int numBuffers);

			// Token: 0x0200003B RID: 59
			internal static class ResetAllBuffers_000002F8$BurstDirectCall
			{
				// Token: 0x0600033A RID: 826 RVA: 0x0000BE7B File Offset: 0x0000A07B
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.Pointer == 0)
					{
						DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.DeferredCompilation, methodof(DrawingData.BuilderData.ResetAllBuffers$BurstManaged(UnsafeAppendBuffer*, int)).MethodHandle, typeof(DrawingData.BuilderData.ResetAllBuffers_000002F8$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.Pointer;
				}

				// Token: 0x0600033B RID: 827 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x0600033C RID: 828 RVA: 0x0000BEC0 File Offset: 0x0000A0C0
				public unsafe static void Constructor()
				{
					DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(DrawingData.BuilderData.ResetAllBuffers(UnsafeAppendBuffer*, int)).MethodHandle);
				}

				// Token: 0x0600033D RID: 829 RVA: 0x00002104 File Offset: 0x00000304
				public static void Initialize()
				{
				}

				// Token: 0x0600033E RID: 830 RVA: 0x0000BED1 File Offset: 0x0000A0D1
				// Note: this type is marked as 'beforefieldinit'.
				static ResetAllBuffers_000002F8$BurstDirectCall()
				{
					DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.Constructor();
				}

				// Token: 0x0600033F RID: 831 RVA: 0x0000BED8 File Offset: 0x0000A0D8
				public unsafe static void Invoke(UnsafeAppendBuffer* buffers, int numBuffers)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = DrawingData.BuilderData.ResetAllBuffers_000002F8$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Unity.Collections.LowLevel.Unsafe.UnsafeAppendBuffer*,System.Int32), buffers, numBuffers, functionPointer);
							return;
						}
					}
					DrawingData.BuilderData.ResetAllBuffers$BurstManaged(buffers, numBuffers);
				}

				// Token: 0x040000DF RID: 223
				private static IntPtr Pointer;

				// Token: 0x040000E0 RID: 224
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x0200003C RID: 60
		internal struct BuilderDataContainer : IDisposable
		{
			// Token: 0x17000017 RID: 23
			// (get) Token: 0x06000340 RID: 832 RVA: 0x0000BF0C File Offset: 0x0000A10C
			public int memoryUsage
			{
				get
				{
					int num = 0;
					if (this.data != null)
					{
						for (int i = 0; i < this.data.Length; i++)
						{
							NativeArray<UnsafeAppendBuffer> commandBuffers = this.data[i].commandBuffers;
							for (int j = 0; j < commandBuffers.Length; j++)
							{
								num += commandBuffers[j].Capacity;
							}
							num += this.data[i].commandBuffers.Length * sizeof(UnsafeAppendBuffer);
						}
					}
					return num;
				}
			}

			// Token: 0x06000341 RID: 833 RVA: 0x0000BF8C File Offset: 0x0000A18C
			public DrawingData.BuilderData.BitPackedMeta Reserve(bool isBuiltInCommandBuilder)
			{
				if (this.data == null)
				{
					this.data = new DrawingData.BuilderData[1];
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].state == DrawingData.BuilderData.State.Free)
					{
						this.data[i].Reserve(i, isBuiltInCommandBuilder);
						return this.data[i].packedMeta;
					}
				}
				DrawingData.BuilderData[] array = new DrawingData.BuilderData[this.data.Length * 2];
				this.data.CopyTo(array, 0);
				this.data = array;
				return this.Reserve(isBuiltInCommandBuilder);
			}

			// Token: 0x06000342 RID: 834 RVA: 0x0000C023 File Offset: 0x0000A223
			public void Release(DrawingData.BuilderData.BitPackedMeta meta)
			{
				this.data[meta.dataIndex].Release();
			}

			// Token: 0x06000343 RID: 835 RVA: 0x0000C03C File Offset: 0x0000A23C
			public bool StillExists(DrawingData.BuilderData.BitPackedMeta meta)
			{
				int dataIndex = meta.dataIndex;
				return this.data != null && dataIndex < this.data.Length && this.data[dataIndex].packedMeta == meta;
			}

			// Token: 0x06000344 RID: 836 RVA: 0x0000C080 File Offset: 0x0000A280
			public ref DrawingData.BuilderData Get(DrawingData.BuilderData.BitPackedMeta meta)
			{
				int dataIndex = meta.dataIndex;
				if (this.data[dataIndex].state == DrawingData.BuilderData.State.Free)
				{
					throw new ArgumentException("Data is not reserved");
				}
				if (this.data[dataIndex].packedMeta != meta)
				{
					throw new ArgumentException("This command builder has already been disposed");
				}
				return ref this.data[dataIndex];
			}

			// Token: 0x06000345 RID: 837 RVA: 0x0000C0E4 File Offset: 0x0000A2E4
			public void DisposeCommandBuildersWithJobDependencies(DrawingData gizmos)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					this.data[i].CheckJobDependency(gizmos, false);
				}
				for (int j = 0; j < this.data.Length; j++)
				{
					this.data[j].CheckJobDependency(gizmos, true);
				}
			}

			// Token: 0x06000346 RID: 838 RVA: 0x0000C148 File Offset: 0x0000A348
			public void ReleaseAllUnused()
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].state == DrawingData.BuilderData.State.WaitingForSplitter)
					{
						this.data[i].Release();
					}
				}
			}

			// Token: 0x06000347 RID: 839 RVA: 0x0000C198 File Offset: 0x0000A398
			public void Dispose()
			{
				if (this.data != null)
				{
					for (int i = 0; i < this.data.Length; i++)
					{
						this.data[i].Dispose();
					}
				}
				this.data = null;
			}

			// Token: 0x040000E1 RID: 225
			private DrawingData.BuilderData[] data;
		}

		// Token: 0x0200003D RID: 61
		internal struct ProcessedBuilderDataContainer
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x06000348 RID: 840 RVA: 0x0000C1D8 File Offset: 0x0000A3D8
			public int memoryUsage
			{
				get
				{
					int num = 0;
					if (this.data != null)
					{
						for (int i = 0; i < this.data.Length; i++)
						{
							NativeArray<DrawingData.ProcessedBuilderData.MeshBuffers> temporaryMeshBuffers = this.data[i].temporaryMeshBuffers;
							for (int j = 0; j < temporaryMeshBuffers.Length; j++)
							{
								int num2 = 0;
								num2 += temporaryMeshBuffers[j].textVertices.Capacity;
								num2 += temporaryMeshBuffers[j].textTriangles.Capacity;
								num2 += temporaryMeshBuffers[j].solidVertices.Capacity;
								num2 += temporaryMeshBuffers[j].solidTriangles.Capacity;
								num2 += temporaryMeshBuffers[j].vertices.Capacity;
								num2 += temporaryMeshBuffers[j].triangles.Capacity;
								num2 += temporaryMeshBuffers[j].capturedState.Capacity;
								num2 += temporaryMeshBuffers[j].splitterOutput.Capacity;
								num += num2;
								Debug.Log(string.Concat(new string[]
								{
									i.ToString(),
									":",
									j.ToString(),
									" ",
									num2.ToString()
								}));
							}
						}
					}
					return num;
				}
			}

			// Token: 0x06000349 RID: 841 RVA: 0x0000C334 File Offset: 0x0000A534
			public int Reserve(DrawingData.ProcessedBuilderData.Type type, DrawingData.BuilderData.Meta meta)
			{
				if (this.data == null)
				{
					this.data = new DrawingData.ProcessedBuilderData[0];
					this.freeSlots = new Stack<int>();
					this.freeLists = new Stack<List<int>>();
					this.hash2index = new Dictionary<ulong, List<int>>();
				}
				if (this.freeSlots.Count == 0)
				{
					DrawingData.ProcessedBuilderData[] array = new DrawingData.ProcessedBuilderData[math.max(4, this.data.Length * 2)];
					this.data.CopyTo(array, 0);
					for (int i = this.data.Length; i < array.Length; i++)
					{
						this.freeSlots.Push(i);
					}
					this.data = array;
				}
				int num = this.freeSlots.Pop();
				this.data[num].Init(type, meta);
				if (!meta.hasher.Equals(DrawingData.Hasher.NotSupplied))
				{
					List<int> list;
					if (!this.hash2index.TryGetValue(meta.hasher.Hash, out list))
					{
						if (this.freeLists.Count == 0)
						{
							this.freeLists.Push(new List<int>());
						}
						list = (this.hash2index[meta.hasher.Hash] = this.freeLists.Pop());
					}
					list.Add(num);
				}
				return num;
			}

			// Token: 0x0600034A RID: 842 RVA: 0x0000C468 File Offset: 0x0000A668
			public ref DrawingData.ProcessedBuilderData Get(int index)
			{
				if (!this.data[index].isValid)
				{
					throw new ArgumentException();
				}
				return ref this.data[index];
			}

			// Token: 0x0600034B RID: 843 RVA: 0x0000C490 File Offset: 0x0000A690
			private void Release(DrawingData gizmos, int i)
			{
				ulong hash = this.data[i].meta.hasher.Hash;
				List<int> list;
				if (!this.data[i].meta.hasher.Equals(DrawingData.Hasher.NotSupplied) && this.hash2index.TryGetValue(hash, out list))
				{
					list.Remove(i);
					if (list.Count == 0)
					{
						this.freeLists.Push(list);
						this.hash2index.Remove(hash);
					}
				}
				this.data[i].Release(gizmos);
				this.freeSlots.Push(i);
			}

			// Token: 0x0600034C RID: 844 RVA: 0x0000C534 File Offset: 0x0000A734
			public void SubmitMeshes(DrawingData gizmos, Camera camera, int versionThreshold, bool allowGizmos, bool allowCameraDefault)
			{
				if (this.data == null)
				{
					return;
				}
				GeometryBuilder.CameraInfo cameraInfo = new GeometryBuilder.CameraInfo(camera);
				int num = 0;
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].meta.version >= versionThreshold && this.data[i].IsValidForCamera(camera, allowGizmos, allowCameraDefault))
					{
						num++;
						this.data[i].Schedule(gizmos, ref cameraInfo);
					}
				}
				JobHandle.ScheduleBatchedJobs();
				for (int j = 0; j < this.data.Length; j++)
				{
					if (this.data[j].isValid && this.data[j].meta.version >= versionThreshold && this.data[j].IsValidForCamera(camera, allowGizmos, allowCameraDefault))
					{
						this.data[j].BuildMeshes(gizmos);
					}
				}
			}

			// Token: 0x0600034D RID: 845 RVA: 0x0000C634 File Offset: 0x0000A834
			public void PoolDynamicMeshes(DrawingData gizmos)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid)
					{
						this.data[i].PoolDynamicMeshes(gizmos);
					}
				}
			}

			// Token: 0x0600034E RID: 846 RVA: 0x0000C684 File Offset: 0x0000A884
			public void CollectMeshes(int versionThreshold, List<DrawingData.RenderedMeshWithType> meshes, Camera camera, bool allowGizmos, bool allowCameraDefault)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].meta.version >= versionThreshold && this.data[i].IsValidForCamera(camera, allowGizmos, allowCameraDefault))
					{
						this.data[i].CollectMeshes(meshes);
					}
				}
			}

			// Token: 0x0600034F RID: 847 RVA: 0x0000C704 File Offset: 0x0000A904
			public void FilterOldPersistentCommands(int version, int lastTickVersion, float time, int sceneModeVersion)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].type == DrawingData.ProcessedBuilderData.Type.Persistent)
					{
						this.data[i].SchedulePersistFilter(version, lastTickVersion, time, sceneModeVersion);
					}
				}
			}

			// Token: 0x06000350 RID: 848 RVA: 0x0000C76C File Offset: 0x0000A96C
			public bool SetVersion(DrawingData.Hasher hasher, int version)
			{
				if (this.data == null)
				{
					return false;
				}
				List<int> list;
				if (this.hash2index.TryGetValue(hasher.Hash, out list))
				{
					for (int i = 0; i < list.Count; i++)
					{
						int num = list[i];
						this.data[num].meta.version = version;
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000351 RID: 849 RVA: 0x0000C7CC File Offset: 0x0000A9CC
			public bool SetVersion(RedrawScope scope, int version)
			{
				if (this.data == null)
				{
					return false;
				}
				bool result = false;
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && (this.data[i].meta.redrawScope1.id == scope.id || this.data[i].meta.redrawScope2.id == scope.id))
					{
						this.data[i].meta.version = version;
						result = true;
					}
				}
				return result;
			}

			// Token: 0x06000352 RID: 850 RVA: 0x0000C870 File Offset: 0x0000AA70
			public bool SetCustomScope(DrawingData.Hasher hasher, RedrawScope scope)
			{
				if (this.data == null)
				{
					return false;
				}
				List<int> list;
				if (this.hash2index.TryGetValue(hasher.Hash, out list))
				{
					for (int i = 0; i < list.Count; i++)
					{
						int num = list[i];
						this.data[num].meta.redrawScope2 = scope;
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000353 RID: 851 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
			public void ReleaseDataOlderThan(DrawingData gizmos, int version)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].meta.version < version)
					{
						this.Release(gizmos, i);
					}
				}
			}

			// Token: 0x06000354 RID: 852 RVA: 0x0000C930 File Offset: 0x0000AB30
			public void ReleaseAllWithHash(DrawingData gizmos, DrawingData.Hasher hasher)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].meta.hasher.Hash == hasher.Hash)
					{
						this.Release(gizmos, i);
					}
				}
			}

			// Token: 0x06000355 RID: 853 RVA: 0x0000C998 File Offset: 0x0000AB98
			public void Dispose(DrawingData gizmos)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid)
					{
						this.Release(gizmos, i);
					}
					this.data[i].Dispose();
				}
				this.data = null;
			}

			// Token: 0x040000E2 RID: 226
			private DrawingData.ProcessedBuilderData[] data;

			// Token: 0x040000E3 RID: 227
			private Dictionary<ulong, List<int>> hash2index;

			// Token: 0x040000E4 RID: 228
			private Stack<int> freeSlots;

			// Token: 0x040000E5 RID: 229
			private Stack<List<int>> freeLists;
		}

		// Token: 0x0200003E RID: 62
		[Flags]
		internal enum MeshType
		{
			// Token: 0x040000E7 RID: 231
			Solid = 1,
			// Token: 0x040000E8 RID: 232
			Lines = 2,
			// Token: 0x040000E9 RID: 233
			Text = 4,
			// Token: 0x040000EA RID: 234
			Custom = 8,
			// Token: 0x040000EB RID: 235
			Pool = 16,
			// Token: 0x040000EC RID: 236
			BaseType = 7
		}

		// Token: 0x0200003F RID: 63
		internal struct MeshWithType
		{
			// Token: 0x040000ED RID: 237
			public Mesh mesh;

			// Token: 0x040000EE RID: 238
			public DrawingData.MeshType type;
		}

		// Token: 0x02000040 RID: 64
		internal struct RenderedMeshWithType
		{
			// Token: 0x040000EF RID: 239
			public Mesh mesh;

			// Token: 0x040000F0 RID: 240
			public DrawingData.MeshType type;

			// Token: 0x040000F1 RID: 241
			public int drawingOrderIndex;

			// Token: 0x040000F2 RID: 242
			public Color color;

			// Token: 0x040000F3 RID: 243
			public Matrix4x4 matrix;
		}

		// Token: 0x02000041 RID: 65
		private struct Range
		{
			// Token: 0x040000F4 RID: 244
			public int start;

			// Token: 0x040000F5 RID: 245
			public int end;
		}

		// Token: 0x02000042 RID: 66
		private class MeshCompareByDrawingOrder : IComparer<DrawingData.RenderedMeshWithType>
		{
			// Token: 0x06000356 RID: 854 RVA: 0x0000C9F4 File Offset: 0x0000ABF4
			public int Compare(DrawingData.RenderedMeshWithType a, DrawingData.RenderedMeshWithType b)
			{
				int num = (int)(a.type & DrawingData.MeshType.BaseType);
				int num2 = (int)(b.type & DrawingData.MeshType.BaseType);
				if (num == num2)
				{
					return a.drawingOrderIndex - b.drawingOrderIndex;
				}
				return num - num2;
			}
		}

		// Token: 0x02000043 RID: 67
		public struct CommandBufferWrapper
		{
			// Token: 0x06000358 RID: 856 RVA: 0x0000CA28 File Offset: 0x0000AC28
			public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties)
			{
				if (this.cmd != null)
				{
					this.cmd.DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, properties);
				}
			}

			// Token: 0x040000F6 RID: 246
			public CommandBuffer cmd;
		}
	}
}
