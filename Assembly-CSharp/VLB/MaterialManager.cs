using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x0200002F RID: 47
	public static class MaterialManager
	{
		// Token: 0x0600014D RID: 333 RVA: 0x000060A5 File Offset: 0x000042A5
		public static Material NewMaterialPersistent(Shader shader, bool gpuInstanced)
		{
			if (!shader)
			{
				Debug.LogError("Invalid VLB Shader. Please try to reset the VLB Config asset or reinstall the plugin.");
				return null;
			}
			Material material = new Material(shader);
			BatchingHelper.SetMaterialProperties(material, gpuInstanced);
			return material;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000060C8 File Offset: 0x000042C8
		public static Material GetInstancedMaterial(uint groupID, ref MaterialManager.StaticPropertiesSD staticProps)
		{
			MaterialManager.IStaticProperties staticProperties = staticProps;
			return MaterialManager.GetInstancedMaterial(MaterialManager.ms_MaterialsGroupSD, groupID, ref staticProperties);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000060F0 File Offset: 0x000042F0
		public static Material GetInstancedMaterial(uint groupID, ref MaterialManager.StaticPropertiesHD staticProps)
		{
			MaterialManager.IStaticProperties staticProperties = staticProps;
			return MaterialManager.GetInstancedMaterial(MaterialManager.ms_MaterialsGroupHD, groupID, ref staticProperties);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006118 File Offset: 0x00004318
		private static Material GetInstancedMaterial(Hashtable groups, uint groupID, ref MaterialManager.IStaticProperties staticProps)
		{
			MaterialManager.MaterialsGroup materialsGroup = (MaterialManager.MaterialsGroup)groups[groupID];
			if (materialsGroup == null)
			{
				materialsGroup = new MaterialManager.MaterialsGroup(staticProps.GetPropertiesCount());
				groups[groupID] = materialsGroup;
			}
			int materialID = staticProps.GetMaterialID();
			Material material = materialsGroup.materials[materialID];
			if (material == null)
			{
				material = Config.Instance.NewMaterialTransient(staticProps.GetShaderMode(), true);
				if (material)
				{
					materialsGroup.materials[materialID] = material;
					staticProps.ApplyToMaterial(material);
				}
			}
			return material;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000619C File Offset: 0x0000439C
		public static bool EnableGPUInstancing(ShaderMode shaderMode, bool enabled)
		{
			if (Config.Instance.GetActualRenderingMode(shaderMode) != RenderingMode.GPUInstancing)
			{
				Debug.LogErrorFormat("To change GPU Instancing at runtime, the VLB plugin's config must be configured to use the GPUInstancing RenderingMode.", Array.Empty<object>());
				return false;
			}
			Hashtable hashtable = (shaderMode == ShaderMode.SD) ? MaterialManager.ms_MaterialsGroupSD : MaterialManager.ms_MaterialsGroupHD;
			bool result = false;
			foreach (object obj in hashtable.Values)
			{
				MaterialManager.MaterialsGroup materialsGroup = (MaterialManager.MaterialsGroup)obj;
				if (materialsGroup != null)
				{
					foreach (Material material in materialsGroup.materials)
					{
						if (material)
						{
							material.enableInstancing = enabled;
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006258 File Offset: 0x00004458
		private static void SetBlendingMode(this Material mat, int nameID, BlendMode value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006262 File Offset: 0x00004462
		private static void SetStencilRef(this Material mat, int nameID, int value)
		{
			mat.SetInt(nameID, value);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000626C File Offset: 0x0000446C
		private static void SetStencilComp(this Material mat, int nameID, CompareFunction value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006276 File Offset: 0x00004476
		private static void SetStencilOp(this Material mat, int nameID, StencilOp value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006280 File Offset: 0x00004480
		private static void SetCull(this Material mat, int nameID, CullMode value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000628A File Offset: 0x0000448A
		private static void SetZWrite(this Material mat, int nameID, MaterialManager.ZWrite value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006294 File Offset: 0x00004494
		private static void SetZTest(this Material mat, int nameID, CompareFunction value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000062A0 File Offset: 0x000044A0
		// Note: this type is marked as 'beforefieldinit'.
		static MaterialManager()
		{
			BlendMode[] array = new BlendMode[3];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.F186F2262AE48F2AA4F90C9A6B35913B0F6B0B895423B6267252259BFD357D3B).FieldHandle);
			MaterialManager.BlendingMode_SrcFactor = array;
			BlendMode[] array2 = new BlendMode[3];
			RuntimeHelpers.InitializeArray(array2, fieldof(<PrivateImplementationDetails>.0A0EC6D4742068B4D88C6145B8224EF1DC240C8A305CDFC50C3AAF9121E6875D).FieldHandle);
			MaterialManager.BlendingMode_DstFactor = array2;
			bool[] array3 = new bool[3];
			array3[0] = true;
			array3[1] = true;
			MaterialManager.BlendingMode_AlphaAsBlack = array3;
			MaterialManager.ms_MaterialsGroupSD = new Hashtable(1);
			MaterialManager.ms_MaterialsGroupHD = new Hashtable(1);
		}

		// Token: 0x0400010E RID: 270
		public static MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400010F RID: 271
		private static readonly BlendMode[] BlendingMode_SrcFactor;

		// Token: 0x04000110 RID: 272
		private static readonly BlendMode[] BlendingMode_DstFactor;

		// Token: 0x04000111 RID: 273
		private static readonly bool[] BlendingMode_AlphaAsBlack;

		// Token: 0x04000112 RID: 274
		private static Hashtable ms_MaterialsGroupSD;

		// Token: 0x04000113 RID: 275
		private static Hashtable ms_MaterialsGroupHD;

		// Token: 0x020000A8 RID: 168
		public enum BlendingMode
		{
			// Token: 0x04000394 RID: 916
			Additive,
			// Token: 0x04000395 RID: 917
			SoftAdditive,
			// Token: 0x04000396 RID: 918
			TraditionalTransparency,
			// Token: 0x04000397 RID: 919
			Count
		}

		// Token: 0x020000A9 RID: 169
		public enum ColorGradient
		{
			// Token: 0x04000399 RID: 921
			Off,
			// Token: 0x0400039A RID: 922
			MatrixLow,
			// Token: 0x0400039B RID: 923
			MatrixHigh,
			// Token: 0x0400039C RID: 924
			Count
		}

		// Token: 0x020000AA RID: 170
		public enum Noise3D
		{
			// Token: 0x0400039E RID: 926
			Off,
			// Token: 0x0400039F RID: 927
			On,
			// Token: 0x040003A0 RID: 928
			Count
		}

		// Token: 0x020000AB RID: 171
		public static class SD
		{
			// Token: 0x020000DB RID: 219
			public enum DepthBlend
			{
				// Token: 0x0400045A RID: 1114
				Off,
				// Token: 0x0400045B RID: 1115
				On,
				// Token: 0x0400045C RID: 1116
				Count
			}

			// Token: 0x020000DC RID: 220
			public enum DynamicOcclusion
			{
				// Token: 0x0400045E RID: 1118
				Off,
				// Token: 0x0400045F RID: 1119
				ClippingPlane,
				// Token: 0x04000460 RID: 1120
				DepthTexture,
				// Token: 0x04000461 RID: 1121
				Count
			}

			// Token: 0x020000DD RID: 221
			public enum MeshSkewing
			{
				// Token: 0x04000463 RID: 1123
				Off,
				// Token: 0x04000464 RID: 1124
				On,
				// Token: 0x04000465 RID: 1125
				Count
			}

			// Token: 0x020000DE RID: 222
			public enum ShaderAccuracy
			{
				// Token: 0x04000467 RID: 1127
				Fast,
				// Token: 0x04000468 RID: 1128
				High,
				// Token: 0x04000469 RID: 1129
				Count
			}
		}

		// Token: 0x020000AC RID: 172
		public static class HD
		{
			// Token: 0x020000DF RID: 223
			public enum Attenuation
			{
				// Token: 0x0400046B RID: 1131
				Linear,
				// Token: 0x0400046C RID: 1132
				Quadratic,
				// Token: 0x0400046D RID: 1133
				Count
			}

			// Token: 0x020000E0 RID: 224
			public enum Shadow
			{
				// Token: 0x0400046F RID: 1135
				Off,
				// Token: 0x04000470 RID: 1136
				On,
				// Token: 0x04000471 RID: 1137
				Count
			}

			// Token: 0x020000E1 RID: 225
			public enum Cookie
			{
				// Token: 0x04000473 RID: 1139
				Off,
				// Token: 0x04000474 RID: 1140
				SingleChannel,
				// Token: 0x04000475 RID: 1141
				RGBA,
				// Token: 0x04000476 RID: 1142
				Count
			}
		}

		// Token: 0x020000AD RID: 173
		private interface IStaticProperties
		{
			// Token: 0x0600049C RID: 1180
			int GetPropertiesCount();

			// Token: 0x0600049D RID: 1181
			int GetMaterialID();

			// Token: 0x0600049E RID: 1182
			void ApplyToMaterial(Material mat);

			// Token: 0x0600049F RID: 1183
			ShaderMode GetShaderMode();
		}

		// Token: 0x020000AE RID: 174
		public struct StaticPropertiesSD : MaterialManager.IStaticProperties
		{
			// Token: 0x060004A0 RID: 1184 RVA: 0x000133FF File Offset: 0x000115FF
			public ShaderMode GetShaderMode()
			{
				return ShaderMode.SD;
			}

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00013402 File Offset: 0x00011602
			public static int staticPropertiesCount
			{
				get
				{
					return 432;
				}
			}

			// Token: 0x060004A2 RID: 1186 RVA: 0x00013409 File Offset: 0x00011609
			public int GetPropertiesCount()
			{
				return MaterialManager.StaticPropertiesSD.staticPropertiesCount;
			}

			// Token: 0x170000DB RID: 219
			// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00013410 File Offset: 0x00011610
			private int blendingModeID
			{
				get
				{
					return (int)this.blendingMode;
				}
			}

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00013418 File Offset: 0x00011618
			private int noise3DID
			{
				get
				{
					if (!Config.Instance.featureEnabledNoise3D)
					{
						return 0;
					}
					return (int)this.noise3D;
				}
			}

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0001342E File Offset: 0x0001162E
			private int depthBlendID
			{
				get
				{
					if (!Config.Instance.featureEnabledDepthBlend)
					{
						return 0;
					}
					return (int)this.depthBlend;
				}
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00013444 File Offset: 0x00011644
			private int colorGradientID
			{
				get
				{
					if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
					{
						return 0;
					}
					return (int)this.colorGradient;
				}
			}

			// Token: 0x170000DF RID: 223
			// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001345A File Offset: 0x0001165A
			private int dynamicOcclusionID
			{
				get
				{
					if (!Config.Instance.featureEnabledDynamicOcclusion)
					{
						return 0;
					}
					return (int)this.dynamicOcclusion;
				}
			}

			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00013470 File Offset: 0x00011670
			private int meshSkewingID
			{
				get
				{
					if (!Config.Instance.featureEnabledMeshSkewing)
					{
						return 0;
					}
					return (int)this.meshSkewing;
				}
			}

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00013486 File Offset: 0x00011686
			private int shaderAccuracyID
			{
				get
				{
					if (!Config.Instance.featureEnabledShaderAccuracyHigh)
					{
						return 0;
					}
					return (int)this.shaderAccuracy;
				}
			}

			// Token: 0x060004AA RID: 1194 RVA: 0x0001349C File Offset: 0x0001169C
			public int GetMaterialID()
			{
				return (((((this.blendingModeID * 2 + this.noise3DID) * 2 + this.depthBlendID) * 3 + this.colorGradientID) * 3 + this.dynamicOcclusionID) * 2 + this.meshSkewingID) * 2 + this.shaderAccuracyID;
			}

			// Token: 0x060004AB RID: 1195 RVA: 0x000134DC File Offset: 0x000116DC
			public void ApplyToMaterial(Material mat)
			{
				mat.SetKeywordEnabled("VLB_ALPHA_AS_BLACK", MaterialManager.BlendingMode_AlphaAsBlack[(int)this.blendingMode]);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_LOW", this.colorGradient == MaterialManager.ColorGradient.MatrixLow);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_HIGH", this.colorGradient == MaterialManager.ColorGradient.MatrixHigh);
				mat.SetKeywordEnabled("VLB_DEPTH_BLEND", this.depthBlend == MaterialManager.SD.DepthBlend.On);
				mat.SetKeywordEnabled("VLB_NOISE_3D", this.noise3D == MaterialManager.Noise3D.On);
				mat.SetKeywordEnabled("VLB_OCCLUSION_CLIPPING_PLANE", this.dynamicOcclusion == MaterialManager.SD.DynamicOcclusion.ClippingPlane);
				mat.SetKeywordEnabled("VLB_OCCLUSION_DEPTH_TEXTURE", this.dynamicOcclusion == MaterialManager.SD.DynamicOcclusion.DepthTexture);
				mat.SetKeywordEnabled("VLB_MESH_SKEWING", this.meshSkewing == MaterialManager.SD.MeshSkewing.On);
				mat.SetKeywordEnabled("VLB_SHADER_ACCURACY_HIGH", this.shaderAccuracy == MaterialManager.SD.ShaderAccuracy.High);
				mat.SetBlendingMode(ShaderProperties.BlendSrcFactor, MaterialManager.BlendingMode_SrcFactor[(int)this.blendingMode]);
				mat.SetBlendingMode(ShaderProperties.BlendDstFactor, MaterialManager.BlendingMode_DstFactor[(int)this.blendingMode]);
				mat.SetZTest(ShaderProperties.ZTest, CompareFunction.LessEqual);
			}

			// Token: 0x040003A1 RID: 929
			public MaterialManager.BlendingMode blendingMode;

			// Token: 0x040003A2 RID: 930
			public MaterialManager.Noise3D noise3D;

			// Token: 0x040003A3 RID: 931
			public MaterialManager.SD.DepthBlend depthBlend;

			// Token: 0x040003A4 RID: 932
			public MaterialManager.ColorGradient colorGradient;

			// Token: 0x040003A5 RID: 933
			public MaterialManager.SD.DynamicOcclusion dynamicOcclusion;

			// Token: 0x040003A6 RID: 934
			public MaterialManager.SD.MeshSkewing meshSkewing;

			// Token: 0x040003A7 RID: 935
			public MaterialManager.SD.ShaderAccuracy shaderAccuracy;
		}

		// Token: 0x020000AF RID: 175
		public struct StaticPropertiesHD : MaterialManager.IStaticProperties
		{
			// Token: 0x060004AC RID: 1196 RVA: 0x000135DA File Offset: 0x000117DA
			public ShaderMode GetShaderMode()
			{
				return ShaderMode.HD;
			}

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x060004AD RID: 1197 RVA: 0x000135DD File Offset: 0x000117DD
			public static int staticPropertiesCount
			{
				get
				{
					return 216 * Config.Instance.raymarchingQualitiesCount;
				}
			}

			// Token: 0x060004AE RID: 1198 RVA: 0x000135EF File Offset: 0x000117EF
			public int GetPropertiesCount()
			{
				return MaterialManager.StaticPropertiesHD.staticPropertiesCount;
			}

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x060004AF RID: 1199 RVA: 0x000135F6 File Offset: 0x000117F6
			private int blendingModeID
			{
				get
				{
					return (int)this.blendingMode;
				}
			}

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x060004B0 RID: 1200 RVA: 0x000135FE File Offset: 0x000117FE
			private int attenuationID
			{
				get
				{
					return (int)this.attenuation;
				}
			}

			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00013606 File Offset: 0x00011806
			private int noise3DID
			{
				get
				{
					if (!Config.Instance.featureEnabledNoise3D)
					{
						return 0;
					}
					return (int)this.noise3D;
				}
			}

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0001361C File Offset: 0x0001181C
			private int colorGradientID
			{
				get
				{
					if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
					{
						return 0;
					}
					return (int)this.colorGradient;
				}
			}

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00013632 File Offset: 0x00011832
			private int dynamicOcclusionID
			{
				get
				{
					if (!Config.Instance.featureEnabledShadow)
					{
						return 0;
					}
					return (int)this.shadow;
				}
			}

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00013648 File Offset: 0x00011848
			private int cookieID
			{
				get
				{
					if (!Config.Instance.featureEnabledCookie)
					{
						return 0;
					}
					return (int)this.cookie;
				}
			}

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001365E File Offset: 0x0001185E
			private int raymarchingQualityID
			{
				get
				{
					return this.raymarchingQualityIndex;
				}
			}

			// Token: 0x060004B6 RID: 1206 RVA: 0x00013668 File Offset: 0x00011868
			public int GetMaterialID()
			{
				return (((((this.blendingModeID * 2 + this.attenuationID) * 2 + this.noise3DID) * 3 + this.colorGradientID) * 2 + this.dynamicOcclusionID) * 3 + this.cookieID) * Config.Instance.raymarchingQualitiesCount + this.raymarchingQualityID;
			}

			// Token: 0x060004B7 RID: 1207 RVA: 0x000136BC File Offset: 0x000118BC
			public void ApplyToMaterial(Material mat)
			{
				mat.SetKeywordEnabled("VLB_ALPHA_AS_BLACK", MaterialManager.BlendingMode_AlphaAsBlack[(int)this.blendingMode]);
				mat.SetKeywordEnabled("VLB_ATTENUATION_LINEAR", this.attenuation == MaterialManager.HD.Attenuation.Linear);
				mat.SetKeywordEnabled("VLB_ATTENUATION_QUAD", this.attenuation == MaterialManager.HD.Attenuation.Quadratic);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_LOW", this.colorGradient == MaterialManager.ColorGradient.MatrixLow);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_HIGH", this.colorGradient == MaterialManager.ColorGradient.MatrixHigh);
				mat.SetKeywordEnabled("VLB_NOISE_3D", this.noise3D == MaterialManager.Noise3D.On);
				mat.SetKeywordEnabled("VLB_SHADOW", this.shadow == MaterialManager.HD.Shadow.On);
				mat.SetKeywordEnabled("VLB_COOKIE_1CHANNEL", this.cookie == MaterialManager.HD.Cookie.SingleChannel);
				mat.SetKeywordEnabled("VLB_COOKIE_RGBA", this.cookie == MaterialManager.HD.Cookie.RGBA);
				for (int i = 0; i < Config.Instance.raymarchingQualitiesCount; i++)
				{
					mat.SetKeywordEnabled(ShaderKeywords.HD.GetRaymarchingQuality(i), this.raymarchingQualityIndex == i);
				}
				mat.SetBlendingMode(ShaderProperties.BlendSrcFactor, MaterialManager.BlendingMode_SrcFactor[(int)this.blendingMode]);
				mat.SetBlendingMode(ShaderProperties.BlendDstFactor, MaterialManager.BlendingMode_DstFactor[(int)this.blendingMode]);
				mat.SetZTest(ShaderProperties.ZTest, CompareFunction.Always);
			}

			// Token: 0x040003A8 RID: 936
			public MaterialManager.BlendingMode blendingMode;

			// Token: 0x040003A9 RID: 937
			public MaterialManager.HD.Attenuation attenuation;

			// Token: 0x040003AA RID: 938
			public MaterialManager.Noise3D noise3D;

			// Token: 0x040003AB RID: 939
			public MaterialManager.ColorGradient colorGradient;

			// Token: 0x040003AC RID: 940
			public MaterialManager.HD.Shadow shadow;

			// Token: 0x040003AD RID: 941
			public MaterialManager.HD.Cookie cookie;

			// Token: 0x040003AE RID: 942
			public int raymarchingQualityIndex;
		}

		// Token: 0x020000B0 RID: 176
		private class MaterialsGroup
		{
			// Token: 0x060004B8 RID: 1208 RVA: 0x000137E4 File Offset: 0x000119E4
			public MaterialsGroup(int count)
			{
				this.materials = new Material[count];
			}

			// Token: 0x040003AF RID: 943
			public Material[] materials;
		}

		// Token: 0x020000B1 RID: 177
		private enum ZWrite
		{
			// Token: 0x040003B1 RID: 945
			Off,
			// Token: 0x040003B2 RID: 946
			On
		}
	}
}
