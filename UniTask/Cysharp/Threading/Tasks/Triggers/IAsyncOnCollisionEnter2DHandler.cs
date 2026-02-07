using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200008C RID: 140
	public interface IAsyncOnCollisionEnter2DHandler
	{
		// Token: 0x06000449 RID: 1097
		UniTask<Collision2D> OnCollisionEnter2DAsync();
	}
}
