using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000D8 RID: 216
	public interface IAsyncOnTriggerStayHandler
	{
		// Token: 0x06000553 RID: 1363
		UniTask<Collider> OnTriggerStayAsync();
	}
}
