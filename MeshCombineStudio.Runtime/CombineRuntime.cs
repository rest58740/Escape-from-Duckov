using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200000C RID: 12
	public class CombineRuntime : MonoBehaviour
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00003120 File Offset: 0x00001320
		private void Start()
		{
			this.Combine();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003128 File Offset: 0x00001328
		private void Combine()
		{
			this.meshCombiner.searchOptions.parentGOs = this.gos;
			this.meshCombiner.CombineAll(this.useSearchConditions);
		}

		// Token: 0x04000026 RID: 38
		public MeshCombiner meshCombiner;

		// Token: 0x04000027 RID: 39
		public bool useSearchConditions = true;

		// Token: 0x04000028 RID: 40
		public GameObject[] gos;
	}
}
