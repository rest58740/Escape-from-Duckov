using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000043 RID: 67
	internal sealed class OfType<TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000304 RID: 772 RVA: 0x0000B99F File Offset: 0x00009B9F
		public OfType(IUniTaskAsyncEnumerable<object> source)
		{
			this.source = source;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000B9AE File Offset: 0x00009BAE
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new OfType<TResult>._OfType(this.source, cancellationToken);
		}

		// Token: 0x04000105 RID: 261
		private readonly IUniTaskAsyncEnumerable<object> source;

		// Token: 0x0200017C RID: 380
		private class _OfType : AsyncEnumeratorBase<object, TResult>
		{
			// Token: 0x0600071E RID: 1822 RVA: 0x0003DEB2 File Offset: 0x0003C0B2
			public _OfType(IUniTaskAsyncEnumerable<object> source, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
			}

			// Token: 0x0600071F RID: 1823 RVA: 0x0003DEBC File Offset: 0x0003C0BC
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (!sourceHasCurrent)
				{
					result = false;
					return true;
				}
				object sourceCurrent = base.SourceCurrent;
				if (sourceCurrent is TResult)
				{
					TResult value = (TResult)((object)sourceCurrent);
					base.Current = value;
					result = true;
					return true;
				}
				result = false;
				return false;
			}
		}
	}
}
