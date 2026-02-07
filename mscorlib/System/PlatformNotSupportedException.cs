using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200016F RID: 367
	[Serializable]
	public class PlatformNotSupportedException : NotSupportedException
	{
		// Token: 0x06000E89 RID: 3721 RVA: 0x0003B9DA File Offset: 0x00039BDA
		public PlatformNotSupportedException() : base("Operation is not supported on this platform.")
		{
			base.HResult = -2146233031;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0003B9F2 File Offset: 0x00039BF2
		public PlatformNotSupportedException(string message) : base(message)
		{
			base.HResult = -2146233031;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0003BA06 File Offset: 0x00039C06
		public PlatformNotSupportedException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233031;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0003BA1B File Offset: 0x00039C1B
		protected PlatformNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
