using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000015 RID: 21
	public class ChannelClosedException : InvalidOperationException
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00002B70 File Offset: 0x00000D70
		public ChannelClosedException() : base("Channel is already closed.")
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002B7D File Offset: 0x00000D7D
		public ChannelClosedException(string message) : base(message)
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002B86 File Offset: 0x00000D86
		public ChannelClosedException(Exception innerException) : base("Channel is already closed", innerException)
		{
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002B94 File Offset: 0x00000D94
		public ChannelClosedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
