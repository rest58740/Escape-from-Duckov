using System;

namespace Sirenix.Utilities
{
	// Token: 0x02000033 RID: 51
	[AttributeUsage(1, AllowMultiple = false, Inherited = false)]
	public class SirenixBuildVersionAttribute : Attribute
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000D0BF File Offset: 0x0000B2BF
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0000D0C7 File Offset: 0x0000B2C7
		public string Version { get; private set; }

		// Token: 0x06000237 RID: 567 RVA: 0x0000D0D0 File Offset: 0x0000B2D0
		public SirenixBuildVersionAttribute(string version)
		{
			this.Version = version;
		}
	}
}
