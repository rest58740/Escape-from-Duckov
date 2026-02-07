using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000024 RID: 36
	public readonly struct MatrixStack : IDisposable
	{
		// Token: 0x06000B51 RID: 2897 RVA: 0x00016296 File Offset: 0x00014496
		internal static void Push(Matrix4x4 prevState)
		{
			MatrixStack.matrices.Push(prevState);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x000162A4 File Offset: 0x000144A4
		internal static void Pop()
		{
			try
			{
				Draw.Matrix = MatrixStack.matrices.Pop();
			}
			catch (Exception ex)
			{
				Debug.LogError("You are popping more Matrix4x4 stacks than you are pushing. error: " + ex.Message);
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x000162EC File Offset: 0x000144EC
		internal MatrixStack(Matrix4x4 mtx)
		{
			MatrixStack.matrices.Push(mtx);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x000162F9 File Offset: 0x000144F9
		public void Dispose()
		{
			MatrixStack.Pop();
		}

		// Token: 0x04000108 RID: 264
		private static readonly Stack<Matrix4x4> matrices = new Stack<Matrix4x4>();
	}
}
