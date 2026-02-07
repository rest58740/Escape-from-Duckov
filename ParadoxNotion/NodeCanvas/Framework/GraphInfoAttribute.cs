using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000003 RID: 3
	[AttributeUsage(4)]
	public class GraphInfoAttribute : Attribute
	{
		// Token: 0x04000001 RID: 1
		public string packageName;

		// Token: 0x04000002 RID: 2
		public string docsURL;

		// Token: 0x04000003 RID: 3
		public string resourcesURL;

		// Token: 0x04000004 RID: 4
		public string forumsURL;
	}
}
