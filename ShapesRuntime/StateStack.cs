using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200003F RID: 63
	public readonly struct StateStack : IDisposable
	{
		// Token: 0x06000C24 RID: 3108 RVA: 0x000186D2 File Offset: 0x000168D2
		internal static void Push(DrawStyle style, Matrix4x4 mtx)
		{
			StyleStack.Push(style);
			MatrixStack.Push(mtx);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x000186E0 File Offset: 0x000168E0
		internal static void Pop()
		{
			MatrixStack.Pop();
			StyleStack.Pop();
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x000186EC File Offset: 0x000168EC
		internal StateStack(DrawStyle style, Matrix4x4 mtx)
		{
			StateStack.Push(style, mtx);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x000186F5 File Offset: 0x000168F5
		public void Dispose()
		{
			StateStack.Pop();
		}
	}
}
