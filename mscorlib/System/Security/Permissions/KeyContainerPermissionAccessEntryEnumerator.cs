using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity;

namespace System.Security.Permissions
{
	// Token: 0x02000448 RID: 1096
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntryEnumerator : IEnumerator
	{
		// Token: 0x06002C78 RID: 11384 RVA: 0x0009FBD5 File Offset: 0x0009DDD5
		internal KeyContainerPermissionAccessEntryEnumerator(ArrayList list)
		{
			this.e = list.GetEnumerator();
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002C79 RID: 11385 RVA: 0x0009FBE9 File Offset: 0x0009DDE9
		public KeyContainerPermissionAccessEntry Current
		{
			get
			{
				return (KeyContainerPermissionAccessEntry)this.e.Current;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002C7A RID: 11386 RVA: 0x0009FBFB File Offset: 0x0009DDFB
		object IEnumerator.Current
		{
			get
			{
				return this.e.Current;
			}
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x0009FC08 File Offset: 0x0009DE08
		public bool MoveNext()
		{
			return this.e.MoveNext();
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x0009FC15 File Offset: 0x0009DE15
		public void Reset()
		{
			this.e.Reset();
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x000173AD File Offset: 0x000155AD
		internal KeyContainerPermissionAccessEntryEnumerator()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002054 RID: 8276
		private IEnumerator e;
	}
}
