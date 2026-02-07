using System;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000716 RID: 1814
	[ComVisible(true)]
	[Serializable]
	public sealed class DispatchWrapper
	{
		// Token: 0x060040D5 RID: 16597 RVA: 0x000E1731 File Offset: 0x000DF931
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public DispatchWrapper(object obj)
		{
			if (obj != null)
			{
				Marshal.Release(Marshal.GetIDispatchForObject(obj));
			}
			this.m_WrappedObject = obj;
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x060040D6 RID: 16598 RVA: 0x000E174F File Offset: 0x000DF94F
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04002AFA RID: 11002
		private object m_WrappedObject;
	}
}
