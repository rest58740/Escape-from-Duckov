using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000094 RID: 148
	public interface IAsyncOnCollisionStay2DHandler
	{
		// Token: 0x06000465 RID: 1125
		UniTask<Collision2D> OnCollisionStay2DAsync();
	}
}
