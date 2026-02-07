using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000392 RID: 914
	public class NotificationText : MonoBehaviour
	{
		// Token: 0x06001FC5 RID: 8133 RVA: 0x000701F1 File Offset: 0x0006E3F1
		public static void Push(string text)
		{
			if (NotificationText.pendingTexts.Count > 0 && NotificationText.pendingTexts.Peek() == text)
			{
				return;
			}
			NotificationText.pendingTexts.Enqueue(text);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0007021E File Offset: 0x0006E41E
		private static string Pop()
		{
			return NotificationText.pendingTexts.Dequeue();
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x0007022A File Offset: 0x0006E42A
		private int PendingCount
		{
			get
			{
				return NotificationText.pendingTexts.Count;
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x00070236 File Offset: 0x0006E436
		private void Update()
		{
			if (!this.showing && this.PendingCount > 0)
			{
				this.ShowNext().Forget();
			}
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x00070254 File Offset: 0x0006E454
		private UniTask ShowNext()
		{
			NotificationText.<ShowNext>d__11 <ShowNext>d__;
			<ShowNext>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowNext>d__.<>4__this = this;
			<ShowNext>d__.<>1__state = -1;
			<ShowNext>d__.<>t__builder.Start<NotificationText.<ShowNext>d__11>(ref <ShowNext>d__);
			return <ShowNext>d__.<>t__builder.Task;
		}

		// Token: 0x040015B9 RID: 5561
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015BA RID: 5562
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040015BB RID: 5563
		[SerializeField]
		private float duration = 1.2f;

		// Token: 0x040015BC RID: 5564
		[SerializeField]
		private float durationIfPending = 0.65f;

		// Token: 0x040015BD RID: 5565
		private static Queue<string> pendingTexts = new Queue<string>();

		// Token: 0x040015BE RID: 5566
		private bool showing;
	}
}
