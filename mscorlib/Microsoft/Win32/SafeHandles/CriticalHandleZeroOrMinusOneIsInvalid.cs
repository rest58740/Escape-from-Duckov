using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000BA RID: 186
	[SecurityCritical]
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	public abstract class CriticalHandleZeroOrMinusOneIsInvalid : CriticalHandle
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x00017694 File Offset: 0x00015894
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected CriticalHandleZeroOrMinusOneIsInvalid() : base(IntPtr.Zero)
		{
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x000176A1 File Offset: 0x000158A1
		public override bool IsInvalid
		{
			[SecurityCritical]
			get
			{
				return this.handle.IsNull() || this.handle == new IntPtr(-1);
			}
		}
	}
}
