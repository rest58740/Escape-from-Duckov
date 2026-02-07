using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x0200001E RID: 30
	[CreateAssetMenu(menuName = "Symmetry Break Studio/Tasty Grass Shader/Preset")]
	[HelpURL("https://github.com/SymmetryBreakStudio/TastyGrassShader/wiki")]
	public class TgsPreset : ScriptableObject
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000086 RID: 134 RVA: 0x0000569F File Offset: 0x0000389F
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000056A7 File Offset: 0x000038A7
		public Hash128 currentBakeSettingsHash { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000056B0 File Offset: 0x000038B0
		// (set) Token: 0x06000089 RID: 137 RVA: 0x000056B8 File Offset: 0x000038B8
		public Hash128 currentRenderingSettingsHash { get; private set; }

		// Token: 0x0600008A RID: 138 RVA: 0x000056C1 File Offset: 0x000038C1
		private void OnValidate()
		{
			this.ApplyChanges();
			this.UpdateFromV1ToV2();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000056D0 File Offset: 0x000038D0
		public void UpdateFromV1ToV2()
		{
			if (this.noiseLayer0.texture == null && this.noiseTexture0 != null)
			{
				this.noiseLayer0.texture = this.noiseTexture0;
			}
			if (this.noiseLayer1.texture == null && this.noiseTexture1 != null)
			{
				this.noiseLayer1.texture = this.noiseTexture1;
			}
			if (this.noiseLayer2.texture == null && this.noiseTexture2 != null)
			{
				this.noiseLayer2.texture = this.noiseTexture2;
			}
			if (this.noiseLayer3.texture == null && this.noiseTexture3 != null)
			{
				this.noiseLayer3.texture = this.noiseTexture3;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000057A8 File Offset: 0x000039A8
		public void ApplyToComputeShader(ComputeShader tgsComputeShader, TgsPreset.Settings settings, int csBakePassId)
		{
			tgsComputeShader.SetFloat(TgsPreset.Density, this.density * TastyGrassShaderGlobalSettings.GlobalDensityScale * settings.amount);
			tgsComputeShader.SetInt(TgsPreset.ClumpCount, this.clumpCount);
			tgsComputeShader.SetFloat(TgsPreset.ClumpPercentage, this.clumpPercentage);
			tgsComputeShader.SetFloat(TgsPreset.ClumpLimitVariation, this.clumpLimitNoiseVariation ? 1f : 0f);
			tgsComputeShader.SetFloat(TgsPreset.Scruffiness, this.scruffiness);
			tgsComputeShader.SetFloat(TgsPreset.BladeLimpness, this.limpness);
			tgsComputeShader.SetFloat(TgsPreset.TwirlStrength, this.twirl);
			tgsComputeShader.SetFloat(TgsPreset.PostVerticalScale, this.flatten);
			tgsComputeShader.SetFloat(TgsPreset.GrowDirectionBySurfaceNormal, this.growDirectionBySurfaceNormal);
			tgsComputeShader.SetFloat(TgsPreset.GrowSlopeLimit, Mathf.Cos(Mathf.Clamp(this.angleLimit + settings.slopeBias, 0f, 180f) * 0.017453292f));
			tgsComputeShader.SetVector(TgsPreset.GrowDirection, this.growDirection);
			tgsComputeShader.SetInt(TgsPreset.StemGenerate, this.stemGenerate ? 1 : 0);
			tgsComputeShader.SetVector(TgsPreset.StemColor, this.stemColor);
			tgsComputeShader.SetFloat(TgsPreset.StemThickness, this.stemThicknessRatio);
			tgsComputeShader.SetFloat(TgsPreset.BladeScaleThreshold, this.minimalHeight * settings.height);
			tgsComputeShader.SetTexture(csBakePassId, TgsPreset.NoiseTexture0, (this.noiseLayer0.texture == null) ? Texture2D.whiteTexture : this.noiseLayer0.texture);
			tgsComputeShader.SetTexture(csBakePassId, TgsPreset.NoiseTexture1, (this.noiseLayer1.texture == null) ? Texture2D.whiteTexture : this.noiseLayer1.texture);
			tgsComputeShader.SetTexture(csBakePassId, TgsPreset.NoiseTexture2, (this.noiseLayer2.texture == null) ? Texture2D.whiteTexture : this.noiseLayer2.texture);
			tgsComputeShader.SetTexture(csBakePassId, TgsPreset.NoiseTexture3, (this.noiseLayer3.texture == null) ? Texture2D.whiteTexture : this.noiseLayer3.texture);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000059CC File Offset: 0x00003BCC
		public void ApplyLayerSettingsToBuffer(ComputeBuffer buffer, TgsPreset.Settings settings)
		{
			TgsPreset.NoiseSettingGPU[] noiseLayerBuffers = TgsPreset._noiseLayerBuffers;
			if (noiseLayerBuffers == null || noiseLayerBuffers.Length != 4)
			{
				TgsPreset._noiseLayerBuffers = new TgsPreset.NoiseSettingGPU[4];
			}
			bool anySolo = this.noiseLayer0.solo || this.noiseLayer1.solo || this.noiseLayer2.solo || this.noiseLayer3.solo;
			TgsPreset._noiseLayerBuffers[0] = new TgsPreset.NoiseSettingGPU(TgsPreset.HandleNoiseSettingSolo(this.noiseLayer0, anySolo), settings);
			TgsPreset._noiseLayerBuffers[1] = new TgsPreset.NoiseSettingGPU(TgsPreset.HandleNoiseSettingSolo(this.noiseLayer1, anySolo), settings);
			TgsPreset._noiseLayerBuffers[2] = new TgsPreset.NoiseSettingGPU(TgsPreset.HandleNoiseSettingSolo(this.noiseLayer2, anySolo), settings);
			TgsPreset._noiseLayerBuffers[3] = new TgsPreset.NoiseSettingGPU(TgsPreset.HandleNoiseSettingSolo(this.noiseLayer3, anySolo), settings);
			buffer.SetData(TgsPreset._noiseLayerBuffers);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005AA8 File Offset: 0x00003CA8
		private static TgsPreset.NoiseSetting HandleNoiseSettingSolo(TgsPreset.NoiseSetting noiseSetting, bool anySolo)
		{
			if (!anySolo)
			{
				return noiseSetting;
			}
			if (!noiseSetting.solo)
			{
				return TgsPreset.NoiseSetting.GetDefault(true);
			}
			return noiseSetting;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public void ApplyToMaterialPropertyBlock(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetFloat(TgsPreset.ArcUp, this.upperArc);
			materialPropertyBlock.SetFloat(TgsPreset.ArcDown, this.lowerArc);
			materialPropertyBlock.SetFloat(TgsPreset.TipRounding, this.tipRounding);
			materialPropertyBlock.SetFloat(TgsPreset.ProceduralShapeBlend, this.proceduralShapeBlend);
			materialPropertyBlock.SetFloat(TgsPreset.Smoothness, this.smoothness);
			materialPropertyBlock.SetFloat(TgsPreset.OcclusionByHeight, this.occlusionByHeight);
			materialPropertyBlock.SetFloat(TgsPreset.WindPivotOffsetByHeight, this.windPivotOffset);
			materialPropertyBlock.SetFloat(TgsPreset.WindIntensityScale, this.windIntensityScale);
			materialPropertyBlock.SetFloat(TgsPreset.ColliderInfluence, this.colliderInfluence);
			materialPropertyBlock.SetFloat(TgsPreset.UvUseCenterTriangle, (this.textureUvLayout == TgsPreset.BladeTextureUvLayout.TriangleCenter) ? 1f : 0f);
			materialPropertyBlock.SetTexture(TgsPreset.MainTex, (this.texture == null) ? Texture2D.whiteTexture : this.texture);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005BAC File Offset: 0x00003DAC
		public void ApplyChanges()
		{
			Hash128 hash = default(Hash128);
			float val = this.density * TastyGrassShaderGlobalSettings.GlobalDensityScale;
			hash.Append(val);
			hash.Append(this.clumpCount);
			hash.Append(this.clumpPercentage);
			hash.Append<bool>(ref this.clumpLimitNoiseVariation);
			hash.Append(this.scruffiness);
			hash.Append(this.limpness);
			hash.Append(this.minimalHeight);
			hash.Append(this.twirl);
			hash.Append(this.flatten);
			hash.Append<Color>(ref this.stemColor);
			hash.Append<bool>(ref this.stemGenerate);
			hash.Append(this.stemThicknessRatio);
			Hash128 hash2 = this.noiseLayer0.GetHash128();
			hash.Append<Hash128>(ref hash2);
			Hash128 hash3 = this.noiseLayer1.GetHash128();
			hash.Append<Hash128>(ref hash3);
			Hash128 hash4 = this.noiseLayer2.GetHash128();
			hash.Append<Hash128>(ref hash4);
			Hash128 hash5 = this.noiseLayer3.GetHash128();
			hash.Append<Hash128>(ref hash5);
			hash.Append(this.growDirectionBySurfaceNormal);
			hash.Append(this.growDirection.x);
			hash.Append(this.growDirection.y);
			hash.Append(this.growDirection.z);
			hash.Append(this.angleLimit);
			if (this.currentBakeSettingsHash != hash)
			{
				foreach (TgsInstance tgsInstance in this.SetDirtyOnChangeList)
				{
					if (tgsInstance == null)
					{
						Debug.LogError("Caught null instance. This should not happen, and indicates a bug in clean up code.");
					}
					else
					{
						tgsInstance.MarkGeometryDirty();
					}
				}
			}
			this.currentBakeSettingsHash = hash;
			Hash128 hash6 = default(Hash128);
			hash6.Append<float>(ref this.upperArc);
			hash6.Append<float>(ref this.lowerArc);
			hash6.Append<float>(ref this.tipRounding);
			hash6.Append<float>(ref this.proceduralShapeBlend);
			hash6.Append<float>(ref this.smoothness);
			hash6.Append<float>(ref this.occlusionByHeight);
			hash6.Append<float>(ref this.windPivotOffset);
			hash6.Append<float>(ref this.windIntensityScale);
			hash6.Append<float>(ref this.colliderInfluence);
			hash6.Append<TgsPreset.BladeTextureUvLayout>(ref this.textureUvLayout);
			hash6.Append((this.texture == null) ? 0 : this.texture.GetInstanceID());
			if (this.currentRenderingSettingsHash != hash6)
			{
				foreach (TgsInstance tgsInstance2 in this.SetDirtyOnChangeList)
				{
					if (tgsInstance2 == null)
					{
						Debug.LogError("Caught null instance. This should not happen, and indicates a bug in clean up code.");
					}
					else
					{
						tgsInstance2.MarkMaterialDirty();
					}
				}
			}
			this.currentRenderingSettingsHash = hash6;
		}

		// Token: 0x040000CD RID: 205
		private static TgsPreset.NoiseSettingGPU[] _noiseLayerBuffers;

		// Token: 0x040000CE RID: 206
		private static readonly int Density = Shader.PropertyToID("_Density");

		// Token: 0x040000CF RID: 207
		private static readonly int ClumpCount = Shader.PropertyToID("_ClumpCount");

		// Token: 0x040000D0 RID: 208
		private static readonly int BladeLimpness = Shader.PropertyToID("_BladeLimpness");

		// Token: 0x040000D1 RID: 209
		private static readonly int GrowDirectionBySurfaceNormal = Shader.PropertyToID("_GrowDirectionBySurfaceNormal");

		// Token: 0x040000D2 RID: 210
		private static readonly int BladeScaleThreshold = Shader.PropertyToID("_BladeScaleThreshold");

		// Token: 0x040000D3 RID: 211
		private static readonly int ArcUp = Shader.PropertyToID("_ArcUp");

		// Token: 0x040000D4 RID: 212
		private static readonly int ArcDown = Shader.PropertyToID("_ArcDown");

		// Token: 0x040000D5 RID: 213
		private static readonly int TipRounding = Shader.PropertyToID("_TipRounding");

		// Token: 0x040000D6 RID: 214
		private static readonly int Smoothness = Shader.PropertyToID("_Smoothness");

		// Token: 0x040000D7 RID: 215
		private static readonly int Scruffiness = Shader.PropertyToID("_Scruffiness");

		// Token: 0x040000D8 RID: 216
		private static readonly int WindIntensityScale = Shader.PropertyToID("_WindIntensityScale");

		// Token: 0x040000D9 RID: 217
		private static readonly int NoiseTexture0 = Shader.PropertyToID("_NoiseTexture0");

		// Token: 0x040000DA RID: 218
		private static readonly int NoiseTexture1 = Shader.PropertyToID("_NoiseTexture1");

		// Token: 0x040000DB RID: 219
		private static readonly int NoiseTexture2 = Shader.PropertyToID("_NoiseTexture2");

		// Token: 0x040000DC RID: 220
		private static readonly int NoiseTexture3 = Shader.PropertyToID("_NoiseTexture3");

		// Token: 0x040000DD RID: 221
		private static readonly int GrowDirection = Shader.PropertyToID("_GrowDirection");

		// Token: 0x040000DE RID: 222
		private static readonly int WindPivotOffsetByHeight = Shader.PropertyToID("_WindPivotOffsetByHeight");

		// Token: 0x040000DF RID: 223
		private static readonly int TwirlStrength = Shader.PropertyToID("_TwirlStrength");

		// Token: 0x040000E0 RID: 224
		private static readonly int PostVerticalScale = Shader.PropertyToID("_PostVerticalScale");

		// Token: 0x040000E1 RID: 225
		private static readonly int StemGenerate = Shader.PropertyToID("_StemGenerate");

		// Token: 0x040000E2 RID: 226
		private static readonly int StemColor = Shader.PropertyToID("_StemColor");

		// Token: 0x040000E3 RID: 227
		private static readonly int StemThickness = Shader.PropertyToID("_StemThickness");

		// Token: 0x040000E4 RID: 228
		private static readonly int ColliderInfluence = Shader.PropertyToID("_ColliderInfluence");

		// Token: 0x040000E5 RID: 229
		private static readonly int GrowSlopeLimit = Shader.PropertyToID("_GrowSlopeLimit");

		// Token: 0x040000E6 RID: 230
		private static readonly int ProceduralShapeBlend = Shader.PropertyToID("_ProceduralShapeBlend");

		// Token: 0x040000E7 RID: 231
		private static readonly int FakeThicknessIntensity = Shader.PropertyToID("_FakeThicknessIntensity");

		// Token: 0x040000E8 RID: 232
		private static readonly int OcclusionByHeight = Shader.PropertyToID("_OcclusionByHeight");

		// Token: 0x040000E9 RID: 233
		private static readonly int UvUseCenterTriangle = Shader.PropertyToID("_UvUseCenterTriangle");

		// Token: 0x040000EA RID: 234
		private static readonly int MainTex = Shader.PropertyToID("_MainTex");

		// Token: 0x040000EB RID: 235
		private static readonly int ProcedualShapeParams = Shader.PropertyToID("_ProcedualShapeParams");

		// Token: 0x040000EC RID: 236
		private static readonly int ShadingParams = Shader.PropertyToID("_ShadingParams");

		// Token: 0x040000ED RID: 237
		private static readonly int UvSettings = Shader.PropertyToID("_UvSettings");

		// Token: 0x040000EE RID: 238
		private static readonly int PhysicalParamers = Shader.PropertyToID("_PhysicalParamers");

		// Token: 0x040000EF RID: 239
		private static readonly int ClumpPercentage = Shader.PropertyToID("_ClumpPercentage");

		// Token: 0x040000F0 RID: 240
		private static readonly int ClumpLimitVariation = Shader.PropertyToID("_ClumpLimitVariation");

		// Token: 0x040000F1 RID: 241
		[Header("Grow Settings")]
		[Tooltip("How much grass to create approximately per square meter.")]
		[Min(0f)]
		public float density = 60f;

		// Token: 0x040000F2 RID: 242
		[Tooltip("Maximum allowed angle between normal and grow direction.")]
		[Range(0f, 180f)]
		public float angleLimit = 85f;

		// Token: 0x040000F3 RID: 243
		[Tooltip("The growing direction of the grass. Other values than (0, 1, 0) can be used to create ground clutter.")]
		public Vector3 growDirection = Vector3.up;

		// Token: 0x040000F4 RID: 244
		[Tooltip("Whether to grow in the upwards direction (at 0.0) or the surface normals (at 1.0).")]
		[Range(0f, 1f)]
		public float growDirectionBySurfaceNormal;

		// Token: 0x040000F5 RID: 245
		[Tooltip("If a blade is smaller than this threshold, it will be removed.")]
		[Min(0f)]
		public float minimalHeight = 0.05f;

		// Token: 0x040000F6 RID: 246
		[Header("Clumping")]
		[Tooltip("How many blades should be created at the same position. Note that blades can't be created across multiple triangles.")]
		[Min(1f)]
		public int clumpCount = 2;

		// Token: 0x040000F7 RID: 247
		[Tooltip("How strong the clumping is.")]
		[Range(0f, 1f)]
		public float clumpPercentage = 0.9f;

		// Token: 0x040000F8 RID: 248
		[Tooltip("If active, noise layers will used the clumped position. This is usefully for objects that are formed from multiple blades, such as some flowers.")]
		public bool clumpLimitNoiseVariation;

		// Token: 0x040000F9 RID: 249
		[Header("Stem")]
		[Tooltip("Generates a stem. Useful for flowers or other plants. Will create one stem per clump.")]
		public bool stemGenerate;

		// Token: 0x040000FA RID: 250
		[Tooltip("The color of the stem.")]
		[DisableGroup("stemGenerate", false)]
		[ColorUsage(false)]
		public Color stemColor = new Color(0.396f, 0.506f, 0.133f);

		// Token: 0x040000FB RID: 251
		[Tooltip("The thickness of the stem in relation to the blade thickness.")]
		[DisableGroup("stemGenerate", false)]
		public float stemThicknessRatio = 0.25f;

		// Token: 0x040000FC RID: 252
		[Header("Detail Layers")]
		[Tooltip("The noise texture used for Layer 0.")]
		[Obsolete]
		[HideInInspector]
		public Texture3D noiseTexture0;

		// Token: 0x040000FD RID: 253
		[Header("Detail Layers")]
		[Tooltip("The settings used for Layer 0.")]
		public TgsPreset.NoiseSetting noiseLayer0 = new TgsPreset.NoiseSetting
		{
			tiling = 30f,
			valueScale = 3f,
			valueOffset = -0.25f,
			height = new Vector2(0.15f, 0.8f),
			offset = new Vector2(-0.35f, -0.15f),
			angle = new Vector2(0.02f, 0.2f),
			thickness = new Vector2(-0.5f, -0.5f),
			skew = new Vector2(--0f, -0.25f),
			colorInfluence = new Vector2(1f, 0.1f),
			color = new Color(0.3942137f, 0.5058824f, 0.1320353f)
		};

		// Token: 0x040000FE RID: 254
		[Space]
		[Tooltip("The noise texture used for Layer 1.")]
		[Obsolete]
		[HideInInspector]
		public Texture3D noiseTexture1;

		// Token: 0x040000FF RID: 255
		[Tooltip("The settings used for Layer 1.")]
		public TgsPreset.NoiseSetting noiseLayer1 = TgsPreset.NoiseSetting.GetDefault(true);

		// Token: 0x04000100 RID: 256
		[Space]
		[Tooltip("The noise texture used for Layer 2.")]
		[Obsolete]
		[HideInInspector]
		public Texture3D noiseTexture2;

		// Token: 0x04000101 RID: 257
		[Tooltip("The settings used for Layer 2.")]
		public TgsPreset.NoiseSetting noiseLayer2 = TgsPreset.NoiseSetting.GetDefault(true);

		// Token: 0x04000102 RID: 258
		[Space]
		[Tooltip("The noise texture used for Layer 3.")]
		[Obsolete]
		[HideInInspector]
		public Texture3D noiseTexture3;

		// Token: 0x04000103 RID: 259
		[Tooltip("The settings used for Layer 3.")]
		public TgsPreset.NoiseSetting noiseLayer3 = TgsPreset.NoiseSetting.GetDefault(true);

		// Token: 0x04000104 RID: 260
		[Header("Effects")]
		[Tooltip("How much the blade is twisted around its center. Even small values greatly enhance the voluminousness of a grass field.")]
		[Range(0f, 0.25f)]
		public float twirl = 0.03f;

		// Token: 0x04000105 RID: 261
		[FormerlySerializedAs("verticalScale")]
		[Tooltip("How much the blade vertices are flattened, based on the ground normal. Can be used to create ground clutter.")]
		public float flatten;

		// Token: 0x04000106 RID: 262
		[Tooltip("Randomization of growing direction. Also affects shading normals.")]
		[Range(0f, 2f)]
		public float scruffiness;

		// Token: 0x04000107 RID: 263
		[Tooltip("How much the tip should be moved back to the ground. Moderate values are good to create ground clutter.")]
		[Range(0f, 1f)]
		public float limpness;

		// Token: 0x04000108 RID: 264
		[Header("Blade Shape")]
		[Tooltip("The optional texture to use.")]
		[UvMappingTexture("textureUvLayout", "proceduralShapeBlend", "upperArc", "lowerArc", "tipRounding")]
		public Texture2D texture;

		// Token: 0x04000109 RID: 265
		[FormerlySerializedAs("textureUvStyle")]
		[Tooltip("What UV layout to use.")]
		public TgsPreset.BladeTextureUvLayout textureUvLayout;

		// Token: 0x0400010A RID: 266
		[Tooltip("Controls the mix between the procedural shape or the texture.")]
		[Range(0f, 1f)]
		public float proceduralShapeBlend;

		// Token: 0x0400010B RID: 267
		[Tooltip("The strength of the upper arc. Lower values lead to stronger arcs.")]
		[Range(0f, 4f)]
		public float upperArc = 0.03f;

		// Token: 0x0400010C RID: 268
		[Tooltip("The strength of the lower arc. Higher values lead to stronger arcs.")]
		[Range(0f, 4f)]
		public float lowerArc = 3.5f;

		// Token: 0x0400010D RID: 269
		[Tooltip("Values greater than 0 will introduce a more rounded shape. Helpful for stylization.")]
		[Range(0f, 0.1f)]
		public float tipRounding;

		// Token: 0x0400010E RID: 270
		[Header("Wind")]
		[Tooltip("Scaling factor for the current wind settings. A value of 0 will disable wind interaction entirely, which is useful for ground clutter.")]
		[Min(0f)]
		public float windIntensityScale = 1f;

		// Token: 0x0400010F RID: 271
		[Tooltip("The offset of the pivot along the growing direction, used for rotating the grass blades in the wind. If you create things like flowers that have height offset applied to them, use this value to make the rotation look correct again.")]
		public float windPivotOffset;

		// Token: 0x04000110 RID: 272
		[Header("Shading")]
		[Tooltip("The PBR smoothness used for shading.")]
		[Range(0f, 1f)]
		public float smoothness;

		// Token: 0x04000111 RID: 273
		[Tooltip("Strength of the occlusion factor that is guessed by the height of the grass blade.")]
		[Range(0f, 1f)]
		public float occlusionByHeight = 1f;

		// Token: 0x04000112 RID: 274
		[Space]
		[Tooltip("How strongly the blades should be pushed away by colliders.")]
		[Range(0f, 1f)]
		public float colliderInfluence = 1f;

		// Token: 0x04000113 RID: 275
		[Header("Runtime Quality")]
		[Min(0f)]
		[Tooltip("The base LOD height for this bake setting. For settings that create smaller blades, also use smaller values here.")]
		public float baseLodFactor = 1f;

		// Token: 0x04000114 RID: 276
		[Tooltip("Whether to cast shadows.")]
		public bool castShadows = true;

		// Token: 0x04000115 RID: 277
		[HideInInspector]
		[NonSerialized]
		public HashSet<TgsInstance> SetDirtyOnChangeList = new HashSet<TgsInstance>();

		// Token: 0x0200001F RID: 31
		public enum BladeTextureUvLayout
		{
			// Token: 0x04000119 RID: 281
			TriangleSingle,
			// Token: 0x0400011A RID: 282
			TriangleCenter
		}

		// Token: 0x02000020 RID: 32
		[Serializable]
		public struct NoiseSetting
		{
			// Token: 0x06000093 RID: 147 RVA: 0x00006278 File Offset: 0x00004478
			public static TgsPreset.NoiseSetting GetDefault(bool withDisabled = false)
			{
				return new TgsPreset.NoiseSetting
				{
					disable = (withDisabled ? 1 : 0),
					tiling = 1f,
					valueScale = 1f,
					color = Color.white
				};
			}

			// Token: 0x06000094 RID: 148 RVA: 0x000062C0 File Offset: 0x000044C0
			public Hash128 GetHash128()
			{
				Hash128 result = default(Hash128);
				result.Append(this.disable);
				result.Append<bool>(ref this.solo);
				result.Append((this.texture == null) ? 0 : this.texture.GetInstanceID());
				result.Append(this.tiling);
				result.Append(this.valueScale);
				result.Append(this.valueOffset);
				result.Append<Vector2>(ref this.height);
				result.Append<Vector2>(ref this.offset);
				result.Append<Vector2>(ref this.angle);
				result.Append<Vector2>(ref this.thickness);
				result.Append<Vector2>(ref this.skew);
				result.Append<Vector2>(ref this.colorInfluence);
				result.Append<Color>(ref this.color);
				return result;
			}

			// Token: 0x0400011B RID: 283
			[Tooltip("If set to true, this layer will not be applied.")]
			[IntAsCheckbox]
			public int disable;

			// Token: 0x0400011C RID: 284
			[Tooltip("Isolate this layer.")]
			public bool solo;

			// Token: 0x0400011D RID: 285
			[Tooltip("The noise texture used. Must not be null.")]
			[DisplayNoiseTexture]
			public Texture3D texture;

			// Token: 0x0400011E RID: 286
			[Space]
			[Tooltip("Tiling factor of the noise texture. Larger values will lead to more repetition. Note that the final values gets divided by 100.")]
			[DisableGroup("disable", true)]
			public float tiling;

			// Token: 0x0400011F RID: 287
			[Tooltip("Scale applied to the raw noise value texture, before being passed to the modifiers. Practically the same as a contrast modifier in image editing.")]
			[DisableGroup("disable", true)]
			public float valueScale;

			// Token: 0x04000120 RID: 288
			[Tooltip("Offset applied to the raw noise value texture, before being passed to the modifiers. Practically the same as a brightness modifier in image editing.")]
			[DisableGroup("disable", true)]
			public float valueOffset;

			// Token: 0x04000121 RID: 289
			[Header("Modifiers")]
			[Tooltip("The height of a grass blade. Negative values will internally be clamped to 0.0, so you can have negative numbers here for masking.")]
			[DisableGroup("disable", true)]
			[MinMax(-2f, 2f, true)]
			public Vector2 height;

			// Token: 0x04000122 RID: 290
			[Tooltip("How far the blade should be created from the geometry. Negative values will internally be clamped to > 0.0, so you can have negative numbers here for masking. You can use this settings to get experimental, for example to create simple flowers (see the flowers preset for that).")]
			[DisableGroup("disable", true)]
			[MinMax(-2f, 2f, true)]
			public Vector2 offset;

			// Token: 0x04000123 RID: 291
			[Space]
			[FormerlySerializedAs("width")]
			[Tooltip("The side-extending width of a blade. Negative values will internally be clamped to 0.0, so you can have negative numbers here for masking.")]
			[DisableGroup("disable", true)]
			[MinMax(-1f, 1f, true)]
			public Vector2 angle;

			// Token: 0x04000124 RID: 292
			[Space]
			[Tooltip("The thickness of a blade. Negative values will lead to inwards bended blades, while positive to outwards bended.")]
			[DisableGroup("disable", true)]
			[MinMax(-0.5f, 0.5f, false)]
			public Vector2 thickness;

			// Token: 0x04000125 RID: 293
			[FormerlySerializedAs("thicknessApex")]
			[Tooltip("The apex or mid-point of the thickness.")]
			[DisableGroup("disable", true)]
			[MinMax(-1f, 1f, false)]
			public Vector2 skew;

			// Token: 0x04000126 RID: 294
			[Space]
			[Tooltip("How much the color property will blend in color to the blade. Negative values will internally be clamped to 0.0, so you can have negative numbers here for masking.")]
			[DisableGroup("disable", true)]
			[MinMax(-1f, 1f, true)]
			public Vector2 colorInfluence;

			// Token: 0x04000127 RID: 295
			[Tooltip("The color to blend in on top of the grass blade, controlled by Color Influence.")]
			[DisableGroup("disable", true)]
			[ColorUsage(false)]
			public Color color;
		}

		// Token: 0x02000021 RID: 33
		public struct NoiseSettingGPU
		{
			// Token: 0x06000095 RID: 149 RVA: 0x00006398 File Offset: 0x00004598
			public NoiseSettingGPU(TgsPreset.NoiseSetting noiseSetting, TgsPreset.Settings settings)
			{
				this.disabled = noiseSetting.disable;
				this.tiling = noiseSetting.tiling * settings.tiling;
				this.valueScale = noiseSetting.valueScale;
				this.valueOffset = noiseSetting.valueOffset;
				this.height = noiseSetting.height * settings.height;
				this.offset = noiseSetting.offset * settings.height;
				this.width = noiseSetting.angle * settings.angle;
				this.thickness = noiseSetting.thickness * settings.thickness;
				this.thicknessApex = noiseSetting.skew;
				this.colorInfluence = noiseSetting.colorInfluence;
				float num;
				float num2;
				float v;
				Color.RGBToHSV(noiseSetting.color * settings.tint, out num, out num2, out v);
				num += settings.hueShift * 0.0027777778f;
				num = Mathf.Repeat(num, 1f);
				num2 *= settings.saturation;
				this.color = Color.HSVToRGB(num, num2, v);
			}

			// Token: 0x04000128 RID: 296
			private const int FloatFields = 4;

			// Token: 0x04000129 RID: 297
			private const int Vector2Fields = 6;

			// Token: 0x0400012A RID: 298
			private const int ColorFields = 1;

			// Token: 0x0400012B RID: 299
			public const int Stride = 80;

			// Token: 0x0400012C RID: 300
			public const int MaxCount = 4;

			// Token: 0x0400012D RID: 301
			public int disabled;

			// Token: 0x0400012E RID: 302
			public float tiling;

			// Token: 0x0400012F RID: 303
			public float valueScale;

			// Token: 0x04000130 RID: 304
			public float valueOffset;

			// Token: 0x04000131 RID: 305
			public Vector2 height;

			// Token: 0x04000132 RID: 306
			public Vector2 offset;

			// Token: 0x04000133 RID: 307
			public Vector2 width;

			// Token: 0x04000134 RID: 308
			public Vector2 thickness;

			// Token: 0x04000135 RID: 309
			public Vector2 thicknessApex;

			// Token: 0x04000136 RID: 310
			public Vector2 colorInfluence;

			// Token: 0x04000137 RID: 311
			public Color color;
		}

		// Token: 0x02000022 RID: 34
		[Serializable]
		public struct Settings
		{
			// Token: 0x06000096 RID: 150 RVA: 0x000064A0 File Offset: 0x000046A0
			public static TgsPreset.Settings GetDefault()
			{
				return new TgsPreset.Settings
				{
					height = 1f,
					tiling = 1f,
					amount = 1f,
					thickness = 1f,
					angle = 1f,
					tint = Color.white,
					saturation = 1f
				};
			}

			// Token: 0x06000097 RID: 151 RVA: 0x0000650C File Offset: 0x0000470C
			public bool HasChanged()
			{
				Hash128 hash = default(Hash128);
				Hash128 hash2 = (this.preset != null) ? this.preset.currentBakeSettingsHash : default(Hash128);
				hash.Append<Hash128>(ref hash2);
				hash.Append(this.height);
				hash.Append(this.tiling);
				hash.Append(this.amount);
				hash.Append(this.thickness);
				hash.Append(this.angle);
				hash.Append(this.slopeBias);
				hash.Append<Color>(ref this.tint);
				hash.Append(this.hueShift);
				hash.Append(this.saturation);
				hash.Append(this.camouflage);
				bool result = this._currentHash != hash;
				this._currentHash = hash;
				return result;
			}

			// Token: 0x06000098 RID: 152 RVA: 0x000065E4 File Offset: 0x000047E4
			public bool IsValid()
			{
				return this.preset != null;
			}

			// Token: 0x04000138 RID: 312
			[NotNull]
			[Tooltip("The preset used. Must not be null.")]
			public TgsPreset preset;

			// Token: 0x04000139 RID: 313
			[Header("Common")]
			[FormerlySerializedAs("density")]
			[Tooltip("Final scaling factor for the amount of the grass blades.")]
			[Min(0f)]
			public float amount;

			// Token: 0x0400013A RID: 314
			[FormerlySerializedAs("scale")]
			[Tooltip("Additional scaling factor for the height of the grass blades.")]
			[Min(0f)]
			public float height;

			// Token: 0x0400013B RID: 315
			[Tooltip("Additional scaling factor for the thickness of the grass blades.")]
			public float thickness;

			// Token: 0x0400013C RID: 316
			[Tooltip("Additional scaling factor for the thickness of the grass blades.")]
			[Min(0f)]
			public float angle;

			// Token: 0x0400013D RID: 317
			[Tooltip("The additional tiling factor for noise textures of the grass.")]
			public float tiling;

			// Token: 0x0400013E RID: 318
			[Tooltip("Additional bias for the maximal allowed slope/angle in degree.")]
			[Range(-180f, 180f)]
			public float slopeBias;

			// Token: 0x0400013F RID: 319
			[Header("Color")]
			[Tooltip("The final color tint for any grass blades.")]
			[ColorUsage(false)]
			public Color tint;

			// Token: 0x04000140 RID: 320
			[Tooltip("Shifts the hue for all colors. Useful to alter the look.")]
			[Range(-180f, 180f)]
			public float hueShift;

			// Token: 0x04000141 RID: 321
			[Tooltip("Controls the saturation of the grass. Useful to alter the look.")]
			[Range(0f, 3f)]
			public float saturation;

			// Token: 0x04000142 RID: 322
			[Space]
			[Tooltip("How much the color of the ground should be adapted. Currently only works with terrain. ")]
			[Range(0f, 1f)]
			public float camouflage;

			// Token: 0x04000143 RID: 323
			private Hash128 _currentHash;
		}
	}
}
