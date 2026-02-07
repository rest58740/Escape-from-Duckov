using System;

namespace System.Threading
{
	// Token: 0x020002EE RID: 750
	[MonoTODO("Useless until the runtime supports it")]
	public class HostExecutionContext : IDisposable
	{
		// Token: 0x060020A8 RID: 8360 RVA: 0x00076989 File Offset: 0x00074B89
		public HostExecutionContext()
		{
			this._state = null;
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00076998 File Offset: 0x00074B98
		public HostExecutionContext(object state)
		{
			this._state = state;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000769A7 File Offset: 0x00074BA7
		public virtual HostExecutionContext CreateCopy()
		{
			return new HostExecutionContext(this._state);
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x000769B4 File Offset: 0x00074BB4
		// (set) Token: 0x060020AC RID: 8364 RVA: 0x000769BC File Offset: 0x00074BBC
		protected internal object State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000769C5 File Offset: 0x00074BC5
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x04001B6C RID: 7020
		private object _state;
	}
}
