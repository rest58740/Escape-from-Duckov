using System;
using DG.DemiLib.Core;
using UnityEngine;

namespace DG.DemiLib
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public struct DeSkinColor
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public DeSkinColor(Color free, Color pro)
		{
			this.free = free;
			this.pro = pro;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public DeSkinColor(float freeGradation, float proGradation)
		{
			this.free = new Color(freeGradation, freeGradation, freeGradation, 1f);
			this.pro = new Color(proGradation, proGradation, proGradation, 1f);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002B10 File Offset: 0x00000D10
		public DeSkinColor(Color color)
		{
			this = default(DeSkinColor);
			this.free = color;
			this.pro = color;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002B27 File Offset: 0x00000D27
		public DeSkinColor(float gradation)
		{
			this = default(DeSkinColor);
			this.free = new Color(gradation, gradation, gradation, 1f);
			this.pro = this.free;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002B4F File Offset: 0x00000D4F
		public static implicit operator Color(DeSkinColor v)
		{
			if (!GUIUtils.isProSkin)
			{
				return v.free;
			}
			return v.pro;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002B65 File Offset: 0x00000D65
		public static implicit operator DeSkinColor(Color v)
		{
			return new DeSkinColor(v);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002B6D File Offset: 0x00000D6D
		public override string ToString()
		{
			return string.Format("{0}, {1}", this.free, this.pro);
		}

		// Token: 0x04000033 RID: 51
		public Color free;

		// Token: 0x04000034 RID: 52
		public Color pro;
	}
}
