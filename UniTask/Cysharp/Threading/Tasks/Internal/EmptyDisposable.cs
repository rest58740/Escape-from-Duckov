using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200011B RID: 283
	internal class EmptyDisposable : IDisposable
	{
		// Token: 0x06000676 RID: 1654 RVA: 0x0000F2C5 File Offset: 0x0000D4C5
		private EmptyDisposable()
		{
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0000F2CD File Offset: 0x0000D4CD
		public void Dispose()
		{
		}

		// Token: 0x0400014A RID: 330
		public static EmptyDisposable Instance = new EmptyDisposable();
	}
}
