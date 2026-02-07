using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x0200007A RID: 122
	public static class LayerUtils
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x0000CFE2 File Offset: 0x0000B1E2
		public static LayerMask CreateFromNames(params string[] layerNames)
		{
			return LayerUtils.LayerNamesToMask(layerNames);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000CFEA File Offset: 0x0000B1EA
		public static LayerMask CreateFromNumbers(params int[] layerNumbers)
		{
			return LayerUtils.LayerNumbersToMask(layerNumbers);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000CFF4 File Offset: 0x0000B1F4
		public static LayerMask LayerNamesToMask(params string[] layerNames)
		{
			LayerMask layerMask = 0;
			foreach (string layerName in layerNames)
			{
				layerMask |= 1 << LayerMask.NameToLayer(layerName);
			}
			return layerMask;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000D038 File Offset: 0x0000B238
		public static LayerMask LayerNumbersToMask(params int[] layerNumbers)
		{
			LayerMask layerMask = 0;
			foreach (int num in layerNumbers)
			{
				layerMask |= 1 << num;
			}
			return layerMask;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000D074 File Offset: 0x0000B274
		public static LayerMask Inverse(this LayerMask mask)
		{
			return ~mask;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000D082 File Offset: 0x0000B282
		public static LayerMask AddToMask(this LayerMask mask, params string[] layerNames)
		{
			return mask | LayerUtils.LayerNamesToMask(layerNames);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000D09B File Offset: 0x0000B29B
		public static LayerMask RemoveFromMask(this LayerMask mask, params string[] layerNames)
		{
			return ~(~mask | LayerUtils.LayerNamesToMask(layerNames));
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
		public static bool ContainsAnyLayer(this LayerMask mask, params string[] layerNames)
		{
			if (layerNames == null)
			{
				return false;
			}
			for (int i = 0; i < layerNames.Length; i++)
			{
				if (mask == (mask | 1 << LayerMask.NameToLayer(layerNames[i])))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000D100 File Offset: 0x0000B300
		public static bool ContainsAllLayers(this LayerMask mask, params string[] layerNames)
		{
			if (layerNames == null)
			{
				return false;
			}
			for (int i = 0; i < layerNames.Length; i++)
			{
				if (mask != (mask | 1 << LayerMask.NameToLayer(layerNames[i])))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000D140 File Offset: 0x0000B340
		public static string[] MaskToNames(this LayerMask mask)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < 32; i++)
			{
				int num = 1 << i;
				if ((mask & num) == num)
				{
					string text = LayerMask.LayerToName(i);
					if (!string.IsNullOrEmpty(text))
					{
						list.Add(text);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000D18E File Offset: 0x0000B38E
		public static string MaskToString(this LayerMask mask)
		{
			return mask.MaskToString(", ");
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000D19B File Offset: 0x0000B39B
		public static string MaskToString(this LayerMask mask, string delimiter)
		{
			return string.Join(delimiter, mask.MaskToNames());
		}
	}
}
