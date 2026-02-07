using System;
using System.Threading.Tasks;

namespace System.Threading
{
	// Token: 0x020002A7 RID: 679
	public readonly struct CancellationTokenRegistration : IEquatable<CancellationTokenRegistration>, IDisposable, IAsyncDisposable
	{
		// Token: 0x06001E0A RID: 7690 RVA: 0x0006F732 File Offset: 0x0006D932
		internal CancellationTokenRegistration(CancellationCallbackInfo callbackInfo, SparselyPopulatedArrayAddInfo<CancellationCallbackInfo> registrationInfo)
		{
			this.m_callbackInfo = callbackInfo;
			this.m_registrationInfo = registrationInfo;
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001E0B RID: 7691 RVA: 0x0006F744 File Offset: 0x0006D944
		public CancellationToken Token
		{
			get
			{
				CancellationCallbackInfo callbackInfo = this.m_callbackInfo;
				if (callbackInfo == null)
				{
					return default(CancellationToken);
				}
				return callbackInfo.CancellationTokenSource.Token;
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0006F770 File Offset: 0x0006D970
		public bool Unregister()
		{
			return this.m_registrationInfo.Source != null && this.m_registrationInfo.Source.SafeAtomicRemove(this.m_registrationInfo.Index, this.m_callbackInfo) == this.m_callbackInfo;
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0006F7C4 File Offset: 0x0006D9C4
		public void Dispose()
		{
			bool flag = this.Unregister();
			CancellationCallbackInfo callbackInfo = this.m_callbackInfo;
			if (callbackInfo != null)
			{
				CancellationTokenSource cancellationTokenSource = callbackInfo.CancellationTokenSource;
				if (cancellationTokenSource.IsCancellationRequested && !cancellationTokenSource.IsCancellationCompleted && !flag && cancellationTokenSource.ThreadIDExecutingCallbacks != Environment.CurrentManagedThreadId)
				{
					cancellationTokenSource.WaitForCallbackToComplete(this.m_callbackInfo);
				}
			}
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x0006F815 File Offset: 0x0006DA15
		public static bool operator ==(CancellationTokenRegistration left, CancellationTokenRegistration right)
		{
			return left.Equals(right);
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0006F81F File Offset: 0x0006DA1F
		public static bool operator !=(CancellationTokenRegistration left, CancellationTokenRegistration right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0006F82C File Offset: 0x0006DA2C
		public override bool Equals(object obj)
		{
			return obj is CancellationTokenRegistration && this.Equals((CancellationTokenRegistration)obj);
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x0006F844 File Offset: 0x0006DA44
		public bool Equals(CancellationTokenRegistration other)
		{
			return this.m_callbackInfo == other.m_callbackInfo && this.m_registrationInfo.Source == other.m_registrationInfo.Source && this.m_registrationInfo.Index == other.m_registrationInfo.Index;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0006F8A0 File Offset: 0x0006DAA0
		public override int GetHashCode()
		{
			if (this.m_registrationInfo.Source != null)
			{
				return this.m_registrationInfo.Source.GetHashCode() ^ this.m_registrationInfo.Index.GetHashCode();
			}
			return this.m_registrationInfo.Index.GetHashCode();
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x0006F8FE File Offset: 0x0006DAFE
		public ValueTask DisposeAsync()
		{
			this.Dispose();
			return new ValueTask(Task.FromResult<object>(null));
		}

		// Token: 0x04001A77 RID: 6775
		private readonly CancellationCallbackInfo m_callbackInfo;

		// Token: 0x04001A78 RID: 6776
		private readonly SparselyPopulatedArrayAddInfo<CancellationCallbackInfo> m_registrationInfo;
	}
}
