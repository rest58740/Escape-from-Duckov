using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x0200003A RID: 58
	public struct FloatOptions : IPlugOptions
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000D93E File Offset: 0x0000BB3E
		public void Reset()
		{
			this.snapping = false;
		}

		// Token: 0x04000108 RID: 264
		public bool snapping;
	}
}
