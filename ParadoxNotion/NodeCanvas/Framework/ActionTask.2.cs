using System;
using System.Collections;
using ParadoxNotion.Services;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000021 RID: 33
	public abstract class ActionTask : Task
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000060F4 File Offset: 0x000042F4
		public float elapsedTime
		{
			get
			{
				if (!this.isRunning)
				{
					return 0f;
				}
				return base.ownerSystem.elapsedTime - this.timeStarted;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00006116 File Offset: 0x00004316
		public bool isRunning
		{
			get
			{
				return this.status == Status.Running;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00006121 File Offset: 0x00004321
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00006129 File Offset: 0x00004329
		public bool isPaused { get; private set; }

		// Token: 0x060001C2 RID: 450 RVA: 0x00006132 File Offset: 0x00004332
		public void ExecuteIndependent(Component agent, IBlackboard blackboard, Action<Status> callback)
		{
			if (!this.isRunning)
			{
				MonoManager.current.StartCoroutine(this.IndependentActionUpdater(agent, blackboard, callback));
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006150 File Offset: 0x00004350
		private IEnumerator IndependentActionUpdater(Component agent, IBlackboard blackboard, Action<Status> callback)
		{
			while (this.Execute(agent, blackboard) == Status.Running)
			{
				yield return null;
			}
			if (callback != null)
			{
				callback.Invoke(this.status);
			}
			yield break;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00006174 File Offset: 0x00004374
		public Status Execute(Component agent, IBlackboard blackboard)
		{
			if (!base.isUserEnabled)
			{
				return Status.Optional;
			}
			if (this.isPaused)
			{
				this.OnResume();
			}
			this.isPaused = false;
			if (this.status == Status.Running)
			{
				this.OnUpdate();
				this.latch = false;
				return this.status;
			}
			if (this.latch)
			{
				this.latch = false;
				return this.status;
			}
			if (!base.Set(agent, blackboard))
			{
				this.latch = false;
				return Status.Failure;
			}
			this.timeStarted = base.ownerSystem.elapsedTime;
			this.status = Status.Running;
			this.OnExecute();
			if (this.status == Status.Running)
			{
				this.OnUpdate();
			}
			this.latch = false;
			return this.status;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00006220 File Offset: 0x00004420
		public void EndAction()
		{
			this.EndAction(true);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006229 File Offset: 0x00004429
		public void EndAction(bool success)
		{
			this.EndAction(new bool?(success));
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00006238 File Offset: 0x00004438
		public void EndAction(bool? success)
		{
			if (this.status != Status.Running)
			{
				if (success == null)
				{
					this.latch = false;
				}
				return;
			}
			this.latch = (success != null);
			this.isPaused = false;
			Status status;
			if (success != null)
			{
				bool? flag = success;
				bool flag2 = true;
				status = ((flag.GetValueOrDefault() == flag2 & flag != null) ? Status.Success : Status.Failure);
			}
			else
			{
				status = Status.Resting;
			}
			this.status = status;
			this.OnStop(success == null);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000062B7 File Offset: 0x000044B7
		public void Pause()
		{
			if (this.status != Status.Running)
			{
				return;
			}
			this.isPaused = true;
			this.OnPause();
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000062D0 File Offset: 0x000044D0
		protected virtual void OnExecute()
		{
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000062D2 File Offset: 0x000044D2
		protected virtual void OnUpdate()
		{
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000062D4 File Offset: 0x000044D4
		protected virtual void OnStop(bool interrupted)
		{
			this.OnStop();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000062DC File Offset: 0x000044DC
		protected virtual void OnStop()
		{
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000062DE File Offset: 0x000044DE
		protected virtual void OnPause()
		{
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000062E0 File Offset: 0x000044E0
		protected virtual void OnResume()
		{
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000062E2 File Offset: 0x000044E2
		[Obsolete("Use 'Execute'")]
		public Status ExecuteAction(Component agent, IBlackboard blackboard)
		{
			return this.Execute(agent, blackboard);
		}

		// Token: 0x04000067 RID: 103
		private Status status = Status.Resting;

		// Token: 0x04000068 RID: 104
		private float timeStarted;

		// Token: 0x04000069 RID: 105
		private bool latch;
	}
}
