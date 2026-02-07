using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000293 RID: 659
	[TypeForwardedFrom("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
	[Serializable]
	public class SemaphoreFullException : SystemException
	{
		// Token: 0x06001D8F RID: 7567 RVA: 0x0006E6A4 File Offset: 0x0006C8A4
		public SemaphoreFullException() : base("Adding the specified count to the semaphore would cause it to exceed its maximum count.")
		{
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x0006E6B1 File Offset: 0x0006C8B1
		public SemaphoreFullException(string message) : base(message)
		{
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x0006E6BA File Offset: 0x0006C8BA
		public SemaphoreFullException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected SemaphoreFullException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
