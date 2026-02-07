using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000710 RID: 1808
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class BestFitMappingAttribute : Attribute
	{
		// Token: 0x060040C1 RID: 16577 RVA: 0x000E162C File Offset: 0x000DF82C
		public BestFitMappingAttribute(bool BestFitMapping)
		{
			this._bestFitMapping = BestFitMapping;
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x000E163B File Offset: 0x000DF83B
		public bool BestFitMapping
		{
			get
			{
				return this._bestFitMapping;
			}
		}

		// Token: 0x04002AED RID: 10989
		internal bool _bestFitMapping;

		// Token: 0x04002AEE RID: 10990
		public bool ThrowOnUnmappableChar;
	}
}
