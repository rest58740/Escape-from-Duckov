using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000003 RID: 3
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Cone")]
	public class Cone : ShapeRenderer
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020F4 File Offset: 0x000002F4
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020FC File Offset: 0x000002FC
		public float Length
		{
			get
			{
				return this.length;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propLength, this.length = Mathf.Max(0f, value));
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002128 File Offset: 0x00000328
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002130 File Offset: 0x00000330
		[Obsolete("this property is obsolete I'm sorry! this was a typo, please use SizeSpace instead!", true)]
		public ThicknessSpace RadiusSpace
		{
			get
			{
				return this.SizeSpace;
			}
			set
			{
				this.SizeSpace = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002139 File Offset: 0x00000339
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002144 File Offset: 0x00000344
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002166 File Offset: 0x00000366
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000216E File Offset: 0x0000036E
		public bool FillCap
		{
			get
			{
				return this.fillCap;
			}
			set
			{
				this.fillCap = value;
				base.UpdateMesh(true);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000217E File Offset: 0x0000037E
		private protected override void SetAllMaterialProperties()
		{
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetFloat(ShapesMaterialUtils.propLength, this.length);
			base.SetInt(ShapesMaterialUtils.propSizeSpace, (int)this.sizeSpace);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021B3 File Offset: 0x000003B3
		private protected override void ShapeClampRanges()
		{
			this.radius = Mathf.Max(0f, this.radius);
			this.length = Mathf.Max(0f, this.length);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000021E1 File Offset: 0x000003E1
		internal override bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000021E4 File Offset: 0x000003E4
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021E7 File Offset: 0x000003E7
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.matCone[base.BlendMode];
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021FC File Offset: 0x000003FC
		private protected override Mesh GetInitialMeshAsset()
		{
			if (!this.fillCap)
			{
				return ShapesMeshUtils.ConeMeshUncapped[(int)this.detailLevel];
			}
			return ShapesMeshUtils.ConeMesh[(int)this.detailLevel];
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002220 File Offset: 0x00000420
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			if (this.sizeSpace != ThicknessSpace.Meters)
			{
				return new Bounds(Vector3.zero, Vector3.zero);
			}
			return new Bounds(new Vector3(0f, 0f, this.length / 2f), new Vector3(this.radius * 2f, this.radius * 2f, this.length));
		}

		// Token: 0x04000001 RID: 1
		[SerializeField]
		private float radius = 1f;

		// Token: 0x04000002 RID: 2
		[SerializeField]
		private float length = 1.5f;

		// Token: 0x04000003 RID: 3
		[SerializeField]
		private ThicknessSpace sizeSpace;

		// Token: 0x04000004 RID: 4
		[SerializeField]
		private bool fillCap = true;
	}
}
