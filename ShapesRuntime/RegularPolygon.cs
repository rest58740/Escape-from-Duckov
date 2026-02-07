using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shapes
{
	// Token: 0x0200000B RID: 11
	[ExecuteAlways]
	[AddComponentMenu("Shapes/RegularPolygon")]
	public class RegularPolygon : ShapeRenderer, IDashable, IFillable
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00004E6F File Offset: 0x0000306F
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00004E78 File Offset: 0x00003078
		public bool Border
		{
			get
			{
				return this.border;
			}
			set
			{
				int propBorder = ShapesMaterialUtils.propBorder;
				this.border = value;
				base.SetIntNow(propBorder, value.AsInt());
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00004E9F File Offset: 0x0000309F
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00004EA7 File Offset: 0x000030A7
		[Obsolete("Please use RegularPolygon.Border instead", true)]
		public bool Hollow
		{
			get
			{
				return this.Border;
			}
			set
			{
				this.Border = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00004EB0 File Offset: 0x000030B0
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00004EB8 File Offset: 0x000030B8
		public int Sides
		{
			get
			{
				return this.sides;
			}
			set
			{
				base.SetIntNow(ShapesMaterialUtils.propSides, this.sides = Mathf.Max(3, value));
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00004EE0 File Offset: 0x000030E0
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00004EE8 File Offset: 0x000030E8
		public float Roundness
		{
			get
			{
				return this.roundness;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propRoundness, this.roundness = Mathf.Clamp01(value));
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00004F0F File Offset: 0x0000310F
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00004F18 File Offset: 0x00003118
		public float Angle
		{
			get
			{
				return this.angle;
			}
			set
			{
				int propAng = ShapesMaterialUtils.propAng;
				this.angle = value;
				base.SetFloatNow(propAng, value);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00004F3A File Offset: 0x0000313A
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00004F44 File Offset: 0x00003144
		public float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propRadius, this.radius = Mathf.Max(0f, value));
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00004F70 File Offset: 0x00003170
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00004F78 File Offset: 0x00003178
		public RegularPolygonGeometry Geometry
		{
			get
			{
				return this.geometry;
			}
			set
			{
				int propAlignment = ShapesMaterialUtils.propAlignment;
				this.geometry = value;
				base.SetIntNow(propAlignment, (int)value);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00004F9A File Offset: 0x0000319A
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00004FA4 File Offset: 0x000031A4
		public ThicknessSpace RadiusSpace
		{
			get
			{
				return this.radiusSpace;
			}
			set
			{
				int propRadiusSpace = ShapesMaterialUtils.propRadiusSpace;
				this.radiusSpace = value;
				base.SetIntNow(propRadiusSpace, (int)value);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00004FC6 File Offset: 0x000031C6
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00004FD0 File Offset: 0x000031D0
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

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00004FFC File Offset: 0x000031FC
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00005004 File Offset: 0x00003204
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

		// Token: 0x0600016C RID: 364 RVA: 0x00005028 File Offset: 0x00003228
		private protected override void SetAllMaterialProperties()
		{
			this.SetFillProperties();
			base.SetIntNow(ShapesMaterialUtils.propBorder, this.border.AsInt());
			base.SetInt(ShapesMaterialUtils.propAlignment, (int)this.geometry);
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetInt(ShapesMaterialUtils.propRadiusSpace, (int)this.radiusSpace);
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetInt(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetFloat(ShapesMaterialUtils.propAng, this.angle);
			base.SetFloat(ShapesMaterialUtils.propSides, (float)this.sides);
			base.SetFloat(ShapesMaterialUtils.propRoundness, this.roundness);
			this.SetAllDashValues(false);
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600016D RID: 365 RVA: 0x000050E1 File Offset: 0x000032E1
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000050E4 File Offset: 0x000032E4
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.matRegularPolygon[base.BlendMode];
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000050FC File Offset: 0x000032FC
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			if (this.radiusSpace != ThicknessSpace.Meters)
			{
				return new Bounds(Vector3.zero, Vector3.zero);
			}
			float num = (this.radiusSpace == ThicknessSpace.Meters) ? (this.radius * 2f) : 0f;
			num += ((this.thicknessSpace == ThicknessSpace.Meters) ? this.thickness : 0f);
			return new Bounds(Vector3.zero, new Vector3(num, num, 0f));
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000516B File Offset: 0x0000336B
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00005173 File Offset: 0x00003373
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00005183 File Offset: 0x00003383
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000518B File Offset: 0x0000338B
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

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000519B File Offset: 0x0000339B
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000051A8 File Offset: 0x000033A8
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

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000051FE File Offset: 0x000033FE
		// (set) Token: 0x06000177 RID: 375 RVA: 0x0000521F File Offset: 0x0000341F
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000523E File Offset: 0x0000343E
		// (set) Token: 0x06000179 RID: 377 RVA: 0x0000524C File Offset: 0x0000344C
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00005273 File Offset: 0x00003473
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00005280 File Offset: 0x00003480
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000052C9 File Offset: 0x000034C9
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000052D8 File Offset: 0x000034D8
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000052FF File Offset: 0x000034FF
		// (set) Token: 0x0600017F RID: 383 RVA: 0x0000530C File Offset: 0x0000350C
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00005333 File Offset: 0x00003533
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00005340 File Offset: 0x00003540
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

		// Token: 0x06000182 RID: 386 RVA: 0x00005367 File Offset: 0x00003567
		private void SetAllDashValues(bool now)
		{
			base.SetAllDashValues(this.dashStyle, this.Dashed, this.matchDashSpacingToSize, this.thickness, true, now);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005389 File Offset: 0x00003589
		private float GetNetDashSpacing()
		{
			return base.GetNetDashSpacing(this.dashStyle, this.dashed, this.matchDashSpacingToSize, this.thickness);
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000184 RID: 388 RVA: 0x000053A9 File Offset: 0x000035A9
		// (set) Token: 0x06000185 RID: 389 RVA: 0x000053B1 File Offset: 0x000035B1
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

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000053C0 File Offset: 0x000035C0
		// (set) Token: 0x06000187 RID: 391 RVA: 0x000053C8 File Offset: 0x000035C8
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000188 RID: 392 RVA: 0x000053ED File Offset: 0x000035ED
		// (set) Token: 0x06000189 RID: 393 RVA: 0x000053FA File Offset: 0x000035FA
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00005424 File Offset: 0x00003624
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00005434 File Offset: 0x00003634
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000545B File Offset: 0x0000365B
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00005468 File Offset: 0x00003668
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000548C File Offset: 0x0000368C
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00005499 File Offset: 0x00003699
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

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000054BD File Offset: 0x000036BD
		// (set) Token: 0x06000191 RID: 401 RVA: 0x000054CA File Offset: 0x000036CA
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000054EE File Offset: 0x000036EE
		// (set) Token: 0x06000193 RID: 403 RVA: 0x000054FC File Offset: 0x000036FC
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00005523 File Offset: 0x00003723
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00005530 File Offset: 0x00003730
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

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00005557 File Offset: 0x00003757
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00005564 File Offset: 0x00003764
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

		// Token: 0x06000198 RID: 408 RVA: 0x0000558C File Offset: 0x0000378C
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

		// Token: 0x04000043 RID: 67
		[FormerlySerializedAs("hollow")]
		[SerializeField]
		private bool border;

		// Token: 0x04000044 RID: 68
		[SerializeField]
		private int sides = 3;

		// Token: 0x04000045 RID: 69
		[SerializeField]
		[Range(0f, 1f)]
		private float roundness;

		// Token: 0x04000046 RID: 70
		[SerializeField]
		private float angle = 1.5707964f;

		// Token: 0x04000047 RID: 71
		[SerializeField]
		private float radius = 1f;

		// Token: 0x04000048 RID: 72
		[SerializeField]
		private AngularUnit angUnitInput = AngularUnit.Degrees;

		// Token: 0x04000049 RID: 73
		[SerializeField]
		private RegularPolygonGeometry geometry;

		// Token: 0x0400004A RID: 74
		[SerializeField]
		private ThicknessSpace radiusSpace;

		// Token: 0x0400004B RID: 75
		[SerializeField]
		private float thickness = 0.5f;

		// Token: 0x0400004C RID: 76
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x0400004D RID: 77
		[SerializeField]
		private bool matchDashSpacingToSize = true;

		// Token: 0x0400004E RID: 78
		[SerializeField]
		private bool dashed;

		// Token: 0x0400004F RID: 79
		[SerializeField]
		private DashStyle dashStyle = DashStyle.defaultDashStyleRing;

		// Token: 0x04000050 RID: 80
		[SerializeField]
		private protected GradientFill fill = GradientFill.defaultFill;

		// Token: 0x04000051 RID: 81
		[SerializeField]
		private protected bool useFill;
	}
}
