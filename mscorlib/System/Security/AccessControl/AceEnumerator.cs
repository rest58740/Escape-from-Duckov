using System;
using System.Collections;
using Unity;

namespace System.Security.AccessControl
{
	// Token: 0x02000505 RID: 1285
	public sealed class AceEnumerator : IEnumerator
	{
		// Token: 0x06003343 RID: 13123 RVA: 0x000BC896 File Offset: 0x000BAA96
		internal AceEnumerator(GenericAcl owner)
		{
			this.current = -1;
			base..ctor();
			this.owner = owner;
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06003344 RID: 13124 RVA: 0x000BC8AC File Offset: 0x000BAAAC
		public GenericAce Current
		{
			get
			{
				if (this.current >= 0)
				{
					return this.owner[this.current];
				}
				return null;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06003345 RID: 13125 RVA: 0x000BC8CA File Offset: 0x000BAACA
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000BC8D2 File Offset: 0x000BAAD2
		public bool MoveNext()
		{
			if (this.current + 1 == this.owner.Count)
			{
				return false;
			}
			this.current++;
			return true;
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x000BC8FA File Offset: 0x000BAAFA
		public void Reset()
		{
			this.current = -1;
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000173AD File Offset: 0x000155AD
		internal AceEnumerator()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400240F RID: 9231
		private GenericAcl owner;

		// Token: 0x04002410 RID: 9232
		private int current;
	}
}
