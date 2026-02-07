using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000B72 RID: 2930
	[ComVisible(true)]
	[Serializable]
	public class IsolatedStorageException : Exception
	{
		// Token: 0x06006A89 RID: 27273 RVA: 0x0016C98C File Offset: 0x0016AB8C
		public IsolatedStorageException() : base(Locale.GetText("An Isolated storage operation failed."))
		{
		}

		// Token: 0x06006A8A RID: 27274 RVA: 0x000328A6 File Offset: 0x00030AA6
		public IsolatedStorageException(string message) : base(message)
		{
		}

		// Token: 0x06006A8B RID: 27275 RVA: 0x000328AF File Offset: 0x00030AAF
		public IsolatedStorageException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06006A8C RID: 27276 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected IsolatedStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
