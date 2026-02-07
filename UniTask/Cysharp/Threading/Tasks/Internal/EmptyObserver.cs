using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200011F RID: 287
	internal class EmptyObserver<T> : IObserver<T>
	{
		// Token: 0x0600068E RID: 1678 RVA: 0x0000F886 File Offset: 0x0000DA86
		private EmptyObserver()
		{
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0000F88E File Offset: 0x0000DA8E
		public void OnCompleted()
		{
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0000F890 File Offset: 0x0000DA90
		public void OnError(Exception error)
		{
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0000F892 File Offset: 0x0000DA92
		public void OnNext(T value)
		{
		}

		// Token: 0x04000156 RID: 342
		public static readonly EmptyObserver<T> Instance = new EmptyObserver<T>();
	}
}
