using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000040 RID: 64
	public readonly struct StyleStack : IDisposable
	{
		// Token: 0x06000C28 RID: 3112 RVA: 0x000186FC File Offset: 0x000168FC
		internal static void Push(DrawStyle prevState)
		{
			StyleStack.styles.Push(prevState);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0001870C File Offset: 0x0001690C
		internal static void Pop()
		{
			try
			{
				Draw.style = StyleStack.styles.Pop();
			}
			catch (Exception ex)
			{
				Debug.LogError("You are popping more DrawStyle stacks than you are pushing. error: " + ex.Message);
			}
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00018754 File Offset: 0x00016954
		internal StyleStack(DrawStyle style)
		{
			StyleStack.styles.Push(style);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00018761 File Offset: 0x00016961
		public void Dispose()
		{
			StyleStack.Pop();
		}

		// Token: 0x040001A9 RID: 425
		private static readonly Stack<DrawStyle> styles = new Stack<DrawStyle>();
	}
}
