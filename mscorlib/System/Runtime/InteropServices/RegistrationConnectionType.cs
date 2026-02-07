using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006CD RID: 1741
	[Flags]
	public enum RegistrationConnectionType
	{
		// Token: 0x04002A09 RID: 10761
		SingleUse = 0,
		// Token: 0x04002A0A RID: 10762
		MultipleUse = 1,
		// Token: 0x04002A0B RID: 10763
		MultiSeparate = 2,
		// Token: 0x04002A0C RID: 10764
		Suspended = 4,
		// Token: 0x04002A0D RID: 10765
		Surrogate = 8
	}
}
