using System;
using UnityEngine.Events;

namespace Duckov.MiniGames
{
	// Token: 0x0200028D RID: 653
	public class VirtualCursorTarget : MiniGameBehaviour
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x0004E7E9 File Offset: 0x0004C9E9
		public bool IsHovering
		{
			get
			{
				return VirtualCursor.IsHovering(this);
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0004E7F1 File Offset: 0x0004C9F1
		public void OnCursorEnter()
		{
			UnityEvent unityEvent = this.onEnter;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0004E803 File Offset: 0x0004CA03
		public void OnCursorExit()
		{
			UnityEvent unityEvent = this.onExit;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0004E815 File Offset: 0x0004CA15
		public void OnClick()
		{
			UnityEvent unityEvent = this.onClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04000F72 RID: 3954
		public UnityEvent onEnter;

		// Token: 0x04000F73 RID: 3955
		public UnityEvent onExit;

		// Token: 0x04000F74 RID: 3956
		public UnityEvent onClick;
	}
}
