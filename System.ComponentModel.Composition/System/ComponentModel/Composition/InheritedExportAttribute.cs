using System;

namespace System.ComponentModel.Composition
{
	// Token: 0x0200004A RID: 74
	[AttributeUsage(1028, AllowMultiple = true, Inherited = true)]
	public class InheritedExportAttribute : ExportAttribute
	{
		// Token: 0x0600020B RID: 523 RVA: 0x00005EFF File Offset: 0x000040FF
		public InheritedExportAttribute() : this(null, null)
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00005F09 File Offset: 0x00004109
		public InheritedExportAttribute(Type contractType) : this(null, contractType)
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00005F13 File Offset: 0x00004113
		public InheritedExportAttribute(string contractName) : this(contractName, null)
		{
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00005F1D File Offset: 0x0000411D
		public InheritedExportAttribute(string contractName, Type contractType) : base(contractName, contractType)
		{
		}
	}
}
