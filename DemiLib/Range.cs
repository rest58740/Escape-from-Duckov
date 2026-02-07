using System;
using UnityEngine;

namespace DG.DemiLib
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public struct Range
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002C04 File Offset: 0x00000E04
		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002C14 File Offset: 0x00000E14
		public float RandomWithin()
		{
			return Random.Range(this.min, this.max);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C28 File Offset: 0x00000E28
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.min.ToString(),
				"/",
				this.max.ToString(),
				")"
			});
		}

		// Token: 0x04000037 RID: 55
		public float min;

		// Token: 0x04000038 RID: 56
		public float max;
	}
}
