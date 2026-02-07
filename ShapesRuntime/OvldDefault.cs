using System;

namespace Shapes
{
	// Token: 0x0200001A RID: 26
	[AttributeUsage(2048)]
	internal class OvldDefault : Attribute
	{
		// Token: 0x06000B17 RID: 2839 RVA: 0x0001512A File Offset: 0x0001332A
		public OvldDefault(string @default)
		{
			this.@default = @default;
		}

		// Token: 0x040000CA RID: 202
		public string @default;
	}
}
