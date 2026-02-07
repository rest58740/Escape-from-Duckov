using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200014E RID: 334
	[TypeForwardedFrom("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
	[Serializable]
	public class InvalidTimeZoneException : Exception
	{
		// Token: 0x06000C96 RID: 3222 RVA: 0x00004B05 File Offset: 0x00002D05
		public InvalidTimeZoneException()
		{
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x000328A6 File Offset: 0x00030AA6
		public InvalidTimeZoneException(string message) : base(message)
		{
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x000328AF File Offset: 0x00030AAF
		public InvalidTimeZoneException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected InvalidTimeZoneException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
