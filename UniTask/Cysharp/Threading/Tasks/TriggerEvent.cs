using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000036 RID: 54
	public struct TriggerEvent<T>
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00004033 File Offset: 0x00002233
		private void LogError(Exception ex)
		{
			Debug.LogException(ex);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000403C File Offset: 0x0000223C
		public void SetResult(T value)
		{
			if (this.iteratingNode != null)
			{
				throw new InvalidOperationException("Can not trigger itself in iterating.");
			}
			for (ITriggerHandler<T> triggerHandler = this.head; triggerHandler != null; triggerHandler = ((triggerHandler == this.iteratingNode) ? triggerHandler.Next : this.iteratingNode))
			{
				this.iteratingNode = triggerHandler;
				try
				{
					triggerHandler.OnNext(value);
				}
				catch (Exception ex)
				{
					this.LogError(ex);
					this.Remove(triggerHandler);
				}
			}
			this.iteratingNode = null;
			if (this.iteratingHead != null)
			{
				this.Add(this.iteratingHead);
				this.iteratingHead = null;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000040D4 File Offset: 0x000022D4
		public void SetCanceled(CancellationToken cancellationToken)
		{
			if (this.iteratingNode != null)
			{
				throw new InvalidOperationException("Can not trigger itself in iterating.");
			}
			ITriggerHandler<T> triggerHandler2;
			for (ITriggerHandler<T> triggerHandler = this.head; triggerHandler != null; triggerHandler = triggerHandler2)
			{
				this.iteratingNode = triggerHandler;
				try
				{
					triggerHandler.OnCanceled(cancellationToken);
				}
				catch (Exception ex)
				{
					this.LogError(ex);
				}
				triggerHandler2 = ((triggerHandler == this.iteratingNode) ? triggerHandler.Next : this.iteratingNode);
				this.iteratingNode = null;
				this.Remove(triggerHandler);
			}
			this.iteratingNode = null;
			if (this.iteratingHead != null)
			{
				this.Add(this.iteratingHead);
				this.iteratingHead = null;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004174 File Offset: 0x00002374
		public void SetCompleted()
		{
			if (this.iteratingNode != null)
			{
				throw new InvalidOperationException("Can not trigger itself in iterating.");
			}
			ITriggerHandler<T> triggerHandler2;
			for (ITriggerHandler<T> triggerHandler = this.head; triggerHandler != null; triggerHandler = triggerHandler2)
			{
				this.iteratingNode = triggerHandler;
				try
				{
					triggerHandler.OnCompleted();
				}
				catch (Exception ex)
				{
					this.LogError(ex);
				}
				triggerHandler2 = ((triggerHandler == this.iteratingNode) ? triggerHandler.Next : this.iteratingNode);
				this.iteratingNode = null;
				this.Remove(triggerHandler);
			}
			this.iteratingNode = null;
			if (this.iteratingHead != null)
			{
				this.Add(this.iteratingHead);
				this.iteratingHead = null;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004214 File Offset: 0x00002414
		public void SetError(Exception exception)
		{
			if (this.iteratingNode != null)
			{
				throw new InvalidOperationException("Can not trigger itself in iterating.");
			}
			ITriggerHandler<T> triggerHandler2;
			for (ITriggerHandler<T> triggerHandler = this.head; triggerHandler != null; triggerHandler = triggerHandler2)
			{
				this.iteratingNode = triggerHandler;
				try
				{
					triggerHandler.OnError(exception);
				}
				catch (Exception ex)
				{
					this.LogError(ex);
				}
				triggerHandler2 = ((triggerHandler == this.iteratingNode) ? triggerHandler.Next : this.iteratingNode);
				this.iteratingNode = null;
				this.Remove(triggerHandler);
			}
			this.iteratingNode = null;
			if (this.iteratingHead != null)
			{
				this.Add(this.iteratingHead);
				this.iteratingHead = null;
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000042B4 File Offset: 0x000024B4
		public void Add(ITriggerHandler<T> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (this.head == null)
			{
				this.head = handler;
				return;
			}
			if (this.iteratingNode != null)
			{
				if (this.iteratingHead == null)
				{
					this.iteratingHead = handler;
					return;
				}
				ITriggerHandler<T> prev = this.iteratingHead.Prev;
				if (prev == null)
				{
					this.iteratingHead.Prev = handler;
					this.iteratingHead.Next = handler;
					handler.Prev = this.iteratingHead;
					return;
				}
				this.iteratingHead.Prev = handler;
				prev.Next = handler;
				handler.Prev = prev;
				return;
			}
			else
			{
				ITriggerHandler<T> prev2 = this.head.Prev;
				if (prev2 == null)
				{
					this.head.Prev = handler;
					this.head.Next = handler;
					handler.Prev = this.head;
					return;
				}
				this.head.Prev = handler;
				prev2.Next = handler;
				handler.Prev = prev2;
				return;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004394 File Offset: 0x00002594
		public void Remove(ITriggerHandler<T> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			ITriggerHandler<T> prev = handler.Prev;
			ITriggerHandler<T> next = handler.Next;
			if (next != null)
			{
				next.Prev = prev;
			}
			if (handler == this.head)
			{
				this.head = next;
			}
			else if (prev != null)
			{
				prev.Next = next;
			}
			if (handler == this.iteratingNode)
			{
				this.iteratingNode = next;
			}
			if (handler == this.iteratingHead)
			{
				this.iteratingHead = next;
			}
			if (this.head != null && this.head.Prev == handler)
			{
				if (prev != this.head)
				{
					this.head.Prev = prev;
				}
				else
				{
					this.head.Prev = null;
				}
			}
			if (this.iteratingHead != null && this.iteratingHead.Prev == handler)
			{
				if (prev != this.iteratingHead.Prev)
				{
					this.iteratingHead.Prev = prev;
				}
				else
				{
					this.iteratingHead.Prev = null;
				}
			}
			handler.Prev = null;
			handler.Next = null;
		}

		// Token: 0x04000078 RID: 120
		private ITriggerHandler<T> head;

		// Token: 0x04000079 RID: 121
		private ITriggerHandler<T> iteratingHead;

		// Token: 0x0400007A RID: 122
		private ITriggerHandler<T> iteratingNode;
	}
}
