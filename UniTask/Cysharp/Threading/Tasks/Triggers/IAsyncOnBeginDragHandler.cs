using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000E4 RID: 228
	public interface IAsyncOnBeginDragHandler
	{
		// Token: 0x0600057D RID: 1405
		UniTask<PointerEventData> OnBeginDragAsync();
	}
}
