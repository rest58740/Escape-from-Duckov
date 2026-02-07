using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x0200001B RID: 27
	public static class TgsManager
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000050BE File Offset: 0x000032BE
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000050C5 File Offset: 0x000032C5
		public static ComputeShader tgsComputeShader { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000050CD File Offset: 0x000032CD
		// (set) Token: 0x0600007E RID: 126 RVA: 0x000050D4 File Offset: 0x000032D4
		public static Material tgsMatDefaultUrp { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000050DC File Offset: 0x000032DC
		public static Material tgsMatNoAlphaClipping { get; }

		// Token: 0x06000080 RID: 128 RVA: 0x000050E3 File Offset: 0x000032E3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void SafeInitialize()
		{
			if (TgsManager._isInitialize)
			{
				return;
			}
			TgsManager._isInitialize = true;
			TgsManager._activeGlobalDensityValue = TastyGrassShaderGlobalSettings.GlobalDensityScale;
			TgsManager.Enable = true;
			RenderPipelineManager.beginCameraRendering += new Action<ScriptableRenderContext, Camera>(TgsManager.OnBeginCameraRendering);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005114 File Offset: 0x00003314
		private static void OnBeginCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
		{
			if (!TgsManager.Enable)
			{
				return;
			}
			if (TgsManager.tgsComputeShader == null || TgsManager.tgsMatDefaultUrp == null)
			{
				TgsManager.tgsComputeShader = Resources.Load<ComputeShader>("Shaders/TastyGrassShaderCompute");
				TgsManager.tgsMatDefaultUrp = Resources.Load<Material>("Materials/Tgs_UrpDefault");
				if (TgsManager.tgsComputeShader == null || TgsManager.tgsMatDefaultUrp == null)
				{
					Debug.LogError("Tasty Grass Shader: unable to locate resources. Ensure that the plugin is installed correctly and that all files in the Resource folder are present. Tasty Grass Shader will not work.");
					return;
				}
			}
			List<TgsInstance> allInstances = TgsInstance.AllInstances;
			NativeArray<TgsManager.TgsInstancePreRendering> instances = new NativeArray<TgsManager.TgsInstancePreRendering>(allInstances.Count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			if (!Mathf.Approximately(TgsManager._activeGlobalDensityValue, TastyGrassShaderGlobalSettings.GlobalDensityScale))
			{
				foreach (TgsInstance tgsInstance in allInstances)
				{
					tgsInstance.MarkGeometryDirty();
				}
				TgsManager._activeGlobalDensityValue = TastyGrassShaderGlobalSettings.GlobalDensityScale;
			}
			int num = 0;
			for (int i = 0; i < allInstances.Count; i++)
			{
				TgsInstance tgsInstance2 = allInstances[i];
				if (tgsInstance2.isGeometryDirty && num < TastyGrassShaderGlobalSettings.GlobalMaxBakesPerFrame)
				{
					tgsInstance2.BakeNextRecipe();
					num++;
				}
				instances[i] = new TgsManager.TgsInstancePreRendering(tgsInstance2);
			}
			List<TgsCollider> activeColliders = TgsCollider._activeColliders;
			NativeArray<float4> collidersIn = new NativeArray<float4>(activeColliders.Count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			for (int j = 0; j < activeColliders.Count; j++)
			{
				TgsCollider tgsCollider = activeColliders[j];
				Transform transform = tgsCollider.transform;
				float4 zero = float4.zero;
				zero.xyz = transform.position;
				zero.w = tgsCollider.radius * tgsCollider.radius * transform.localScale.magnitude;
				collidersIn[j] = zero;
			}
			NativeArray<float4> collidersOut = new NativeArray<float4>(instances.Length * 8, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			new TgsManager.TgsPreRenderJob
			{
				Instances = instances,
				CollidersIn = collidersIn,
				CollidersOut = collidersOut,
				CameraPosition = renderingCamera.transform.position,
				CameraFovScalingFactor = TgsInstance.ComputeCameraFovScalingFactor(renderingCamera),
				LodScale = TastyGrassShaderGlobalSettings.GlobalLodScale,
				LodFalloffExp = TastyGrassShaderGlobalSettings.GlobalLodFalloffExponent
			}.Schedule(instances.Length, 64, default(JobHandle)).Complete();
			if (TgsManager._colliderBuffer == null)
			{
				TgsManager._colliderBuffer = new Vector4[8];
			}
			bool singlePassVr = renderingCamera.stereoEnabled && XRSettings.stereoRenderingMode > XRSettings.StereoRenderingMode.MultiPass;
			Material material = (TastyGrassShaderGlobalSettings.CustomRenderingMaterial == null) ? TgsManager.tgsMatDefaultUrp : TastyGrassShaderGlobalSettings.CustomRenderingMaterial;
			if (!material.shader)
			{
				return;
			}
			LocalKeyword localKeyword = new LocalKeyword(material.shader, "TGS_USE_ALPHACLIP");
			if (localKeyword.isValid)
			{
				material.SetKeyword(localKeyword, TastyGrassShaderGlobalSettings.NoAlphaToCoverage);
			}
			material.SetInteger(TgsManager.TgsUseAlphaToCoverage, TastyGrassShaderGlobalSettings.NoAlphaToCoverage ? 0 : 1);
			for (int k = 0; k < instances.Length; k++)
			{
				TgsManager.TgsInstancePreRendering tgsInstancePreRendering = instances[k];
				if (tgsInstancePreRendering.renderingVertexCount > 0)
				{
					if (tgsInstancePreRendering.colliderCount > 0)
					{
						int num2 = k * 8;
						for (int l = 0; l < tgsInstancePreRendering.colliderCount; l++)
						{
							TgsManager._colliderBuffer[l] = collidersOut[num2 + l];
						}
					}
					TgsInstance tgsInstance3 = allInstances[k];
					if (!tgsInstance3.Hide)
					{
						tgsInstance3.DrawAndUpdateMaterialPropertyBlock(tgsInstancePreRendering, renderingCamera, TgsManager._colliderBuffer, tgsInstancePreRendering.colliderCount, singlePassVr, material);
					}
				}
			}
			collidersOut.Dispose();
			collidersIn.Dispose();
			instances.Dispose();
		}

		// Token: 0x040000B8 RID: 184
		public const int MaxColliderPerInstance = 8;

		// Token: 0x040000B9 RID: 185
		public static bool Enable = true;

		// Token: 0x040000BA RID: 186
		private static float _activeGlobalDensityValue;

		// Token: 0x040000BB RID: 187
		private static bool _isInitialize;

		// Token: 0x040000BC RID: 188
		private static Vector4[] _colliderBuffer;

		// Token: 0x040000BD RID: 189
		private static readonly int TgsUseAlphaToCoverage = Shader.PropertyToID("_Tgs_UseAlphaToCoverage");

		// Token: 0x0200001C RID: 28
		public struct TgsInstancePreRendering
		{
			// Token: 0x06000083 RID: 131 RVA: 0x000054C4 File Offset: 0x000036C4
			public TgsInstancePreRendering(TgsInstance instance)
			{
				this.aabb = instance.tightBounds;
				if (instance.activeTgsInstanceRecipe.Settings.preset)
				{
					this.lodBiasByPreset = instance.activeTgsInstanceRecipe.Settings.preset.baseLodFactor;
				}
				else
				{
					this.lodBiasByPreset = 0f;
				}
				this.colliderCount = 0;
				this.renderingVertexCount = 0;
				this.bladeCount = instance.bladeCount;
			}

			// Token: 0x040000C1 RID: 193
			public Bounds aabb;

			// Token: 0x040000C2 RID: 194
			public float lodBiasByPreset;

			// Token: 0x040000C3 RID: 195
			public int bladeCount;

			// Token: 0x040000C4 RID: 196
			public int colliderCount;

			// Token: 0x040000C5 RID: 197
			public int renderingVertexCount;
		}

		// Token: 0x0200001D RID: 29
		[BurstCompile]
		private struct TgsPreRenderJob : IJobParallelFor
		{
			// Token: 0x06000084 RID: 132 RVA: 0x00005538 File Offset: 0x00003738
			private static float DistanceToAabbSqr(float3 pos, float3x2 aabb)
			{
				float3 y = math.clamp(pos, aabb.c0, aabb.c1);
				return math.distancesq(pos, y);
			}

			// Token: 0x06000085 RID: 133 RVA: 0x00005560 File Offset: 0x00003760
			public void Execute(int index)
			{
				TgsManager.TgsInstancePreRendering tgsInstancePreRendering = this.Instances[index];
				float3x2 aabb = new float3x2(tgsInstancePreRendering.aabb.min, tgsInstancePreRendering.aabb.max);
				int num = 0;
				float num2 = math.sqrt(TgsManager.TgsPreRenderJob.DistanceToAabbSqr(this.CameraPosition, aabb));
				int num3 = (int)math.round(math.saturate(math.pow(32f / num2 / this.CameraFovScalingFactor * this.LodScale * tgsInstancePreRendering.lodBiasByPreset, this.LodFalloffExp)) * (float)tgsInstancePreRendering.bladeCount);
				if (num3 > 0)
				{
					int num4 = index * 8;
					int num5 = 0;
					while (num5 < this.CollidersIn.Length && num < 8)
					{
						float4 @float = this.CollidersIn[num5];
						float3 xyz = @float.xyz;
						float w = @float.w;
						if (TgsManager.TgsPreRenderJob.DistanceToAabbSqr(xyz, aabb) < w)
						{
							this.CollidersOut[num4 + num] = new float4(xyz.x, xyz.y, xyz.z, math.sqrt(w));
							num++;
						}
						num5++;
					}
				}
				tgsInstancePreRendering.colliderCount = num;
				tgsInstancePreRendering.renderingVertexCount = num3 * 3;
				this.Instances[index] = tgsInstancePreRendering;
			}

			// Token: 0x040000C6 RID: 198
			public NativeArray<TgsManager.TgsInstancePreRendering> Instances;

			// Token: 0x040000C7 RID: 199
			[WriteOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> CollidersOut;

			// Token: 0x040000C8 RID: 200
			[ReadOnly]
			public NativeArray<float4> CollidersIn;

			// Token: 0x040000C9 RID: 201
			[ReadOnly]
			public float3 CameraPosition;

			// Token: 0x040000CA RID: 202
			[ReadOnly]
			public float CameraFovScalingFactor;

			// Token: 0x040000CB RID: 203
			[ReadOnly]
			public float LodScale;

			// Token: 0x040000CC RID: 204
			[ReadOnly]
			public float LodFalloffExp;
		}
	}
}
