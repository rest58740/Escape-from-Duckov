using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000004 RID: 4
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Cuboid")]
	public class Cuboid : ShapeRenderer
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000022AD File Offset: 0x000004AD
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000022B8 File Offset: 0x000004B8
		public Vector3 Size
		{
			get
			{
				return this.size;
			}
			set
			{
				int propSize = ShapesMaterialUtils.propSize;
				this.size = value;
				base.SetVector3Now(propSize, value);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000022DA File Offset: 0x000004DA
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000022E4 File Offset: 0x000004E4
		public ThicknessSpace SizeSpace
		{
			get
			{
				return this.sizeSpace;
			}
			set
			{
				int propSizeSpace = ShapesMaterialUtils.propSizeSpace;
				this.sizeSpace = value;
				base.SetIntNow(propSizeSpace, (int)value);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002306 File Offset: 0x00000506
		private protected override void SetAllMaterialProperties()
		{
			base.SetVector3(ShapesMaterialUtils.propSize, this.size);
			base.SetInt(ShapesMaterialUtils.propSizeSpace, (int)this.sizeSpace);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000232A File Offset: 0x0000052A
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000232D File Offset: 0x0000052D
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002330 File Offset: 0x00000530
		private protected override void ShapeClampRanges()
		{
			this.size = Vector3.Max(default(Vector3), this.size);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002357 File Offset: 0x00000557
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.matCuboid[base.BlendMode];
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000236C File Offset: 0x0000056C
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.CuboidMesh[0];
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002378 File Offset: 0x00000578
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			if (this.sizeSpace != ThicknessSpace.Meters)
			{
				return new Bounds(default(Vector3), Vector3.zero);
			}
			return new Bounds(default(Vector3), this.size);
		}

		// Token: 0x04000005 RID: 5
		[SerializeField]
		private Vector3 size = Vector3.one;

		// Token: 0x04000006 RID: 6
		[SerializeField]
		private ThicknessSpace sizeSpace;
	}
}
