using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000005 RID: 5
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Disc")]
	public class Disc : ShapeRenderer, IDashable
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023C8 File Offset: 0x000005C8
		public bool HasThickness
		{
			get
			{
				return this.type.HasThickness();
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000023D5 File Offset: 0x000005D5
		public bool HasSector
		{
			get
			{
				return this.type.HasSector();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000023E2 File Offset: 0x000005E2
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000023EA File Offset: 0x000005EA
		public DiscType Type
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

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000023FF File Offset: 0x000005FF
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002407 File Offset: 0x00000607
		public Disc.DiscColorMode ColorMode
		{
			get
			{
				return this.colorMode;
			}
			set
			{
				this.colorMode = value;
				base.ApplyProperties();
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002416 File Offset: 0x00000616
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002420 File Offset: 0x00000620
		public override Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColor(propColor, value);
				int propColorOuterStart = ShapesMaterialUtils.propColorOuterStart;
				this.colorOuterStart = value;
				base.SetColor(propColorOuterStart, value);
				int propColorInnerEnd = ShapesMaterialUtils.propColorInnerEnd;
				this.colorInnerEnd = value;
				base.SetColor(propColorInnerEnd, value);
				int propColorOuterEnd = ShapesMaterialUtils.propColorOuterEnd;
				this.colorOuterEnd = value;
				base.SetColorNow(propColorOuterEnd, value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002481 File Offset: 0x00000681
		// (set) Token: 0x0600002A RID: 42 RVA: 0x0000248C File Offset: 0x0000068C
		public Color ColorInnerStart
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000024AE File Offset: 0x000006AE
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000024B8 File Offset: 0x000006B8
		public Color ColorOuterStart
		{
			get
			{
				return this.colorOuterStart;
			}
			set
			{
				int propColorOuterStart = ShapesMaterialUtils.propColorOuterStart;
				this.colorOuterStart = value;
				base.SetColorNow(propColorOuterStart, value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000024DA File Offset: 0x000006DA
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000024E4 File Offset: 0x000006E4
		public Color ColorInnerEnd
		{
			get
			{
				return this.colorInnerEnd;
			}
			set
			{
				int propColorInnerEnd = ShapesMaterialUtils.propColorInnerEnd;
				this.colorInnerEnd = value;
				base.SetColorNow(propColorInnerEnd, value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002506 File Offset: 0x00000706
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002510 File Offset: 0x00000710
		public Color ColorOuterEnd
		{
			get
			{
				return this.colorOuterEnd;
			}
			set
			{
				int propColorOuterEnd = ShapesMaterialUtils.propColorOuterEnd;
				this.colorOuterEnd = value;
				base.SetColorNow(propColorOuterEnd, value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002532 File Offset: 0x00000732
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000253C File Offset: 0x0000073C
		public Color ColorOuter
		{
			get
			{
				return this.ColorOuterStart;
			}
			set
			{
				int propColorOuterStart = ShapesMaterialUtils.propColorOuterStart;
				this.colorOuterStart = value;
				base.SetColor(propColorOuterStart, value);
				int propColorOuterEnd = ShapesMaterialUtils.propColorOuterEnd;
				this.colorOuterEnd = value;
				base.SetColorNow(propColorOuterEnd, value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002573 File Offset: 0x00000773
		// (set) Token: 0x06000034 RID: 52 RVA: 0x0000257C File Offset: 0x0000077C
		public Color ColorInner
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColor(propColor, value);
				int propColorInnerEnd = ShapesMaterialUtils.propColorInnerEnd;
				this.colorInnerEnd = value;
				base.SetColorNow(propColorInnerEnd, value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000025B3 File Offset: 0x000007B3
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000025BC File Offset: 0x000007BC
		public Color ColorStart
		{
			get
			{
				return base.Color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColor(propColor, value);
				int propColorOuterStart = ShapesMaterialUtils.propColorOuterStart;
				this.colorOuterStart = value;
				base.SetColorNow(propColorOuterStart, value);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000025F3 File Offset: 0x000007F3
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000025FC File Offset: 0x000007FC
		public Color ColorEnd
		{
			get
			{
				return this.colorInnerEnd;
			}
			set
			{
				int propColorInnerEnd = ShapesMaterialUtils.propColorInnerEnd;
				this.colorInnerEnd = value;
				base.SetColor(propColorInnerEnd, value);
				int propColorOuterEnd = ShapesMaterialUtils.propColorOuterEnd;
				this.colorOuterEnd = value;
				base.SetColorNow(propColorOuterEnd, value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002633 File Offset: 0x00000833
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0000263C File Offset: 0x0000083C
		public DiscGeometry Geometry
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

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000265E File Offset: 0x0000085E
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002668 File Offset: 0x00000868
		public float AngRadiansStart
		{
			get
			{
				return this.angRadiansStart;
			}
			set
			{
				int propAngStart = ShapesMaterialUtils.propAngStart;
				this.angRadiansStart = value;
				base.SetFloatNow(propAngStart, value);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000268A File Offset: 0x0000088A
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002694 File Offset: 0x00000894
		public float AngRadiansEnd
		{
			get
			{
				return this.angRadiansEnd;
			}
			set
			{
				int propAngEnd = ShapesMaterialUtils.propAngEnd;
				this.angRadiansEnd = value;
				base.SetFloatNow(propAngEnd, value);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000026B6 File Offset: 0x000008B6
		// (set) Token: 0x06000040 RID: 64 RVA: 0x000026C0 File Offset: 0x000008C0
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000026EC File Offset: 0x000008EC
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000026F4 File Offset: 0x000008F4
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002716 File Offset: 0x00000916
		// (set) Token: 0x06000044 RID: 68 RVA: 0x0000271E File Offset: 0x0000091E
		[Obsolete("this property is obsolete, this was a typo! please use Thickness instead!", true)]
		public float RadiusInner
		{
			get
			{
				return this.Thickness;
			}
			set
			{
				this.Thickness = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002727 File Offset: 0x00000927
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002730 File Offset: 0x00000930
		public float Thickness
		{
			get
			{
				return this.thickness;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propThickness, this.thickness = Mathf.Max(0f, value));
				if (this.HasThickness && this.dashed && this.dashStyle.space == DashSpace.Relative)
				{
					this.SetAllDashValues(true);
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002781 File Offset: 0x00000981
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000278C File Offset: 0x0000098C
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000027AE File Offset: 0x000009AE
		// (set) Token: 0x0600004A RID: 74 RVA: 0x000027B8 File Offset: 0x000009B8
		public ArcEndCap ArcEndCaps
		{
			get
			{
				return this.arcEndCaps;
			}
			set
			{
				int propRoundCaps = ShapesMaterialUtils.propRoundCaps;
				this.arcEndCaps = value;
				base.SetIntNow(propRoundCaps, (int)value);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000027DC File Offset: 0x000009DC
		private protected override void SetAllMaterialProperties()
		{
			base.SetInt(ShapesMaterialUtils.propAlignment, (int)this.geometry);
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetInt(ShapesMaterialUtils.propRadiusSpace, (int)this.radiusSpace);
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetInt(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetInt(ShapesMaterialUtils.propRoundCaps, (int)this.arcEndCaps);
			base.SetFloat(ShapesMaterialUtils.propAngStart, this.angRadiansStart);
			base.SetFloat(ShapesMaterialUtils.propAngEnd, this.angRadiansEnd);
			switch (this.ColorMode)
			{
			case Disc.DiscColorMode.Single:
				base.SetColor(ShapesMaterialUtils.propColorOuterStart, base.Color);
				base.SetColor(ShapesMaterialUtils.propColorInnerEnd, base.Color);
				base.SetColor(ShapesMaterialUtils.propColorOuterEnd, base.Color);
				break;
			case Disc.DiscColorMode.Radial:
				base.SetColor(ShapesMaterialUtils.propColorOuterStart, this.ColorOuterStart);
				base.SetColor(ShapesMaterialUtils.propColorInnerEnd, base.Color);
				base.SetColor(ShapesMaterialUtils.propColorOuterEnd, this.ColorOuterStart);
				break;
			case Disc.DiscColorMode.Angular:
				base.SetColor(ShapesMaterialUtils.propColorOuterStart, base.Color);
				base.SetColor(ShapesMaterialUtils.propColorInnerEnd, this.ColorInnerEnd);
				base.SetColor(ShapesMaterialUtils.propColorOuterEnd, this.ColorInnerEnd);
				break;
			case Disc.DiscColorMode.Bilinear:
				base.SetColor(ShapesMaterialUtils.propColorOuterStart, this.ColorOuterStart);
				base.SetColor(ShapesMaterialUtils.propColorInnerEnd, this.ColorInnerEnd);
				base.SetColor(ShapesMaterialUtils.propColorOuterEnd, this.ColorOuterEnd);
				break;
			}
			this.SetAllDashValues(false);
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000296F File Offset: 0x00000B6F
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002972 File Offset: 0x00000B72
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.GetDiscMaterial(this.type)[base.BlendMode];
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002990 File Offset: 0x00000B90
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			float num = (this.radiusSpace == ThicknessSpace.Meters) ? (2f * this.radius) : 0f;
			num += ((this.HasThickness && this.thicknessSpace == ThicknessSpace.Meters) ? this.thickness : 0f);
			return new Bounds(Vector3.zero, new Vector3(num, num, (this.geometry == DiscGeometry.Billboard) ? num : 0f));
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000029FB File Offset: 0x00000BFB
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002A03 File Offset: 0x00000C03
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

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002A13 File Offset: 0x00000C13
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002A1B File Offset: 0x00000C1B
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002A2B File Offset: 0x00000C2B
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002A38 File Offset: 0x00000C38
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002A8E File Offset: 0x00000C8E
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002AAF File Offset: 0x00000CAF
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002ACE File Offset: 0x00000CCE
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002ADC File Offset: 0x00000CDC
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002B03 File Offset: 0x00000D03
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002B10 File Offset: 0x00000D10
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002B59 File Offset: 0x00000D59
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002B68 File Offset: 0x00000D68
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002B8F File Offset: 0x00000D8F
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002B9C File Offset: 0x00000D9C
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002BC3 File Offset: 0x00000DC3
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002BD0 File Offset: 0x00000DD0
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

		// Token: 0x06000061 RID: 97 RVA: 0x00002BF7 File Offset: 0x00000DF7
		private void SetAllDashValues(bool now)
		{
			base.SetAllDashValues(this.dashStyle, this.Dashed, this.matchDashSpacingToSize, this.thickness, true, now);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002C19 File Offset: 0x00000E19
		private float GetNetDashSpacing()
		{
			return base.GetNetDashSpacing(this.dashStyle, this.dashed, this.matchDashSpacingToSize, this.thickness);
		}

		// Token: 0x04000007 RID: 7
		[SerializeField]
		private DiscType type;

		// Token: 0x04000008 RID: 8
		[SerializeField]
		private Disc.DiscColorMode colorMode;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorOuterStart = Color.white;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorInnerEnd = Color.white;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorOuterEnd = Color.white;

		// Token: 0x0400000C RID: 12
		[SerializeField]
		private DiscGeometry geometry;

		// Token: 0x0400000D RID: 13
		[SerializeField]
		private AngularUnit angUnitInput = AngularUnit.Degrees;

		// Token: 0x0400000E RID: 14
		[SerializeField]
		private float angRadiansStart;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private float angRadiansEnd = 2.3561945f;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		private float radius = 1f;

		// Token: 0x04000011 RID: 17
		[SerializeField]
		private ThicknessSpace radiusSpace;

		// Token: 0x04000012 RID: 18
		[SerializeField]
		private float thickness = 0.5f;

		// Token: 0x04000013 RID: 19
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x04000014 RID: 20
		[SerializeField]
		private ArcEndCap arcEndCaps;

		// Token: 0x04000015 RID: 21
		[SerializeField]
		private bool matchDashSpacingToSize = true;

		// Token: 0x04000016 RID: 22
		[SerializeField]
		private bool dashed;

		// Token: 0x04000017 RID: 23
		[SerializeField]
		private DashStyle dashStyle = DashStyle.defaultDashStyleRing;

		// Token: 0x02000080 RID: 128
		public enum DiscColorMode
		{
			// Token: 0x04000318 RID: 792
			Single,
			// Token: 0x04000319 RID: 793
			Radial,
			// Token: 0x0400031A RID: 794
			Angular,
			// Token: 0x0400031B RID: 795
			Bilinear
		}
	}
}
