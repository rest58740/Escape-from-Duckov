using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000194 RID: 404
	[TypeForwardedFrom("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
	[Serializable]
	public class TimeZoneNotFoundException : Exception
	{
		// Token: 0x0600102B RID: 4139 RVA: 0x00004B05 File Offset: 0x00002D05
		public TimeZoneNotFoundException()
		{
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000328A6 File Offset: 0x00030AA6
		public TimeZoneNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x000328AF File Offset: 0x00030AAF
		public TimeZoneNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected TimeZoneNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
