using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000014 RID: 20
	public readonly struct ColorStack : IDisposable
	{
		// Token: 0x0600027C RID: 636 RVA: 0x0000743F File Offset: 0x0000563F
		internal static void Push(Color prevState)
		{
			ColorStack.colors.Push(prevState);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000744C File Offset: 0x0000564C
		internal static void Pop()
		{
			try
			{
				Draw.Color = ColorStack.colors.Pop();
			}
			catch (Exception ex)
			{
				Debug.LogError("You are popping more Color stacks than you are pushing. error: " + ex.Message);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00007494 File Offset: 0x00005694
		internal ColorStack(Color mtx)
		{
			ColorStack.colors.Push(mtx);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000074A1 File Offset: 0x000056A1
		public void Dispose()
		{
			ColorStack.Pop();
		}

		// Token: 0x04000096 RID: 150
		private static readonly Stack<Color> colors = new Stack<Color>();
	}
}
