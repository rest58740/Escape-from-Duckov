using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000DA RID: 218
	public interface IAsyncOnTriggerStay2DHandler
	{
		// Token: 0x0600055A RID: 1370
		UniTask<Collider2D> OnTriggerStay2DAsync();
	}
}
