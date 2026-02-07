using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000017 RID: 23
	public class TgsInstance
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003FA4 File Offset: 0x000021A4
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003FAC File Offset: 0x000021AC
		public int bladeCount { get; private set; }

		// Token: 0x06000057 RID: 87 RVA: 0x00003FB5 File Offset: 0x000021B5
		public TgsInstance()
		{
			TgsGlobalStatus.instances++;
			TgsInstance.AllInstances.Add(this);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003FE0 File Offset: 0x000021E0
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003FE8 File Offset: 0x000021E8
		public TgsInstance.TgsInstanceRecipe activeTgsInstanceRecipe { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003FF1 File Offset: 0x000021F1
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003FF9 File Offset: 0x000021F9
		public bool isGeometryDirty { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00004002 File Offset: 0x00002202
		// (set) Token: 0x0600005D RID: 93 RVA: 0x0000400A File Offset: 0x0000220A
		public bool isMaterialDirty { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00004013 File Offset: 0x00002213
		// (set) Token: 0x0600005F RID: 95 RVA: 0x0000401B File Offset: 0x0000221B
		public Bounds tightBounds { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00004024 File Offset: 0x00002224
		// (set) Token: 0x06000061 RID: 97 RVA: 0x0000402C File Offset: 0x0000222C
		public Bounds looseBounds { get; private set; }

		// Token: 0x06000062 RID: 98 RVA: 0x00004035 File Offset: 0x00002235
		public void MarkGeometryDirty()
		{
			this.isGeometryDirty = true;
			if (this._hasFinishedBaking)
			{
				TgsGlobalStatus.instancesReady--;
				this._hasFinishedBaking = false;
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004059 File Offset: 0x00002259
		public void MarkMaterialDirty()
		{
			this.isMaterialDirty = true;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004062 File Offset: 0x00002262
		public static bool MeshHasVertexColor(Mesh placementMesh, Object errorMessageContext, bool requested)
		{
			if (placementMesh.GetVertexAttributeOffset(VertexAttribute.Color) == -1)
			{
				if (requested)
				{
					Debug.LogError("Density by vertex color was requested, but no color attribute could be found. Will use constant amount.", errorMessageContext);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004080 File Offset: 0x00002280
		private void BakeFromMesh(ComputeShader tgsComputeShader, Mesh sharedMesh, TgsInstance.TgsInstanceRecipe tgsInstanceRecipe, Object errorMessageContext = null)
		{
			if (tgsInstanceRecipe.Settings.preset == null)
			{
				return;
			}
			this.looseBounds = tgsInstanceRecipe.WorldSpaceBounds;
			this.activeTgsInstanceRecipe = tgsInstanceRecipe;
			int kernelIndex = tgsComputeShader.FindKernel("MeshPass");
			bool flag = TgsInstance.MeshHasVertexColor(sharedMesh, errorMessageContext, tgsInstanceRecipe.DistributionByVertexColorEnabled);
			Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(sharedMesh);
			Mesh.MeshData meshData = meshDataArray[0];
			NativeArray<Vector3> nativeArray = new NativeArray<Vector3>(meshData.vertexCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<Vector3> nativeArray2 = new NativeArray<Vector3>(meshData.vertexCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<Color> nativeArray3 = new NativeArray<Color>(meshData.vertexCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray4 = new NativeArray<int>(meshData.GetSubMesh(0).indexCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
			meshData.GetVertices(nativeArray);
			meshData.GetNormals(nativeArray2);
			if (flag)
			{
				meshData.GetColors(nativeArray3);
			}
			meshData.GetIndices(nativeArray4, 0, true);
			ComputeBuffer computeBuffer = new ComputeBuffer(nativeArray.Length, 12);
			ComputeBuffer computeBuffer2 = new ComputeBuffer(nativeArray2.Length, 12);
			ComputeBuffer computeBuffer3 = new ComputeBuffer(nativeArray3.Length, 16);
			ComputeBuffer computeBuffer4 = new ComputeBuffer(nativeArray4.Length, 4);
			computeBuffer.SetData<Vector3>(nativeArray);
			computeBuffer2.SetData<Vector3>(nativeArray2);
			computeBuffer3.SetData<Color>(nativeArray3);
			computeBuffer4.SetData<int>(nativeArray4);
			int num = nativeArray4.Length / 3;
			tgsComputeShader.SetVector(TgsInstance.DensityMapChannelMask, tgsInstanceRecipe.DistributionByVertexColorMask);
			tgsComputeShader.SetInt(TgsInstance.DensityUseChannelMask, (tgsInstanceRecipe.DistributionByVertexColorEnabled && flag) ? 1 : 0);
			tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.PlacementVertices, computeBuffer);
			tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.PlacementNormals, computeBuffer2);
			tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.PlacementColors, computeBuffer3);
			tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.PlacementIndices, computeBuffer4);
			TgsInstance.SetupBuffersForCompute(tgsComputeShader, tgsInstanceRecipe.Settings, num, tgsInstanceRecipe.CamouflageTexture, tgsInstanceRecipe.CamouflageFactor, tgsInstanceRecipe.CamouflageTextureScaleOffset, ref this._instanceMetaData, this._instanceMetaDataCPU, ref this._placementTriangleBuffer, ref this._grassNoiseLayers);
			tgsComputeShader.SetMatrix(TgsInstance.ObjectToWorld, tgsInstanceRecipe.LocalToWorldMatrix);
			tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.PlacementTrianglesAppend, this._placementTriangleBuffer);
			tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.MetaDataRW, this._instanceMetaData);
			tgsComputeShader.SetVector(TgsInstance.PositionBoundMin, tgsInstanceRecipe.WorldSpaceBounds.min);
			tgsComputeShader.SetVector(TgsInstance.PositionBoundMax, tgsInstanceRecipe.WorldSpaceBounds.max);
			uint rhs;
			uint num2;
			uint num3;
			tgsComputeShader.GetKernelThreadGroupSizes(kernelIndex, out rhs, out num2, out num3);
			int threadGroupsX = TgsInstance.CeilingDivision(num, (int)rhs);
			tgsComputeShader.Dispatch(kernelIndex, threadGroupsX, 1, 1);
			computeBuffer4.Dispose();
			computeBuffer3.Dispose();
			computeBuffer2.Dispose();
			computeBuffer.Dispose();
			nativeArray4.Dispose();
			nativeArray3.Dispose();
			nativeArray2.Dispose();
			nativeArray.Dispose();
			meshDataArray.Dispose();
			this._instanceMetaData.GetData(this._instanceMetaDataCPU);
			Bounds tightBounds;
			float num4;
			TgsInstance.UnpackAndApplyTightBounds(tgsComputeShader, this._instanceMetaDataCPU[0], this._materialPropertyBlock, tgsInstanceRecipe.WorldSpaceBounds, out tightBounds, out num4);
			this.tightBounds = tightBounds;
			int estMaxBlades = (int)this._instanceMetaDataCPU[0].estMaxBlades;
			int bladeCount;
			TgsInstance.BakeFromPlacementBufferOrSetNull(tgsComputeShader, ref this._bakeOutputBuffer, this._placementTriangleBuffer, this._materialPropertyBlock, (int)this._instanceMetaDataCPU[0].placementTriangles, estMaxBlades, out bladeCount);
			this.bladeCount = bladeCount;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000043B4 File Offset: 0x000025B4
		internal void BakeNextRecipe()
		{
			ComputeShader tgsComputeShader = TgsManager.tgsComputeShader;
			this.isGeometryDirty = false;
			if (this.activeTgsInstanceRecipe.Settings.preset != null)
			{
				this.activeTgsInstanceRecipe.Settings.preset.SetDirtyOnChangeList.Remove(this);
			}
			if (this._nextTgsInstanceRecipe.Settings.preset != null)
			{
				this._nextTgsInstanceRecipe.Settings.preset.SetDirtyOnChangeList.Add(this);
			}
			if (this._materialPropertyBlock == null)
			{
				this._materialPropertyBlock = new MaterialPropertyBlock();
			}
			TgsInstance.TgsInstanceRecipe.InstanceBakeMode bakeMode = this._nextTgsInstanceRecipe.BakeMode;
			if (bakeMode != TgsInstance.TgsInstanceRecipe.InstanceBakeMode.FromMesh)
			{
				if (bakeMode != TgsInstance.TgsInstanceRecipe.InstanceBakeMode.FromHeightmap)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.BakeFromHeightmap(tgsComputeShader, this._nextTgsInstanceRecipe, this._nextTgsInstanceRecipe.HeightmapTexture, this._nextTgsInstanceRecipe.WorldSpaceBounds, this._nextTgsInstanceRecipe.HeightmapChunkPixelOffset, this._nextTgsInstanceRecipe.ChunkPixelSize);
			}
			else
			{
				this.BakeFromMesh(tgsComputeShader, this._nextTgsInstanceRecipe.SharedMesh, this._nextTgsInstanceRecipe, null);
			}
			this.activeTgsInstanceRecipe = this._nextTgsInstanceRecipe;
			if (!this._hasFinishedBaking)
			{
				TgsGlobalStatus.instancesReady++;
			}
			this._hasFinishedBaking = true;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000044E4 File Offset: 0x000026E4
		private void BakeFromHeightmap(ComputeShader tgsComputeShader, TgsInstance.TgsInstanceRecipe tgsInstanceRecipe, Texture heightmapTexture, Bounds heightmapChunkBoundsWs, Vector2Int heightmapChunkPixelOffset, Vector2Int chunkPixelSize)
		{
			this.looseBounds = heightmapChunkBoundsWs;
			int kernelIndex = tgsComputeShader.FindKernel("TerrainPass");
			tgsComputeShader.SetTexture(kernelIndex, TgsInstance.Heightmap, heightmapTexture);
			tgsComputeShader.SetVector(TgsInstance.HeightmapResolutionXy, new Vector2((float)heightmapTexture.width, (float)heightmapTexture.height));
			tgsComputeShader.SetTexture(kernelIndex, TgsInstance.DensityMap, tgsInstanceRecipe.DistributionTexture);
			tgsComputeShader.SetVector(TgsInstance.DensityMapChannelMask, tgsInstanceRecipe.DistributionTextureChannelMask);
			tgsComputeShader.SetInt(TgsInstance.DensityUseChannelMask, tgsInstanceRecipe.DistributionByTextureEnabled ? 1 : 0);
			float num = (float)tgsInstanceRecipe.DistributionTexture.width / (float)heightmapTexture.width / (float)tgsInstanceRecipe.DistributionTexture.width;
			float num2 = (float)tgsInstanceRecipe.DistributionTexture.height / (float)heightmapTexture.height / (float)tgsInstanceRecipe.DistributionTexture.height;
			tgsComputeShader.SetVector(TgsInstance.DensityMapUvFromHeightmapIdx, new Vector4(num * tgsInstanceRecipe.DistributionTextureScaleOffset.x, num2 * tgsInstanceRecipe.DistributionTextureScaleOffset.y, tgsInstanceRecipe.DistributionTextureScaleOffset.z, tgsInstanceRecipe.DistributionTextureScaleOffset.w));
			tgsComputeShader.SetVector(TgsInstance.HeightmapChunkOffsetSize, new Vector4((float)heightmapChunkPixelOffset.x, (float)heightmapChunkPixelOffset.y, (float)chunkPixelSize.x, (float)chunkPixelSize.y));
			int num3 = chunkPixelSize.x * chunkPixelSize.y * 2;
			TgsInstance.SetupBuffersForCompute(tgsComputeShader, tgsInstanceRecipe.Settings, num3, tgsInstanceRecipe.CamouflageTexture, tgsInstanceRecipe.CamouflageFactor, tgsInstanceRecipe.CamouflageTextureScaleOffset, ref this._instanceMetaData, this._instanceMetaDataCPU, ref this._placementTriangleBuffer, ref this._grassNoiseLayers);
			tgsComputeShader.SetVector(TgsInstance.PositionBoundMin, heightmapChunkBoundsWs.min);
			tgsComputeShader.SetVector(TgsInstance.PositionBoundMax, heightmapChunkBoundsWs.max);
			tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.PlacementTrianglesAppend, this._placementTriangleBuffer);
			tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.MetaDataRW, this._instanceMetaData);
			tgsComputeShader.SetMatrix(TgsInstance.ObjectToWorld, tgsInstanceRecipe.LocalToWorldMatrix);
			uint rhs;
			uint num4;
			uint num5;
			tgsComputeShader.GetKernelThreadGroupSizes(kernelIndex, out rhs, out num4, out num5);
			int threadGroupsX = TgsInstance.CeilingDivision(num3, (int)rhs);
			tgsComputeShader.Dispatch(kernelIndex, threadGroupsX, 1, 1);
			this._instanceMetaData.GetData(this._instanceMetaDataCPU);
			int estMaxBlades = (int)this._instanceMetaDataCPU[0].estMaxBlades;
			int placementTriangles = (int)this._instanceMetaDataCPU[0].placementTriangles;
			Bounds tightBounds;
			float num6;
			TgsInstance.UnpackAndApplyTightBounds(tgsComputeShader, this._instanceMetaDataCPU[0], this._materialPropertyBlock, tgsInstanceRecipe.WorldSpaceBounds, out tightBounds, out num6);
			this.tightBounds = tightBounds;
			int bladeCount;
			TgsInstance.BakeFromPlacementBufferOrSetNull(tgsComputeShader, ref this._bakeOutputBuffer, this._placementTriangleBuffer, this._materialPropertyBlock, placementTriangles, estMaxBlades, out bladeCount);
			this.bladeCount = bladeCount;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004778 File Offset: 0x00002978
		public void SetBakeParameters(TgsInstance.TgsInstanceRecipe nextTgsInstanceRecipe)
		{
			this._nextTgsInstanceRecipe = nextTgsInstanceRecipe;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004784 File Offset: 0x00002984
		internal void DrawAndUpdateMaterialPropertyBlock(TgsManager.TgsInstancePreRendering instance, Camera renderingCamera, Vector4[] colliderBuffer, int colliderCount, bool singlePassVr, Material renderingMaterial)
		{
			TgsPreset preset = this.activeTgsInstanceRecipe.Settings.preset;
			if (this._bakeOutputBuffer == null || this.UsedWindSettings == null)
			{
				return;
			}
			this._materialPropertyBlock.SetInt(TgsInstance.SphereColliderCount, colliderCount);
			if (colliderCount > 0)
			{
				this._materialPropertyBlock.SetVectorArray(TgsInstance.SphereCollider, colliderBuffer);
			}
			if (this.isMaterialDirty)
			{
				this.isMaterialDirty = false;
				preset.ApplyToMaterialPropertyBlock(this._materialPropertyBlock);
			}
			this.UsedWindSettings.ApplyToMaterialPropertyBlock(this._materialPropertyBlock);
			bool castShadows = preset.castShadows;
			Graphics.DrawProcedural(renderingMaterial, this.tightBounds, MeshTopology.Triangles, instance.renderingVertexCount, singlePassVr ? 2 : 1, renderingCamera, this._materialPropertyBlock, castShadows ? ShadowCastingMode.TwoSided : ShadowCastingMode.Off, true, this.UnityLayer);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004845 File Offset: 0x00002A45
		public int GetGrassBufferMemoryByteSize()
		{
			if (this._bakeOutputBuffer != null && this._bakeOutputBuffer.IsValid())
			{
				return this._bakeOutputBuffer.count * this._bakeOutputBuffer.stride;
			}
			return 0;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004875 File Offset: 0x00002A75
		public int GetPlacementBufferMemoryByteSize()
		{
			if (this._placementTriangleBuffer != null && this._placementTriangleBuffer.IsValid())
			{
				return this._placementTriangleBuffer.count * this._placementTriangleBuffer.stride;
			}
			return 0;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000048A8 File Offset: 0x00002AA8
		public void Release()
		{
			ComputeBuffer placementTriangleBuffer = this._placementTriangleBuffer;
			if (placementTriangleBuffer != null)
			{
				placementTriangleBuffer.Release();
			}
			this._placementTriangleBuffer = null;
			ComputeBuffer instanceMetaData = this._instanceMetaData;
			if (instanceMetaData != null)
			{
				instanceMetaData.Release();
			}
			this._instanceMetaData = null;
			ComputeBuffer bakeOutputBuffer = this._bakeOutputBuffer;
			if (bakeOutputBuffer != null)
			{
				bakeOutputBuffer.Release();
			}
			this._bakeOutputBuffer = null;
			ComputeBuffer grassNoiseLayers = this._grassNoiseLayers;
			if (grassNoiseLayers != null)
			{
				grassNoiseLayers.Release();
			}
			this._grassNoiseLayers = null;
			TgsGlobalStatus.instances--;
			if (this._hasFinishedBaking)
			{
				TgsGlobalStatus.instancesReady--;
			}
			TgsInstance.AllInstances.Remove(this);
			if (this.activeTgsInstanceRecipe.Settings.preset != null)
			{
				this.activeTgsInstanceRecipe.Settings.preset.SetDirtyOnChangeList.Remove(this);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004975 File Offset: 0x00002B75
		public static float ComputeCameraFovScalingFactor(Camera camera)
		{
			return 2f * Mathf.Tan(camera.fieldOfView * 0.017453292f * 0.5f);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004994 File Offset: 0x00002B94
		public static int CeilingDivision(int lhs, int rhs)
		{
			return (lhs + rhs - 1) / rhs;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000499D File Offset: 0x00002B9D
		public static float CeilingDivisionFloat(float lhs, float rhs)
		{
			return (lhs + rhs - 1f) / rhs;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000049AC File Offset: 0x00002BAC
		private static Vector3 UnpackVector3_32Bit(uint vX, uint vY, uint vZ, Vector3 min, Vector3 max)
		{
			double num = 4294967294.0;
			double num2 = vX / num;
			double num3 = vY / num;
			double num4 = vZ / num;
			return new Vector3(Mathf.Lerp(min.x, max.x, (float)num2), Mathf.Lerp(min.y, max.y, (float)num3), Mathf.Lerp(min.z, max.z, (float)num4));
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004A18 File Offset: 0x00002C18
		private static void UnpackAndApplyTightBounds(ComputeShader tgsComputeShader, TgsInstance.InstanceMetaDataGPU instanceMetaDataGPU, MaterialPropertyBlock materialPropertyBlock, Bounds looseBounds, out Bounds tightBounds, out float tightBoundsMaxSideLength)
		{
			Vector3 min = TgsInstance.UnpackVector3_32Bit(instanceMetaDataGPU.boundsMinX, instanceMetaDataGPU.boundsMinY, instanceMetaDataGPU.boundsMinZ, looseBounds.min, looseBounds.max);
			Vector3 max = TgsInstance.UnpackVector3_32Bit(instanceMetaDataGPU.boundsMaxX, instanceMetaDataGPU.boundsMaxY, instanceMetaDataGPU.boundsMaxZ, looseBounds.min, looseBounds.max);
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(min, max);
			bounds.Expand(4f);
			tightBounds = bounds;
			tightBoundsMaxSideLength = Mathf.Max(tightBounds.size.x, Mathf.Max(tightBounds.size.y, tightBounds.size.z));
			materialPropertyBlock.SetVector(TgsInstance.PositionBoundMin, tightBounds.min);
			materialPropertyBlock.SetVector(TgsInstance.PositionBoundMax, tightBounds.max);
			tgsComputeShader.SetVector(TgsInstance.PositionBoundMin, tightBounds.min);
			tgsComputeShader.SetVector(TgsInstance.PositionBoundMax, tightBounds.max);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004B24 File Offset: 0x00002D24
		private static void SetupBuffersForCompute(ComputeShader tgsComputeShader, TgsPreset.Settings settings, int placementMeshTriangleCount, Texture colorMap, float colorMapBlend, Vector4 colorMapScaleOffset, ref ComputeBuffer instanceMetaData, TgsInstance.InstanceMetaDataGPU[] instanceMetaDataCPU, ref ComputeBuffer placementTriangleBuffer, ref ComputeBuffer grassNoiseLayers)
		{
			int num = tgsComputeShader.FindKernel("BakePass");
			ComputeBuffer computeBuffer = instanceMetaData;
			if (computeBuffer != null)
			{
				computeBuffer.Release();
			}
			instanceMetaData = new ComputeBuffer(1, 32);
			instanceMetaDataCPU[0] = new TgsInstance.InstanceMetaDataGPU
			{
				boundsMinX = uint.MaxValue,
				boundsMinY = uint.MaxValue,
				boundsMinZ = uint.MaxValue
			};
			instanceMetaData.SetData(instanceMetaDataCPU);
			if (placementTriangleBuffer == null || placementMeshTriangleCount != placementTriangleBuffer.count)
			{
				ComputeBuffer computeBuffer2 = placementTriangleBuffer;
				if (computeBuffer2 != null)
				{
					computeBuffer2.Release();
				}
				placementTriangleBuffer = new ComputeBuffer(placementMeshTriangleCount, 104, ComputeBufferType.Append);
			}
			placementTriangleBuffer.SetCounterValue(0U);
			if (grassNoiseLayers == null)
			{
				grassNoiseLayers = new ComputeBuffer(4, 80);
			}
			settings.preset.ApplyLayerSettingsToBuffer(grassNoiseLayers, settings);
			tgsComputeShader.SetBuffer(num, TgsInstance.NoiseParams, grassNoiseLayers);
			tgsComputeShader.SetBuffer(num, TgsInstance.PlacementTrianglesR, placementTriangleBuffer);
			tgsComputeShader.SetInt(TgsInstance.PlacementTriangleCount, placementMeshTriangleCount);
			settings.preset.ApplyToComputeShader(tgsComputeShader, settings, num);
			tgsComputeShader.SetTexture(num, TgsInstance.ColorMap, colorMap);
			tgsComputeShader.SetFloat(TgsInstance.ColorMapBlend, colorMapBlend);
			tgsComputeShader.SetVector(TgsInstance.ColorMapSt, colorMapScaleOffset);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004C3C File Offset: 0x00002E3C
		private static void BakeFromPlacementBufferOrSetNull(ComputeShader tgsComputeShader, ref ComputeBuffer bakeOutputBuffer, ComputeBuffer placementTriangleBuffer, MaterialPropertyBlock materialPropertyBlock, int placementTriangleCount, int estMaxBlades, out int bladeCount)
		{
			if (placementTriangleCount > 0 && estMaxBlades > 0)
			{
				if (bakeOutputBuffer == null || bakeOutputBuffer.count != estMaxBlades)
				{
					ComputeBuffer computeBuffer = bakeOutputBuffer;
					if (computeBuffer != null)
					{
						computeBuffer.Release();
					}
					bakeOutputBuffer = new ComputeBuffer(estMaxBlades, 16, ComputeBufferType.Append);
				}
				bakeOutputBuffer.SetCounterValue(0U);
				int kernelIndex = tgsComputeShader.FindKernel("BakePass");
				tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.PlacementTrianglesR, placementTriangleBuffer);
				tgsComputeShader.SetBuffer(kernelIndex, TgsInstance.GrassNodePrimitivesAppend, bakeOutputBuffer);
				tgsComputeShader.SetInt(TgsInstance.UsedPlacementTriangleCount, placementTriangleCount);
				int num = TgsInstance.CeilingDivision(placementTriangleCount, 65535);
				for (int i = 0; i < num; i++)
				{
					tgsComputeShader.SetInt(TgsInstance.PlacementTriangleOffset, i * 65535);
					tgsComputeShader.Dispatch(kernelIndex, placementTriangleCount, 1, 1);
				}
				ComputeBuffer computeBuffer2 = new ComputeBuffer(1, 4, ComputeBufferType.Raw);
				ComputeBuffer.CopyCount(bakeOutputBuffer, computeBuffer2, 0);
				computeBuffer2.GetData(TgsInstance._bladeCountTmpBuffer);
				computeBuffer2.Dispose();
				bladeCount = TgsInstance._bladeCountTmpBuffer[0];
			}
			else
			{
				bladeCount = 0;
				ComputeBuffer computeBuffer3 = bakeOutputBuffer;
				if (computeBuffer3 != null)
				{
					computeBuffer3.Release();
				}
				bakeOutputBuffer = null;
			}
			materialPropertyBlock.SetBuffer(TgsInstance.GrassNodePrimitives, bakeOutputBuffer);
		}

		// Token: 0x04000064 RID: 100
		private const int GrassNodeCompressedStride = 16;

		// Token: 0x04000065 RID: 101
		private const int GrassNodeReferenceStride = 60;

		// Token: 0x04000066 RID: 102
		private const int GrassNodeStride = 16;

		// Token: 0x04000067 RID: 103
		private const int BladesPerTriangleUpperLimit = 4096;

		// Token: 0x04000068 RID: 104
		private const float GrassMaxVertexRangeSize = 2f;

		// Token: 0x04000069 RID: 105
		private const float MaxGrassRootOffset = 4f;

		// Token: 0x0400006A RID: 106
		private const int SizeofPlacementTriangle = 104;

		// Token: 0x0400006B RID: 107
		private static readonly int[] _bladeCountTmpBuffer = new int[1];

		// Token: 0x0400006C RID: 108
		public static List<TgsInstance> AllInstances = new List<TgsInstance>();

		// Token: 0x0400006D RID: 109
		private static readonly int PlacementVertices = Shader.PropertyToID("_PlacementVertices");

		// Token: 0x0400006E RID: 110
		private static readonly int PlacementIndices = Shader.PropertyToID("_PlacementIndices");

		// Token: 0x0400006F RID: 111
		private static readonly int GrassNodePrimitivesAppend = Shader.PropertyToID("_GrassFieldPrimitivesAppend");

		// Token: 0x04000070 RID: 112
		private static readonly int IndirectDrawArgs = Shader.PropertyToID("_IndirectDrawArgs");

		// Token: 0x04000071 RID: 113
		private static readonly int GrassNodePrimitives = Shader.PropertyToID("_GrassFieldPrimitives");

		// Token: 0x04000072 RID: 114
		private static readonly int PositionBoundMin = Shader.PropertyToID("_PositionBoundMin");

		// Token: 0x04000073 RID: 115
		private static readonly int PositionBoundMax = Shader.PropertyToID("_PositionBoundMax");

		// Token: 0x04000074 RID: 116
		private static readonly int ObjectToWorld = Shader.PropertyToID("_ObjectToWorld");

		// Token: 0x04000075 RID: 117
		private static readonly int Heightmap = Shader.PropertyToID("_Heightmap");

		// Token: 0x04000076 RID: 118
		private static readonly int HeightmapResolutionXy = Shader.PropertyToID("_HeightmapResolutionXy");

		// Token: 0x04000077 RID: 119
		private static readonly int HeightmapChunkOffsetSize = Shader.PropertyToID("_HeightmapChunkOffsetSize");

		// Token: 0x04000078 RID: 120
		private static readonly int DensityUseChannelMask = Shader.PropertyToID("_DensityUseChannelMask");

		// Token: 0x04000079 RID: 121
		private static readonly int DensityMapChannelMask = Shader.PropertyToID("_DensityMapChannelMask");

		// Token: 0x0400007A RID: 122
		private static readonly int DensityMap = Shader.PropertyToID("_DensityMap");

		// Token: 0x0400007B RID: 123
		private static readonly int DensityMapUvFromHeightmapIdx = Shader.PropertyToID("_DensityMapUvFromHeightmapIdx");

		// Token: 0x0400007C RID: 124
		private static readonly int NoiseParams = Shader.PropertyToID("_NoiseParams");

		// Token: 0x0400007D RID: 125
		private static readonly int SphereCollider = Shader.PropertyToID("_SphereCollider");

		// Token: 0x0400007E RID: 126
		private static readonly int SphereColliderCount = Shader.PropertyToID("_SphereColliderCount");

		// Token: 0x0400007F RID: 127
		private static readonly int PlacementTrianglesR = Shader.PropertyToID("_PlacementTrianglesR");

		// Token: 0x04000080 RID: 128
		private static readonly int PlacementTriangleCount = Shader.PropertyToID("_PlacementTriangleCount");

		// Token: 0x04000081 RID: 129
		private static readonly int UsedPlacementTriangleCount = Shader.PropertyToID("_UsedPlacementTriangleCount");

		// Token: 0x04000082 RID: 130
		private static readonly int PlacementTrianglesAppend = Shader.PropertyToID("_PlacementTrianglesAppend");

		// Token: 0x04000083 RID: 131
		private static readonly int MetaDataRW = Shader.PropertyToID("_MetaDataRW");

		// Token: 0x04000084 RID: 132
		private static readonly int PlacementTriangleOffset = Shader.PropertyToID("_PlacementTriangleOffset");

		// Token: 0x04000085 RID: 133
		private static readonly int ColorMap = Shader.PropertyToID("_ColorMap");

		// Token: 0x04000086 RID: 134
		private static readonly int ColorMapBlend = Shader.PropertyToID("_ColorMapBlend");

		// Token: 0x04000087 RID: 135
		private static readonly int ColorMapSt = Shader.PropertyToID("_ColorMapST");

		// Token: 0x04000088 RID: 136
		private static readonly int PlacementNormals = Shader.PropertyToID("_PlacementNormals");

		// Token: 0x04000089 RID: 137
		private static readonly int PlacementColors = Shader.PropertyToID("_PlacementColors");

		// Token: 0x0400008A RID: 138
		private readonly TgsInstance.InstanceMetaDataGPU[] _instanceMetaDataCPU = new TgsInstance.InstanceMetaDataGPU[1];

		// Token: 0x0400008B RID: 139
		private ComputeBuffer _bakeOutputBuffer;

		// Token: 0x0400008D RID: 141
		private ComputeBuffer _grassNoiseLayers;

		// Token: 0x0400008E RID: 142
		private ComputeBuffer _instanceMetaData;

		// Token: 0x0400008F RID: 143
		private bool _hasFinishedBaking;

		// Token: 0x04000090 RID: 144
		private MaterialPropertyBlock _materialPropertyBlock;

		// Token: 0x04000091 RID: 145
		private ComputeBuffer _placementTriangleBuffer;

		// Token: 0x04000092 RID: 146
		public bool Hide;

		// Token: 0x04000093 RID: 147
		private TgsInstance.TgsInstanceRecipe _nextTgsInstanceRecipe;

		// Token: 0x04000094 RID: 148
		public TgsWindSettings UsedWindSettings;

		// Token: 0x0400009A RID: 154
		public int UnityLayer;

		// Token: 0x02000018 RID: 24
		private struct InstanceMetaDataGPU
		{
			// Token: 0x0400009B RID: 155
			public const int Stride = 32;

			// Token: 0x0400009C RID: 156
			public uint estMaxBlades;

			// Token: 0x0400009D RID: 157
			public uint placementTriangles;

			// Token: 0x0400009E RID: 158
			public uint boundsMinX;

			// Token: 0x0400009F RID: 159
			public uint boundsMinY;

			// Token: 0x040000A0 RID: 160
			public uint boundsMinZ;

			// Token: 0x040000A1 RID: 161
			public uint boundsMaxX;

			// Token: 0x040000A2 RID: 162
			public uint boundsMaxY;

			// Token: 0x040000A3 RID: 163
			public uint boundsMaxZ;
		}

		// Token: 0x02000019 RID: 25
		public struct TgsInstanceRecipe
		{
			// Token: 0x06000075 RID: 117 RVA: 0x00004F20 File Offset: 0x00003120
			private static TgsInstance.TgsInstanceRecipe GetDefaultInstance()
			{
				return new TgsInstance.TgsInstanceRecipe
				{
					DistributionTexture = Texture2D.whiteTexture,
					DistributionTextureChannelMask = new Vector4(1f, 0f, 0f, 0f),
					CamouflageTexture = Texture2D.whiteTexture,
					HeightmapTexture = Texture2D.blackTexture,
					DistributionByVertexColorMask = Color.white
				};
			}

			// Token: 0x06000076 RID: 118 RVA: 0x00004F88 File Offset: 0x00003188
			public static TgsInstance.TgsInstanceRecipe BakeFromHeightmap(Matrix4x4 localToWorldMatrix, TgsPreset.Settings settings, Texture heightmapTexture, Bounds heightmapChunkBounds, Vector2Int heightmapChunkPixelOffset, Vector2Int chunkPixelSize)
			{
				TgsInstance.TgsInstanceRecipe defaultInstance = TgsInstance.TgsInstanceRecipe.GetDefaultInstance();
				defaultInstance.BakeMode = TgsInstance.TgsInstanceRecipe.InstanceBakeMode.FromHeightmap;
				defaultInstance.LocalToWorldMatrix = localToWorldMatrix;
				defaultInstance.Settings = settings;
				defaultInstance.HeightmapTexture = heightmapTexture;
				defaultInstance.WorldSpaceBounds = heightmapChunkBounds;
				defaultInstance.HeightmapChunkPixelOffset = heightmapChunkPixelOffset;
				defaultInstance.ChunkPixelSize = chunkPixelSize;
				return defaultInstance;
			}

			// Token: 0x06000077 RID: 119 RVA: 0x00004FD8 File Offset: 0x000031D8
			public static TgsInstance.TgsInstanceRecipe BakeFromMesh(Matrix4x4 localToWorldMatrix, TgsPreset.Settings settings, Mesh sharedMesh, Bounds worldSpaceBounds)
			{
				TgsInstance.TgsInstanceRecipe defaultInstance = TgsInstance.TgsInstanceRecipe.GetDefaultInstance();
				defaultInstance.BakeMode = TgsInstance.TgsInstanceRecipe.InstanceBakeMode.FromMesh;
				defaultInstance.LocalToWorldMatrix = localToWorldMatrix;
				defaultInstance.Settings = settings;
				defaultInstance.SharedMesh = sharedMesh;
				defaultInstance.WorldSpaceBounds = worldSpaceBounds;
				return defaultInstance;
			}

			// Token: 0x06000078 RID: 120 RVA: 0x00005014 File Offset: 0x00003214
			public void SetupDistributionByTexture(Texture densityTexture, Vector4 channelMask, Vector4 scaleOffset)
			{
				if (this.BakeMode == TgsInstance.TgsInstanceRecipe.InstanceBakeMode.FromMesh)
				{
					Debug.LogError("SetupDistributionByTexture() is not supported with meshes currently.");
					return;
				}
				this.DistributionByTextureEnabled = true;
				this.DistributionTexture = ((densityTexture == null) ? Texture2D.whiteTexture : densityTexture);
				this.DistributionTextureChannelMask = channelMask;
				this.DistributionTextureScaleOffset = scaleOffset;
			}

			// Token: 0x06000079 RID: 121 RVA: 0x00005060 File Offset: 0x00003260
			public void SetupCamouflage(Texture colorMap, Vector4 colorMapScaleOffset, float blendFactor)
			{
				if (this.BakeMode == TgsInstance.TgsInstanceRecipe.InstanceBakeMode.FromMesh)
				{
					Debug.LogError("SetupCamouflage() is not supported with meshes currently.");
					return;
				}
				this.CamouflageTexture = ((colorMap == null) ? Texture2D.whiteTexture : colorMap);
				this.CamouflageTextureScaleOffset = colorMapScaleOffset;
				this.CamouflageFactor = blendFactor;
			}

			// Token: 0x0600007A RID: 122 RVA: 0x0000509A File Offset: 0x0000329A
			public void SetupDistributionByVertexColor(Color mask)
			{
				if (this.BakeMode == TgsInstance.TgsInstanceRecipe.InstanceBakeMode.FromHeightmap)
				{
					Debug.LogError("SetupDistributionByVertexColor() is not supported with heightmaps.");
					return;
				}
				this.DistributionByVertexColorEnabled = true;
				this.DistributionByVertexColorMask = mask;
			}

			// Token: 0x040000A4 RID: 164
			internal TgsPreset.Settings Settings;

			// Token: 0x040000A5 RID: 165
			internal Matrix4x4 LocalToWorldMatrix;

			// Token: 0x040000A6 RID: 166
			internal Bounds WorldSpaceBounds;

			// Token: 0x040000A7 RID: 167
			internal bool DistributionByTextureEnabled;

			// Token: 0x040000A8 RID: 168
			internal Texture DistributionTexture;

			// Token: 0x040000A9 RID: 169
			internal Vector4 DistributionTextureChannelMask;

			// Token: 0x040000AA RID: 170
			internal Vector4 DistributionTextureScaleOffset;

			// Token: 0x040000AB RID: 171
			internal float CamouflageFactor;

			// Token: 0x040000AC RID: 172
			internal Texture CamouflageTexture;

			// Token: 0x040000AD RID: 173
			internal Vector4 CamouflageTextureScaleOffset;

			// Token: 0x040000AE RID: 174
			internal Texture HeightmapTexture;

			// Token: 0x040000AF RID: 175
			internal Vector2Int HeightmapChunkPixelOffset;

			// Token: 0x040000B0 RID: 176
			internal Vector2Int ChunkPixelSize;

			// Token: 0x040000B1 RID: 177
			internal Mesh SharedMesh;

			// Token: 0x040000B2 RID: 178
			internal TgsInstance.TgsInstanceRecipe.InstanceBakeMode BakeMode;

			// Token: 0x040000B3 RID: 179
			internal bool DistributionByVertexColorEnabled;

			// Token: 0x040000B4 RID: 180
			internal Color DistributionByVertexColorMask;

			// Token: 0x0200001A RID: 26
			internal enum InstanceBakeMode
			{
				// Token: 0x040000B6 RID: 182
				FromMesh,
				// Token: 0x040000B7 RID: 183
				FromHeightmap
			}
		}
	}
}
