using System;
using Duckov;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class PostAudioEventOnEnter : StateMachineBehaviour
{
	// Token: 0x06000CB5 RID: 3253 RVA: 0x0003612F File Offset: 0x0003432F
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		AudioManager.Post(this.eventName, animator.gameObject);
	}

	// Token: 0x04000B10 RID: 2832
	[SerializeField]
	private string eventName;
}
