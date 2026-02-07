using System;
using UnityEngine.Events;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000064 RID: 100
	internal class TextSelectionEventConverter : UnityEvent<ValueTuple<string, int, int>>, IDisposable
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x00009F8F File Offset: 0x0000818F
		public TextSelectionEventConverter(UnityEvent<string, int, int> unityEvent)
		{
			this.innerEvent = unityEvent;
			this.invokeDelegate = new UnityAction<string, int, int>(this.InvokeCore);
			this.innerEvent.AddListener(this.invokeDelegate);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00009FC1 File Offset: 0x000081C1
		private void InvokeCore(string item1, int item2, int item3)
		{
			base.Invoke(new ValueTuple<string, int, int>(item1, item2, item3));
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00009FD1 File Offset: 0x000081D1
		public void Dispose()
		{
			this.innerEvent.RemoveListener(this.invokeDelegate);
		}

		// Token: 0x040000CF RID: 207
		private readonly UnityEvent<string, int, int> innerEvent;

		// Token: 0x040000D0 RID: 208
		private readonly UnityAction<string, int, int> invokeDelegate;
	}
}
