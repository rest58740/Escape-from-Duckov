using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000A RID: 10
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Rectangle")]
	public class Rectangle : ShapeRenderer, IDashable, IFillable
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000045D0 File Offset: 0x000027D0
		public bool IsBorder
		{
			get
			{
				return this.type == Rectangle.RectangleType.HardBorder || this.type == Rectangle.RectangleType.RoundedBorder;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000045E6 File Offset: 0x000027E6
		[Obsolete("Please use IsBorder instead", true)]
		public bool IsHollow
		{
			get
			{
				return this.type == Rectangle.RectangleType.HardBorder || this.type == Rectangle.RectangleType.RoundedBorder;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000045FC File Offset: 0x000027FC
		public bool IsRounded
		{
			get
			{
				return this.type == Rectangle.RectangleType.RoundedSolid || this.type == Rectangle.RectangleType.RoundedBorder;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00004612 File Offset: 0x00002812
		// (set) Token: 0x06000112 RID: 274 RVA: 0x0000461A File Offset: 0x0000281A
		public RectPivot Pivot
		{
			get
			{
				return this.pivot;
			}
			set
			{
				this.pivot = value;
				this.UpdateRectPositioningNow();
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004629 File Offset: 0x00002829
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00004631 File Offset: 0x00002831
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
				this.UpdateRectPositioningNow();
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00004640 File Offset: 0x00002840
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00004648 File Offset: 0x00002848
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
				this.UpdateRectPositioningNow();
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00004657 File Offset: 0x00002857
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000465F File Offset: 0x0000285F
		public Rectangle.RectangleType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				base.UpdateMaterial();
				base.ApplyProperties();
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00004674 File Offset: 0x00002874
		// (set) Token: 0x0600011A RID: 282 RVA: 0x0000467C File Offset: 0x0000287C
		public Rectangle.RectangleCornerRadiusMode CornerRadiusMode
		{
			get
			{
				return this.cornerRadiusMode;
			}
			set
			{
				this.cornerRadiusMode = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00004685 File Offset: 0x00002885
		// (set) Token: 0x0600011C RID: 284 RVA: 0x0000468D File Offset: 0x0000288D
		[Obsolete("Radius is deprecated, please use CornerRadius instead", true)]
		public float Radius
		{
			get
			{
				return this.CornerRadius;
			}
			set
			{
				this.CornerRadius = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00004696 File Offset: 0x00002896
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000046A4 File Offset: 0x000028A4
		public float CornerRadius
		{
			get
			{
				return this.cornerRadii.x;
			}
			set
			{
				float num = Mathf.Max(0f, value);
				base.SetVector4Now(ShapesMaterialUtils.propCornerRadii, this.cornerRadii = new Vector4(num, num, num, num));
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000046DA File Offset: 0x000028DA
		// (set) Token: 0x06000120 RID: 288 RVA: 0x000046E4 File Offset: 0x000028E4
		public Vector4 CornerRadii
		{
			get
			{
				return this.cornerRadii;
			}
			set
			{
				base.SetVector4Now(ShapesMaterialUtils.propCornerRadii, this.cornerRadii = new Vector4(Mathf.Max(0f, value.x), Mathf.Max(0f, value.y), Mathf.Max(0f, value.z), Mathf.Max(0f, value.w)));
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000121 RID: 289 RVA: 0x0000474A File Offset: 0x0000294A
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00004752 File Offset: 0x00002952
		[Obsolete("Please use CornerRadii instead because I did a typo~", true)]
		public Vector4 CornerRadiii
		{
			get
			{
				return this.CornerRadii;
			}
			set
			{
				this.CornerRadii = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000475B File Offset: 0x0000295B
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00004764 File Offset: 0x00002964
		public float Thickness
		{
			get
			{
				return this.thickness;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propThickness, this.thickness = Mathf.Max(0f, value));
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004790 File Offset: 0x00002990
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00004798 File Offset: 0x00002998
		public ThicknessSpace ThicknessSpace
		{
			get
			{
				return this.thicknessSpace;
			}
			set
			{
				int propThicknessSpace = ShapesMaterialUtils.propThicknessSpace;
				this.thicknessSpace = value;
				base.SetIntNow(propThicknessSpace, (int)value);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000047BA File Offset: 0x000029BA
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000047BD File Offset: 0x000029BD
		private void UpdateRectPositioningNow()
		{
			base.SetVector4Now(ShapesMaterialUtils.propRect, this.GetPositioningRect());
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000047D0 File Offset: 0x000029D0
		private void UpdateRectPositioning()
		{
			base.SetVector4(ShapesMaterialUtils.propRect, this.GetPositioningRect());
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000047E4 File Offset: 0x000029E4
		private Vector4 GetPositioningRect()
		{
			float x = (this.pivot == RectPivot.Corner) ? 0f : (-this.width / 2f);
			float y = (this.pivot == RectPivot.Corner) ? 0f : (-this.height / 2f);
			return new Vector4(x, y, this.width, this.height);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000483C File Offset: 0x00002A3C
		private protected override void SetAllMaterialProperties()
		{
			if (this.cornerRadiusMode == Rectangle.RectangleCornerRadiusMode.PerCorner)
			{
				base.SetVector4(ShapesMaterialUtils.propCornerRadii, this.cornerRadii);
			}
			else if (this.cornerRadiusMode == Rectangle.RectangleCornerRadiusMode.Uniform)
			{
				base.SetVector4(ShapesMaterialUtils.propCornerRadii, new Vector4(this.CornerRadius, this.CornerRadius, this.CornerRadius, this.CornerRadius));
			}
			this.UpdateRectPositioning();
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetIntNow(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			this.SetFillProperties();
			this.SetAllDashValues(false);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000048CA File Offset: 0x00002ACA
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.GetRectMaterial(this.type)[base.BlendMode];
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000048E8 File Offset: 0x00002AE8
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			Vector2 vector = new Vector2(this.width, this.height);
			return new Bounds((this.pivot == RectPivot.Center) ? default(Vector2) : (vector / 2f), vector);
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00004937 File Offset: 0x00002B37
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000493F File Offset: 0x00002B3F
		public bool MatchDashSpacingToSize
		{
			get
			{
				return this.matchDashSpacingToSize;
			}
			set
			{
				this.matchDashSpacingToSize = value;
				this.SetAllDashValues(true);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000494F File Offset: 0x00002B4F
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00004957 File Offset: 0x00002B57
		public bool Dashed
		{
			get
			{
				return this.dashed;
			}
			set
			{
				this.dashed = value;
				this.SetAllDashValues(true);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00004967 File Offset: 0x00002B67
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00004974 File Offset: 0x00002B74
		public float DashSize
		{
			get
			{
				return this.dashStyle.size;
			}
			set
			{
				this.dashStyle.size = value;
				float netAbsoluteSize = this.dashStyle.GetNetAbsoluteSize(this.dashed, this.thickness);
				if (this.matchDashSpacingToSize)
				{
					base.SetFloat(ShapesMaterialUtils.propDashSpacing, this.GetNetDashSpacing());
				}
				base.SetFloatNow(ShapesMaterialUtils.propDashSize, netAbsoluteSize);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000049CA File Offset: 0x00002BCA
		// (set) Token: 0x06000135 RID: 309 RVA: 0x000049EB File Offset: 0x00002BEB
		public float DashSpacing
		{
			get
			{
				if (!this.matchDashSpacingToSize)
				{
					return this.dashStyle.spacing;
				}
				return this.dashStyle.size;
			}
			set
			{
				this.dashStyle.spacing = value;
				base.SetFloatNow(ShapesMaterialUtils.propDashSpacing, this.GetNetDashSpacing());
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00004A0A File Offset: 0x00002C0A
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00004A18 File Offset: 0x00002C18
		public float DashOffset
		{
			get
			{
				return this.dashStyle.offset;
			}
			set
			{
				int propDashOffset = ShapesMaterialUtils.propDashOffset;
				this.dashStyle.offset = value;
				base.SetFloatNow(propDashOffset, value);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00004A3F File Offset: 0x00002C3F
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00004A4C File Offset: 0x00002C4C
		public DashSpace DashSpace
		{
			get
			{
				return this.dashStyle.space;
			}
			set
			{
				int propDashSpace = ShapesMaterialUtils.propDashSpace;
				this.dashStyle.space = value;
				base.SetInt(propDashSpace, (int)value);
				base.SetFloatNow(ShapesMaterialUtils.propDashSize, this.dashStyle.GetNetAbsoluteSize(this.dashed, this.thickness));
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00004A95 File Offset: 0x00002C95
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public DashSnapping DashSnap
		{
			get
			{
				return this.dashStyle.snap;
			}
			set
			{
				int propDashSnap = ShapesMaterialUtils.propDashSnap;
				this.dashStyle.snap = value;
				base.SetIntNow(propDashSnap, (int)value);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00004ACB File Offset: 0x00002CCB
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00004AD8 File Offset: 0x00002CD8
		public DashType DashType
		{
			get
			{
				return this.dashStyle.type;
			}
			set
			{
				int propDashType = ShapesMaterialUtils.propDashType;
				this.dashStyle.type = value;
				base.SetIntNow(propDashType, (int)value);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00004AFF File Offset: 0x00002CFF
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00004B0C File Offset: 0x00002D0C
		public float DashShapeModifier
		{
			get
			{
				return this.dashStyle.shapeModifier;
			}
			set
			{
				int propDashShapeModifier = ShapesMaterialUtils.propDashShapeModifier;
				this.dashStyle.shapeModifier = value;
				base.SetFloatNow(propDashShapeModifier, value);
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00004B33 File Offset: 0x00002D33
		private void SetAllDashValues(bool now)
		{
			base.SetAllDashValues(this.dashStyle, this.Dashed, this.matchDashSpacingToSize, this.thickness, true, now);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00004B55 File Offset: 0x00002D55
		private float GetNetDashSpacing()
		{
			return base.GetNetDashSpacing(this.dashStyle, this.dashed, this.matchDashSpacingToSize, this.thickness);
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00004B75 File Offset: 0x00002D75
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00004B7D File Offset: 0x00002D7D
		public GradientFill Fill
		{
			get
			{
				return this.fill;
			}
			set
			{
				this.fill = value;
				this.SetFillProperties();
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00004B8C File Offset: 0x00002D8C
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00004B94 File Offset: 0x00002D94
		public bool UseFill
		{
			get
			{
				return this.useFill;
			}
			set
			{
				this.useFill = value;
				base.SetIntNow(ShapesMaterialUtils.propFillType, this.fill.GetShaderFillTypeInt(this.useFill));
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00004BB9 File Offset: 0x00002DB9
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00004BC6 File Offset: 0x00002DC6
		public FillType FillType
		{
			get
			{
				return this.fill.type;
			}
			set
			{
				this.fill.type = value;
				base.SetIntNow(ShapesMaterialUtils.propFillType, this.fill.GetShaderFillTypeInt(this.useFill));
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00004BF0 File Offset: 0x00002DF0
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00004C00 File Offset: 0x00002E00
		public FillSpace FillSpace
		{
			get
			{
				return this.fill.space;
			}
			set
			{
				int propFillSpace = ShapesMaterialUtils.propFillSpace;
				this.fill.space = value;
				base.SetIntNow(propFillSpace, (int)value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00004C27 File Offset: 0x00002E27
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00004C34 File Offset: 0x00002E34
		public Vector3 FillRadialOrigin
		{
			get
			{
				return this.fill.radialOrigin;
			}
			set
			{
				this.fill.radialOrigin = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00004C58 File Offset: 0x00002E58
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00004C65 File Offset: 0x00002E65
		public float FillRadialRadius
		{
			get
			{
				return this.fill.radialRadius;
			}
			set
			{
				this.fill.radialRadius = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00004C89 File Offset: 0x00002E89
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00004C96 File Offset: 0x00002E96
		public Vector3 FillLinearStart
		{
			get
			{
				return this.fill.linearStart;
			}
			set
			{
				this.fill.linearStart = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00004CBA File Offset: 0x00002EBA
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public Vector3 FillLinearEnd
		{
			get
			{
				return this.fill.linearEnd;
			}
			set
			{
				int propFillEnd = ShapesMaterialUtils.propFillEnd;
				this.fill.linearEnd = value;
				base.SetVector3Now(propFillEnd, value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00004CEF File Offset: 0x00002EEF
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00004CFC File Offset: 0x00002EFC
		public Color FillColorStart
		{
			get
			{
				return this.fill.colorStart;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.fill.colorStart = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00004D23 File Offset: 0x00002F23
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00004D30 File Offset: 0x00002F30
		public Color FillColorEnd
		{
			get
			{
				return this.fill.colorEnd;
			}
			set
			{
				int propColorEnd = ShapesMaterialUtils.propColorEnd;
				this.fill.colorEnd = value;
				base.SetColorNow(propColorEnd, value);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00004D58 File Offset: 0x00002F58
		private void SetFillProperties()
		{
			if (this.useFill)
			{
				base.SetInt(ShapesMaterialUtils.propFillSpace, (int)this.fill.space);
				base.SetVector4(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
				base.SetVector3(ShapesMaterialUtils.propFillEnd, this.fill.linearEnd);
				base.SetColor(ShapesMaterialUtils.propColor, this.fill.colorStart);
				base.SetColor(ShapesMaterialUtils.propColorEnd, this.fill.colorEnd);
			}
			base.SetInt(ShapesMaterialUtils.propFillType, this.fill.GetShaderFillTypeInt(this.useFill));
		}

		// Token: 0x04000036 RID: 54
		[SerializeField]
		private RectPivot pivot = RectPivot.Center;

		// Token: 0x04000037 RID: 55
		[SerializeField]
		private float width = 1f;

		// Token: 0x04000038 RID: 56
		[SerializeField]
		private float height = 1f;

		// Token: 0x04000039 RID: 57
		[SerializeField]
		private Rectangle.RectangleType type;

		// Token: 0x0400003A RID: 58
		[SerializeField]
		private Rectangle.RectangleCornerRadiusMode cornerRadiusMode;

		// Token: 0x0400003B RID: 59
		[SerializeField]
		private Vector4 cornerRadii = new Vector4(0.25f, 0.25f, 0.25f, 0.25f);

		// Token: 0x0400003C RID: 60
		[Tooltip("The thickness of the rectangle, in the given thickness space")]
		[SerializeField]
		private float thickness = 0.1f;

		// Token: 0x0400003D RID: 61
		[Tooltip("The space in which thickness is defined")]
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x0400003E RID: 62
		[SerializeField]
		private bool matchDashSpacingToSize = true;

		// Token: 0x0400003F RID: 63
		[SerializeField]
		private bool dashed;

		// Token: 0x04000040 RID: 64
		[SerializeField]
		private DashStyle dashStyle = DashStyle.defaultDashStyleRing;

		// Token: 0x04000041 RID: 65
		[SerializeField]
		private protected GradientFill fill = GradientFill.defaultFill;

		// Token: 0x04000042 RID: 66
		[SerializeField]
		private protected bool useFill;

		// Token: 0x02000084 RID: 132
		public enum RectangleType
		{
			// Token: 0x0400032B RID: 811
			HardSolid,
			// Token: 0x0400032C RID: 812
			RoundedSolid,
			// Token: 0x0400032D RID: 813
			HardBorder,
			// Token: 0x0400032E RID: 814
			RoundedBorder
		}

		// Token: 0x02000085 RID: 133
		public enum RectangleCornerRadiusMode
		{
			// Token: 0x04000330 RID: 816
			Uniform,
			// Token: 0x04000331 RID: 817
			PerCorner
		}
	}
}
