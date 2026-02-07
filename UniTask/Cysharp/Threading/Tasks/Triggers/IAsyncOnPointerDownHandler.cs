using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000F6 RID: 246
	public interface IAsyncOnPointerDownHandler
	{
		// Token: 0x060005BC RID: 1468
		UniTask<PointerEventData> OnPointerDownAsync();
	}
}
