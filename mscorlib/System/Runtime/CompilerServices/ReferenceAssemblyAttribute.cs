using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000800 RID: 2048
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	[Serializable]
	public sealed class ReferenceAssemblyAttribute : Attribute
	{
		// Token: 0x06004611 RID: 17937 RVA: 0x00002050 File Offset: 0x00000250
		public ReferenceAssemblyAttribute()
		{
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x000E583E File Offset: 0x000E3A3E
		public ReferenceAssemblyAttribute(string description)
		{
			this.Description = description;
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06004613 RID: 17939 RVA: 0x000E584D File Offset: 0x000E3A4D
		public string Description { get; }
	}
}
