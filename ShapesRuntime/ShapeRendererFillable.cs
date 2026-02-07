using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000E RID: 14
	[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
	public abstract class ShapeRendererFillable : ShapeRenderer
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000066A3 File Offset: 0x000048A3
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		private int FillTypeShaderInt
		{
			get
			{
				if (!this.useFill)
				{
					return -1;
				}
				return (int)this.fill.type;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000066BA File Offset: 0x000048BA
		// (set) Token: 0x06000205 RID: 517 RVA: 0x000066C2 File Offset: 0x000048C2
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public bool UseFill
		{
			get
			{
				return this.useFill;
			}
			set
			{
				this.useFill = value;
				base.SetIntNow(ShapesMaterialUtils.propFillType, this.FillTypeShaderInt);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000066DC File Offset: 0x000048DC
		// (set) Token: 0x06000207 RID: 519 RVA: 0x000066DF File Offset: 0x000048DF
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public FillType FillType
		{
			get
			{
				return FillType.LinearGradient;
			}
			set
			{
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000066E1 File Offset: 0x000048E1
		// (set) Token: 0x06000209 RID: 521 RVA: 0x000066E4 File Offset: 0x000048E4
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public FillSpace FillSpace
		{
			get
			{
				return FillSpace.Local;
			}
			set
			{
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000066E8 File Offset: 0x000048E8
		// (set) Token: 0x0600020B RID: 523 RVA: 0x000066FE File Offset: 0x000048FE
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public Vector3 FillRadialOrigin
		{
			get
			{
				return default(Vector3);
			}
			set
			{
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00006700 File Offset: 0x00004900
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00006707 File Offset: 0x00004907
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public float FillRadialRadius
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000670C File Offset: 0x0000490C
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00006722 File Offset: 0x00004922
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public Vector3 FillLinearStart
		{
			get
			{
				return default(Vector3);
			}
			set
			{
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00006724 File Offset: 0x00004924
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000673A File Offset: 0x0000493A
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public Vector3 FillLinearEnd
		{
			get
			{
				return default(Vector3);
			}
			set
			{
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000673C File Offset: 0x0000493C
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00006752 File Offset: 0x00004952
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public Color FillColorStart
		{
			get
			{
				return default(Color);
			}
			set
			{
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00006754 File Offset: 0x00004954
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000676A File Offset: 0x0000496A
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		public Color FillColorEnd
		{
			get
			{
				return default(Color);
			}
			set
			{
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000676C File Offset: 0x0000496C
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		private protected void SetFillProperties()
		{
		}

		// Token: 0x04000076 RID: 118
		private const string OBSOLETE = "Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable";

		// Token: 0x04000077 RID: 119
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		private protected GradientFill fill;

		// Token: 0x04000078 RID: 120
		[Obsolete("Shapes now use the IFillable interface instead of inheriting from ShapeRendererFillable", true)]
		private protected bool useFill;
	}
}
