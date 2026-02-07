using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000095 RID: 149
	[AttributeUsage(1, AllowMultiple = true)]
	public sealed class RegisterDictionaryKeyPathProviderAttribute : Attribute
	{
		// Token: 0x06000477 RID: 1143 RVA: 0x0001FF63 File Offset: 0x0001E163
		public RegisterDictionaryKeyPathProviderAttribute(Type providerType)
		{
			this.ProviderType = providerType;
		}

		// Token: 0x0400018F RID: 399
		public readonly Type ProviderType;
	}
}
