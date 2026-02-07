using System;
using UnityEngine;

namespace DG.DemiLib
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public struct IntRange
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002B8F File Offset: 0x00000D8F
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B9F File Offset: 0x00000D9F
		public float RandomWithin()
		{
			return (float)Random.Range(this.min, this.max + 1);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002BB8 File Offset: 0x00000DB8
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

		// Token: 0x04000035 RID: 53
		public int min;

		// Token: 0x04000036 RID: 54
		public int max;
	}
}
