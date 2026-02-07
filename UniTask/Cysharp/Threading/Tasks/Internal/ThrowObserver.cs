using System;
using System.Runtime.ExceptionServices;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000120 RID: 288
	internal class ThrowObserver<T> : IObserver<T>
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
		private ThrowObserver()
		{
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public void OnCompleted()
		{
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0000F8AA File Offset: 0x0000DAAA
		public void OnError(Exception error)
		{
			ExceptionDispatchInfo.Capture(error).Throw();
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0000F8B7 File Offset: 0x0000DAB7
		public void OnNext(T value)
		{
		}

		// Token: 0x04000157 RID: 343
		public static readonly ThrowObserver<T> Instance = new ThrowObserver<T>();
	}
}
