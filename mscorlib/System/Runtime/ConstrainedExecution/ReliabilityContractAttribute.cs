using System;

namespace System.Runtime.ConstrainedExecution
{
	// Token: 0x020007D5 RID: 2005
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Interface, Inherited = false)]
	public sealed class ReliabilityContractAttribute : Attribute
	{
		// Token: 0x060045B2 RID: 17842 RVA: 0x000E5107 File Offset: 0x000E3307
		public ReliabilityContractAttribute(Consistency consistencyGuarantee, Cer cer)
		{
			this.ConsistencyGuarantee = consistencyGuarantee;
			this.Cer = cer;
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060045B3 RID: 17843 RVA: 0x000E511D File Offset: 0x000E331D
		public Consistency ConsistencyGuarantee { get; }

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060045B4 RID: 17844 RVA: 0x000E5125 File Offset: 0x000E3325
		public Cer Cer { get; }
	}
}
