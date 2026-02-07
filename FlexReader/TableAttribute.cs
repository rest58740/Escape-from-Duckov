using System;

namespace FlexFramework.Excel
{
	// Token: 0x02000025 RID: 37
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public sealed class TableAttribute : Attribute
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000501F File Offset: 0x0000321F
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00005027 File Offset: 0x00003227
		public int[] Ignore { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005030 File Offset: 0x00003230
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00005038 File Offset: 0x00003238
		public bool SafeMode { get; set; }

		// Token: 0x06000102 RID: 258 RVA: 0x00005041 File Offset: 0x00003241
		public TableAttribute(params int[] ignore)
		{
			this.Ignore = ignore;
		}
	}
}
