using System;

namespace FMODUnity
{
	// Token: 0x0200011F RID: 287
	[Serializable]
	public class CodecChannelCount
	{
		// Token: 0x06000767 RID: 1895 RVA: 0x0000A5A2 File Offset: 0x000087A2
		public CodecChannelCount()
		{
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0000A5AA File Offset: 0x000087AA
		public CodecChannelCount(CodecChannelCount other)
		{
			this.format = other.format;
			this.channels = other.channels;
		}

		// Token: 0x04000605 RID: 1541
		public CodecType format;

		// Token: 0x04000606 RID: 1542
		public int channels;
	}
}
