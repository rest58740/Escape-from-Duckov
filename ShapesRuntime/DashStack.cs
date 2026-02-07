using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000015 RID: 21
	public readonly struct DashStack : IDisposable
	{
		// Token: 0x06000281 RID: 641 RVA: 0x000074B4 File Offset: 0x000056B4
		internal static void Push(bool prevOn, DashStyle prevState)
		{
			DashStack.dashes.Push(new ValueTuple<bool, DashStyle>(prevOn, prevState));
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000074C8 File Offset: 0x000056C8
		internal static void Pop()
		{
			try
			{
				ValueTuple<bool, DashStyle> valueTuple = DashStack.dashes.Pop();
				Draw.UseDashes = valueTuple.Item1;
				Draw.DashStyle = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				Debug.LogError("You are popping more DashStyle stacks than you are pushing. error: " + ex.Message);
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00007520 File Offset: 0x00005720
		internal DashStack(bool on, DashStyle dash)
		{
			DashStack.dashes.Push(new ValueTuple<bool, DashStyle>(on, dash));
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00007533 File Offset: 0x00005733
		public void Dispose()
		{
			DashStack.Pop();
		}

		// Token: 0x04000097 RID: 151
		private static readonly Stack<ValueTuple<bool, DashStyle>> dashes = new Stack<ValueTuple<bool, DashStyle>>();
	}
}
