using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000035 RID: 53
	public interface ITriggerHandler<T>
	{
		// Token: 0x060000D7 RID: 215
		void OnNext(T value);

		// Token: 0x060000D8 RID: 216
		void OnError(Exception ex);

		// Token: 0x060000D9 RID: 217
		void OnCompleted();

		// Token: 0x060000DA RID: 218
		void OnCanceled(CancellationToken cancellationToken);

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000DB RID: 219
		// (set) Token: 0x060000DC RID: 220
		ITriggerHandler<T> Prev { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000DD RID: 221
		// (set) Token: 0x060000DE RID: 222
		ITriggerHandler<T> Next { get; set; }
	}
}
