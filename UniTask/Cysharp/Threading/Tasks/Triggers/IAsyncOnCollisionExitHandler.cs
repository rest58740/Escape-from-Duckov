using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200008E RID: 142
	public interface IAsyncOnCollisionExitHandler
	{
		// Token: 0x06000450 RID: 1104
		UniTask<Collision> OnCollisionExitAsync();
	}
}
