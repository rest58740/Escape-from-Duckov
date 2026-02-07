using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity;

namespace System.Security.Policy
{
	// Token: 0x02000405 RID: 1029
	[ComVisible(true)]
	public sealed class ApplicationTrustEnumerator : IEnumerator
	{
		// Token: 0x06002A15 RID: 10773 RVA: 0x00098945 File Offset: 0x00096B45
		internal ApplicationTrustEnumerator(ApplicationTrustCollection atc)
		{
			this.trusts = atc;
			this.current = -1;
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002A16 RID: 10774 RVA: 0x0009895B File Offset: 0x00096B5B
		public ApplicationTrust Current
		{
			get
			{
				return this.trusts[this.current];
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06002A17 RID: 10775 RVA: 0x0009895B File Offset: 0x00096B5B
		object IEnumerator.Current
		{
			get
			{
				return this.trusts[this.current];
			}
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x0009896E File Offset: 0x00096B6E
		public void Reset()
		{
			this.current = -1;
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x00098977 File Offset: 0x00096B77
		[SecuritySafeCritical]
		public bool MoveNext()
		{
			if (this.current == this.trusts.Count - 1)
			{
				return false;
			}
			this.current++;
			return true;
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000173AD File Offset: 0x000155AD
		internal ApplicationTrustEnumerator()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04001F62 RID: 8034
		private ApplicationTrustCollection trusts;

		// Token: 0x04001F63 RID: 8035
		private int current;
	}
}
