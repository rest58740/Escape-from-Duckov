using System;

namespace Sirenix.Utilities
{
	// Token: 0x02000032 RID: 50
	[AttributeUsage(1, AllowMultiple = false, Inherited = false)]
	public class SirenixBuildNameAttribute : Attribute
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000D09F File Offset: 0x0000B29F
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000D0A7 File Offset: 0x0000B2A7
		public string BuildName { get; private set; }

		// Token: 0x06000234 RID: 564 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		public SirenixBuildNameAttribute(string buildName)
		{
			this.BuildName = buildName;
		}
	}
}
