using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200003D RID: 61
	public class MeshFilterRevert : MonoBehaviour
	{
		// Token: 0x06000153 RID: 339 RVA: 0x0000D0DF File Offset: 0x0000B2DF
		public bool DestroyAndReferenceMeshFilter(MeshFilter mf)
		{
			return true;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000D0E2 File Offset: 0x0000B2E2
		public void RevertMeshFilter(MeshFilter mf)
		{
		}

		// Token: 0x0400019B RID: 411
		public string guid = string.Empty;

		// Token: 0x0400019C RID: 412
		public string meshName;
	}
}
