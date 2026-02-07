using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shapes
{
	// Token: 0x02000013 RID: 19
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Triangle")]
	public class Triangle : ShapeRenderer, IDashable
	{
		// Token: 0x170000E9 RID: 233
		public Vector3 this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.A;
				case 1:
					return this.B;
				case 2:
					return this.C;
				default:
					throw new IndexOutOfRangeException(string.Format("Triangle only has four vertices, 0 to 2, you tried to access element {0}", index));
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.A = value;
					return;
				case 1:
					this.B = value;
					return;
				case 2:
					this.C = value;
					return;
				default:
					throw new IndexOutOfRangeException(string.Format("Triangle only has four vertices, 0 to 2, you tried to set element {0}", index));
				}
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00006D32 File Offset: 0x00004F32
		public Vector3 GetTriangleVertex(int index)
		{
			return this[index];
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00006D3C File Offset: 0x00004F3C
		public Vector3 SetTriangleVertex(int index, Vector3 value)
		{
			this[index] = value;
			return value;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00006D54 File Offset: 0x00004F54
		public Color GetTriangleColor(int index)
		{
			switch (index)
			{
			case 0:
				return this.Color;
			case 1:
				return this.ColorB;
			case 2:
				return this.ColorC;
			default:
				throw new IndexOutOfRangeException(string.Format("Triangle only has four vertices, 0 to 2, you tried to access element {0}", index));
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00006D94 File Offset: 0x00004F94
		public void SetTriangleColor(int index, Color color)
		{
			switch (index)
			{
			case 0:
				this.Color = color;
				return;
			case 1:
				this.ColorB = color;
				return;
			case 2:
				this.ColorC = color;
				return;
			default:
				throw new IndexOutOfRangeException(string.Format("Triangle only has four vertices, 0 to 3, you tried to set element {0}", index));
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00006DE2 File Offset: 0x00004FE2
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00006DEA File Offset: 0x00004FEA
		public Triangle.TriangleColorMode ColorMode
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00006DF9 File Offset: 0x00004FF9
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00006E04 File Offset: 0x00005004
		public Vector3 A
		{
			get
			{
				return this.a;
			}
			set
			{
				int propA = ShapesMaterialUtils.propA;
				this.a = value;
				base.SetVector3Now(propA, value);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00006E26 File Offset: 0x00005026
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00006E30 File Offset: 0x00005030
		public Vector3 B
		{
			get
			{
				return this.b;
			}
			set
			{
				int propB = ShapesMaterialUtils.propB;
				this.b = value;
				base.SetVector3Now(propB, value);
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00006E52 File Offset: 0x00005052
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00006E5C File Offset: 0x0000505C
		public Vector3 C
		{
			get
			{
				return this.c;
			}
			set
			{
				int propC = ShapesMaterialUtils.propC;
				this.c = value;
				base.SetVector3Now(propC, value);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00006E7E File Offset: 0x0000507E
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00006E88 File Offset: 0x00005088
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

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00006EAF File Offset: 0x000050AF
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00006EB7 File Offset: 0x000050B7
		[Obsolete("Please use Triangle.Border instead", true)]
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

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00006EC0 File Offset: 0x000050C0
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00006EC8 File Offset: 0x000050C8
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

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00006EF4 File Offset: 0x000050F4
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00006EFC File Offset: 0x000050FC
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

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00006F1E File Offset: 0x0000511E
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00006F28 File Offset: 0x00005128
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

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00006F4F File Offset: 0x0000514F
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00006F58 File Offset: 0x00005158
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
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColor(propColorB, value);
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColorNow(propColorC, value);
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00006FA4 File Offset: 0x000051A4
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00006FAC File Offset: 0x000051AC
		public Color ColorA
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00006FCE File Offset: 0x000051CE
		// (set) Token: 0x0600025F RID: 607 RVA: 0x00006FD8 File Offset: 0x000051D8
		public Color ColorB
		{
			get
			{
				return this.colorB;
			}
			set
			{
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColorNow(propColorB, value);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00006FFA File Offset: 0x000051FA
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00007004 File Offset: 0x00005204
		public Color ColorC
		{
			get
			{
				return this.colorC;
			}
			set
			{
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColorNow(propColorC, value);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00007028 File Offset: 0x00005228
		private protected override void SetAllMaterialProperties()
		{
			base.SetVector3(ShapesMaterialUtils.propA, this.a);
			base.SetVector3(ShapesMaterialUtils.propB, this.b);
			base.SetVector3(ShapesMaterialUtils.propC, this.c);
			if (this.colorMode == Triangle.TriangleColorMode.Single)
			{
				base.SetColor(ShapesMaterialUtils.propColorB, this.Color);
				base.SetColor(ShapesMaterialUtils.propColorC, this.Color);
			}
			else
			{
				base.SetColor(ShapesMaterialUtils.propColorB, this.colorB);
				base.SetColor(ShapesMaterialUtils.propColorC, this.colorC);
			}
			base.SetFloat(ShapesMaterialUtils.propRoundness, this.roundness);
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetFloat(ShapesMaterialUtils.propThicknessSpace, (float)this.thicknessSpace);
			base.SetFloat(ShapesMaterialUtils.propBorder, (float)this.border.AsInt());
			this.SetAllDashValues(false);
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00007108 File Offset: 0x00005308
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000710B File Offset: 0x0000530B
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.TriangleMesh[0];
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00007114 File Offset: 0x00005314
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.matTriangle[base.BlendMode];
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000712C File Offset: 0x0000532C
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			Vector3 vector = Vector3.Min(Vector3.Min(this.a, this.b), this.c);
			Vector3 vector2 = Vector3.Max(Vector3.Max(this.a, this.b), this.c);
			return new Bounds((vector + vector2) / 2f, ShapesMath.Abs(vector2 - vector));
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00007195 File Offset: 0x00005395
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000719D File Offset: 0x0000539D
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

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000269 RID: 617 RVA: 0x000071AD File Offset: 0x000053AD
		// (set) Token: 0x0600026A RID: 618 RVA: 0x000071B5 File Offset: 0x000053B5
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

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600026B RID: 619 RVA: 0x000071C5 File Offset: 0x000053C5
		// (set) Token: 0x0600026C RID: 620 RVA: 0x000071D4 File Offset: 0x000053D4
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

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000722A File Offset: 0x0000542A
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000724B File Offset: 0x0000544B
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

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000726A File Offset: 0x0000546A
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00007278 File Offset: 0x00005478
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000729F File Offset: 0x0000549F
		// (set) Token: 0x06000272 RID: 626 RVA: 0x000072AC File Offset: 0x000054AC
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000273 RID: 627 RVA: 0x000072F5 File Offset: 0x000054F5
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00007304 File Offset: 0x00005504
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

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000732B File Offset: 0x0000552B
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00007338 File Offset: 0x00005538
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

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000735F File Offset: 0x0000555F
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000736C File Offset: 0x0000556C
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

		// Token: 0x06000279 RID: 633 RVA: 0x00007393 File Offset: 0x00005593
		private void SetAllDashValues(bool now)
		{
			base.SetAllDashValues(this.dashStyle, this.Dashed, this.matchDashSpacingToSize, this.thickness, true, now);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000073B5 File Offset: 0x000055B5
		private float GetNetDashSpacing()
		{
			return base.GetNetDashSpacing(this.dashStyle, this.dashed, this.matchDashSpacingToSize, this.thickness);
		}

		// Token: 0x04000089 RID: 137
		[SerializeField]
		private Triangle.TriangleColorMode colorMode;

		// Token: 0x0400008A RID: 138
		[SerializeField]
		private Vector3 a = Vector3.zero;

		// Token: 0x0400008B RID: 139
		[SerializeField]
		private Vector3 b = Vector3.up;

		// Token: 0x0400008C RID: 140
		[SerializeField]
		private Vector3 c = Vector3.right;

		// Token: 0x0400008D RID: 141
		[FormerlySerializedAs("hollow")]
		[SerializeField]
		private bool border;

		// Token: 0x0400008E RID: 142
		[SerializeField]
		private float thickness = 0.5f;

		// Token: 0x0400008F RID: 143
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x04000090 RID: 144
		[SerializeField]
		[Range(0f, 1f)]
		private float roundness;

		// Token: 0x04000091 RID: 145
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorB = Color.white;

		// Token: 0x04000092 RID: 146
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorC = Color.white;

		// Token: 0x04000093 RID: 147
		[SerializeField]
		private bool matchDashSpacingToSize = true;

		// Token: 0x04000094 RID: 148
		[SerializeField]
		private bool dashed;

		// Token: 0x04000095 RID: 149
		[SerializeField]
		private DashStyle dashStyle = DashStyle.defaultDashStyleRing;

		// Token: 0x02000088 RID: 136
		public enum TriangleColorMode
		{
			// Token: 0x04000337 RID: 823
			Single,
			// Token: 0x04000338 RID: 824
			PerCorner
		}
	}
}
