using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000033 RID: 51
	[ExecuteInEditMode]
	public class GarbageCollectMesh : MonoBehaviour
	{
		// Token: 0x06000121 RID: 289 RVA: 0x0000B0EA File Offset: 0x000092EA
		private void OnDestroy()
		{
			if (this.mesh != null)
			{
				UnityEngine.Object.Destroy(this.mesh);
			}
		}

		// Token: 0x0400013A RID: 314
		public Mesh mesh;
	}
}
