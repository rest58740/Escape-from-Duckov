using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000008 RID: 8
	public static class BatchingHelper
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002887 File Offset: 0x00000A87
		public static bool IsGpuInstancingEnabled(Material material)
		{
			return material.enableInstancing;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000288F File Offset: 0x00000A8F
		public static void SetMaterialProperties(Material material, bool enableGpuInstancing)
		{
			material.enableInstancing = enableGpuInstancing;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002898 File Offset: 0x00000A98
		public static bool forceEnableDepthBlend
		{
			get
			{
				RenderingMode actualRenderingMode = Config.Instance.GetActualRenderingMode(ShaderMode.SD);
				return actualRenderingMode == RenderingMode.GPUInstancing || actualRenderingMode == RenderingMode.SRPBatcher;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000028BC File Offset: 0x00000ABC
		private static bool DoesRenderingModePreventBatching(ShaderMode shaderMode, ref string reasons)
		{
			RenderingMode actualRenderingMode = Config.Instance.GetActualRenderingMode(shaderMode);
			if (actualRenderingMode != RenderingMode.GPUInstancing && actualRenderingMode != RenderingMode.SRPBatcher)
			{
				reasons = string.Format("Current Rendering Mode is '{0}'. To enable batching, use '{1}'", actualRenderingMode, RenderingMode.GPUInstancing);
				if (Config.Instance.renderPipeline != RenderPipeline.BuiltIn)
				{
					reasons += string.Format(" or '{0}'", RenderingMode.SRPBatcher);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002920 File Offset: 0x00000B20
		public static bool CanBeBatched(VolumetricLightBeamSD beamA, VolumetricLightBeamSD beamB, ref string reasons)
		{
			if (BatchingHelper.DoesRenderingModePreventBatching(ShaderMode.SD, ref reasons))
			{
				return false;
			}
			bool flag = true;
			flag &= BatchingHelper.CanBeBatched(beamA, ref reasons);
			flag &= BatchingHelper.CanBeBatched(beamB, ref reasons);
			if (Config.Instance.featureEnabledDynamicOcclusion && beamA.GetComponent<DynamicOcclusionAbstractBase>() == null != (beamB.GetComponent<DynamicOcclusionAbstractBase>() == null))
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("{0}/{1}: dynamically occluded and non occluded beams cannot be batched together", beamA.name, beamB.name));
				flag = false;
			}
			if (Config.Instance.featureEnabledColorGradient != FeatureEnabledColorGradient.Off && beamA.colorMode != beamB.colorMode)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'Color Mode' mismatch: {0} / {1}", beamA.colorMode, beamB.colorMode));
				flag = false;
			}
			if (beamA.blendingMode != beamB.blendingMode)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'Blending Mode' mismatch: {0} / {1}", beamA.blendingMode, beamB.blendingMode));
				flag = false;
			}
			if (Config.Instance.featureEnabledNoise3D && beamA.isNoiseEnabled != beamB.isNoiseEnabled)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'3D Noise' enabled mismatch: {0} / {1}", beamA.noiseMode, beamB.noiseMode));
				flag = false;
			}
			if (Config.Instance.featureEnabledDepthBlend && !BatchingHelper.forceEnableDepthBlend && beamA.depthBlendDistance > 0f != beamB.depthBlendDistance > 0f)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'Opaque Geometry Blending' mismatch: {0} / {1}", beamA.depthBlendDistance, beamB.depthBlendDistance));
				flag = false;
			}
			if (Config.Instance.featureEnabledShaderAccuracyHigh && beamA.shaderAccuracy != beamB.shaderAccuracy)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'Shader Accuracy' mismatch: {0} / {1}", beamA.shaderAccuracy, beamB.shaderAccuracy));
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public static bool CanBeBatched(VolumetricLightBeamSD beam, ref string reasons)
		{
			bool result = true;
			if (Config.Instance.GetActualRenderingMode(ShaderMode.SD) == RenderingMode.GPUInstancing && beam.geomMeshType != MeshType.Shared)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("{0} is not using shared mesh", beam.name));
				result = false;
			}
			if (Config.Instance.featureEnabledDynamicOcclusion && beam.GetComponent<DynamicOcclusionDepthBuffer>() != null)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("{0} is using the DynamicOcclusion DepthBuffer feature", beam.name));
				result = false;
			}
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B58 File Offset: 0x00000D58
		public static bool CanBeBatched(VolumetricLightBeamHD beamA, VolumetricLightBeamHD beamB, ref string reasons)
		{
			if (BatchingHelper.DoesRenderingModePreventBatching(ShaderMode.HD, ref reasons))
			{
				return false;
			}
			bool flag = true;
			flag &= BatchingHelper.CanBeBatched(beamA, ref reasons);
			flag &= BatchingHelper.CanBeBatched(beamB, ref reasons);
			if (Config.Instance.featureEnabledColorGradient != FeatureEnabledColorGradient.Off && beamA.colorMode != beamB.colorMode)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'Color Mode' mismatch: {0} / {1}", beamA.colorMode, beamB.colorMode));
				flag = false;
			}
			if (beamA.blendingMode != beamB.blendingMode)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'Blending Mode' mismatch: {0} / {1}", beamA.blendingMode, beamB.blendingMode));
				flag = false;
			}
			if (beamA.attenuationEquation != beamB.attenuationEquation)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'Attenuation Equation' mismatch: {0} / {1}", beamA.attenuationEquation, beamB.attenuationEquation));
				flag = false;
			}
			if (Config.Instance.featureEnabledNoise3D && beamA.isNoiseEnabled != beamB.isNoiseEnabled)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'3D Noise' enabled mismatch: {0} / {1}", beamA.noiseMode, beamB.noiseMode));
				flag = false;
			}
			if (beamA.raymarchingQualityID != beamB.raymarchingQualityID)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("'Raymarching Quality' mismatch: {0} / {1}", Config.Instance.GetRaymarchingQualityForUniqueID(beamA.raymarchingQualityID).name, Config.Instance.GetRaymarchingQualityForUniqueID(beamB.raymarchingQualityID).name));
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002CC4 File Offset: 0x00000EC4
		public static bool CanBeBatched(VolumetricLightBeamHD beam, ref string reasons)
		{
			bool result = true;
			if (Config.Instance.featureEnabledShadow && beam.GetAdditionalComponentShadow() != null)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("{0} is using the Shadow feature", beam.name));
				result = false;
			}
			if (Config.Instance.featureEnabledCookie && beam.GetAdditionalComponentCookie() != null)
			{
				BatchingHelper.AppendErrorMessage(ref reasons, string.Format("{0} is using the Cookie feature", beam.name));
				result = false;
			}
			return result;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002D38 File Offset: 0x00000F38
		public static bool CanBeBatched(VolumetricLightBeamAbstractBase beamA, VolumetricLightBeamAbstractBase beamB, ref string reasons)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = beamA as VolumetricLightBeamSD;
			if (volumetricLightBeamSD != null)
			{
				VolumetricLightBeamSD volumetricLightBeamSD2 = beamB as VolumetricLightBeamSD;
				if (volumetricLightBeamSD2 != null)
				{
					return BatchingHelper.CanBeBatched(volumetricLightBeamSD, volumetricLightBeamSD2, ref reasons);
				}
			}
			VolumetricLightBeamHD volumetricLightBeamHD = beamA as VolumetricLightBeamHD;
			if (volumetricLightBeamHD != null)
			{
				VolumetricLightBeamHD volumetricLightBeamHD2 = beamB as VolumetricLightBeamHD;
				if (volumetricLightBeamHD2 != null)
				{
					return BatchingHelper.CanBeBatched(volumetricLightBeamHD, volumetricLightBeamHD2, ref reasons);
				}
			}
			return false;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002D80 File Offset: 0x00000F80
		private static void AppendErrorMessage(ref string message, string toAppend)
		{
			if (message != "")
			{
				message += "\n";
			}
			message = message + "- " + toAppend;
		}
	}
}
