using System;

namespace System.Reflection
{
	// Token: 0x020008B3 RID: 2227
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public sealed class ObfuscateAssemblyAttribute : Attribute
	{
		// Token: 0x060049B2 RID: 18866 RVA: 0x000EF17C File Offset: 0x000ED37C
		public ObfuscateAssemblyAttribute(bool assemblyIsPrivate)
		{
			this.AssemblyIsPrivate = assemblyIsPrivate;
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x060049B3 RID: 18867 RVA: 0x000EF192 File Offset: 0x000ED392
		public bool AssemblyIsPrivate { get; }

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x060049B4 RID: 18868 RVA: 0x000EF19A File Offset: 0x000ED39A
		// (set) Token: 0x060049B5 RID: 18869 RVA: 0x000EF1A2 File Offset: 0x000ED3A2
		public bool StripAfterObfuscation { get; set; } = true;
	}
}
