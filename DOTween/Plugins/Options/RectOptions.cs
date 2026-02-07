using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x0200003B RID: 59
	public struct RectOptions : IPlugOptions
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000D947 File Offset: 0x0000BB47
		public void Reset()
		{
			this.snapping = false;
		}

		// Token: 0x04000109 RID: 265
		public bool snapping;
	}
}
