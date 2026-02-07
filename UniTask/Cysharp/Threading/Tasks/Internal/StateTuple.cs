using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000110 RID: 272
	internal static class StateTuple
	{
		// Token: 0x0600064C RID: 1612 RVA: 0x0000E8D3 File Offset: 0x0000CAD3
		public static StateTuple<T1> Create<T1>(T1 item1)
		{
			return StatePool<T1>.Create(item1);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0000E8DB File Offset: 0x0000CADB
		public static StateTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return StatePool<T1, T2>.Create(item1, item2);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0000E8E4 File Offset: 0x0000CAE4
		public static StateTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return StatePool<T1, T2, T3>.Create(item1, item2, item3);
		}
	}
}
