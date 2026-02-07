using System;
using UnityEngine;

namespace DG.DemiLib
{
	// Token: 0x02000009 RID: 9
	public abstract class DeScope : IDisposable
	{
		// Token: 0x0600000B RID: 11
		protected abstract void CloseScope();

		// Token: 0x0600000C RID: 12 RVA: 0x00002A78 File Offset: 0x00000C78
		~DeScope()
		{
			if (!this._disposed)
			{
				Debug.LogError("Scope was not disposed! You should use the 'using' keyword or manually call Dispose.");
				this.Dispose();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			this.CloseScope();
		}

		// Token: 0x04000032 RID: 50
		private bool _disposed;
	}
}
