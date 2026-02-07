using System;
using System.Collections.Generic;

namespace System.Threading.Tasks
{
	// Token: 0x0200030A RID: 778
	public class TaskCompletionSource<TResult>
	{
		// Token: 0x0600216E RID: 8558 RVA: 0x000784F5 File Offset: 0x000766F5
		public TaskCompletionSource()
		{
			this._task = new Task<TResult>();
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00078508 File Offset: 0x00076708
		public TaskCompletionSource(TaskCreationOptions creationOptions) : this(null, creationOptions)
		{
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x00078512 File Offset: 0x00076712
		public TaskCompletionSource(object state) : this(state, TaskCreationOptions.None)
		{
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x0007851C File Offset: 0x0007671C
		public TaskCompletionSource(object state, TaskCreationOptions creationOptions)
		{
			this._task = new Task<TResult>(state, creationOptions);
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x00078531 File Offset: 0x00076731
		public Task<TResult> Task
		{
			get
			{
				return this._task;
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x0007853C File Offset: 0x0007673C
		private void SpinUntilCompleted()
		{
			SpinWait spinWait = default(SpinWait);
			while (!this._task.IsCompleted)
			{
				spinWait.SpinOnce();
			}
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00078567 File Offset: 0x00076767
		public bool TrySetException(Exception exception)
		{
			if (exception == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exception);
			}
			bool flag = this._task.TrySetException(exception);
			if (!flag && !this._task.IsCompleted)
			{
				this.SpinUntilCompleted();
			}
			return flag;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x00078598 File Offset: 0x00076798
		public bool TrySetException(IEnumerable<Exception> exceptions)
		{
			if (exceptions == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exceptions);
			}
			List<Exception> list = new List<Exception>();
			foreach (Exception ex in exceptions)
			{
				if (ex == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.TaskCompletionSourceT_TrySetException_NullException, ExceptionArgument.exceptions);
				}
				list.Add(ex);
			}
			if (list.Count == 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.TaskCompletionSourceT_TrySetException_NoExceptions, ExceptionArgument.exceptions);
			}
			bool flag = this._task.TrySetException(list);
			if (!flag && !this._task.IsCompleted)
			{
				this.SpinUntilCompleted();
			}
			return flag;
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x00078630 File Offset: 0x00076830
		public void SetException(Exception exception)
		{
			if (exception == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exception);
			}
			if (!this.TrySetException(exception))
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted);
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x0007864C File Offset: 0x0007684C
		public void SetException(IEnumerable<Exception> exceptions)
		{
			if (!this.TrySetException(exceptions))
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted);
			}
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x0007865E File Offset: 0x0007685E
		public bool TrySetResult(TResult result)
		{
			bool flag = this._task.TrySetResult(result);
			if (!flag)
			{
				this.SpinUntilCompleted();
			}
			return flag;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x00078675 File Offset: 0x00076875
		public void SetResult(TResult result)
		{
			if (!this.TrySetResult(result))
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted);
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x00078688 File Offset: 0x00076888
		public bool TrySetCanceled()
		{
			return this.TrySetCanceled(default(CancellationToken));
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000786A4 File Offset: 0x000768A4
		public bool TrySetCanceled(CancellationToken cancellationToken)
		{
			bool flag = this._task.TrySetCanceled(cancellationToken);
			if (!flag && !this._task.IsCompleted)
			{
				this.SpinUntilCompleted();
			}
			return flag;
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000786C8 File Offset: 0x000768C8
		public void SetCanceled()
		{
			if (!this.TrySetCanceled())
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted);
			}
		}

		// Token: 0x04001BC8 RID: 7112
		private readonly Task<TResult> _task;
	}
}
