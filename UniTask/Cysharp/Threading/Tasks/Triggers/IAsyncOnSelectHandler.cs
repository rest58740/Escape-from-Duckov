using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000100 RID: 256
	public interface IAsyncOnSelectHandler
	{
		// Token: 0x060005DF RID: 1503
		UniTask<BaseEventData> OnSelectAsync();
	}
}
