using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000487 RID: 1159
	[ComVisible(true)]
	public abstract class DeriveBytes : IDisposable
	{
		// Token: 0x06002EB1 RID: 11953
		public abstract byte[] GetBytes(int cb);

		// Token: 0x06002EB2 RID: 11954
		public abstract void Reset();

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000A6B27 File Offset: 0x000A4D27
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
