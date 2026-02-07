using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000013 RID: 19
	public abstract class ChannelReader<T>
	{
		// Token: 0x06000059 RID: 89
		public abstract bool TryRead(out T item);

		// Token: 0x0600005A RID: 90
		public abstract UniTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600005B RID: 91
		public abstract UniTask Completion { get; }

		// Token: 0x0600005C RID: 92 RVA: 0x00002ADC File Offset: 0x00000CDC
		public virtual UniTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			T value;
			if (this.TryRead(out value))
			{
				return UniTask.FromResult<T>(value);
			}
			return this.ReadAsyncCore(cancellationToken);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002B04 File Offset: 0x00000D04
		private UniTask<T> ReadAsyncCore(CancellationToken cancellationToken = default(CancellationToken))
		{
			ChannelReader<T>.<ReadAsyncCore>d__5 <ReadAsyncCore>d__;
			<ReadAsyncCore>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<ReadAsyncCore>d__.<>4__this = this;
			<ReadAsyncCore>d__.cancellationToken = cancellationToken;
			<ReadAsyncCore>d__.<>1__state = -1;
			<ReadAsyncCore>d__.<>t__builder.Start<ChannelReader<T>.<ReadAsyncCore>d__5>(ref <ReadAsyncCore>d__);
			return <ReadAsyncCore>d__.<>t__builder.Task;
		}

		// Token: 0x0600005E RID: 94
		public abstract IUniTaskAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
