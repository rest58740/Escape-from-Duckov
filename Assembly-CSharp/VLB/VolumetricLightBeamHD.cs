using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200002A RID: 42
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[SelectionBase]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-hd/")]
	[AddComponentMenu("VLB/HD/Volumetric Light Beam HD")]
	public class VolumetricLightBeamHD : VolumetricLightBeamAbstractBase
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004956 File Offset: 0x00002B56
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x0000495E File Offset: 0x00002B5E
		public bool colorFromLight
		{
			get
			{
				return this.m_ColorFromLight;
			}
			set
			{
				if (this.m_ColorFromLight != value)
				{
					this.m_ColorFromLight = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004976 File Offset: 0x00002B76
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x0000498C File Offset: 0x00002B8C
		public ColorMode colorMode
		{
			get
			{
				if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
				{
					return ColorMode.Flat;
				}
				return this.m_ColorMode;
			}
			set
			{
				if (this.m_ColorMode != value)
				{
					this.m_ColorMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.ColorMode);
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000049AB File Offset: 0x00002BAB
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x000049B3 File Offset: 0x00002BB3
		public Color colorFlat
		{
			get
			{
				return this.m_ColorFlat;
			}
			set
			{
				if (this.m_ColorFlat != value)
				{
					this.m_ColorFlat = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Color);
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000049D8 File Offset: 0x00002BD8
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000049E0 File Offset: 0x00002BE0
		public Gradient colorGradient
		{
			get
			{
				return this.m_ColorGradient;
			}
			set
			{
				if (this.m_ColorGradient != value)
				{
					this.m_ColorGradient = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Color);
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004A00 File Offset: 0x00002C00
		private bool useColorFromAttachedLightSpot
		{
			get
			{
				return this.colorFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004A18 File Offset: 0x00002C18
		private bool useColorTemperatureFromAttachedLightSpot
		{
			get
			{
				return this.useColorFromAttachedLightSpot && base.lightSpotAttached.useColorTemperature && Config.Instance.useLightColorTemperature;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004A3B File Offset: 0x00002C3B
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00004A43 File Offset: 0x00002C43
		public float intensity
		{
			get
			{
				return this.m_Intensity;
			}
			set
			{
				if (this.m_Intensity != value)
				{
					this.m_Intensity = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Intensity);
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004A62 File Offset: 0x00002C62
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00004A6A File Offset: 0x00002C6A
		public float intensityMultiplier
		{
			get
			{
				return this.m_IntensityMultiplier;
			}
			set
			{
				if (this.m_IntensityMultiplier != value)
				{
					this.m_IntensityMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004A82 File Offset: 0x00002C82
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00004A9F File Offset: 0x00002C9F
		public bool useIntensityFromAttachedLightSpot
		{
			get
			{
				return this.intensityMultiplier >= 0f && base.lightSpotAttached != null;
			}
			set
			{
				this.intensityMultiplier = (value ? 1f : -1f) * Mathf.Abs(this.intensityMultiplier);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004AC2 File Offset: 0x00002CC2
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00004ACA File Offset: 0x00002CCA
		public float hdrpExposureWeight
		{
			get
			{
				return this.m_HDRPExposureWeight;
			}
			set
			{
				if (this.m_HDRPExposureWeight != value)
				{
					this.m_HDRPExposureWeight = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.HDRPExposureWeight);
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004AE9 File Offset: 0x00002CE9
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00004AF1 File Offset: 0x00002CF1
		public BlendingMode blendingMode
		{
			get
			{
				return this.m_BlendingMode;
			}
			set
			{
				if (this.m_BlendingMode != value)
				{
					this.m_BlendingMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.BlendingMode);
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004B11 File Offset: 0x00002D11
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004B19 File Offset: 0x00002D19
		public float spotAngle
		{
			get
			{
				return this.m_SpotAngle;
			}
			set
			{
				if (this.m_SpotAngle != value)
				{
					this.m_SpotAngle = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004B39 File Offset: 0x00002D39
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004B41 File Offset: 0x00002D41
		public float spotAngleMultiplier
		{
			get
			{
				return this.m_SpotAngleMultiplier;
			}
			set
			{
				if (this.m_SpotAngleMultiplier != value)
				{
					this.m_SpotAngleMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004B59 File Offset: 0x00002D59
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004B76 File Offset: 0x00002D76
		public bool useSpotAngleFromAttachedLightSpot
		{
			get
			{
				return this.spotAngleMultiplier >= 0f && base.lightSpotAttached != null;
			}
			set
			{
				this.spotAngleMultiplier = (value ? 1f : -1f) * Mathf.Abs(this.spotAngleMultiplier);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004B99 File Offset: 0x00002D99
		public float coneAngle
		{
			get
			{
				return Mathf.Atan2(this.coneRadiusEnd - this.coneRadiusStart, this.maxGeometryDistance) * 57.29578f * 2f;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004BBF File Offset: 0x00002DBF
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00004BC7 File Offset: 0x00002DC7
		public float coneRadiusStart
		{
			get
			{
				return this.m_ConeRadiusStart;
			}
			set
			{
				if (this.m_ConeRadiusStart != value)
				{
					this.m_ConeRadiusStart = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00004BE7 File Offset: 0x00002DE7
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00004BFA File Offset: 0x00002DFA
		public float coneRadiusEnd
		{
			get
			{
				return Utils.ComputeConeRadiusEnd(this.maxGeometryDistance, this.spotAngle);
			}
			set
			{
				this.spotAngle = Utils.ComputeSpotAngle(this.maxGeometryDistance, value);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00004C10 File Offset: 0x00002E10
		public float coneVolume
		{
			get
			{
				float coneRadiusStart = this.coneRadiusStart;
				float coneRadiusEnd = this.coneRadiusEnd;
				return 1.0471976f * (coneRadiusStart * coneRadiusStart + coneRadiusStart * coneRadiusEnd + coneRadiusEnd * coneRadiusEnd) * this.fallOffEnd;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004C44 File Offset: 0x00002E44
		public float GetConeApexOffsetZ(bool counterApplyScaleForUnscalableBeam)
		{
			float num = this.coneRadiusStart / this.coneRadiusEnd;
			if (num == 1f)
			{
				return float.MaxValue;
			}
			float num2 = this.maxGeometryDistance * num / (1f - num);
			if (counterApplyScaleForUnscalableBeam && !this.scalable)
			{
				num2 /= this.GetLossyScale().z;
			}
			return num2;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004C98 File Offset: 0x00002E98
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00004CA0 File Offset: 0x00002EA0
		public bool scalable
		{
			get
			{
				return this.m_Scalable;
			}
			set
			{
				if (this.m_Scalable != value)
				{
					this.m_Scalable = value;
					this.SetPropertyDirty(DirtyProps.Attenuation);
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004CBD File Offset: 0x00002EBD
		public override bool IsScalable()
		{
			return this.scalable;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004CC5 File Offset: 0x00002EC5
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00004CCD File Offset: 0x00002ECD
		public AttenuationEquationHD attenuationEquation
		{
			get
			{
				return this.m_AttenuationEquation;
			}
			set
			{
				if (this.m_AttenuationEquation != value)
				{
					this.m_AttenuationEquation = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Attenuation);
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004CF0 File Offset: 0x00002EF0
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public float fallOffStart
		{
			get
			{
				return this.m_FallOffStart;
			}
			set
			{
				if (this.m_FallOffStart != value)
				{
					this.m_FallOffStart = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004D18 File Offset: 0x00002F18
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00004D20 File Offset: 0x00002F20
		public float fallOffEnd
		{
			get
			{
				return this.m_FallOffEnd;
			}
			set
			{
				if (this.m_FallOffEnd != value)
				{
					this.m_FallOffEnd = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004D40 File Offset: 0x00002F40
		public float maxGeometryDistance
		{
			get
			{
				return this.fallOffEnd;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00004D48 File Offset: 0x00002F48
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00004D50 File Offset: 0x00002F50
		public float fallOffEndMultiplier
		{
			get
			{
				return this.m_FallOffEndMultiplier;
			}
			set
			{
				if (this.m_FallOffEndMultiplier != value)
				{
					this.m_FallOffEndMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00004D68 File Offset: 0x00002F68
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00004D85 File Offset: 0x00002F85
		public bool useFallOffEndFromAttachedLightSpot
		{
			get
			{
				return this.fallOffEndMultiplier >= 0f && base.lightSpotAttached != null;
			}
			set
			{
				this.fallOffEndMultiplier = (value ? 1f : -1f) * Mathf.Abs(this.fallOffEndMultiplier);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00004DA8 File Offset: 0x00002FA8
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public float sideSoftness
		{
			get
			{
				return this.m_SideSoftness;
			}
			set
			{
				if (this.m_SideSoftness != value)
				{
					this.m_SideSoftness = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.SideSoftness);
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00004DD3 File Offset: 0x00002FD3
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00004DDB File Offset: 0x00002FDB
		public float jitteringFactor
		{
			get
			{
				return this.m_JitteringFactor;
			}
			set
			{
				if (this.m_JitteringFactor != value)
				{
					this.m_JitteringFactor = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004DFE File Offset: 0x00002FFE
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00004E06 File Offset: 0x00003006
		public int jitteringFrameRate
		{
			get
			{
				return this.m_JitteringFrameRate;
			}
			set
			{
				if (this.m_JitteringFrameRate != value)
				{
					this.m_JitteringFrameRate = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00004E29 File Offset: 0x00003029
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004E31 File Offset: 0x00003031
		public MinMaxRangeFloat jitteringLerpRange
		{
			get
			{
				return this.m_JitteringLerpRange;
			}
			set
			{
				if (this.m_JitteringLerpRange != value)
				{
					this.m_JitteringLerpRange = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004E59 File Offset: 0x00003059
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00004E61 File Offset: 0x00003061
		public NoiseMode noiseMode
		{
			get
			{
				return this.m_NoiseMode;
			}
			set
			{
				if (this.m_NoiseMode != value)
				{
					this.m_NoiseMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseMode);
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004E84 File Offset: 0x00003084
		public bool isNoiseEnabled
		{
			get
			{
				return this.noiseMode > NoiseMode.Disabled;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004E8F File Offset: 0x0000308F
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004E97 File Offset: 0x00003097
		public float noiseIntensity
		{
			get
			{
				return this.m_NoiseIntensity;
			}
			set
			{
				if (this.m_NoiseIntensity != value)
				{
					this.m_NoiseIntensity = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseIntensity);
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004EBA File Offset: 0x000030BA
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004EC2 File Offset: 0x000030C2
		public bool noiseScaleUseGlobal
		{
			get
			{
				return this.m_NoiseScaleUseGlobal;
			}
			set
			{
				if (this.m_NoiseScaleUseGlobal != value)
				{
					this.m_NoiseScaleUseGlobal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00004EE5 File Offset: 0x000030E5
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004EED File Offset: 0x000030ED
		public float noiseScaleLocal
		{
			get
			{
				return this.m_NoiseScaleLocal;
			}
			set
			{
				if (this.m_NoiseScaleLocal != value)
				{
					this.m_NoiseScaleLocal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004F10 File Offset: 0x00003110
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004F18 File Offset: 0x00003118
		public bool noiseVelocityUseGlobal
		{
			get
			{
				return this.m_NoiseVelocityUseGlobal;
			}
			set
			{
				if (this.m_NoiseVelocityUseGlobal != value)
				{
					this.m_NoiseVelocityUseGlobal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004F3B File Offset: 0x0000313B
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004F43 File Offset: 0x00003143
		public Vector3 noiseVelocityLocal
		{
			get
			{
				return this.m_NoiseVelocityLocal;
			}
			set
			{
				if (this.m_NoiseVelocityLocal != value)
				{
					this.m_NoiseVelocityLocal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004F6B File Offset: 0x0000316B
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004F73 File Offset: 0x00003173
		public int raymarchingQualityID
		{
			get
			{
				return this.m_RaymarchingQualityID;
			}
			set
			{
				if (this.m_RaymarchingQualityID != value)
				{
					this.m_RaymarchingQualityID = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.RaymarchingQuality);
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004F96 File Offset: 0x00003196
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004FA8 File Offset: 0x000031A8
		public int raymarchingQualityIndex
		{
			get
			{
				return Config.Instance.GetRaymarchingQualityIndexForUniqueID(this.raymarchingQualityID);
			}
			set
			{
				this.raymarchingQualityID = Config.Instance.GetRaymarchingQualityForIndex(this.raymarchingQualityIndex).uniqueID;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004FC5 File Offset: 0x000031C5
		public override BeamGeometryAbstractBase GetBeamGeometry()
		{
			return this.m_BeamGeom;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004FCD File Offset: 0x000031CD
		protected override void SetBeamGeometryNull()
		{
			this.m_BeamGeom = null;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004FD6 File Offset: 0x000031D6
		public int blendingModeAsInt
		{
			get
			{
				return Mathf.Clamp((int)this.blendingMode, 0, Enum.GetValues(typeof(BlendingMode)).Length);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004FF8 File Offset: 0x000031F8
		public Quaternion beamInternalLocalRotation
		{
			get
			{
				if (this.GetDimensions() != Dimensions.Dim3D)
				{
					return Quaternion.LookRotation(Vector3.right, Vector3.up);
				}
				return Quaternion.identity;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00005017 File Offset: 0x00003217
		public Vector3 beamLocalForward
		{
			get
			{
				if (this.GetDimensions() != Dimensions.Dim3D)
				{
					return Vector3.right;
				}
				return Vector3.forward;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000502C File Offset: 0x0000322C
		public Vector3 beamGlobalForward
		{
			get
			{
				return base.transform.TransformDirection(this.beamLocalForward);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005040 File Offset: 0x00003240
		public override Vector3 GetLossyScale()
		{
			if (this.GetDimensions() != Dimensions.Dim3D)
			{
				return new Vector3(base.transform.lossyScale.z, base.transform.lossyScale.y, base.transform.lossyScale.x);
			}
			return base.transform.lossyScale;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005096 File Offset: 0x00003296
		public VolumetricCookieHD GetAdditionalComponentCookie()
		{
			return base.GetComponent<VolumetricCookieHD>();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000509E File Offset: 0x0000329E
		public VolumetricShadowHD GetAdditionalComponentShadow()
		{
			return base.GetComponent<VolumetricShadowHD>();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000050A6 File Offset: 0x000032A6
		public void SetPropertyDirty(DirtyProps flags)
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.SetPropertyDirty(flags);
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000050C1 File Offset: 0x000032C1
		public virtual Dimensions GetDimensions()
		{
			return Dimensions.Dim3D;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000050C4 File Offset: 0x000032C4
		public virtual bool DoesSupportSorting2D()
		{
			return false;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000050C7 File Offset: 0x000032C7
		public virtual int GetSortingLayerID()
		{
			return 0;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000050CA File Offset: 0x000032CA
		public virtual int GetSortingOrder()
		{
			return 0;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000050CD File Offset: 0x000032CD
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000050D5 File Offset: 0x000032D5
		public uint _INTERNAL_InstancedMaterialGroupID { get; protected set; }

		// Token: 0x06000108 RID: 264 RVA: 0x000050DE File Offset: 0x000032DE
		public float GetInsideBeamFactor(Vector3 posWS)
		{
			return this.GetInsideBeamFactorFromObjectSpacePos(base.transform.InverseTransformPoint(posWS));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000050F4 File Offset: 0x000032F4
		public float GetInsideBeamFactorFromObjectSpacePos(Vector3 posOS)
		{
			if (this.GetDimensions() == Dimensions.Dim2D)
			{
				posOS = new Vector3(posOS.z, posOS.y, posOS.x);
			}
			if (posOS.z < 0f)
			{
				return -1f;
			}
			Vector2 normalized = new Vector2(posOS.xy().magnitude, posOS.z + this.GetConeApexOffsetZ(true)).normalized;
			return Mathf.Clamp((Mathf.Abs(Mathf.Sin(this.coneAngle * 0.017453292f / 2f)) - Mathf.Abs(normalized.x)) / 0.1f, -1f, 1f);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000051A0 File Offset: 0x000033A0
		public override void GenerateGeometry()
		{
			if (this.pluginVersion == -1)
			{
				this.raymarchingQualityID = Config.Instance.defaultRaymarchingQualityUniqueID;
			}
			if (!Config.Instance.IsRaymarchingQualityUniqueIDValid(this.raymarchingQualityID))
			{
				Debug.LogErrorFormat(base.gameObject, "HD Beam '{0}': fallback to default quality '{1}'", new object[]
				{
					base.name,
					Config.Instance.GetRaymarchingQualityForUniqueID(Config.Instance.defaultRaymarchingQualityUniqueID).name
				});
				this.raymarchingQualityID = Config.Instance.defaultRaymarchingQualityUniqueID;
				Utils.MarkCurrentSceneDirty();
			}
			this.HandleBackwardCompatibility(this.pluginVersion, 20200);
			this.pluginVersion = 20200;
			this.ValidateProperties();
			if (this.m_BeamGeom == null)
			{
				this.m_BeamGeom = Utils.NewWithComponent<BeamGeometryHD>("Beam Geometry");
				this.m_BeamGeom.Initialize(this);
			}
			this.m_BeamGeom.RegenerateMesh();
			this.m_BeamGeom.visible = base.enabled;
			base.GenerateGeometry();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005296 File Offset: 0x00003496
		public virtual void UpdateAfterManualPropertyChange()
		{
			this.ValidateProperties();
			this.SetPropertyDirty(DirtyProps.All);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000052A9 File Offset: 0x000034A9
		private void Start()
		{
			base.InitLightSpotAttachedCached();
			this.GenerateGeometry();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000052B7 File Offset: 0x000034B7
		private void OnEnable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.visible = true;
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000052D2 File Offset: 0x000034D2
		private void OnDisable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.visible = false;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000052ED File Offset: 0x000034ED
		private void OnDidApplyAnimationProperties()
		{
			this.AssignPropertiesFromAttachedSpotLight();
			this.UpdateAfterManualPropertyChange();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000052FC File Offset: 0x000034FC
		public void AssignPropertiesFromAttachedSpotLight()
		{
			Light lightSpotAttached = base.lightSpotAttached;
			if (lightSpotAttached)
			{
				if (this.useIntensityFromAttachedLightSpot)
				{
					this.intensity = SpotLightHelper.GetIntensity(lightSpotAttached) * this.intensityMultiplier;
				}
				if (this.useFallOffEndFromAttachedLightSpot)
				{
					this.fallOffEnd = SpotLightHelper.GetFallOffEnd(lightSpotAttached) * this.fallOffEndMultiplier;
				}
				if (this.useSpotAngleFromAttachedLightSpot)
				{
					this.spotAngle = Mathf.Clamp(SpotLightHelper.GetSpotAngle(lightSpotAttached) * this.spotAngleMultiplier, 0.1f, 179.9f);
				}
				if (this.m_ColorFromLight)
				{
					this.colorMode = ColorMode.Flat;
					if (this.useColorTemperatureFromAttachedLightSpot)
					{
						Color b = Mathf.CorrelatedColorTemperatureToRGB(lightSpotAttached.colorTemperature);
						this.colorFlat = (lightSpotAttached.color.linear * b).gamma;
						return;
					}
					this.colorFlat = lightSpotAttached.color;
				}
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000053D0 File Offset: 0x000035D0
		private void ClampProperties()
		{
			this.m_Intensity = Mathf.Max(this.m_Intensity, 0f);
			this.m_FallOffEnd = Mathf.Max(0.01f, this.m_FallOffEnd);
			this.m_FallOffStart = Mathf.Clamp(this.m_FallOffStart, 0f, this.m_FallOffEnd - 0.01f);
			this.m_SpotAngle = Mathf.Clamp(this.m_SpotAngle, 0.1f, 179.9f);
			this.m_ConeRadiusStart = Mathf.Max(this.m_ConeRadiusStart, 0f);
			this.m_SideSoftness = Mathf.Clamp(this.m_SideSoftness, 0.0001f, 10f);
			this.m_JitteringFactor = Mathf.Max(this.m_JitteringFactor, 0f);
			this.m_JitteringFrameRate = Mathf.Clamp(this.m_JitteringFrameRate, 0, 120);
			this.m_NoiseIntensity = Mathf.Clamp(this.m_NoiseIntensity, 0f, 1f);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000054BC File Offset: 0x000036BC
		private void ValidateProperties()
		{
			this.AssignPropertiesFromAttachedSpotLight();
			this.ClampProperties();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000054CA File Offset: 0x000036CA
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
			if (serializedVersion == -1)
			{
				return;
			}
			if (serializedVersion == newVersion)
			{
				return;
			}
			Utils.MarkCurrentSceneDirty();
		}

		// Token: 0x040000DB RID: 219
		public new const string ClassName = "VolumetricLightBeamHD";

		// Token: 0x040000DC RID: 220
		[SerializeField]
		private bool m_ColorFromLight = true;

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private ColorMode m_ColorMode;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		private Color m_ColorFlat = Consts.Beam.FlatColor;

		// Token: 0x040000DF RID: 223
		[SerializeField]
		private Gradient m_ColorGradient;

		// Token: 0x040000E0 RID: 224
		[SerializeField]
		private BlendingMode m_BlendingMode;

		// Token: 0x040000E1 RID: 225
		[SerializeField]
		private float m_Intensity = 1f;

		// Token: 0x040000E2 RID: 226
		[SerializeField]
		private float m_IntensityMultiplier = 1f;

		// Token: 0x040000E3 RID: 227
		[SerializeField]
		private float m_HDRPExposureWeight;

		// Token: 0x040000E4 RID: 228
		[SerializeField]
		private float m_SpotAngle = 35f;

		// Token: 0x040000E5 RID: 229
		[SerializeField]
		private float m_SpotAngleMultiplier = 1f;

		// Token: 0x040000E6 RID: 230
		[SerializeField]
		private float m_ConeRadiusStart = 0.1f;

		// Token: 0x040000E7 RID: 231
		[SerializeField]
		private bool m_Scalable = true;

		// Token: 0x040000E8 RID: 232
		[SerializeField]
		private float m_FallOffStart;

		// Token: 0x040000E9 RID: 233
		[SerializeField]
		private float m_FallOffEnd = 3f;

		// Token: 0x040000EA RID: 234
		[SerializeField]
		private float m_FallOffEndMultiplier = 1f;

		// Token: 0x040000EB RID: 235
		[SerializeField]
		private AttenuationEquationHD m_AttenuationEquation = AttenuationEquationHD.Quadratic;

		// Token: 0x040000EC RID: 236
		[SerializeField]
		private float m_SideSoftness = 1f;

		// Token: 0x040000ED RID: 237
		[SerializeField]
		private int m_RaymarchingQualityID = -1;

		// Token: 0x040000EE RID: 238
		[SerializeField]
		private float m_JitteringFactor;

		// Token: 0x040000EF RID: 239
		[SerializeField]
		private int m_JitteringFrameRate = 60;

		// Token: 0x040000F0 RID: 240
		[MinMaxRange(0f, 1f)]
		[SerializeField]
		private MinMaxRangeFloat m_JitteringLerpRange = Consts.Beam.HD.JitteringLerpRange;

		// Token: 0x040000F1 RID: 241
		[SerializeField]
		private NoiseMode m_NoiseMode;

		// Token: 0x040000F2 RID: 242
		[SerializeField]
		private float m_NoiseIntensity = 0.5f;

		// Token: 0x040000F3 RID: 243
		[SerializeField]
		private bool m_NoiseScaleUseGlobal = true;

		// Token: 0x040000F4 RID: 244
		[SerializeField]
		private float m_NoiseScaleLocal = 0.5f;

		// Token: 0x040000F5 RID: 245
		[SerializeField]
		private bool m_NoiseVelocityUseGlobal = true;

		// Token: 0x040000F6 RID: 246
		[SerializeField]
		private Vector3 m_NoiseVelocityLocal = Consts.Beam.NoiseVelocityDefault;

		// Token: 0x040000F8 RID: 248
		protected BeamGeometryHD m_BeamGeom;
	}
}
