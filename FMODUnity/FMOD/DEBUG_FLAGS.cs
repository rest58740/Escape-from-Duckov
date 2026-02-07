using System;

namespace FMOD
{
	// Token: 0x0200000E RID: 14
	[Flags]
	public enum DEBUG_FLAGS : uint
	{
		// Token: 0x0400009A RID: 154
		NONE = 0U,
		// Token: 0x0400009B RID: 155
		ERROR = 1U,
		// Token: 0x0400009C RID: 156
		WARNING = 2U,
		// Token: 0x0400009D RID: 157
		LOG = 4U,
		// Token: 0x0400009E RID: 158
		TYPE_MEMORY = 256U,
		// Token: 0x0400009F RID: 159
		TYPE_FILE = 512U,
		// Token: 0x040000A0 RID: 160
		TYPE_CODEC = 1024U,
		// Token: 0x040000A1 RID: 161
		TYPE_TRACE = 2048U,
		// Token: 0x040000A2 RID: 162
		DISPLAY_TIMESTAMPS = 65536U,
		// Token: 0x040000A3 RID: 163
		DISPLAY_LINENUMBERS = 131072U,
		// Token: 0x040000A4 RID: 164
		DISPLAY_THREAD = 262144U
	}
}
