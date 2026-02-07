using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	public struct GradientFill
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x00019148 File Offset: 0x00017348
		public static GradientFill Linear(Vector3 start, Vector3 end, Color colorStart, Color colorEnd, FillSpace space = FillSpace.Local)
		{
			return new GradientFill
			{
				type = FillType.LinearGradient,
				colorStart = colorStart,
				colorEnd = colorEnd,
				space = space,
				linearStart = start,
				linearEnd = end,
				radialOrigin = GradientFill.defaultFill.radialOrigin,
				radialRadius = GradientFill.defaultFill.radialRadius
			};
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x000191B4 File Offset: 0x000173B4
		public static GradientFill Radial(Vector3 origin, float radius, Color colorInner, Color colorOuter, FillSpace space = FillSpace.Local)
		{
			return new GradientFill
			{
				type = FillType.RadialGradient,
				space = space,
				colorStart = colorInner,
				colorEnd = colorOuter,
				linearStart = GradientFill.defaultFill.linearStart,
				linearEnd = GradientFill.defaultFill.linearEnd,
				radialOrigin = origin,
				radialRadius = radius
			};
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00019220 File Offset: 0x00017420
		internal Vector4 GetShaderStartVector()
		{
			if (this.type == FillType.LinearGradient)
			{
				return this.linearStart;
			}
			return new Vector4(this.radialOrigin.x, this.radialOrigin.y, this.radialOrigin.z, this.radialRadius);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0001926D File Offset: 0x0001746D
		internal int GetShaderFillTypeInt(bool use)
		{
			if (!use)
			{
				return -1;
			}
			return (int)this.type;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0001927C File Offset: 0x0001747C
		[Obsolete("Use GradientFill.Linear instead", true)]
		public static GradientFill CreateLinear(Vector3 start, Vector3 end, Color colorStart, Color colorEnd, FillSpace space)
		{
			return default(GradientFill);
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00019294 File Offset: 0x00017494
		[Obsolete("Use GradientFill.Radial instead", true)]
		public static GradientFill CreateRadial(Vector3 origin, float radius, Color colorInner, Color colorOuter, FillSpace space)
		{
			return default(GradientFill);
		}

		// Token: 0x040001EE RID: 494
		internal const int FILL_NONE = -1;

		// Token: 0x040001EF RID: 495
		public static readonly GradientFill defaultFill = new GradientFill
		{
			type = FillType.LinearGradient,
			space = FillSpace.Local,
			colorStart = Color.black,
			colorEnd = Color.white,
			linearStart = Vector3.zero,
			linearEnd = Vector3.up,
			radialOrigin = Vector3.zero,
			radialRadius = 1f
		};

		// Token: 0x040001F0 RID: 496
		public FillType type;

		// Token: 0x040001F1 RID: 497
		public FillSpace space;

		// Token: 0x040001F2 RID: 498
		public Color colorStart;

		// Token: 0x040001F3 RID: 499
		public Color colorEnd;

		// Token: 0x040001F4 RID: 500
		public Vector3 linearStart;

		// Token: 0x040001F5 RID: 501
		public Vector3 linearEnd;

		// Token: 0x040001F6 RID: 502
		public Vector3 radialOrigin;

		// Token: 0x040001F7 RID: 503
		public float radialRadius;
	}
}
