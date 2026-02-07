using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading
{
	// Token: 0x0200029E RID: 670
	[DebuggerDisplay("IsCancellationRequested = {IsCancellationRequested}")]
	public readonly struct CancellationToken
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x0006EB58 File Offset: 0x0006CD58
		public static CancellationToken None
		{
			get
			{
				return default(CancellationToken);
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x0006EB6E File Offset: 0x0006CD6E
		public bool IsCancellationRequested
		{
			get
			{
				return this._source != null && this._source.IsCancellationRequested;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x0006EB85 File Offset: 0x0006CD85
		public bool CanBeCanceled
		{
			get
			{
				return this._source != null;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x0006EB90 File Offset: 0x0006CD90
		public WaitHandle WaitHandle
		{
			get
			{
				return (this._source ?? CancellationTokenSource.s_neverCanceledSource).WaitHandle;
			}
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0006EBA6 File Offset: 0x0006CDA6
		internal CancellationToken(CancellationTokenSource source)
		{
			this._source = source;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0006EBAF File Offset: 0x0006CDAF
		public CancellationToken(bool canceled)
		{
			this = new CancellationToken(canceled ? CancellationTokenSource.s_canceledSource : null);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0006EBC2 File Offset: 0x0006CDC2
		public CancellationTokenRegistration Register(Action callback)
		{
			Action<object> callback2 = CancellationToken.s_actionToActionObjShunt;
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return this.Register(callback2, callback, false, true);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0006EBE1 File Offset: 0x0006CDE1
		public CancellationTokenRegistration Register(Action callback, bool useSynchronizationContext)
		{
			Action<object> callback2 = CancellationToken.s_actionToActionObjShunt;
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return this.Register(callback2, callback, useSynchronizationContext, true);
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0006EC00 File Offset: 0x0006CE00
		public CancellationTokenRegistration Register(Action<object> callback, object state)
		{
			return this.Register(callback, state, false, true);
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0006EC0C File Offset: 0x0006CE0C
		public CancellationTokenRegistration Register(Action<object> callback, object state, bool useSynchronizationContext)
		{
			return this.Register(callback, state, useSynchronizationContext, true);
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0006EC18 File Offset: 0x0006CE18
		internal CancellationTokenRegistration InternalRegisterWithoutEC(Action<object> callback, object state)
		{
			return this.Register(callback, state, false, false);
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0006EC24 File Offset: 0x0006CE24
		[MethodImpl(MethodImplOptions.NoInlining)]
		public CancellationTokenRegistration Register(Action<object> callback, object state, bool useSynchronizationContext, bool useExecutionContext)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			CancellationTokenSource source = this._source;
			if (source == null)
			{
				return default(CancellationTokenRegistration);
			}
			return source.InternalRegister(callback, state, useSynchronizationContext ? SynchronizationContext.Current : null, useExecutionContext ? ExecutionContext.Capture() : null);
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0006EC72 File Offset: 0x0006CE72
		public bool Equals(CancellationToken other)
		{
			return this._source == other._source;
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0006EC82 File Offset: 0x0006CE82
		public override bool Equals(object other)
		{
			return other is CancellationToken && this.Equals((CancellationToken)other);
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0006EC9A File Offset: 0x0006CE9A
		public override int GetHashCode()
		{
			return (this._source ?? CancellationTokenSource.s_neverCanceledSource).GetHashCode();
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x0006ECB0 File Offset: 0x0006CEB0
		public static bool operator ==(CancellationToken left, CancellationToken right)
		{
			return left.Equals(right);
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x0006ECBA File Offset: 0x0006CEBA
		public static bool operator !=(CancellationToken left, CancellationToken right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x0006ECC7 File Offset: 0x0006CEC7
		public void ThrowIfCancellationRequested()
		{
			if (this.IsCancellationRequested)
			{
				this.ThrowOperationCanceledException();
			}
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x0006ECD7 File Offset: 0x0006CED7
		private void ThrowOperationCanceledException()
		{
			throw new OperationCanceledException("The operation was canceled.", this);
		}

		// Token: 0x04001A56 RID: 6742
		private readonly CancellationTokenSource _source;

		// Token: 0x04001A57 RID: 6743
		private static readonly Action<object> s_actionToActionObjShunt = delegate(object obj)
		{
			((Action)obj)();
		};
	}
}
