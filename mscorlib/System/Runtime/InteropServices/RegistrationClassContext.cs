using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200074D RID: 1869
	[Flags]
	public enum RegistrationClassContext
	{
		// Token: 0x04002BDF RID: 11231
		DisableActivateAsActivator = 32768,
		// Token: 0x04002BE0 RID: 11232
		EnableActivateAsActivator = 65536,
		// Token: 0x04002BE1 RID: 11233
		EnableCodeDownload = 8192,
		// Token: 0x04002BE2 RID: 11234
		FromDefaultContext = 131072,
		// Token: 0x04002BE3 RID: 11235
		InProcessHandler = 2,
		// Token: 0x04002BE4 RID: 11236
		InProcessHandler16 = 32,
		// Token: 0x04002BE5 RID: 11237
		InProcessServer = 1,
		// Token: 0x04002BE6 RID: 11238
		InProcessServer16 = 8,
		// Token: 0x04002BE7 RID: 11239
		LocalServer = 4,
		// Token: 0x04002BE8 RID: 11240
		NoCodeDownload = 1024,
		// Token: 0x04002BE9 RID: 11241
		NoCustomMarshal = 4096,
		// Token: 0x04002BEA RID: 11242
		NoFailureLog = 16384,
		// Token: 0x04002BEB RID: 11243
		RemoteServer = 16,
		// Token: 0x04002BEC RID: 11244
		Reserved1 = 64,
		// Token: 0x04002BED RID: 11245
		Reserved2 = 128,
		// Token: 0x04002BEE RID: 11246
		Reserved3 = 256,
		// Token: 0x04002BEF RID: 11247
		Reserved4 = 512,
		// Token: 0x04002BF0 RID: 11248
		Reserved5 = 2048
	}
}
