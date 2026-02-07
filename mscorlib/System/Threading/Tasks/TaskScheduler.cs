using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	// Token: 0x02000379 RID: 889
	[DebuggerDisplay("Id={Id}")]
	[DebuggerTypeProxy(typeof(TaskScheduler.SystemThreadingTasks_TaskSchedulerDebugView))]
	public abstract class TaskScheduler
	{
		// Token: 0x06002510 RID: 9488
		protected internal abstract void QueueTask(Task task);

		// Token: 0x06002511 RID: 9489
		protected abstract bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued);

		// Token: 0x06002512 RID: 9490
		protected abstract IEnumerable<Task> GetScheduledTasks();

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06002513 RID: 9491 RVA: 0x0008408E File Offset: 0x0008228E
		public virtual int MaximumConcurrencyLevel
		{
			get
			{
				return int.MaxValue;
			}
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x00084098 File Offset: 0x00082298
		internal bool TryRunInline(Task task, bool taskWasPreviouslyQueued)
		{
			TaskScheduler executingTaskScheduler = task.ExecutingTaskScheduler;
			if (executingTaskScheduler != this && executingTaskScheduler != null)
			{
				return executingTaskScheduler.TryRunInline(task, taskWasPreviouslyQueued);
			}
			StackGuard currentStackGuard;
			if (executingTaskScheduler == null || task.m_action == null || task.IsDelegateInvoked || task.IsCanceled || !(currentStackGuard = Task.CurrentStackGuard).TryBeginInliningScope())
			{
				return false;
			}
			bool flag = false;
			try
			{
				flag = this.TryExecuteTaskInline(task, taskWasPreviouslyQueued);
			}
			finally
			{
				currentStackGuard.EndInliningScope();
			}
			if (flag && !task.IsDelegateInvoked && !task.IsCanceled)
			{
				throw new InvalidOperationException("The TryExecuteTaskInline call to the underlying scheduler succeeded, but the task body was not invoked.");
			}
			return flag;
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected internal virtual bool TryDequeue(Task task)
		{
			return false;
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal virtual void NotifyWorkItemProgress()
		{
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x000040F7 File Offset: 0x000022F7
		internal virtual bool RequiresAtomicStartTransition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x0008412C File Offset: 0x0008232C
		private void AddToActiveTaskSchedulers()
		{
			ConditionalWeakTable<TaskScheduler, object> conditionalWeakTable = TaskScheduler.s_activeTaskSchedulers;
			if (conditionalWeakTable == null)
			{
				Interlocked.CompareExchange<ConditionalWeakTable<TaskScheduler, object>>(ref TaskScheduler.s_activeTaskSchedulers, new ConditionalWeakTable<TaskScheduler, object>(), null);
				conditionalWeakTable = TaskScheduler.s_activeTaskSchedulers;
			}
			conditionalWeakTable.Add(this, null);
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x00084161 File Offset: 0x00082361
		public static TaskScheduler Default
		{
			get
			{
				return TaskScheduler.s_defaultTaskScheduler;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x00084168 File Offset: 0x00082368
		public static TaskScheduler Current
		{
			get
			{
				return TaskScheduler.InternalCurrent ?? TaskScheduler.Default;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x00084178 File Offset: 0x00082378
		internal static TaskScheduler InternalCurrent
		{
			get
			{
				Task internalCurrent = Task.InternalCurrent;
				if (internalCurrent == null || (internalCurrent.CreationOptions & TaskCreationOptions.HideScheduler) != TaskCreationOptions.None)
				{
					return null;
				}
				return internalCurrent.ExecutingTaskScheduler;
			}
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000841A1 File Offset: 0x000823A1
		public static TaskScheduler FromCurrentSynchronizationContext()
		{
			return new SynchronizationContextTaskScheduler();
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x000841A8 File Offset: 0x000823A8
		public int Id
		{
			get
			{
				if (this.m_taskSchedulerId == 0)
				{
					int num;
					do
					{
						num = Interlocked.Increment(ref TaskScheduler.s_taskSchedulerIdCounter);
					}
					while (num == 0);
					Interlocked.CompareExchange(ref this.m_taskSchedulerId, num, 0);
				}
				return this.m_taskSchedulerId;
			}
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000841E5 File Offset: 0x000823E5
		protected bool TryExecuteTask(Task task)
		{
			if (task.ExecutingTaskScheduler != this)
			{
				throw new InvalidOperationException("ExecuteTask may not be called for a task which was previously queued to a different TaskScheduler.");
			}
			return task.ExecuteEntry(true);
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06002520 RID: 9504 RVA: 0x00084204 File Offset: 0x00082404
		// (remove) Token: 0x06002521 RID: 9505 RVA: 0x00084254 File Offset: 0x00082454
		public static event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException
		{
			add
			{
				if (value != null)
				{
					using (LockHolder.Hold(TaskScheduler._unobservedTaskExceptionLockObject))
					{
						TaskScheduler._unobservedTaskException = (EventHandler<UnobservedTaskExceptionEventArgs>)Delegate.Combine(TaskScheduler._unobservedTaskException, value);
					}
				}
			}
			remove
			{
				using (LockHolder.Hold(TaskScheduler._unobservedTaskExceptionLockObject))
				{
					TaskScheduler._unobservedTaskException = (EventHandler<UnobservedTaskExceptionEventArgs>)Delegate.Remove(TaskScheduler._unobservedTaskException, value);
				}
			}
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x000842A4 File Offset: 0x000824A4
		internal static void PublishUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs ueea)
		{
			using (LockHolder.Hold(TaskScheduler._unobservedTaskExceptionLockObject))
			{
				EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskException = TaskScheduler._unobservedTaskException;
				if (unobservedTaskException != null)
				{
					unobservedTaskException(sender, ueea);
				}
			}
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000842F0 File Offset: 0x000824F0
		internal Task[] GetScheduledTasksForDebugger()
		{
			IEnumerable<Task> scheduledTasks = this.GetScheduledTasks();
			if (scheduledTasks == null)
			{
				return null;
			}
			Task[] array = scheduledTasks as Task[];
			if (array == null)
			{
				array = new LowLevelList<Task>(scheduledTasks).ToArray();
			}
			Task[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				int id = array2[i].Id;
			}
			return array;
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x0008433C File Offset: 0x0008253C
		internal static TaskScheduler[] GetTaskSchedulersForDebugger()
		{
			if (TaskScheduler.s_activeTaskSchedulers == null)
			{
				return new TaskScheduler[]
				{
					TaskScheduler.s_defaultTaskScheduler
				};
			}
			LowLevelList<TaskScheduler> lowLevelList = new LowLevelList<TaskScheduler>();
			foreach (KeyValuePair<TaskScheduler, object> keyValuePair in ((IEnumerable<KeyValuePair<TaskScheduler, object>>)TaskScheduler.s_activeTaskSchedulers))
			{
				lowLevelList.Add(keyValuePair.Key);
			}
			if (!lowLevelList.Contains(TaskScheduler.s_defaultTaskScheduler))
			{
				lowLevelList.Add(TaskScheduler.s_defaultTaskScheduler);
			}
			TaskScheduler[] array = lowLevelList.ToArray();
			TaskScheduler[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				int id = array2[i].Id;
			}
			return array;
		}

		// Token: 0x04001D4D RID: 7501
		private static ConditionalWeakTable<TaskScheduler, object> s_activeTaskSchedulers;

		// Token: 0x04001D4E RID: 7502
		private static readonly TaskScheduler s_defaultTaskScheduler = new ThreadPoolTaskScheduler();

		// Token: 0x04001D4F RID: 7503
		internal static int s_taskSchedulerIdCounter;

		// Token: 0x04001D50 RID: 7504
		private volatile int m_taskSchedulerId;

		// Token: 0x04001D51 RID: 7505
		private static EventHandler<UnobservedTaskExceptionEventArgs> _unobservedTaskException;

		// Token: 0x04001D52 RID: 7506
		private static readonly Lock _unobservedTaskExceptionLockObject = new Lock();

		// Token: 0x0200037A RID: 890
		internal sealed class SystemThreadingTasks_TaskSchedulerDebugView
		{
			// Token: 0x06002526 RID: 9510 RVA: 0x00084402 File Offset: 0x00082602
			public SystemThreadingTasks_TaskSchedulerDebugView(TaskScheduler scheduler)
			{
				this.m_taskScheduler = scheduler;
			}

			// Token: 0x1700047E RID: 1150
			// (get) Token: 0x06002527 RID: 9511 RVA: 0x00084411 File Offset: 0x00082611
			public int Id
			{
				get
				{
					return this.m_taskScheduler.Id;
				}
			}

			// Token: 0x1700047F RID: 1151
			// (get) Token: 0x06002528 RID: 9512 RVA: 0x0008441E File Offset: 0x0008261E
			public IEnumerable<Task> ScheduledTasks
			{
				get
				{
					return this.m_taskScheduler.GetScheduledTasks();
				}
			}

			// Token: 0x04001D53 RID: 7507
			private readonly TaskScheduler m_taskScheduler;
		}
	}
}
