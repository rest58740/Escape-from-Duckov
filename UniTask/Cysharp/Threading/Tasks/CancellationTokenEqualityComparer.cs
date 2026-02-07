using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200000C RID: 12
	public class CancellationTokenEqualityComparer : IEqualityComparer<CancellationToken>
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000027C8 File Offset: 0x000009C8
		public bool Equals(CancellationToken x, CancellationToken y)
		{
			return x.Equals(y);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000027D2 File Offset: 0x000009D2
		public int GetHashCode(CancellationToken obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000016 RID: 22
		public static readonly IEqualityComparer<CancellationToken> Default = new CancellationTokenEqualityComparer();
	}
}
