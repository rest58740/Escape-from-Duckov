using System;
using System.IO;
using System.Security;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000B5 RID: 181
	[SecurityCritical]
	public sealed class SafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x0001747F File Offset: 0x0001567F
		private SafeFileHandle() : base(true)
		{
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000174AE File Offset: 0x000156AE
		public SafeFileHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(preexistingHandle);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000175F8 File Offset: 0x000157F8
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			MonoIOError monoIOError;
			MonoIO.Close(this.handle, out monoIOError);
			return monoIOError == MonoIOError.ERROR_SUCCESS;
		}
	}
}
