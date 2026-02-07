using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200001D RID: 29
	public readonly struct GradientFillStack : IDisposable
	{
		// Token: 0x06000B26 RID: 2854 RVA: 0x00015758 File Offset: 0x00013958
		internal static void Push(bool prevOn, GradientFill prevState)
		{
			GradientFillStack.gradients.Push(new ValueTuple<bool, GradientFill>(prevOn, prevState));
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0001576C File Offset: 0x0001396C
		internal static void Pop()
		{
			try
			{
				ValueTuple<bool, GradientFill> valueTuple = GradientFillStack.gradients.Pop();
				Draw.UseGradientFill = valueTuple.Item1;
				Draw.GradientFill = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				Debug.LogError("You are popping more GradientFill stacks than you are pushing. error: " + ex.Message);
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000157C4 File Offset: 0x000139C4
		internal GradientFillStack(bool on, GradientFill gradient)
		{
			GradientFillStack.gradients.Push(new ValueTuple<bool, GradientFill>(on, gradient));
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000157D7 File Offset: 0x000139D7
		public void Dispose()
		{
			GradientFillStack.Pop();
		}

		// Token: 0x040000F3 RID: 243
		private static readonly Stack<ValueTuple<bool, GradientFill>> gradients = new Stack<ValueTuple<bool, GradientFill>>();
	}
}
