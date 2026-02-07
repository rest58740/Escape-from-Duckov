using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000036 RID: 54
	public struct UintOptions : IPlugOptions
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000D915 File Offset: 0x0000BB15
		public void Reset()
		{
			this.isNegativeChangeValue = false;
		}

		// Token: 0x04000103 RID: 259
		public bool isNegativeChangeValue;
	}
}
