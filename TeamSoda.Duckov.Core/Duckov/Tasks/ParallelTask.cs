using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x0200038C RID: 908
	public class ParallelTask : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001F8B RID: 8075 RVA: 0x0006F95C File Offset: 0x0006DB5C
		private void Start()
		{
			if (this.beginOnStart)
			{
				this.Begin();
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0006F96C File Offset: 0x0006DB6C
		private UniTask MainTask()
		{
			ParallelTask.<MainTask>d__7 <MainTask>d__;
			<MainTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<MainTask>d__.<>4__this = this;
			<MainTask>d__.<>1__state = -1;
			<MainTask>d__.<>t__builder.Start<ParallelTask.<MainTask>d__7>(ref <MainTask>d__);
			return <MainTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0006F9AF File Offset: 0x0006DBAF
		public void Begin()
		{
			if (this.running)
			{
				return;
			}
			this.running = true;
			this.complete = false;
			UnityEvent unityEvent = this.onBegin;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.MainTask().Forget();
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0006F9E4 File Offset: 0x0006DBE4
		public bool IsComplete()
		{
			return this.complete;
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0006F9EC File Offset: 0x0006DBEC
		public bool IsPending()
		{
			return this.running;
		}

		// Token: 0x04001592 RID: 5522
		[SerializeField]
		private bool beginOnStart;

		// Token: 0x04001593 RID: 5523
		[SerializeField]
		private List<MonoBehaviour> tasks;

		// Token: 0x04001594 RID: 5524
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x04001595 RID: 5525
		[SerializeField]
		private UnityEvent onComplete;

		// Token: 0x04001596 RID: 5526
		private bool running;

		// Token: 0x04001597 RID: 5527
		private bool complete;
	}
}
