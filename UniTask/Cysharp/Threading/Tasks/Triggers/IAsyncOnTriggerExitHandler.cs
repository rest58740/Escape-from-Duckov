using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000D4 RID: 212
	public interface IAsyncOnTriggerExitHandler
	{
		// Token: 0x06000545 RID: 1349
		UniTask<Collider> OnTriggerExitAsync();
	}
}
