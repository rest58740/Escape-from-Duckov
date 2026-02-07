using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000052 RID: 82
	public struct GameObjectLayer
	{
		// Token: 0x060001AF RID: 431 RVA: 0x0000F027 File Offset: 0x0000D227
		public GameObjectLayer(GameObject go)
		{
			this.go = go;
			this.layer = go.layer;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000F03C File Offset: 0x0000D23C
		public void RestoreLayer()
		{
			this.go.layer = this.layer;
		}

		// Token: 0x0400021B RID: 539
		public GameObject go;

		// Token: 0x0400021C RID: 540
		public int layer;
	}
}
