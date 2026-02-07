using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000121 RID: 289
	internal class DisposedObserver<T> : IObserver<T>
	{
		// Token: 0x06000698 RID: 1688 RVA: 0x0000F8C5 File Offset: 0x0000DAC5
		private DisposedObserver()
		{
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0000F8CD File Offset: 0x0000DACD
		public void OnCompleted()
		{
			throw new ObjectDisposedException("");
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0000F8D9 File Offset: 0x0000DAD9
		public void OnError(Exception error)
		{
			throw new ObjectDisposedException("");
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0000F8E5 File Offset: 0x0000DAE5
		public void OnNext(T value)
		{
			throw new ObjectDisposedException("");
		}

		// Token: 0x04000158 RID: 344
		public static readonly DisposedObserver<T> Instance = new DisposedObserver<T>();
	}
}
