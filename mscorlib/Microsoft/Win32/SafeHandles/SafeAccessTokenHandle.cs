using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000BC RID: 188
	[SecurityCritical]
	public sealed class SafeAccessTokenHandle : SafeHandle
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x000175C8 File Offset: 0x000157C8
		private SafeAccessTokenHandle() : base(IntPtr.Zero, true)
		{
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000176E4 File Offset: 0x000158E4
		public SafeAccessTokenHandle(IntPtr handle) : base(IntPtr.Zero, true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x000176F9 File Offset: 0x000158F9
		public static SafeAccessTokenHandle InvalidHandle
		{
			[SecurityCritical]
			get
			{
				return new SafeAccessTokenHandle(IntPtr.Zero);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00017705 File Offset: 0x00015905
		public override bool IsInvalid
		{
			[SecurityCritical]
			get
			{
				return this.handle == IntPtr.Zero || this.handle == new IntPtr(-1);
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000040F7 File Offset: 0x000022F7
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			return true;
		}
	}
}
