using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class TgsTerrainLayer
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00002E58 File Offset: 0x00001058
		internal TgsTerrainLayer()
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002EAC File Offset: 0x000010AC
		public void CheckForSettingsChange()
		{
			Hash128 hash = default(Hash128);
			hash.Append(this.targetTerrainLayer);
			hash.Append<TgsTerrainLayer.TerrainLayerDistribution>(ref this.distribution);
			hash.Append((this.distributionTexture == null) ? 0 : this.distributionTexture.GetInstanceID());
			hash.Append<Vector2>(ref this.scaling);
			hash.Append<Vector2>(ref this.scaling);
			hash.Append<Color>(ref this.colorMask);
			hash.Append<int>(ref this.chunkSize);
			if (this.settings.HasChanged() || this._bakeSettingsHash != hash)
			{
				this.MarkGeometryDirty();
				this._bakeSettingsHash = hash;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002F5A File Offset: 0x0000115A
		public void DrawToUserDetailMapDeferred(Vector3 worldSpaceCoord, float brushRadius, float brushOpacity, float noiseIntensity = 0f, float noiseScale = 1f)
		{
			this._drawPending = true;
			this._drawPointWs = worldSpaceCoord;
			this._drawRadius = brushRadius * 0.5f;
			this._drawOpacity = brushOpacity;
			this._drawNoiseIntensity = noiseIntensity;
			this._drawNoiseScale = noiseScale;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002F8E File Offset: 0x0000118E
		public void FillUserDetailMapDeferred(Texture2D texture, Vector4 channelWeights, bool additiveBlend)
		{
			this._fillPending = true;
			this._fillTexture = ((texture == null) ? Texture2D.blackTexture : texture);
			this._fillChannelWeights = channelWeights;
			this._fillAdditiveBlend = additiveBlend;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002FBC File Offset: 0x000011BC
		public void SetOverlappingChunksDirty(Bounds changedArea)
		{
			foreach (TgsInstance tgsInstance in this._chunks)
			{
				if (tgsInstance.looseBounds.Intersects(changedArea))
				{
					tgsInstance.MarkGeometryDirty();
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003020 File Offset: 0x00001220
		public void MarkGeometryDirty()
		{
			foreach (TgsInstance tgsInstance in this._chunks)
			{
				tgsInstance.MarkGeometryDirty();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003070 File Offset: 0x00001270
		public void MarkMaterialDirty()
		{
			foreach (TgsInstance tgsInstance in this._chunks)
			{
				tgsInstance.MarkMaterialDirty();
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000030C0 File Offset: 0x000012C0
		public void SetWindSettings(TgsWindSettings windSettings)
		{
			foreach (TgsInstance tgsInstance in this._chunks)
			{
				tgsInstance.UsedWindSettings = windSettings;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003114 File Offset: 0x00001314
		public void CheckForChange(Transform transform, Terrain terrain, Texture2D[] terrainDensityMaps, TerrainLayer[] terrainLayers, List<RectInt> changedTexelRegions, int unityLayerMask)
		{
			if (this.settings.preset == null)
			{
				return;
			}
			TerrainData terrainData = terrain.terrainData;
			Vector3 position = transform.position;
			bool flag = false;
			Bounds bounds = default(Bounds);
			Vector2Int v = Vector2Int.zero;
			float num = 0f;
			Bounds terrainWorldSpaceBounds = TgsTerrainLayer.GetTerrainWorldSpaceBounds(terrain, position);
			Vector3 heightmapScale = terrain.terrainData.heightmapScale;
			Vector2Int a = new Vector2Int(terrainData.heightmapTexture.width, terrainData.heightmapTexture.height);
			Vector2Int b = new Vector2Int((int)TgsInstance.CeilingDivisionFloat((float)this.chunkSize, heightmapScale.x), (int)TgsInstance.CeilingDivisionFloat((float)this.chunkSize, heightmapScale.z));
			Vector2Int vector2Int = new Vector2Int(TgsInstance.CeilingDivision(a.x, b.x), TgsInstance.CeilingDivision(a.y, b.y));
			int num2 = vector2Int.x * vector2Int.y;
			if (this.distribution == TgsTerrainLayer.TerrainLayerDistribution.TastyGrassShaderPaintTool && (this._paintedDensityMapRt == null || this._paintedDensityMapRt.width != a.x || this._paintedDensityMapRt.height != a.y))
			{
				if (this._paintedDensityMapRt != null)
				{
					this._paintedDensityMapRt.Release();
				}
				this._paintedDensityMapRt = new RenderTexture(a.x, a.y, 1, RenderTextureFormat.R8)
				{
					enableRandomWrite = true
				};
				this._paintedDensityMapRt.Create();
				Graphics.Blit(Texture2D.blackTexture, this._paintedDensityMapRt);
			}
			this.SetupChunks(num2);
			if (this._drawPending)
			{
				Vector2 b2 = new Vector2(terrainData.size.x, terrainData.size.z);
				Vector2 b3 = new Vector2(position.x, position.z);
				Vector2 a2 = new Vector2(this._drawPointWs.x, this._drawPointWs.z);
				Vector2 b4 = new Vector2((float)this._paintedDensityMapRt.width, (float)this._paintedDensityMapRt.height);
				v = Vector2Int.FloorToInt((a2 - b3) / b2 * b4);
				num = this._drawRadius * terrainData.heightmapScale.x;
				Vector2Int v2 = Vector2Int.FloorToInt(v - new Vector2(num, num) * 0.5f);
				Vector2Int v3 = Vector2Int.CeilToInt(v + new Vector2(num, num) * 0.5f);
				Vector2 vector = v2 / b4 * b2 + b3;
				Vector2 vector2 = v3 / b4 * b2 + b3;
				bounds.SetMinMax(new Vector3(vector.x, this._drawPointWs.y - this._drawRadius, vector.y), new Vector3(vector2.x, this._drawPointWs.y + this._drawRadius, vector2.y));
			}
			for (int i = 0; i < this._chunks.Count; i++)
			{
				TgsInstance tgsInstance = this._chunks[i];
				tgsInstance.Hide = this.hide;
				tgsInstance.UnityLayer = unityLayerMask;
				Vector2Int a3 = new Vector2Int(i / vector2Int.x, i % vector2Int.x);
				Vector2Int minPosition = a3 * b;
				Vector2Int vector2Int2 = (a3 + Vector2Int.one) * b;
				RectInt other = default(RectInt);
				other.SetMinMax(minPosition, vector2Int2);
				vector2Int2 = Vector2Int.Min(a - Vector2Int.one, vector2Int2);
				Vector3 b5 = new Vector3((float)minPosition.x * heightmapScale.x, 0f, (float)minPosition.y * heightmapScale.z);
				Vector3 b6 = new Vector3((float)vector2Int2.x * heightmapScale.x, terrainData.size.y, (float)vector2Int2.y * heightmapScale.z);
				Bounds bounds2 = default(Bounds);
				bounds2.min = terrainWorldSpaceBounds.min + b5;
				bounds2.max = terrainWorldSpaceBounds.min + b6;
				foreach (RectInt rectInt in changedTexelRegions)
				{
					if (rectInt.Overlaps(other))
					{
						tgsInstance.MarkGeometryDirty();
						break;
					}
				}
				if ((this._drawPending || this._fillPending) && tgsInstance.looseBounds.Intersects(bounds))
				{
					tgsInstance.MarkGeometryDirty();
				}
				if (tgsInstance.isGeometryDirty)
				{
					flag = true;
				}
			}
			if (!flag && this._chunks.Count > 0)
			{
				return;
			}
			Texture2D densityTexture = Texture2D.whiteTexture;
			Vector4 zero = Vector4.zero;
			switch (this.distribution)
			{
			case TgsTerrainLayer.TerrainLayerDistribution.Fill:
			case TgsTerrainLayer.TerrainLayerDistribution.ByCustomTexture:
				break;
			case TgsTerrainLayer.TerrainLayerDistribution.TastyGrassShaderPaintTool:
			{
				Graphics.Blit(this.paintedDensityMapStorage, this._paintedDensityMapRt);
				ComputeShader tgsComputeShader = TgsManager.tgsComputeShader;
				bool flag2 = false;
				if (this._fillPending)
				{
					this._fillPending = false;
					flag2 = true;
					int kernelIndex = tgsComputeShader.FindKernel("FillToDensityMap");
					tgsComputeShader.SetTexture(kernelIndex, TgsTerrainLayer.DensityMapRW, this._paintedDensityMapRt);
					tgsComputeShader.SetTexture(kernelIndex, "_FillMap", this._fillTexture);
					tgsComputeShader.SetVector("_Fill_ChannelWeights", this._fillChannelWeights);
					tgsComputeShader.SetInt("_Fill_Additive", this._fillAdditiveBlend ? 1 : 0);
					tgsComputeShader.SetInt(TgsTerrainLayer.DensityMapDimensionX, this._paintedDensityMapRt.width);
					tgsComputeShader.SetInt(TgsTerrainLayer.DensityMapDimensionY, this._paintedDensityMapRt.height);
					Graphics.SetRandomWriteTarget(0, this._paintedDensityMapRt);
					uint rhs;
					uint rhs2;
					uint num3;
					tgsComputeShader.GetKernelThreadGroupSizes(kernelIndex, out rhs, out rhs2, out num3);
					int threadGroupsX = TgsInstance.CeilingDivision(Mathf.CeilToInt((float)this._paintedDensityMapRt.width), (int)rhs);
					int threadGroupsY = TgsInstance.CeilingDivision(Mathf.CeilToInt((float)this._paintedDensityMapRt.height), (int)rhs2);
					tgsComputeShader.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
					Graphics.ClearRandomWriteTargets();
					this.MarkGeometryDirty();
				}
				if (this._drawPending)
				{
					this._drawPending = false;
					flag2 = true;
					int kernelIndex2 = tgsComputeShader.FindKernel("DrawToDensityMap");
					Graphics.SetRandomWriteTarget(0, this._paintedDensityMapRt);
					tgsComputeShader.SetTexture(kernelIndex2, TgsTerrainLayer.DensityMapRW, this._paintedDensityMapRt);
					tgsComputeShader.SetInt(TgsTerrainLayer.DensityMapDimensionX, this._paintedDensityMapRt.width);
					tgsComputeShader.SetInt(TgsTerrainLayer.DensityMapDimensionY, this._paintedDensityMapRt.height);
					tgsComputeShader.SetVector(TgsTerrainLayer.PaintBrush, new Vector4((float)v.x, (float)v.y, num * 0.5f, this._drawOpacity * Time.deltaTime));
					tgsComputeShader.SetVector(TgsTerrainLayer.PaintNoise, new Vector4(this._drawNoiseScale, this._drawNoiseIntensity));
					uint num3;
					uint rhs3;
					uint rhs4;
					tgsComputeShader.GetKernelThreadGroupSizes(kernelIndex2, out rhs3, out rhs4, out num3);
					int threadGroupsX2 = TgsInstance.CeilingDivision(Mathf.CeilToInt(num), (int)rhs3);
					int threadGroupsY2 = TgsInstance.CeilingDivision(Mathf.CeilToInt(num), (int)rhs4);
					tgsComputeShader.Dispatch(kernelIndex2, threadGroupsX2, threadGroupsY2, 1);
					Graphics.ClearRandomWriteTargets();
					this._dirtyChunks.Add(bounds);
				}
				if (flag2 || this.paintedDensityMapStorage == null)
				{
					if (this.paintedDensityMapStorage == null || this.paintedDensityMapStorage.width != this._paintedDensityMapRt.width || this.paintedDensityMapStorage.height != this._paintedDensityMapRt.height)
					{
						this.paintedDensityMapStorage = new Texture2D(this._paintedDensityMapRt.width, this._paintedDensityMapRt.height, TextureFormat.R8, false);
					}
					this.paintedDensityMapStorage.wrapMode = TextureWrapMode.Clamp;
					RenderTexture active = RenderTexture.active;
					RenderTexture.active = this._paintedDensityMapRt;
					this.paintedDensityMapStorage.ReadPixels(new Rect(0f, 0f, (float)this.paintedDensityMapStorage.width, (float)this.paintedDensityMapStorage.height), 0, 0);
					this.paintedDensityMapStorage.Apply();
					RenderTexture.active = active;
				}
				break;
			}
			case TgsTerrainLayer.TerrainLayerDistribution.ByTerrainLayer:
			{
				int num4 = this.targetTerrainLayer / 4;
				if (num4 < terrainDensityMaps.Length)
				{
					densityTexture = terrainDensityMaps[num4];
				}
				int num5 = this.targetTerrainLayer % 4;
				zero = new Vector4((num5 == 0) ? 1f : 0f, (num5 == 1) ? 1f : 0f, (num5 == 2) ? 1f : 0f, (num5 == 3) ? 1f : 0f);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
			for (int j = 0; j < num2; j++)
			{
				Vector2Int a4 = new Vector2Int(j / vector2Int.x, j % vector2Int.x);
				Vector2Int vector2Int3 = a4 * b;
				Vector2Int vector2Int4 = (a4 + Vector2Int.one) * b;
				default(RectInt).SetMinMax(vector2Int3, vector2Int4);
				vector2Int4 = Vector2Int.Min(a - Vector2Int.one, vector2Int4);
				Vector3 b7 = new Vector3((float)vector2Int3.x * heightmapScale.x, 0f, (float)vector2Int3.y * heightmapScale.z);
				Vector3 b8 = new Vector3((float)vector2Int4.x * heightmapScale.x, terrainData.size.y, (float)vector2Int4.y * heightmapScale.z);
				Bounds heightmapChunkBounds = new Bounds
				{
					min = terrainWorldSpaceBounds.min + b7,
					max = terrainWorldSpaceBounds.min + b8
				};
				Vector2Int chunkPixelSize = vector2Int4 - vector2Int3;
				if (chunkPixelSize.x > 0 && chunkPixelSize.y > 0 && this._chunks[j].isGeometryDirty)
				{
					TgsInstance.TgsInstanceRecipe bakeParameters = TgsInstance.TgsInstanceRecipe.BakeFromHeightmap(localToWorldMatrix, this.settings, terrainData.heightmapTexture, heightmapChunkBounds, vector2Int3, chunkPixelSize);
					switch (this.distribution)
					{
					case TgsTerrainLayer.TerrainLayerDistribution.Fill:
						break;
					case TgsTerrainLayer.TerrainLayerDistribution.TastyGrassShaderPaintTool:
						bakeParameters.SetupDistributionByTexture(this.paintedDensityMapStorage, new Vector4(1f, 0f, 0f, 0f), new Vector4(1f, 1f, 0f, 0f));
						break;
					case TgsTerrainLayer.TerrainLayerDistribution.ByTerrainLayer:
						bakeParameters.SetupDistributionByTexture(densityTexture, zero, new Vector4(1f, 1f, 0f, 0f));
						break;
					case TgsTerrainLayer.TerrainLayerDistribution.ByCustomTexture:
						bakeParameters.SetupDistributionByTexture(this.distributionTexture, this.colorMask, new Vector4(this.scaling.x, this.scaling.y, this.offset.x, this.offset.y));
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					if (this.targetTerrainLayer < terrainLayers.Length)
					{
						TerrainLayer terrainLayer = terrainLayers[this.targetTerrainLayer];
						Vector4 colorMapScaleOffset;
						colorMapScaleOffset.x = 1f / terrainLayer.tileSize.x;
						colorMapScaleOffset.y = 1f / terrainLayer.tileSize.y;
						Vector3 position2 = transform.position;
						colorMapScaleOffset.z = position2.x;
						colorMapScaleOffset.w = position2.y;
						bakeParameters.SetupCamouflage(terrainLayer.diffuseTexture, colorMapScaleOffset, this.settings.camouflage);
					}
					this._chunks[j].SetBakeParameters(bakeParameters);
				}
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003C80 File Offset: 0x00001E80
		private void SetupChunks(int reqCount)
		{
			if (reqCount != this._chunks.Count)
			{
				foreach (TgsInstance tgsInstance in this._chunks)
				{
					tgsInstance.Release();
				}
				this._chunks.Clear();
				this._chunks.Capacity = Mathf.Max(1, reqCount);
				for (int i = 0; i < reqCount; i++)
				{
					TgsInstance tgsInstance2 = new TgsInstance();
					tgsInstance2.MarkGeometryDirty();
					tgsInstance2.MarkMaterialDirty();
					this._chunks.Add(tgsInstance2);
				}
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003D28 File Offset: 0x00001F28
		public int GetChunkCount()
		{
			return this._chunks.Count;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003D38 File Offset: 0x00001F38
		public int GetGrassMemoryBufferByteSize()
		{
			int num = 0;
			foreach (TgsInstance tgsInstance in this._chunks)
			{
				num += tgsInstance.GetGrassBufferMemoryByteSize();
			}
			return num;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003D90 File Offset: 0x00001F90
		public int GetPlacementMemoryBufferByteSize()
		{
			int num = 0;
			foreach (TgsInstance tgsInstance in this._chunks)
			{
				num += tgsInstance.GetPlacementBufferMemoryByteSize();
			}
			return num;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003DE8 File Offset: 0x00001FE8
		public void Release()
		{
			if (this._paintedDensityMapRt != null)
			{
				this._paintedDensityMapRt.Release();
				this._paintedDensityMapRt = null;
			}
			this.SetupChunks(-1);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003E14 File Offset: 0x00002014
		public string GetEditorName(int index)
		{
			return string.Format("#{0} - {1} {2}", index, this.hide ? "[Hidden]" : "", (this.settings.preset != null) ? this.settings.preset.name : "NO PRESET");
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003E70 File Offset: 0x00002070
		public void DrawGizmos()
		{
			if (this.hide)
			{
				return;
			}
			foreach (TgsInstance tgsInstance in this._chunks)
			{
				Gizmos.color = Color.red;
				Bounds tightBounds = tgsInstance.tightBounds;
				Gizmos.DrawWireCube(tightBounds.center, tightBounds.size);
				Gizmos.color = Color.black;
				Bounds looseBounds = tgsInstance.looseBounds;
				Gizmos.DrawWireCube(looseBounds.center, looseBounds.size);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003F0C File Offset: 0x0000210C
		private static Bounds GetTerrainWorldSpaceBounds(Terrain terrain, Vector3 worldPos)
		{
			Vector3 size = terrain.terrainData.size;
			Bounds result = new Bounds
			{
				min = worldPos
			};
			result.max = result.min + size;
			return result;
		}

		// Token: 0x04000042 RID: 66
		private static readonly int DensityMapRW = Shader.PropertyToID("_DensityMapRW");

		// Token: 0x04000043 RID: 67
		private static readonly int PaintBrush = Shader.PropertyToID("_PaintBrush");

		// Token: 0x04000044 RID: 68
		private static readonly int DensityMapDimensionX = Shader.PropertyToID("_DensityMap_DimensionX");

		// Token: 0x04000045 RID: 69
		private static readonly int DensityMapDimensionY = Shader.PropertyToID("_DensityMap_DimensionY");

		// Token: 0x04000046 RID: 70
		private static readonly int PaintNoise = Shader.PropertyToID("_PaintNoise");

		// Token: 0x04000047 RID: 71
		[HideInInspector]
		public bool hide;

		// Token: 0x04000048 RID: 72
		[FormerlySerializedAs("quickSettings")]
		public TgsPreset.Settings settings = TgsPreset.Settings.GetDefault();

		// Token: 0x04000049 RID: 73
		[Header("Terrain Specific")]
		[FormerlySerializedAs("densityMode")]
		[Tooltip("What mode is used to control the amount of the layer.")]
		public TgsTerrainLayer.TerrainLayerDistribution distribution;

		// Token: 0x0400004A RID: 74
		[Space]
		[HideInInspector]
		public Texture2D paintedDensityMapStorage;

		// Token: 0x0400004B RID: 75
		[FormerlySerializedAs("unityTerrainLayerIndex")]
		[Tooltip("The index of the Unity Terrain Layer. This is both used for getting the splatmap (if the amount mode is FromTerrainSplatmap) and what color texture to use for blending in.")]
		[UnityTerrainLayerIndex]
		public int targetTerrainLayer;

		// Token: 0x0400004C RID: 76
		[HideGroup("distribution", 3)]
		public Texture2D distributionTexture;

		// Token: 0x0400004D RID: 77
		[HideGroup("distribution", 3)]
		public Vector2 scaling = Vector2.one;

		// Token: 0x0400004E RID: 78
		[HideGroup("distribution", 3)]
		public Vector2 offset;

		// Token: 0x0400004F RID: 79
		[HideGroup("distribution", 3)]
		public Color colorMask = Color.white;

		// Token: 0x04000050 RID: 80
		[FormerlySerializedAs("sizePerChunk")]
		[Min(8f)]
		public int chunkSize = 32;

		// Token: 0x04000051 RID: 81
		private Hash128 _bakeSettingsHash;

		// Token: 0x04000052 RID: 82
		private List<TgsInstance> _chunks = new List<TgsInstance>();

		// Token: 0x04000053 RID: 83
		private List<Bounds> _dirtyChunks = new List<Bounds>();

		// Token: 0x04000054 RID: 84
		private float _drawNoiseIntensity;

		// Token: 0x04000055 RID: 85
		private float _drawNoiseScale;

		// Token: 0x04000056 RID: 86
		private float _drawOpacity;

		// Token: 0x04000057 RID: 87
		private bool _drawPending;

		// Token: 0x04000058 RID: 88
		private Vector3 _drawPointWs;

		// Token: 0x04000059 RID: 89
		private float _drawRadius;

		// Token: 0x0400005A RID: 90
		private bool _fillAdditiveBlend;

		// Token: 0x0400005B RID: 91
		private Vector4 _fillChannelWeights;

		// Token: 0x0400005C RID: 92
		private bool _fillPending;

		// Token: 0x0400005D RID: 93
		private Texture2D _fillTexture;

		// Token: 0x0400005E RID: 94
		private RenderTexture _paintedDensityMapRt;

		// Token: 0x02000016 RID: 22
		public enum TerrainLayerDistribution
		{
			// Token: 0x04000060 RID: 96
			Fill,
			// Token: 0x04000061 RID: 97
			TastyGrassShaderPaintTool,
			// Token: 0x04000062 RID: 98
			ByTerrainLayer,
			// Token: 0x04000063 RID: 99
			ByCustomTexture
		}
	}
}
