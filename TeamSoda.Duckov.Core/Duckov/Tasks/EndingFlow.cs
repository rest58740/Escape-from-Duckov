using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x0200038A RID: 906
	[Obsolete]
	public class EndingFlow : MonoBehaviour
	{
		// Token: 0x06001F80 RID: 8064 RVA: 0x0006F801 File Offset: 0x0006DA01
		private void Start()
		{
			this.Task().Forget();
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0006F810 File Offset: 0x0006DA10
		private UniTask Task()
		{
			EndingFlow.<Task>d__4 <Task>d__;
			<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Task>d__.<>4__this = this;
			<Task>d__.<>1__state = -1;
			<Task>d__.<>t__builder.Start<EndingFlow.<Task>d__4>(ref <Task>d__);
			return <Task>d__.<>t__builder.Task;
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0006F854 File Offset: 0x0006DA54
		private UniTask WaitForTaskBehaviour(MonoBehaviour mono)
		{
			EndingFlow.<WaitForTaskBehaviour>d__5 <WaitForTaskBehaviour>d__;
			<WaitForTaskBehaviour>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForTaskBehaviour>d__.mono = mono;
			<WaitForTaskBehaviour>d__.<>1__state = -1;
			<WaitForTaskBehaviour>d__.<>t__builder.Start<EndingFlow.<WaitForTaskBehaviour>d__5>(ref <WaitForTaskBehaviour>d__);
			return <WaitForTaskBehaviour>d__.<>t__builder.Task;
		}

		// Token: 0x04001585 RID: 5509
		[SerializeField]
		private List<MonoBehaviour> taskBehaviours = new List<MonoBehaviour>();

		// Token: 0x04001586 RID: 5510
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x04001587 RID: 5511
		[SerializeField]
		private UnityEvent onEnd;
	}
}
