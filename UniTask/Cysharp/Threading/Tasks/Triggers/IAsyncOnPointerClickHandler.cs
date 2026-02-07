using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000F4 RID: 244
	public interface IAsyncOnPointerClickHandler
	{
		// Token: 0x060005B5 RID: 1461
		UniTask<PointerEventData> OnPointerClickAsync();
	}
}
