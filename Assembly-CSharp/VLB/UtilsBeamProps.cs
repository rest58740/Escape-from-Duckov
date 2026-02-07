using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000045 RID: 69
	public static class UtilsBeamProps
	{
		// Token: 0x06000295 RID: 661 RVA: 0x0000A788 File Offset: 0x00008988
		public static bool CanChangeDuringPlaytime(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			return !volumetricLightBeamSD || volumetricLightBeamSD.trackChangesDuringPlaytime;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000A7AC File Offset: 0x000089AC
		public static Quaternion GetInternalLocalRotation(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.beamInternalLocalRotation;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.beamInternalLocalRotation;
			}
			return Quaternion.identity;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A7EC File Offset: 0x000089EC
		public static void SetIntensityFromLight(VolumetricLightBeamAbstractBase self, bool fromLight)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				volumetricLightBeamSD.intensityFromLight = fromLight;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				volumetricLightBeamHD.useIntensityFromAttachedLightSpot = fromLight;
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000A828 File Offset: 0x00008A28
		public static float GetThickness(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return Mathf.Clamp01(1f - volumetricLightBeamSD.fresnelPow / 10f);
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return Mathf.Clamp01(1f - volumetricLightBeamHD.sideSoftness / 10f);
			}
			return 0f;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000A888 File Offset: 0x00008A88
		public static void SetThickness(VolumetricLightBeamAbstractBase self, float value)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				volumetricLightBeamSD.fresnelPow = (1f - value) * 10f;
				return;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				volumetricLightBeamHD.sideSoftness = (1f - value) * 10f;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A8DC File Offset: 0x00008ADC
		public static float GetFallOffEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.fallOffEnd;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.fallOffEnd;
			}
			return 0f;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000A91C File Offset: 0x00008B1C
		public static ColorMode GetColorMode(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.usedColorMode;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.colorMode;
			}
			return ColorMode.Flat;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000A958 File Offset: 0x00008B58
		public static Color GetColorFlat(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.color;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.colorFlat;
			}
			return Color.white;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000A998 File Offset: 0x00008B98
		public static Gradient GetColorGradient(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.colorGradient;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.colorGradient;
			}
			return null;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000A9D4 File Offset: 0x00008BD4
		public static void SetColorFromLight(VolumetricLightBeamAbstractBase self, bool fromLight)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				volumetricLightBeamSD.colorFromLight = fromLight;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				volumetricLightBeamHD.colorFromLight = fromLight;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000AA10 File Offset: 0x00008C10
		public static float GetConeAngle(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneAngle;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.coneAngle;
			}
			return 0f;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000AA50 File Offset: 0x00008C50
		public static void SetSpotAngleFromLight(VolumetricLightBeamAbstractBase self, bool fromLight)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				volumetricLightBeamSD.spotAngleFromLight = fromLight;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				volumetricLightBeamHD.useSpotAngleFromAttachedLightSpot = fromLight;
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000AA8C File Offset: 0x00008C8C
		public static float GetConeRadiusStart(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneRadiusStart;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.coneRadiusStart;
			}
			return 0f;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000AACC File Offset: 0x00008CCC
		public static float GetConeRadiusEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneRadiusEnd;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.coneRadiusEnd;
			}
			return 0f;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000AB0C File Offset: 0x00008D0C
		public static int GetSortingLayerID(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.sortingLayerID;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.GetSortingLayerID();
			}
			return 0;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000AB48 File Offset: 0x00008D48
		public static int GetSortingOrder(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.sortingOrder;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.GetSortingOrder();
			}
			return 0;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000AB84 File Offset: 0x00008D84
		public static bool GetFadeOutEnabled(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			return volumetricLightBeamSD && volumetricLightBeamSD.isFadeOutEnabled;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000ABA8 File Offset: 0x00008DA8
		public static float GetFadeOutEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.fadeOutEnd;
			}
			return 0f;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		public static void SetFallOffEndFromLight(VolumetricLightBeamAbstractBase self, bool fromLight)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				volumetricLightBeamSD.fallOffEndFromLight = fromLight;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				volumetricLightBeamHD.useFallOffEndFromAttachedLightSpot = fromLight;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000AC0C File Offset: 0x00008E0C
		public static Dimensions GetDimensions(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.dimensions;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.GetDimensions();
			}
			return Dimensions.Dim3D;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000AC48 File Offset: 0x00008E48
		public static int GetGeomSides(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.geomSides;
			}
			return Config.Instance.sharedMeshSides;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000AC75 File Offset: 0x00008E75
		public static AttenuationEquation ConvertAttenuation(AttenuationEquationHD value)
		{
			return (AttenuationEquation)value;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000AC78 File Offset: 0x00008E78
		public static AttenuationEquationHD ConvertAttenuation(AttenuationEquation value)
		{
			if (value == AttenuationEquation.Blend)
			{
				return AttenuationEquationHD.Linear;
			}
			return (AttenuationEquationHD)value;
		}
	}
}
