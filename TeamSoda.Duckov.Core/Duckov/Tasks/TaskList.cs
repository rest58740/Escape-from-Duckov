using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x0200038B RID: 907
	public class TaskList : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001F84 RID: 8068 RVA: 0x0006F8AA File Offset: 0x0006DAAA
		private void Start()
		{
			if (this.beginOnStart)
			{
				this.Begin();
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0006F8BC File Offset: 0x0006DABC
		private UniTask MainTask()
		{
			TaskList.<MainTask>d__10 <MainTask>d__;
			<MainTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<MainTask>d__.<>4__this = this;
			<MainTask>d__.<>1__state = -1;
			<MainTask>d__.<>t__builder.Start<TaskList.<MainTask>d__10>(ref <MainTask>d__);
			return <MainTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x0006F8FF File Offset: 0x0006DAFF
		public void Begin()
		{
			if (this.running)
			{
				return;
			}
			this.skip = false;
			this.running = true;
			this.complete = false;
			UnityEvent unityEvent = this.onBegin;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.MainTask().Forget();
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0006F93B File Offset: 0x0006DB3B
		public bool IsComplete()
		{
			return this.complete;
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x0006F943 File Offset: 0x0006DB43
		public bool IsPending()
		{
			return this.running;
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x0006F94B File Offset: 0x0006DB4B
		public void Skip()
		{
			this.skip = true;
		}

		// Token: 0x04001588 RID: 5512
		[SerializeField]
		private bool beginOnStart;

		// Token: 0x04001589 RID: 5513
		[SerializeField]
		private List<MonoBehaviour> tasks;

		// Token: 0x0400158A RID: 5514
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x0400158B RID: 5515
		[SerializeField]
		private UnityEvent onComplete;

		// Token: 0x0400158C RID: 5516
		[SerializeField]
		private bool listenToSkipSignal;

		// Token: 0x0400158D RID: 5517
		private bool running;

		// Token: 0x0400158E RID: 5518
		private bool complete;

		// Token: 0x0400158F RID: 5519
		private int currentTaskIndex;

		// Token: 0x04001590 RID: 5520
		private ITaskBehaviour currentTask;

		// Token: 0x04001591 RID: 5521
		private bool skip;
	}
}
