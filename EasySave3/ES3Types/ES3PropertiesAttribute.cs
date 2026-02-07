using System;

namespace ES3Types
{
	// Token: 0x0200002C RID: 44
	[AttributeUsage(AttributeTargets.Class)]
	public class ES3PropertiesAttribute : Attribute
	{
		// Token: 0x06000258 RID: 600 RVA: 0x00009367 File Offset: 0x00007567
		public ES3PropertiesAttribute(params string[] members)
		{
			this.members = members;
		}

		// Token: 0x04000071 RID: 113
		public readonly string[] members;
	}
}
