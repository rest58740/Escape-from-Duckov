using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000F RID: 15
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Sphere")]
	public class Sphere : ShapeRenderer
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00006776 File Offset: 0x00004976
		// (set) Token: 0x06000219 RID: 537 RVA: 0x00006780 File Offset: 0x00004980
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000067AC File Offset: 0x000049AC
		// (set) Token: 0x0600021B RID: 539 RVA: 0x000067B4 File Offset: 0x000049B4
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

		// Token: 0x0600021C RID: 540 RVA: 0x000067D6 File Offset: 0x000049D6
		private protected override void SetAllMaterialProperties()
		{
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetInt(ShapesMaterialUtils.propRadiusSpace, (int)this.radiusSpace);
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600021D RID: 541 RVA: 0x000067FA File Offset: 0x000049FA
		internal override bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600021E RID: 542 RVA: 0x000067FD File Offset: 0x000049FD
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006800 File Offset: 0x00004A00
		private protected override void ShapeClampRanges()
		{
			this.radius = Mathf.Max(0f, this.radius);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00006818 File Offset: 0x00004A18
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.matSphere[base.BlendMode];
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000682D File Offset: 0x00004A2D
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.SphereMesh[(int)this.detailLevel];
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000683C File Offset: 0x00004A3C
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			float num = (this.radiusSpace == ThicknessSpace.Meters) ? (2f * this.radius) : 0f;
			return new Bounds(Vector3.zero, new Vector3(num, num, num));
		}

		// Token: 0x04000079 RID: 121
		[SerializeField]
		private float radius = 1f;

		// Token: 0x0400007A RID: 122
		[SerializeField]
		private ThicknessSpace radiusSpace;
	}
}
