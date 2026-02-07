using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200007A RID: 122
	internal static class ShapesMeshUtils
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0001C722 File Offset: 0x0001A922
		public static Mesh[] QuadMesh
		{
			get
			{
				return ShapesAssets.Instance.meshQuad;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0001C72E File Offset: 0x0001A92E
		public static Mesh[] TriangleMesh
		{
			get
			{
				return ShapesAssets.Instance.meshTriangle;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0001C73A File Offset: 0x0001A93A
		public static Mesh[] SphereMesh
		{
			get
			{
				return ShapesAssets.Instance.meshSphere;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x0001C746 File Offset: 0x0001A946
		public static Mesh[] CuboidMesh
		{
			get
			{
				return ShapesAssets.Instance.meshCube;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x0001C752 File Offset: 0x0001A952
		public static Mesh[] TorusMesh
		{
			get
			{
				return ShapesAssets.Instance.meshTorus;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x0001C75E File Offset: 0x0001A95E
		public static Mesh[] ConeMesh
		{
			get
			{
				return ShapesAssets.Instance.meshCone;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x0001C76A File Offset: 0x0001A96A
		public static Mesh[] ConeMeshUncapped
		{
			get
			{
				return ShapesAssets.Instance.meshConeUncapped;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x0001C776 File Offset: 0x0001A976
		public static Mesh[] CylinderMesh
		{
			get
			{
				return ShapesAssets.Instance.meshCylinder;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0001C782 File Offset: 0x0001A982
		public static Mesh[] CapsuleMesh
		{
			get
			{
				return ShapesAssets.Instance.meshCapsule;
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0001C78E File Offset: 0x0001A98E
		private static Mesh EnsureValidMeshBounds(Mesh mesh, Bounds bounds)
		{
			mesh.hideFlags = HideFlags.HideInInspector;
			mesh.bounds = bounds;
			return mesh;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0001C79F File Offset: 0x0001A99F
		public static Mesh GetLineMesh(LineGeometry geometry, LineEndCap endCaps, DetailLevel detail)
		{
			if (geometry <= LineGeometry.Billboard)
			{
				return ShapesMeshUtils.QuadMesh[0];
			}
			if (geometry != LineGeometry.Volumetric3D)
			{
				return null;
			}
			if (endCaps != LineEndCap.Round)
			{
				return ShapesMeshUtils.CylinderMesh[(int)detail];
			}
			return ShapesMeshUtils.CapsuleMesh[(int)detail];
		}

		// Token: 0x040002FE RID: 766
		private static Mesh quadMesh;

		// Token: 0x040002FF RID: 767
		private static Mesh triangleMesh;

		// Token: 0x04000300 RID: 768
		private static Mesh sphereMesh;

		// Token: 0x04000301 RID: 769
		private static Mesh cuboidMesh;

		// Token: 0x04000302 RID: 770
		private static Mesh torusMesh;

		// Token: 0x04000303 RID: 771
		private static Mesh coneMesh;

		// Token: 0x04000304 RID: 772
		private static Mesh coneMeshUncapped;

		// Token: 0x04000305 RID: 773
		private static Mesh cylinderMesh;

		// Token: 0x04000306 RID: 774
		private static Mesh capsuleMesh;
	}
}
