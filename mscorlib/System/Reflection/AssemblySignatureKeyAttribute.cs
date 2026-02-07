using System;

namespace System.Reflection
{
	// Token: 0x0200088E RID: 2190
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	public sealed class AssemblySignatureKeyAttribute : Attribute
	{
		// Token: 0x06004871 RID: 18545 RVA: 0x000EE15B File Offset: 0x000EC35B
		public AssemblySignatureKeyAttribute(string publicKey, string countersignature)
		{
			this.PublicKey = publicKey;
			this.Countersignature = countersignature;
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06004872 RID: 18546 RVA: 0x000EE171 File Offset: 0x000EC371
		public string PublicKey { get; }

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06004873 RID: 18547 RVA: 0x000EE179 File Offset: 0x000EC379
		public string Countersignature { get; }
	}
}
