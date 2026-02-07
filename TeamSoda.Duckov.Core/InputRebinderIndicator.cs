using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001C5 RID: 453
public class InputRebinderIndicator : MonoBehaviour
{
	// Token: 0x06000DAF RID: 3503 RVA: 0x00039698 File Offset: 0x00037898
	private void Awake()
	{
		InputRebinder.OnRebindBegin = (Action<InputAction>)Delegate.Combine(InputRebinder.OnRebindBegin, new Action<InputAction>(this.OnRebindBegin));
		InputRebinder.OnRebindComplete = (Action<InputAction>)Delegate.Combine(InputRebinder.OnRebindComplete, new Action<InputAction>(this.OnRebindComplete));
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x000396F0 File Offset: 0x000378F0
	private void OnRebindComplete(InputAction action)
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x000396FD File Offset: 0x000378FD
	private void OnRebindBegin(InputAction action)
	{
		this.fadeGroup.Show();
	}

	// Token: 0x04000BC9 RID: 3017
	[SerializeField]
	private FadeGroup fadeGroup;
}
