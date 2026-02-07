using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000801 RID: 2049
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	[Serializable]
	public sealed class RuntimeCompatibilityAttribute : Attribute
	{
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06004615 RID: 17941 RVA: 0x000E5855 File Offset: 0x000E3A55
		// (set) Token: 0x06004616 RID: 17942 RVA: 0x000E585D File Offset: 0x000E3A5D
		public bool WrapNonExceptionThrows { get; set; }
	}
}
