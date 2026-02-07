using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000104 RID: 260
	public interface IAsyncOnUpdateSelectedHandler
	{
		// Token: 0x060005ED RID: 1517
		UniTask<BaseEventData> OnUpdateSelectedAsync();
	}
}
