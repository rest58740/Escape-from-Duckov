using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000115 RID: 277
	internal class StateTuple<T1, T2, T3> : IDisposable
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x0000E9FB File Offset: 0x0000CBFB
		public void Deconstruct(out T1 item1, out T2 item2, out T3 item3)
		{
			item1 = this.Item1;
			item2 = this.Item2;
			item3 = this.Item3;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0000EA21 File Offset: 0x0000CC21
		public void Dispose()
		{
			StatePool<T1, T2, T3>.Return(this);
		}

		// Token: 0x04000125 RID: 293
		public T1 Item1;

		// Token: 0x04000126 RID: 294
		public T2 Item2;

		// Token: 0x04000127 RID: 295
		public T3 Item3;
	}
}
