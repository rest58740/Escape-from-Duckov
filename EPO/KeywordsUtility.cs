using System;
using System.Collections.Generic;

namespace EPOOutline
{
	// Token: 0x0200000E RID: 14
	public static class KeywordsUtility
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002FAD File Offset: 0x000011AD
		public static string GetBackKeyword(ComplexMaskingMode mode)
		{
			switch (mode)
			{
			case ComplexMaskingMode.None:
				return string.Empty;
			case ComplexMaskingMode.ObstaclesMode:
				return "BACK_OBSTACLE_RENDERING";
			case ComplexMaskingMode.MaskingMode:
				return "BACK_MASKING_RENDERING";
			default:
				throw new ArgumentException("Unknown rendering mode");
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002FDF File Offset: 0x000011DF
		public static string GetTextureArrayCutoutKeyword()
		{
			return "TEXARRAY_CUTOUT";
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002FE6 File Offset: 0x000011E6
		public static string GetDilateQualityKeyword(DilateQuality quality)
		{
			switch (quality)
			{
			case DilateQuality.Base:
				return "BASE_QUALITY_DILATE";
			case DilateQuality.High:
				return "HIGH_QUALITY_DILATE";
			case DilateQuality.Ultra:
				return "ULTRA_QUALITY_DILATE";
			default:
				throw new Exception("Unknown dilate quality level");
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003018 File Offset: 0x00001218
		public static string GetEnabledInfoBufferKeyword()
		{
			return "USE_INFO_BUFFER";
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000301F File Offset: 0x0000121F
		public static string GetInfoBufferStageKeyword()
		{
			return "INFO_BUFFER_STAGE";
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003026 File Offset: 0x00001226
		public static string GetBlurKeyword(BlurType type)
		{
			return KeywordsUtility.BlurTypes[type];
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003033 File Offset: 0x00001233
		public static string GetCutoutKeyword()
		{
			return "USE_CUTOUT";
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000303C File Offset: 0x0000123C
		public static void GetAllBlurKeywords(List<string> list)
		{
			list.Clear();
			foreach (KeyValuePair<BlurType, string> keyValuePair in KeywordsUtility.BlurTypes)
			{
				list.Add(keyValuePair.Value);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000309C File Offset: 0x0000129C
		public static void GetAllDilateKeywords(List<string> list)
		{
			list.Clear();
			foreach (KeyValuePair<DilateQuality, string> keyValuePair in KeywordsUtility.DilateQualityKeywords)
			{
				list.Add(keyValuePair.Value);
			}
		}

		// Token: 0x04000036 RID: 54
		private static Dictionary<BlurType, string> BlurTypes = new Dictionary<BlurType, string>
		{
			{
				BlurType.Box,
				"BOX_BLUR"
			},
			{
				BlurType.Gaussian5x5,
				"GAUSSIAN5X5"
			},
			{
				BlurType.Gaussian9x9,
				"GAUSSIAN9X9"
			},
			{
				BlurType.Gaussian13x13,
				"GAUSSIAN13X13"
			}
		};

		// Token: 0x04000037 RID: 55
		private static Dictionary<DilateQuality, string> DilateQualityKeywords = new Dictionary<DilateQuality, string>
		{
			{
				DilateQuality.Base,
				"BASE_QUALITY_DILATE"
			},
			{
				DilateQuality.High,
				"HIGH_QUALITY_DILATE"
			},
			{
				DilateQuality.Ultra,
				"ULTRA_QUALITY_DILATE"
			}
		};
	}
}
