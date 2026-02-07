using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020008B0 RID: 2224
	public sealed class Missing : ISerializable
	{
		// Token: 0x06004970 RID: 18800 RVA: 0x0000259F File Offset: 0x0000079F
		private Missing()
		{
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x0001B98F File Offset: 0x00019B8F
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x04002EEF RID: 12015
		public static readonly Missing Value = new Missing();
	}
}
