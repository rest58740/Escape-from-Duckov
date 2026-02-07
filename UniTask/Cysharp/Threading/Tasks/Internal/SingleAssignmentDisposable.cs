using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200011C RID: 284
	internal sealed class SingleAssignmentDisposable : IDisposable
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0000F2DC File Offset: 0x0000D4DC
		public bool IsDisposed
		{
			get
			{
				object obj = this.gate;
				bool result;
				lock (obj)
				{
					result = this.disposed;
				}
				return result;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0000F320 File Offset: 0x0000D520
		// (set) Token: 0x0600067B RID: 1659 RVA: 0x0000F328 File Offset: 0x0000D528
		public IDisposable Disposable
		{
			get
			{
				return this.current;
			}
			set
			{
				IDisposable disposable = null;
				object obj = this.gate;
				bool flag2;
				lock (obj)
				{
					flag2 = this.disposed;
					disposable = this.current;
					if (!flag2)
					{
						if (value == null)
						{
							return;
						}
						this.current = value;
					}
				}
				if (flag2 && value != null)
				{
					value.Dispose();
					return;
				}
				if (disposable != null)
				{
					throw new InvalidOperationException("Disposable is already set");
				}
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0000F39C File Offset: 0x0000D59C
		public void Dispose()
		{
			IDisposable disposable = null;
			object obj = this.gate;
			lock (obj)
			{
				if (!this.disposed)
				{
					this.disposed = true;
					disposable = this.current;
					this.current = null;
				}
			}
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x0400014B RID: 331
		private readonly object gate = new object();

		// Token: 0x0400014C RID: 332
		private IDisposable current;

		// Token: 0x0400014D RID: 333
		private bool disposed;
	}
}
