using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200003B RID: 59
	public static class GlobalMeshSD
	{
		// Token: 0x060001EC RID: 492 RVA: 0x00008A30 File Offset: 0x00006C30
		public static Mesh Get()
		{
			bool sd_requiresDoubleSidedMesh = Config.Instance.SD_requiresDoubleSidedMesh;
			if (GlobalMeshSD.ms_Mesh == null || GlobalMeshSD.ms_DoubleSided != sd_requiresDoubleSidedMesh)
			{
				GlobalMeshSD.Destroy();
				GlobalMeshSD.ms_Mesh = MeshGenerator.GenerateConeZ_Radii(1f, 1f, 1f, Config.Instance.sharedMeshSides, Config.Instance.sharedMeshSegments, true, sd_requiresDoubleSidedMesh);
				GlobalMeshSD.ms_Mesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				GlobalMeshSD.ms_DoubleSided = sd_requiresDoubleSidedMesh;
			}
			return GlobalMeshSD.ms_Mesh;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00008AAB File Offset: 0x00006CAB
		public static void Destroy()
		{
			if (GlobalMeshSD.ms_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(GlobalMeshSD.ms_Mesh);
				GlobalMeshSD.ms_Mesh = null;
			}
		}

		// Token: 0x04000140 RID: 320
		private static Mesh ms_Mesh;

		// Token: 0x04000141 RID: 321
		private static bool ms_DoubleSided;
	}
}
