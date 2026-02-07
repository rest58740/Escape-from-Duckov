using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000B8 RID: 184
	[SecurityCritical]
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	public abstract class SafeHandleZeroOrMinusOneIsInvalid : SafeHandle
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x00017642 File Offset: 0x00015842
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected SafeHandleZeroOrMinusOneIsInvalid(bool ownsHandle) : base(IntPtr.Zero, ownsHandle)
		{
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00017650 File Offset: 0x00015850
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
