using System;

namespace FMOD
{
	// Token: 0x0200000F RID: 15
	[Flags]
	public enum MEMORY_TYPE : uint
	{
		// Token: 0x040000A6 RID: 166
		NORMAL = 0U,
		// Token: 0x040000A7 RID: 167
		STREAM_FILE = 1U,
		// Token: 0x040000A8 RID: 168
		STREAM_DECODE = 2U,
		// Token: 0x040000A9 RID: 169
		SAMPLEDATA = 4U,
		// Token: 0x040000AA RID: 170
		DSP_BUFFER = 8U,
		// Token: 0x040000AB RID: 171
		PLUGIN = 16U,
		// Token: 0x040000AC RID: 172
		PERSISTENT = 2097152U,
		// Token: 0x040000AD RID: 173
		ALL = 4294967295U
	}
}
