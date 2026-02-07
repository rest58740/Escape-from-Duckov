using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000B4 RID: 180
	public interface IAsyncOnParticleCollisionHandler
	{
		// Token: 0x060004D5 RID: 1237
		UniTask<GameObject> OnParticleCollisionAsync();
	}
}
