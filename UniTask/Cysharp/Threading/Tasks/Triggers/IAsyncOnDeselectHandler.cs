using System;
using UnityEngine.EventSystems;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000E8 RID: 232
	public interface IAsyncOnDeselectHandler
	{
		// Token: 0x0600058B RID: 1419
		UniTask<BaseEventData> OnDeselectAsync();
	}
}
