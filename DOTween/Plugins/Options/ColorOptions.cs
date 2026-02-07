using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000039 RID: 57
	public struct ColorOptions : IPlugOptions
	{
		// Token: 0x06000255 RID: 597 RVA: 0x0000D935 File Offset: 0x0000BB35
		public void Reset()
		{
			this.alphaOnly = false;
		}

		// Token: 0x04000107 RID: 263
		public bool alphaOnly;
	}
}
