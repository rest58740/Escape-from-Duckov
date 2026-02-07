using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000D2 RID: 210
	public interface IAsyncOnTriggerEnter2DHandler
	{
		// Token: 0x0600053E RID: 1342
		UniTask<Collider2D> OnTriggerEnter2DAsync();
	}
}
