using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000111 RID: 273
	internal class StateTuple<T1> : IDisposable
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x0000E8EE File Offset: 0x0000CAEE
		public void Deconstruct(out T1 item1)
		{
			item1 = this.Item1;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0000E8FC File Offset: 0x0000CAFC
		public void Dispose()
		{
			StatePool<T1>.Return(this);
		}

		// Token: 0x04000120 RID: 288
		public T1 Item1;
	}
}
