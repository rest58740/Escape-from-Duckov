using System;

namespace System.Runtime
{
	// Token: 0x0200054C RID: 1356
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyTargetedPatchBandAttribute : Attribute
	{
		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060035A7 RID: 13735 RVA: 0x000C1F81 File Offset: 0x000C0181
		public string TargetedPatchBand { get; }

		// Token: 0x060035A8 RID: 13736 RVA: 0x000C1F89 File Offset: 0x000C0189
		public AssemblyTargetedPatchBandAttribute(string targetedPatchBand)
		{
			this.TargetedPatchBand = targetedPatchBand;
		}
	}
}
