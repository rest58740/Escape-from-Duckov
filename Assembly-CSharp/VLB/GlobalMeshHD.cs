using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000026 RID: 38
	public static class GlobalMeshHD
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00004598 File Offset: 0x00002798
		public static Mesh Get()
		{
			if (GlobalMeshHD.ms_Mesh == null)
			{
				GlobalMeshHD.Destroy();
				GlobalMeshHD.ms_Mesh = MeshGenerator.GenerateConeZ_Radii_DoubleCaps(1f, 1f, 1f, Config.Instance.sharedMeshSides, true);
				GlobalMeshHD.ms_Mesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			}
			return GlobalMeshHD.ms_Mesh;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000045EF File Offset: 0x000027EF
		public static void Destroy()
		{
			if (GlobalMeshHD.ms_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(GlobalMeshHD.ms_Mesh);
				GlobalMeshHD.ms_Mesh = null;
			}
		}

		// Token: 0x040000CA RID: 202
		private static Mesh ms_Mesh;
	}
}
