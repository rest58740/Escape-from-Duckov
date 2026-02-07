using System;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000035 RID: 53
	[AttributeUsage(452, AllowMultiple = true, Inherited = false)]
	public class ExportAttribute : Attribute
	{
		// Token: 0x060001AA RID: 426 RVA: 0x0000585F File Offset: 0x00003A5F
		public ExportAttribute() : this(null, null)
		{
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005869 File Offset: 0x00003A69
		public ExportAttribute(Type contractType) : this(null, contractType)
		{
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00005873 File Offset: 0x00003A73
		public ExportAttribute(string contractName) : this(contractName, null)
		{
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000587D File Offset: 0x00003A7D
		public ExportAttribute(string contractName, Type contractType)
		{
			this.ContractName = contractName;
			this.ContractType = contractType;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00005893 File Offset: 0x00003A93
		// (set) Token: 0x060001AF RID: 431 RVA: 0x0000589B File Offset: 0x00003A9B
		public string ContractName { get; private set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000058A4 File Offset: 0x00003AA4
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x000058AC File Offset: 0x00003AAC
		public Type ContractType { get; private set; }
	}
}
