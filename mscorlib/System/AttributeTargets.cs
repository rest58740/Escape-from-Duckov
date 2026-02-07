using System;

namespace System
{
	// Token: 0x020000FD RID: 253
	[Flags]
	public enum AttributeTargets
	{
		// Token: 0x04001054 RID: 4180
		Assembly = 1,
		// Token: 0x04001055 RID: 4181
		Module = 2,
		// Token: 0x04001056 RID: 4182
		Class = 4,
		// Token: 0x04001057 RID: 4183
		Struct = 8,
		// Token: 0x04001058 RID: 4184
		Enum = 16,
		// Token: 0x04001059 RID: 4185
		Constructor = 32,
		// Token: 0x0400105A RID: 4186
		Method = 64,
		// Token: 0x0400105B RID: 4187
		Property = 128,
		// Token: 0x0400105C RID: 4188
		Field = 256,
		// Token: 0x0400105D RID: 4189
		Event = 512,
		// Token: 0x0400105E RID: 4190
		Interface = 1024,
		// Token: 0x0400105F RID: 4191
		Parameter = 2048,
		// Token: 0x04001060 RID: 4192
		Delegate = 4096,
		// Token: 0x04001061 RID: 4193
		ReturnValue = 8192,
		// Token: 0x04001062 RID: 4194
		GenericParameter = 16384,
		// Token: 0x04001063 RID: 4195
		All = 32767
	}
}
