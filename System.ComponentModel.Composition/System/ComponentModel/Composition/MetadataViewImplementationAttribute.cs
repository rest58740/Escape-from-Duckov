using System;

namespace System.ComponentModel.Composition
{
	// Token: 0x0200004E RID: 78
	[AttributeUsage(1024, AllowMultiple = false, Inherited = false)]
	public sealed class MetadataViewImplementationAttribute : Attribute
	{
		// Token: 0x0600021B RID: 539 RVA: 0x000066CC File Offset: 0x000048CC
		public MetadataViewImplementationAttribute(Type implementationType)
		{
			this.ImplementationType = implementationType;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600021C RID: 540 RVA: 0x000066DB File Offset: 0x000048DB
		// (set) Token: 0x0600021D RID: 541 RVA: 0x000066E3 File Offset: 0x000048E3
		public Type ImplementationType { get; private set; }
	}
}
