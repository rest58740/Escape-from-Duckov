using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x0200003C RID: 60
	public struct StringOptions : IPlugOptions
	{
		// Token: 0x06000258 RID: 600 RVA: 0x0000D950 File Offset: 0x0000BB50
		public void Reset()
		{
			this.richTextEnabled = false;
			this.scrambleMode = ScrambleMode.None;
			this.scrambledChars = null;
			this.startValueStrippedLength = (this.changeValueStrippedLength = 0);
		}

		// Token: 0x0400010A RID: 266
		public bool richTextEnabled;

		// Token: 0x0400010B RID: 267
		public ScrambleMode scrambleMode;

		// Token: 0x0400010C RID: 268
		public char[] scrambledChars;

		// Token: 0x0400010D RID: 269
		internal int startValueStrippedLength;

		// Token: 0x0400010E RID: 270
		internal int changeValueStrippedLength;
	}
}
