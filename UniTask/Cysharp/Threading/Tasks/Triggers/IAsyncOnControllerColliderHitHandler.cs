using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000096 RID: 150
	public interface IAsyncOnControllerColliderHitHandler
	{
		// Token: 0x0600046C RID: 1132
		UniTask<ControllerColliderHit> OnControllerColliderHitAsync();
	}
}
