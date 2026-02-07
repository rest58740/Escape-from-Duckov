using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FOW
{
	// Token: 0x02000005 RID: 5
	[DefaultExecutionOrder(-100)]
	public class FogOfWarWorld : MonoBehaviour
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000029B9 File Offset: 0x00000BB9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void OnLoad()
		{
			FogOfWarWorld.ResetStatics();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000029C0 File Offset: 0x00000BC0
		private static void ResetStatics()
		{
			FogOfWarWorld.instance = null;
			FogOfWarWorld.HidersList = new List<FogOfWarHider>();
			FogOfWarWorld.PartialHiders = new List<PartialHider>();
			FogOfWarWorld.NumHiders = 0;
			FogOfWarWorld.RevealersToRegister = new List<FogOfWarRevealer>();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000029EC File Offset: 0x00000BEC
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000029F4 File Offset: 0x00000BF4
		private void OnEnable()
		{
			this.Initialize();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000029FC File Offset: 0x00000BFC
		private void OnDisable()
		{
			this.Cleanup();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A04 File Offset: 0x00000C04
		private void OnDestroy()
		{
			this.Cleanup();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A0C File Offset: 0x00000C0C
		private void Update()
		{
			if (this.UpdateMethod == FogOfWarWorld.FowUpdateMethod.Update)
			{
				this.CalculateFOW();
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002A1C File Offset: 0x00000C1C
		private void LateUpdate()
		{
			if (this.UpdateMethod == FogOfWarWorld.FowUpdateMethod.LateUpdate)
			{
				this.CalculateFOW();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002A30 File Offset: 0x00000C30
		private void CalculateFOW()
		{
			if (FogOfWarWorld._numRevealers > 0)
			{
				switch (this.RevealerUpdateMode)
				{
				case FogOfWarWorld.RevealerUpdateMethod.Every_Frame:
					for (int i = 0; i < FogOfWarWorld._numRevealers; i++)
					{
						FogOfWarWorld.Revealers[i].RevealHiders();
						if (!FogOfWarWorld.Revealers[i].StaticRevealer)
						{
							FogOfWarWorld.Revealers[i].LineOfSightPhase1();
						}
					}
					for (int j = 0; j < FogOfWarWorld._numRevealers; j++)
					{
						if (!FogOfWarWorld.Revealers[j].StaticRevealer)
						{
							FogOfWarWorld.Revealers[j].LineOfSightPhase2();
						}
					}
					break;
				case FogOfWarWorld.RevealerUpdateMethod.N_Per_Frame:
				{
					int num = this.currentIndex;
					for (int k = 0; k < Mathf.Clamp(this.MaxNumRevealersPerFrame, 0, FogOfWarWorld.numDynamicRevealers); k++)
					{
						num = (num + 1) % FogOfWarWorld._numRevealers;
						FogOfWarWorld.Revealers[num].RevealHiders();
						if (!FogOfWarWorld.Revealers[num].StaticRevealer)
						{
							FogOfWarWorld.Revealers[num].LineOfSightPhase1();
						}
						else
						{
							k--;
						}
					}
					for (int l = 0; l < Mathf.Clamp(this.MaxNumRevealersPerFrame, 0, FogOfWarWorld.numDynamicRevealers); l++)
					{
						this.currentIndex = (this.currentIndex + 1) % FogOfWarWorld._numRevealers;
						if (!FogOfWarWorld.Revealers[this.currentIndex].StaticRevealer)
						{
							FogOfWarWorld.Revealers[this.currentIndex].LineOfSightPhase2();
						}
						else
						{
							l--;
						}
					}
					break;
				}
				}
			}
			if (this.UseMiniMap || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Texture || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Both)
			{
				if (this.UseRegrow)
				{
					Graphics.Blit(this.FOW_RT, this.FOW_TEMP_RT);
					Graphics.Blit(this.FOW_TEMP_RT, this.FOW_RT, this.FowTextureMaterial, 0);
					return;
				}
				Graphics.Blit(null, this.FOW_RT, this.FowTextureMaterial, 0);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002BEC File Offset: 0x00000DEC
		private void Cleanup()
		{
			Shader.SetGlobalFloat("FowEffectStrength", 0f);
			int numRevealers = FogOfWarWorld._numRevealers;
			for (int i = 0; i < numRevealers; i++)
			{
				FogOfWarRevealer fogOfWarRevealer = FogOfWarWorld.Revealers[0];
				fogOfWarRevealer.DeregisterRevealer();
				FogOfWarWorld.RevealersToRegister.Add(fogOfWarRevealer);
			}
			if (FogOfWarWorld.CircleBuffer != null)
			{
				FogOfWarWorld.IndicesBuffer.Dispose();
				FogOfWarWorld.CircleBuffer.Dispose();
				FogOfWarWorld.AnglesBuffer.Dispose();
			}
			FogOfWarWorld.instance = null;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002C60 File Offset: 0x00000E60
		public void Initialize()
		{
			if (FogOfWarWorld.instance != null)
			{
				return;
			}
			FogOfWarWorld.instance = this;
			Shader.SetGlobalFloat("FowEffectStrength", 1f);
			FogOfWarWorld.maxCones = this.MaxPossibleRevealers * this.MaxPossibleSegmentsPerRevealer;
			FogOfWarWorld.Revealers = new FogOfWarRevealer[this.MaxPossibleRevealers];
			FogOfWarWorld.IndicesBuffer = new ComputeBuffer(this.MaxPossibleRevealers, Marshal.SizeOf(typeof(int)), ComputeBufferType.Default);
			FogOfWarWorld.CircleBuffer = new ComputeBuffer(this.MaxPossibleRevealers, Marshal.SizeOf(typeof(FogOfWarWorld.RevealerStruct)), ComputeBufferType.Default);
			this.anglesArray = new FogOfWarWorld.ConeEdgeStruct[this.MaxPossibleSegmentsPerRevealer];
			FogOfWarWorld.AnglesBuffer = new ComputeBuffer(FogOfWarWorld.maxCones, Marshal.SizeOf(typeof(FogOfWarWorld.ConeEdgeStruct)), ComputeBufferType.Default);
			this.FogOfWarMaterial = new Material(Shader.Find("Hidden/FullScreen/FOW/SolidColor"));
			if (this.UseMiniMap || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Texture || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Both)
			{
				this.FowTextureMaterial = new Material(Shader.Find("Hidden/FullScreen/FOW/FOW_RT"));
				this.InitFOWRT();
				this.UpdateMaterialProperties(this.FowTextureMaterial);
				this.FowTextureMaterial.SetBuffer(Shader.PropertyToID("_ActiveCircleIndices"), FogOfWarWorld.IndicesBuffer);
				this.FowTextureMaterial.SetBuffer(Shader.PropertyToID("_CircleBuffer"), FogOfWarWorld.CircleBuffer);
				this.FowTextureMaterial.SetBuffer(Shader.PropertyToID("_ConeBuffer"), FogOfWarWorld.AnglesBuffer);
				this.FowTextureMaterial.EnableKeyword("IGNORE_HEIGHT");
			}
			this.SetFogShader();
			this.UpdateAllMaterialProperties();
			this.SetAllMaterialBounds();
			foreach (FogOfWarRevealer fogOfWarRevealer in FogOfWarWorld.RevealersToRegister)
			{
				if (fogOfWarRevealer != null)
				{
					fogOfWarRevealer.RegisterRevealer();
				}
			}
			FogOfWarWorld.RevealersToRegister.Clear();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002E44 File Offset: 0x00001044
		public void SetFogShader()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.UsingSoftening = false;
			string text = "Hidden/FullScreen/FOW";
			switch (this.FogAppearance)
			{
			case FogOfWarWorld.FogOfWarAppearance.Solid_Color:
				text += "/SolidColor";
				break;
			case FogOfWarWorld.FogOfWarAppearance.GrayScale:
				text += "/GrayScale";
				break;
			case FogOfWarWorld.FogOfWarAppearance.Blur:
				text += "/Blur";
				break;
			case FogOfWarWorld.FogOfWarAppearance.Texture_Sample:
				text += "/TextureSample";
				break;
			case FogOfWarWorld.FogOfWarAppearance.Outline:
				text += "/Outline";
				break;
			case FogOfWarWorld.FogOfWarAppearance.None:
				text = "Hidden/BlitCopy";
				break;
			}
			this.FogOfWarMaterial.shader = Shader.Find(text);
			this.InitializeFogProperties(this.FogOfWarMaterial);
			this.UpdateMaterialProperties(this.FogOfWarMaterial);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002F04 File Offset: 0x00001104
		public void InitializeFogProperties(Material material)
		{
			material.DisableKeyword("IS_2D");
			material.DisableKeyword("IS_3D");
			if (!this.is2D)
			{
				material.EnableKeyword("IS_3D");
				switch (this.gamePlane)
				{
				case FogOfWarWorld.GamePlane.XZ:
					material.SetInt("_fowPlane", 1);
					FogOfWarWorld.UpVector = Vector3.up;
					break;
				case FogOfWarWorld.GamePlane.XY:
					material.SetInt("_fowPlane", 2);
					FogOfWarWorld.UpVector = -Vector3.forward;
					break;
				case FogOfWarWorld.GamePlane.ZY:
					material.SetInt("_fowPlane", 3);
					FogOfWarWorld.UpVector = Vector3.right;
					break;
				}
			}
			else
			{
				FogOfWarWorld.UpVector = -Vector3.forward;
				material.EnableKeyword("IS_2D");
				material.SetInt("_fowPlane", 0);
			}
			material.SetBuffer(Shader.PropertyToID("_ActiveCircleIndices"), FogOfWarWorld.IndicesBuffer);
			material.SetBuffer(Shader.PropertyToID("_CircleBuffer"), FogOfWarWorld.CircleBuffer);
			material.SetBuffer(Shader.PropertyToID("_ConeBuffer"), FogOfWarWorld.AnglesBuffer);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003008 File Offset: 0x00001208
		public void UpdateAllMaterialProperties()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.UpdateMaterialProperties(this.FogOfWarMaterial);
			if (this.FowTextureMaterial != null)
			{
				this.UpdateMaterialProperties(this.FowTextureMaterial);
			}
			foreach (PartialHider partialHider in FogOfWarWorld.PartialHiders)
			{
				this.UpdateMaterialProperties(partialHider.HiderMaterial);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003090 File Offset: 0x00001290
		public void UpdateMaterialProperties(Material material)
		{
			material.DisableKeyword("HARD");
			material.DisableKeyword("SOFT");
			this.UsingSoftening = false;
			FogOfWarWorld.FogOfWarType fogType = this.FogType;
			if (fogType != FogOfWarWorld.FogOfWarType.Hard)
			{
				if (fogType == FogOfWarWorld.FogOfWarType.Soft)
				{
					material.EnableKeyword("SOFT");
					this.UsingSoftening = true;
				}
			}
			else
			{
				material.EnableKeyword("HARD");
			}
			material.SetInt("BLEED", 0);
			if (this.AllowBleeding)
			{
				material.SetInt("BLEED", 1);
			}
			material.SetColor(this.materialColorID, (material == this.FowTextureMaterial) ? this.MiniMapColor : this.UnknownColor);
			material.SetFloat(this.unobscuredBlurRadiusID, this.UnobscuredSoftenDistance);
			material.DisableKeyword("INNER_SOFTEN");
			if (this.FogType == FogOfWarWorld.FogOfWarType.Soft && this.UseInnerSoften)
			{
				material.EnableKeyword("INNER_SOFTEN");
				material.SetFloat(Shader.PropertyToID("_fadeOutDegrees"), this.InnerSoftenAngle);
			}
			else
			{
				material.SetFloat(Shader.PropertyToID("_fadeOutDegrees"), 0f);
			}
			material.SetFloat(this.extraRadiusID, this.SightExtraAmount);
			material.SetFloat(Shader.PropertyToID("_edgeSoftenDistance"), this.EdgeSoftenDistance);
			material.SetFloat(this.maxDistanceID, this.MaxFogDistance);
			material.SetInt("_pixelate", 0);
			if (this.PixelateFog && !this.WorldSpacePixelate)
			{
				material.SetInt("_pixelate", 1);
			}
			material.SetInt("_pixelateWS", 0);
			if (this.PixelateFog && this.WorldSpacePixelate)
			{
				material.SetInt("_pixelateWS", 1);
			}
			if (this.PixelateFog)
			{
				material.SetFloat(this.extraRadiusID, this.SightExtraAmount + 1f / this.PixelDensity);
			}
			material.SetFloat("_pixelDensity", this.PixelDensity);
			material.SetVector("_pixelOffset", this.PixelGridOffset);
			material.SetInt("_ditherFog", 0);
			if (this.UseDithering)
			{
				material.SetInt("_ditherFog", 1);
			}
			material.SetFloat("_ditherSize", this.DitherSize);
			material.SetInt("_invertEffect", 0);
			if (this.InvertFowEffect)
			{
				material.SetInt("_invertEffect", 1);
			}
			switch (this.FogFade)
			{
			case FogOfWarWorld.FogOfWarFadeType.Linear:
				material.SetInt("_fadeType", 0);
				break;
			case FogOfWarWorld.FogOfWarFadeType.Exponential:
				material.SetInt("_fadeType", 4);
				material.SetFloat(this.fadePowerID, this.FogFadePower);
				break;
			case FogOfWarWorld.FogOfWarFadeType.Smooth:
				material.SetInt("_fadeType", 1);
				break;
			case FogOfWarWorld.FogOfWarFadeType.Smoother:
				material.SetInt("_fadeType", 2);
				break;
			case FogOfWarWorld.FogOfWarFadeType.Smoothstep:
				material.SetInt("_fadeType", 3);
				break;
			}
			material.SetInt("BLEND_MAX", 1);
			FogOfWarWorld.FogOfWarBlendMode blendType = this.BlendType;
			if (blendType != FogOfWarWorld.FogOfWarBlendMode.Max)
			{
				if (blendType == FogOfWarWorld.FogOfWarBlendMode.Addative)
				{
					material.SetInt("BLEND_MAX", 0);
				}
			}
			else
			{
				material.SetInt("BLEND_MAX", 1);
			}
			switch (this.FogAppearance)
			{
			case FogOfWarWorld.FogOfWarAppearance.GrayScale:
				material.SetFloat(this.saturationStrengthID, this.SaturationStrength);
				break;
			case FogOfWarWorld.FogOfWarAppearance.Blur:
				material.SetFloat(this.blurStrengthID, this.BlurStrength);
				material.SetFloat(this.blurPixelOffsetMinID, (float)Screen.height * (this.BlurDistanceScreenPercentMin / 100f));
				material.SetFloat(this.blurPixelOffsetMaxID, (float)Screen.height * (this.BlurDistanceScreenPercentMax / 100f));
				material.SetInt(this.blurSamplesID, this.BlurSamples);
				material.SetFloat(this.blurPeriodID, 6.2831855f / (float)this.BlurSamples);
				break;
			case FogOfWarWorld.FogOfWarAppearance.Texture_Sample:
				material.SetTexture(this.fowTetureID, this.FogTexture);
				material.SetInt("_skipTriplanar", 0);
				if (!this.UseTriplanar)
				{
					material.SetInt("_skipTriplanar", 1);
					material.SetVector("_fowAxis", FogOfWarWorld.UpVector);
				}
				material.SetVector(this.fowTilingID, this.FogTextureTiling);
				material.SetVector(this.fowSpeedID, this.FogScrollSpeed);
				break;
			case FogOfWarWorld.FogOfWarAppearance.Outline:
				material.SetFloat("lineThickness", this.OutlineThickness);
				break;
			}
			material.DisableKeyword("SAMPLE_REALTIME");
			if (this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Pixel_Perfect || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Both)
			{
				material.EnableKeyword("SAMPLE_REALTIME");
			}
			material.DisableKeyword("SAMPLE_TEXTURE");
			material.DisableKeyword("USE_TEXTURE_BLUR");
			if (this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Texture || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Both)
			{
				material.SetTexture("_FowRT", this.FOW_RT);
				material.EnableKeyword("SAMPLE_TEXTURE");
				if (this.UseConstantBlur)
				{
					material.EnableKeyword("USE_TEXTURE_BLUR");
					material.SetFloat("_Sample_Blur_Quality", (float)this.ConstantTextureBlurQuality);
					material.SetFloat("_Sample_Blur_Amount", this.ConstantTextureBlurAmount);
				}
			}
			if (material == this.FowTextureMaterial)
			{
				material.SetFloat("_regrowSpeed", this.FogRegrowSpeed);
				material.SetFloat("_maxRegrowAmount", this.MaxFogRegrowAmount);
				material.EnableKeyword("SAMPLE_REALTIME");
				material.DisableKeyword("SAMPLE_TEXTURE");
				material.DisableKeyword("USE_REGROW");
				if (this.UseRegrow)
				{
					material.EnableKeyword("USE_REGROW");
					material.DisableKeyword("USE_FADEIN");
					if (this.RevealerFadeIn)
					{
						material.EnableKeyword("USE_FADEIN");
					}
				}
			}
			material.DisableKeyword("USE_WORLD_BOUNDS");
			if (this.UseRegrow)
			{
				material.EnableKeyword("USE_WORLD_BOUNDS");
			}
			material.SetFloat("_worldBoundsInfluence", 0f);
			if (this.UseWorldBounds)
			{
				material.SetFloat("_worldBoundsSoftenDistance", this.WorldBoundsSoftenDistance);
				material.SetFloat("_worldBoundsInfluence", this.WorldBoundsInfluence);
			}
			this.SetMaterialBounds(material);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003636 File Offset: 0x00001836
		public void UpdateWorldBounds(Vector3 center, Vector3 extent)
		{
			this.WorldBounds.center = center;
			this.WorldBounds.extents = extent;
			this.SetAllMaterialBounds();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003656 File Offset: 0x00001856
		public void UpdateWorldBounds(Bounds newBounds)
		{
			this.WorldBounds = newBounds;
			this.SetAllMaterialBounds();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003665 File Offset: 0x00001865
		private void SetAllMaterialBounds()
		{
			if (this.FogOfWarMaterial != null)
			{
				this.SetMaterialBounds(this.FogOfWarMaterial);
			}
			if (this.FowTextureMaterial != null)
			{
				this.SetMaterialBounds(this.FowTextureMaterial);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000369C File Offset: 0x0000189C
		private void SetMaterialBounds(Material mat)
		{
			Vector4 boundsVectorForShader = this.GetBoundsVectorForShader();
			if (mat != null)
			{
				mat.SetVector("_worldBounds", boundsVectorForShader);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000036C8 File Offset: 0x000018C8
		public Vector4 GetBoundsVectorForShader()
		{
			if (this.is2D)
			{
				return new Vector4(this.WorldBounds.size.x, this.WorldBounds.center.x, this.WorldBounds.size.y, this.WorldBounds.center.y);
			}
			switch (this.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return new Vector4(this.WorldBounds.size.x, this.WorldBounds.center.x, this.WorldBounds.size.z, this.WorldBounds.center.z);
			case FogOfWarWorld.GamePlane.XY:
				return new Vector4(this.WorldBounds.size.x, this.WorldBounds.center.x, this.WorldBounds.size.y, this.WorldBounds.center.y);
			case FogOfWarWorld.GamePlane.ZY:
				return new Vector4(this.WorldBounds.size.z, this.WorldBounds.center.z, this.WorldBounds.size.z, this.WorldBounds.center.z);
			default:
				return new Vector4(this.WorldBounds.size.x, this.WorldBounds.center.x, this.WorldBounds.size.z, this.WorldBounds.center.z);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003858 File Offset: 0x00001A58
		public Vector2 GetFowPositionFromWorldPosition(Vector3 WorldPosition)
		{
			if (this.is2D)
			{
				return new Vector2(WorldPosition.x, WorldPosition.y);
			}
			switch (this.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return new Vector2(WorldPosition.x, WorldPosition.z);
			case FogOfWarWorld.GamePlane.XY:
				return new Vector2(WorldPosition.x, WorldPosition.y);
			case FogOfWarWorld.GamePlane.ZY:
				return new Vector2(WorldPosition.z, WorldPosition.y);
			default:
				return new Vector2(WorldPosition.x, WorldPosition.z);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000038E4 File Offset: 0x00001AE4
		private void SetNumRevealers()
		{
			if (this.FogOfWarMaterial != null)
			{
				this.SetNumRevealers(this.FogOfWarMaterial);
			}
			if (this.FowTextureMaterial != null)
			{
				this.SetNumRevealers(this.FowTextureMaterial);
			}
			foreach (PartialHider partialHider in FogOfWarWorld.PartialHiders)
			{
				this.SetNumRevealers(partialHider.HiderMaterial);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003970 File Offset: 0x00001B70
		public void SetNumRevealers(Material material)
		{
			material.SetInt(this.numRevealersID, FogOfWarWorld._numRevealers);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003984 File Offset: 0x00001B84
		public int RegisterRevealer(FogOfWarRevealer newRevealer)
		{
			FogOfWarWorld._numRevealers++;
			if (!newRevealer.StaticRevealer)
			{
				FogOfWarWorld.numDynamicRevealers++;
			}
			this.SetNumRevealers();
			int num = FogOfWarWorld._numRevealers - 1;
			FogOfWarWorld.Revealers[num] = newRevealer;
			if (FogOfWarWorld.numDeregistered > 0)
			{
				FogOfWarWorld.numDeregistered--;
				num = FogOfWarWorld.DeregisteredIDs[0];
				FogOfWarWorld.DeregisteredIDs.RemoveAt(0);
			}
			newRevealer.IndexID = FogOfWarWorld._numRevealers - 1;
			FogOfWarWorld.indiciesDataToSet[0] = num;
			FogOfWarWorld.IndicesBuffer.SetData(FogOfWarWorld.indiciesDataToSet, 0, FogOfWarWorld._numRevealers - 1, 1);
			return num;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003A20 File Offset: 0x00001C20
		public void DeRegisterRevealer(FogOfWarRevealer toRemove)
		{
			int indexID = toRemove.IndexID;
			FogOfWarWorld.DeregisteredIDs.Add(toRemove.FogOfWarID);
			FogOfWarWorld.numDeregistered++;
			FogOfWarWorld._numRevealers--;
			if (!toRemove.StaticRevealer)
			{
				FogOfWarWorld.numDynamicRevealers--;
			}
			FogOfWarRevealer fogOfWarRevealer = FogOfWarWorld.Revealers[FogOfWarWorld._numRevealers];
			if (toRemove != fogOfWarRevealer)
			{
				FogOfWarWorld.Revealers[indexID] = fogOfWarRevealer;
				FogOfWarWorld.indiciesDataToSet[0] = fogOfWarRevealer.FogOfWarID;
				FogOfWarWorld.IndicesBuffer.SetData(FogOfWarWorld.indiciesDataToSet, 0, indexID, 1);
				fogOfWarRevealer.IndexID = indexID;
			}
			this.SetNumRevealers();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003ABC File Offset: 0x00001CBC
		public void UpdateRevealerData(int id, FogOfWarWorld.RevealerStruct data, int numHits, float[] radii, float[] distances, bool[] hits)
		{
			data.StartIndex = id * this.MaxPossibleSegmentsPerRevealer;
			this._revealerDataToSet[0] = data;
			FogOfWarWorld.CircleBuffer.SetData(this._revealerDataToSet, 0, id, 1);
			if (numHits > this.MaxPossibleSegmentsPerRevealer)
			{
				Debug.LogError(string.Format("the revealer is trying to register {0} segments. this is more than was set by maxPossibleSegmentsPerRevealer", numHits));
				return;
			}
			for (int i = 0; i < numHits; i++)
			{
				this.anglesArray[i].angle = radii[i];
				this.anglesArray[i].length = distances[i];
				this.anglesArray[i].cutShort = (hits[i] ? 1 : 0);
			}
			FogOfWarWorld.AnglesBuffer.SetData(this.anglesArray, 0, id * this.MaxPossibleSegmentsPerRevealer, numHits);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003B84 File Offset: 0x00001D84
		public static bool TestPointVisibility(Vector3 point)
		{
			for (int i = 0; i < FogOfWarWorld._numRevealers; i++)
			{
				if (FogOfWarWorld.Revealers[i].TestPoint(point))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003BB3 File Offset: 0x00001DB3
		public void SetFowAppearance(FogOfWarWorld.FogOfWarAppearance AppearanceMode)
		{
			this.FogAppearance = AppearanceMode;
			if (!Application.isPlaying)
			{
				return;
			}
			base.enabled = false;
			base.enabled = true;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003BD2 File Offset: 0x00001DD2
		public FogOfWarWorld.FogOfWarAppearance GetFowAppearance()
		{
			return this.FogAppearance;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003BDC File Offset: 0x00001DDC
		public void InitFOWRT()
		{
			RenderTexture active = RenderTexture.active;
			RenderTextureFormat format = RenderTextureFormat.ARGBHalf;
			this.FOW_RT = new RenderTexture(this.FowResX, this.FowResY, 0, format, RenderTextureReadWrite.Linear);
			this.FOW_RT.antiAliasing = 8;
			this.FOW_RT.filterMode = FilterMode.Trilinear;
			this.FOW_RT.anisoLevel = 9;
			this.FOW_RT.Create();
			RenderTexture.active = this.FOW_RT;
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 1f - this.InitialFogExplorationValue));
			GL.End();
			if (this.UseMiniMap && this.UIImage != null)
			{
				this.UIImage.texture = this.FOW_RT;
			}
			if (this.UseRegrow)
			{
				this.FOW_TEMP_RT = new RenderTexture(this.FOW_RT);
				this.FOW_TEMP_RT.Create();
			}
			RenderTexture.active = active;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003CC9 File Offset: 0x00001EC9
		public RenderTexture GetFOWRT()
		{
			return this.FOW_RT;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003CD1 File Offset: 0x00001ED1
		[Obsolete("Please use ClearFowTexture() instead")]
		public void ClearRegrowTexture()
		{
			this.ClearFowTexture();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003CDC File Offset: 0x00001EDC
		public void ClearFowTexture()
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.FOW_RT;
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 1f - this.InitialFogExplorationValue));
			GL.End();
			RenderTexture.active = this.FOW_TEMP_RT;
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 1f - this.InitialFogExplorationValue));
			GL.End();
			RenderTexture.active = active;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003D70 File Offset: 0x00001F70
		public byte[] GetFowTextureSaveData()
		{
			Texture2D texture2D = new Texture2D(this.FOW_RT.width, this.FOW_RT.height, TextureFormat.RGBAHalf, false, true);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.FOW_RT;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)this.FOW_RT.width, (float)this.FOW_RT.height), 0, 0, false);
			texture2D.Apply(false, false);
			RenderTexture.active = active;
			UnityEngine.Object.Destroy(texture2D);
			return texture2D.EncodeToPNG();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003DF6 File Offset: 0x00001FF6
		public void LoadFowTextureData(byte[] save)
		{
			this.ClearFowTexture();
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBAHalf, false, true);
			texture2D.LoadImage(save);
			Graphics.Blit(texture2D, this.FOW_RT);
		}

		// Token: 0x04000012 RID: 18
		public static FogOfWarWorld instance;

		// Token: 0x04000013 RID: 19
		public FogOfWarWorld.FowUpdateMethod UpdateMethod = FogOfWarWorld.FowUpdateMethod.LateUpdate;

		// Token: 0x04000014 RID: 20
		public bool UsingSoftening;

		// Token: 0x04000015 RID: 21
		public FogOfWarWorld.FogOfWarType FogType = FogOfWarWorld.FogOfWarType.Soft;

		// Token: 0x04000016 RID: 22
		public FogOfWarWorld.FogOfWarFadeType FogFade = FogOfWarWorld.FogOfWarFadeType.Smoothstep;

		// Token: 0x04000017 RID: 23
		public FogOfWarWorld.FogOfWarBlendMode BlendType;

		// Token: 0x04000018 RID: 24
		public float EdgeSoftenDistance = 0.1f;

		// Token: 0x04000019 RID: 25
		public float UnobscuredSoftenDistance = 0.25f;

		// Token: 0x0400001A RID: 26
		public bool UseInnerSoften = true;

		// Token: 0x0400001B RID: 27
		public float InnerSoftenAngle = 5f;

		// Token: 0x0400001C RID: 28
		public bool AllowBleeding;

		// Token: 0x0400001D RID: 29
		public float SightExtraAmount = 0.01f;

		// Token: 0x0400001E RID: 30
		public float MaxFogDistance = 10000f;

		// Token: 0x0400001F RID: 31
		public bool PixelateFog;

		// Token: 0x04000020 RID: 32
		public bool WorldSpacePixelate;

		// Token: 0x04000021 RID: 33
		public float PixelDensity = 2f;

		// Token: 0x04000022 RID: 34
		public bool RoundRevealerPosition;

		// Token: 0x04000023 RID: 35
		public Vector2 PixelGridOffset;

		// Token: 0x04000024 RID: 36
		public bool UseDithering;

		// Token: 0x04000025 RID: 37
		public float DitherSize = 20f;

		// Token: 0x04000026 RID: 38
		public bool InvertFowEffect;

		// Token: 0x04000027 RID: 39
		public float FogFadePower = 1f;

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private FogOfWarWorld.FogOfWarAppearance FogAppearance;

		// Token: 0x04000029 RID: 41
		[Tooltip("The color of the fog")]
		public Color UnknownColor = new Color(0.35f, 0.35f, 0.35f);

		// Token: 0x0400002A RID: 42
		public float SaturationStrength;

		// Token: 0x0400002B RID: 43
		public float BlurStrength = 1f;

		// Token: 0x0400002C RID: 44
		[Range(0f, 2f)]
		public float BlurDistanceScreenPercentMin = 0.1f;

		// Token: 0x0400002D RID: 45
		[Range(0f, 2f)]
		public float BlurDistanceScreenPercentMax = 1f;

		// Token: 0x0400002E RID: 46
		public int BlurSamples = 6;

		// Token: 0x0400002F RID: 47
		public Texture2D FogTexture;

		// Token: 0x04000030 RID: 48
		public bool UseTriplanar = true;

		// Token: 0x04000031 RID: 49
		public Vector2 FogTextureTiling = Vector2.one;

		// Token: 0x04000032 RID: 50
		public Vector2 FogScrollSpeed = Vector2.one;

		// Token: 0x04000033 RID: 51
		public float OutlineThickness = 0.1f;

		// Token: 0x04000034 RID: 52
		public FogOfWarWorld.FogSampleMode FOWSamplingMode;

		// Token: 0x04000035 RID: 53
		public bool UseRegrow;

		// Token: 0x04000036 RID: 54
		public bool RevealerFadeIn;

		// Token: 0x04000037 RID: 55
		public float FogRegrowSpeed = 0.5f;

		// Token: 0x04000038 RID: 56
		public float InitialFogExplorationValue;

		// Token: 0x04000039 RID: 57
		public float MaxFogRegrowAmount = 0.3f;

		// Token: 0x0400003A RID: 58
		private RenderTexture FOW_RT;

		// Token: 0x0400003B RID: 59
		private RenderTexture FOW_TEMP_RT;

		// Token: 0x0400003C RID: 60
		public Material FowTextureMaterial;

		// Token: 0x0400003D RID: 61
		public int FowResX = 256;

		// Token: 0x0400003E RID: 62
		public int FowResY = 256;

		// Token: 0x0400003F RID: 63
		public bool UseConstantBlur = true;

		// Token: 0x04000040 RID: 64
		public int ConstantTextureBlurQuality = 2;

		// Token: 0x04000041 RID: 65
		public float ConstantTextureBlurAmount = 0.75f;

		// Token: 0x04000042 RID: 66
		public bool UseWorldBounds;

		// Token: 0x04000043 RID: 67
		public float WorldBoundsSoftenDistance = 1f;

		// Token: 0x04000044 RID: 68
		public float WorldBoundsInfluence = 1f;

		// Token: 0x04000045 RID: 69
		public Bounds WorldBounds = new Bounds(Vector3.zero, Vector3.one);

		// Token: 0x04000046 RID: 70
		public bool UseMiniMap;

		// Token: 0x04000047 RID: 71
		public Color MiniMapColor = new Color(0.4f, 0.4f, 0.4f, 0.95f);

		// Token: 0x04000048 RID: 72
		public RawImage UIImage;

		// Token: 0x04000049 RID: 73
		[FormerlySerializedAs("revealerMode")]
		public FogOfWarWorld.RevealerUpdateMethod RevealerUpdateMode;

		// Token: 0x0400004A RID: 74
		[Tooltip("The number of revealers to update each frame. Only used when Revealer Mode is set to N_Per_Frame")]
		public int MaxNumRevealersPerFrame = 3;

		// Token: 0x0400004B RID: 75
		[Tooltip("The Max possible number of revealers. Keep this as low as possible to use less GPU memory")]
		public int MaxPossibleRevealers = 256;

		// Token: 0x0400004C RID: 76
		[Tooltip("The Max possible number of segments per revealer. Keep this as low as possible to use less GPU memory")]
		public int MaxPossibleSegmentsPerRevealer = 128;

		// Token: 0x0400004D RID: 77
		public bool is2D;

		// Token: 0x0400004E RID: 78
		public FogOfWarWorld.GamePlane gamePlane;

		// Token: 0x0400004F RID: 79
		public Material FogOfWarMaterial;

		// Token: 0x04000050 RID: 80
		private static int maxCones;

		// Token: 0x04000051 RID: 81
		public static ComputeBuffer IndicesBuffer;

		// Token: 0x04000052 RID: 82
		public static ComputeBuffer CircleBuffer;

		// Token: 0x04000053 RID: 83
		public static ComputeBuffer AnglesBuffer;

		// Token: 0x04000054 RID: 84
		public static FogOfWarRevealer[] Revealers;

		// Token: 0x04000055 RID: 85
		private static int _numRevealers;

		// Token: 0x04000056 RID: 86
		public static int numDynamicRevealers;

		// Token: 0x04000057 RID: 87
		public static List<FogOfWarHider> HidersList = new List<FogOfWarHider>();

		// Token: 0x04000058 RID: 88
		public static List<PartialHider> PartialHiders = new List<PartialHider>();

		// Token: 0x04000059 RID: 89
		public static int NumHiders;

		// Token: 0x0400005A RID: 90
		public static List<FogOfWarRevealer> RevealersToRegister = new List<FogOfWarRevealer>();

		// Token: 0x0400005B RID: 91
		public static List<int> DeregisteredIDs = new List<int>();

		// Token: 0x0400005C RID: 92
		private static int numDeregistered = 0;

		// Token: 0x0400005D RID: 93
		private static int[] indiciesDataToSet = new int[1];

		// Token: 0x0400005E RID: 94
		private int numRevealersID = Shader.PropertyToID("_NumRevealers");

		// Token: 0x0400005F RID: 95
		private int materialColorID = Shader.PropertyToID("_unKnownColor");

		// Token: 0x04000060 RID: 96
		private int unobscuredBlurRadiusID = Shader.PropertyToID("_unboscuredFadeOutDistance");

		// Token: 0x04000061 RID: 97
		private int extraRadiusID = Shader.PropertyToID("_extraRadius");

		// Token: 0x04000062 RID: 98
		private int maxDistanceID = Shader.PropertyToID("_maxDistance");

		// Token: 0x04000063 RID: 99
		private int fadePowerID = Shader.PropertyToID("_fadePower");

		// Token: 0x04000064 RID: 100
		private int saturationStrengthID = Shader.PropertyToID("_saturationStrength");

		// Token: 0x04000065 RID: 101
		private int blurStrengthID = Shader.PropertyToID("_blurStrength");

		// Token: 0x04000066 RID: 102
		private int blurPixelOffsetMinID = Shader.PropertyToID("_blurPixelOffsetMin");

		// Token: 0x04000067 RID: 103
		private int blurPixelOffsetMaxID = Shader.PropertyToID("_blurPixelOffsetMax");

		// Token: 0x04000068 RID: 104
		private int blurSamplesID = Shader.PropertyToID("_blurSamples");

		// Token: 0x04000069 RID: 105
		private int blurPeriodID = Shader.PropertyToID("_samplePeriod");

		// Token: 0x0400006A RID: 106
		private int fowTetureID = Shader.PropertyToID("_fowTexture");

		// Token: 0x0400006B RID: 107
		private int fowTilingID = Shader.PropertyToID("_fowTiling");

		// Token: 0x0400006C RID: 108
		private int fowSpeedID = Shader.PropertyToID("_fowScrollSpeed");

		// Token: 0x0400006D RID: 109
		private int currentIndex;

		// Token: 0x0400006E RID: 110
		private FogOfWarWorld.ConeEdgeStruct[] anglesArray;

		// Token: 0x0400006F RID: 111
		public static Vector3 UpVector;

		// Token: 0x04000070 RID: 112
		public static Vector3 ForwardVector;

		// Token: 0x04000071 RID: 113
		private FogOfWarWorld.RevealerStruct[] _revealerDataToSet = new FogOfWarWorld.RevealerStruct[1];

		// Token: 0x02000018 RID: 24
		public struct RevealerStruct
		{
			// Token: 0x04000104 RID: 260
			public Vector2 CircleOrigin;

			// Token: 0x04000105 RID: 261
			public int StartIndex;

			// Token: 0x04000106 RID: 262
			public int NumSegments;

			// Token: 0x04000107 RID: 263
			public float CircleHeight;

			// Token: 0x04000108 RID: 264
			public float UnobscuredRadius;

			// Token: 0x04000109 RID: 265
			public float CircleRadius;

			// Token: 0x0400010A RID: 266
			public float CircleFade;

			// Token: 0x0400010B RID: 267
			public float VisionHeight;

			// Token: 0x0400010C RID: 268
			public float HeightFade;

			// Token: 0x0400010D RID: 269
			public float Opacity;
		}

		// Token: 0x02000019 RID: 25
		public struct ConeEdgeStruct
		{
			// Token: 0x0400010E RID: 270
			public float angle;

			// Token: 0x0400010F RID: 271
			public float length;

			// Token: 0x04000110 RID: 272
			public int cutShort;
		}

		// Token: 0x0200001A RID: 26
		public enum FowUpdateMethod
		{
			// Token: 0x04000112 RID: 274
			Update,
			// Token: 0x04000113 RID: 275
			LateUpdate
		}

		// Token: 0x0200001B RID: 27
		public enum RevealerUpdateMethod
		{
			// Token: 0x04000115 RID: 277
			Every_Frame,
			// Token: 0x04000116 RID: 278
			N_Per_Frame,
			// Token: 0x04000117 RID: 279
			Controlled_ElseWhere
		}

		// Token: 0x0200001C RID: 28
		public enum FogSampleMode
		{
			// Token: 0x04000119 RID: 281
			Pixel_Perfect,
			// Token: 0x0400011A RID: 282
			Texture,
			// Token: 0x0400011B RID: 283
			Both
		}

		// Token: 0x0200001D RID: 29
		public enum FogOfWarType
		{
			// Token: 0x0400011D RID: 285
			Hard,
			// Token: 0x0400011E RID: 286
			Soft
		}

		// Token: 0x0200001E RID: 30
		public enum FogOfWarFadeType
		{
			// Token: 0x04000120 RID: 288
			Linear,
			// Token: 0x04000121 RID: 289
			Exponential,
			// Token: 0x04000122 RID: 290
			Smooth,
			// Token: 0x04000123 RID: 291
			Smoother,
			// Token: 0x04000124 RID: 292
			Smoothstep
		}

		// Token: 0x0200001F RID: 31
		public enum FogOfWarBlendMode
		{
			// Token: 0x04000126 RID: 294
			Max,
			// Token: 0x04000127 RID: 295
			Addative
		}

		// Token: 0x02000020 RID: 32
		public enum FogOfWarAppearance
		{
			// Token: 0x04000129 RID: 297
			Solid_Color,
			// Token: 0x0400012A RID: 298
			GrayScale,
			// Token: 0x0400012B RID: 299
			Blur,
			// Token: 0x0400012C RID: 300
			Texture_Sample,
			// Token: 0x0400012D RID: 301
			Outline,
			// Token: 0x0400012E RID: 302
			None
		}

		// Token: 0x02000021 RID: 33
		public enum GamePlane
		{
			// Token: 0x04000130 RID: 304
			XZ,
			// Token: 0x04000131 RID: 305
			XY,
			// Token: 0x04000132 RID: 306
			ZY
		}

		// Token: 0x02000022 RID: 34
		[BurstCompile(CompileSynchronously = true)]
		private struct SetAnglesBuffersJob : IJobParallelFor
		{
			// Token: 0x060000B9 RID: 185 RVA: 0x00007CC7 File Offset: 0x00005EC7
			public void Execute(int index)
			{
				this.AnglesArray[index] = this.Angles[index];
			}

			// Token: 0x04000133 RID: 307
			[ReadOnly]
			public NativeArray<FogOfWarWorld.ConeEdgeStruct> Angles;

			// Token: 0x04000134 RID: 308
			[WriteOnly]
			public NativeArray<FogOfWarWorld.ConeEdgeStruct> AnglesArray;
		}
	}
}
