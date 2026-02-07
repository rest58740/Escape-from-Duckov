using System;
using UnityEngine;

namespace EPOOutline
{
	// Token: 0x0200001B RID: 27
	public static class RendererUtility
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x000060DC File Offset: 0x000042DC
		public static int GetSubmeshCount(Renderer renderer)
		{
			if (renderer is MeshRenderer)
			{
				return renderer.GetComponent<MeshFilter>().sharedMesh.subMeshCount;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer != null)
			{
				return skinnedMeshRenderer.sharedMesh.subMeshCount;
			}
			return 1;
		}
	}
}
