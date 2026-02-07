using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000EA RID: 234
	public interface IAsyncOnDragHandler
	{
		// Token: 0x06000592 RID: 1426
		UniTask<PointerEventData> OnDragAsync();
	}
}
