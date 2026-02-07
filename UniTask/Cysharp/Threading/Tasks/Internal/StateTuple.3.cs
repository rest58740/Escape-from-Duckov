using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000113 RID: 275
	internal class StateTuple<T1, T2> : IDisposable
	{
		// Token: 0x06000655 RID: 1621 RVA: 0x0000E961 File Offset: 0x0000CB61
		public void Deconstruct(out T1 item1, out T2 item2)
		{
			item1 = this.Item1;
			item2 = this.Item2;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0000E97B File Offset: 0x0000CB7B
		public void Dispose()
		{
			StatePool<T1, T2>.Return(this);
		}

		// Token: 0x04000122 RID: 290
		public T1 Item1;

		// Token: 0x04000123 RID: 291
		public T2 Item2;
	}
}
