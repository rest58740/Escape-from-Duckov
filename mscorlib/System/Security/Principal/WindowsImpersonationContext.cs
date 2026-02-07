using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity;

namespace System.Security.Principal
{
	// Token: 0x020004ED RID: 1261
	[ComVisible(true)]
	public class WindowsImpersonationContext : IDisposable
	{
		// Token: 0x0600326F RID: 12911 RVA: 0x000B9913 File Offset: 0x000B7B13
		internal WindowsImpersonationContext(IntPtr token)
		{
			this._token = WindowsImpersonationContext.DuplicateToken(token);
			if (!WindowsImpersonationContext.SetCurrentToken(token))
			{
				throw new SecurityException("Couldn't impersonate token.");
			}
			this.undo = false;
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000B9941 File Offset: 0x000B7B41
		[ComVisible(false)]
		public void Dispose()
		{
			if (!this.undo)
			{
				this.Undo();
			}
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000B9951 File Offset: 0x000B7B51
		[ComVisible(false)]
		protected virtual void Dispose(bool disposing)
		{
			if (!this.undo)
			{
				this.Undo();
			}
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x000B996A File Offset: 0x000B7B6A
		public void Undo()
		{
			if (!WindowsImpersonationContext.RevertToSelf())
			{
				WindowsImpersonationContext.CloseToken(this._token);
				throw new SecurityException("Couldn't switch back to original token.");
			}
			WindowsImpersonationContext.CloseToken(this._token);
			this.undo = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003273 RID: 12915
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CloseToken(IntPtr token);

		// Token: 0x06003274 RID: 12916
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr DuplicateToken(IntPtr token);

		// Token: 0x06003275 RID: 12917
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetCurrentToken(IntPtr token);

		// Token: 0x06003276 RID: 12918
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RevertToSelf();

		// Token: 0x06003277 RID: 12919 RVA: 0x000173AD File Offset: 0x000155AD
		internal WindowsImpersonationContext()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002344 RID: 9028
		private IntPtr _token;

		// Token: 0x04002345 RID: 9029
		private bool undo;
	}
}
