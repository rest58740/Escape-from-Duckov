using System;
using System.Configuration.Assemblies;

namespace System.Reflection
{
	// Token: 0x0200087D RID: 2173
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyAlgorithmIdAttribute : Attribute
	{
		// Token: 0x0600484E RID: 18510 RVA: 0x000EDFE5 File Offset: 0x000EC1E5
		public AssemblyAlgorithmIdAttribute(AssemblyHashAlgorithm algorithmId)
		{
			this.AlgorithmId = algorithmId;
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x000EDFE5 File Offset: 0x000EC1E5
		[CLSCompliant(false)]
		public AssemblyAlgorithmIdAttribute(uint algorithmId)
		{
			this.AlgorithmId = algorithmId;
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06004850 RID: 18512 RVA: 0x000EDFF4 File Offset: 0x000EC1F4
		[CLSCompliant(false)]
		public uint AlgorithmId { get; }
	}
}
