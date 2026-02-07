using System;

namespace Steamworks
{
	// Token: 0x0200018B RID: 395
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
	internal class CallbackIdentityAttribute : Attribute
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x0000D5CB File Offset: 0x0000B7CB
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x0000D5D3 File Offset: 0x0000B7D3
		public int Identity { get; set; }

		// Token: 0x06000904 RID: 2308 RVA: 0x0000D5DC File Offset: 0x0000B7DC
		public CallbackIdentityAttribute(int callbackNum)
		{
			this.Identity = callbackNum;
		}
	}
}
