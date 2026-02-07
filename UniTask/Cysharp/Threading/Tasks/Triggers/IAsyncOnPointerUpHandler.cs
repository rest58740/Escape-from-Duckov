using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000FC RID: 252
	public interface IAsyncOnPointerUpHandler
	{
		// Token: 0x060005D1 RID: 1489
		UniTask<PointerEventData> OnPointerUpAsync();
	}
}
