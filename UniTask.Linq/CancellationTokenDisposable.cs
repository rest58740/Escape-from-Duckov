using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000068 RID: 104
	internal sealed class CancellationTokenDisposable : IDisposable
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000C613 File Offset: 0x0000A813
		public CancellationToken Token
		{
			get
			{
				return this.cts.Token;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C620 File Offset: 0x0000A820
		public void Dispose()
		{
			if (!this.cts.IsCancellationRequested)
			{
				this.cts.Cancel();
			}
		}

		// Token: 0x0400015E RID: 350
		private readonly CancellationTokenSource cts = new CancellationTokenSource();
	}
}
