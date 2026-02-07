using System;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000047 RID: 71
	[AttributeUsage(2432, AllowMultiple = false, Inherited = false)]
	public class ImportManyAttribute : Attribute, IAttributedImport
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00005E74 File Offset: 0x00004074
		public ImportManyAttribute() : this(null)
		{
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00005E7D File Offset: 0x0000407D
		public ImportManyAttribute(Type contractType) : this(null, contractType)
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00005E87 File Offset: 0x00004087
		public ImportManyAttribute(string contractName) : this(contractName, null)
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00005E91 File Offset: 0x00004091
		public ImportManyAttribute(string contractName, Type contractType)
		{
			this.ContractName = contractName;
			this.ContractType = contractType;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00005EA7 File Offset: 0x000040A7
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00005EAF File Offset: 0x000040AF
		public string ContractName { get; private set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00005EB8 File Offset: 0x000040B8
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00005EC0 File Offset: 0x000040C0
		public Type ContractType { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00005EC9 File Offset: 0x000040C9
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00005ED1 File Offset: 0x000040D1
		public bool AllowRecomposition { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00005EDA File Offset: 0x000040DA
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00005EE2 File Offset: 0x000040E2
		public CreationPolicy RequiredCreationPolicy { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00005EEB File Offset: 0x000040EB
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00005EF3 File Offset: 0x000040F3
		public ImportSource Source { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00005EFC File Offset: 0x000040FC
		ImportCardinality IAttributedImport.Cardinality
		{
			get
			{
				return ImportCardinality.ZeroOrMore;
			}
		}
	}
}
