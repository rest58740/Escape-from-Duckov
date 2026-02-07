using System;

namespace System.Reflection
{
	// Token: 0x020008B4 RID: 2228
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
	public sealed class ObfuscationAttribute : Attribute
	{
		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x060049B7 RID: 18871 RVA: 0x000EF1D3 File Offset: 0x000ED3D3
		// (set) Token: 0x060049B8 RID: 18872 RVA: 0x000EF1DB File Offset: 0x000ED3DB
		public bool StripAfterObfuscation { get; set; } = true;

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x060049B9 RID: 18873 RVA: 0x000EF1E4 File Offset: 0x000ED3E4
		// (set) Token: 0x060049BA RID: 18874 RVA: 0x000EF1EC File Offset: 0x000ED3EC
		public bool Exclude { get; set; } = true;

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x000EF1F5 File Offset: 0x000ED3F5
		// (set) Token: 0x060049BC RID: 18876 RVA: 0x000EF1FD File Offset: 0x000ED3FD
		public bool ApplyToMembers { get; set; } = true;

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x000EF206 File Offset: 0x000ED406
		// (set) Token: 0x060049BE RID: 18878 RVA: 0x000EF20E File Offset: 0x000ED40E
		public string Feature { get; set; } = "all";
	}
}
