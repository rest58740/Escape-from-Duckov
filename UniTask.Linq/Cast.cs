using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200000D RID: 13
	internal sealed class Cast<TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000239 RID: 569 RVA: 0x00008538 File Offset: 0x00006738
		public Cast(IUniTaskAsyncEnumerable<object> source)
		{
			this.source = source;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00008547 File Offset: 0x00006747
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Cast<TResult>._Cast(this.source, cancellationToken);
		}

		// Token: 0x04000018 RID: 24
		private readonly IUniTaskAsyncEnumerable<object> source;

		// Token: 0x020000DE RID: 222
		private class _Cast : AsyncEnumeratorBase<object, TResult>
		{
			// Token: 0x0600049C RID: 1180 RVA: 0x00017BC9 File Offset: 0x00015DC9
			public _Cast(IUniTaskAsyncEnumerable<object> source, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
			}

			// Token: 0x0600049D RID: 1181 RVA: 0x00017BD3 File Offset: 0x00015DD3
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (sourceHasCurrent)
				{
					base.Current = (TResult)((object)base.SourceCurrent);
					result = true;
					return true;
				}
				result = false;
				return true;
			}
		}
	}
}
