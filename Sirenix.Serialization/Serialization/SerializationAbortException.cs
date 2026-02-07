using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000074 RID: 116
	public class SerializationAbortException : Exception
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x0001A526 File Offset: 0x00018726
		public SerializationAbortException(string message) : base(message)
		{
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0001A52F File Offset: 0x0001872F
		public SerializationAbortException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
