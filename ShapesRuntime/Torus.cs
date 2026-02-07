using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000012 RID: 18
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Torus")]
	public class Torus : ShapeRenderer
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00006A3B File Offset: 0x00004C3B
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00006A44 File Offset: 0x00004C44
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00006A70 File Offset: 0x00004C70
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00006A78 File Offset: 0x00004C78
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00006AA4 File Offset: 0x00004CA4
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00006AAC File Offset: 0x00004CAC
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00006ACE File Offset: 0x00004CCE
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00006AD8 File Offset: 0x00004CD8
		public ThicknessSpace RadiusSpace
		{
			get
			{
				return this.radiusSpace;
			}
			set
			{
				int propThicknessSpace = ShapesMaterialUtils.propThicknessSpace;
				this.radiusSpace = value;
				base.SetIntNow(propThicknessSpace, (int)value);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00006AFA File Offset: 0x00004CFA
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00006B04 File Offset: 0x00004D04
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00006B26 File Offset: 0x00004D26
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00006B30 File Offset: 0x00004D30
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

		// Token: 0x0600023B RID: 571 RVA: 0x00006B54 File Offset: 0x00004D54
		private protected override void SetAllMaterialProperties()
		{
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetInt(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetInt(ShapesMaterialUtils.propRadiusSpace, (int)this.radiusSpace);
			base.SetFloat(ShapesMaterialUtils.propAngStart, this.angRadiansStart);
			base.SetFloat(ShapesMaterialUtils.propAngEnd, this.angRadiansEnd);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00006BC7 File Offset: 0x00004DC7
		private protected override void ShapeClampRanges()
		{
			this.radius = Mathf.Max(0f, this.radius);
			this.thickness = Mathf.Max(0f, this.thickness);
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00006BF5 File Offset: 0x00004DF5
		internal override bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00006BF8 File Offset: 0x00004DF8
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.matTorus[base.BlendMode];
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00006C0D File Offset: 0x00004E0D
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.TorusMesh[(int)this.detailLevel];
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00006C1C File Offset: 0x00004E1C
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			float num = (this.radiusSpace == ThicknessSpace.Meters) ? (this.radius * 2f) : 0f;
			float num2 = (this.thicknessSpace == ThicknessSpace.Meters) ? this.thickness : 0f;
			num += num2;
			return new Bounds(Vector3.zero, new Vector3(num, num, num2));
		}

		// Token: 0x04000082 RID: 130
		[SerializeField]
		private float radius = 1f;

		// Token: 0x04000083 RID: 131
		[SerializeField]
		private float thickness = 0.5f;

		// Token: 0x04000084 RID: 132
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x04000085 RID: 133
		[SerializeField]
		private ThicknessSpace radiusSpace;

		// Token: 0x04000086 RID: 134
		[SerializeField]
		private AngularUnit angUnitInput = AngularUnit.Degrees;

		// Token: 0x04000087 RID: 135
		[SerializeField]
		private float angRadiansStart;

		// Token: 0x04000088 RID: 136
		[SerializeField]
		private float angRadiansEnd = 6.2831855f;
	}
}
