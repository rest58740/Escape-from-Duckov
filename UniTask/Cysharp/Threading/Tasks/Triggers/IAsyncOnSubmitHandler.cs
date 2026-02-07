using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000102 RID: 258
	public interface IAsyncOnSubmitHandler
	{
		// Token: 0x060005E6 RID: 1510
		UniTask<BaseEventData> OnSubmitAsync();
	}
}
