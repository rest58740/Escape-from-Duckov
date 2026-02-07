using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000EC RID: 236
	public interface IAsyncOnDropHandler
	{
		// Token: 0x06000599 RID: 1433
		UniTask<PointerEventData> OnDropAsync();
	}
}
