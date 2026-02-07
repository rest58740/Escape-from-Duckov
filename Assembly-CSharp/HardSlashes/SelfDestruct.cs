using System;
using UnityEngine;

namespace HardSlashes
{
	// Token: 0x0200005F RID: 95
	public class SelfDestruct : MonoBehaviour
	{
		// Token: 0x06000339 RID: 825 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		private void Start()
		{
			if (this.selfdestruct_in != 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.selfdestruct_in);
			}
		}

		// Token: 0x04000224 RID: 548
		public float selfdestruct_in = 4f;
	}
}
