using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000456 RID: 1110
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public abstract class SecurityAttribute : Attribute
	{
		// Token: 0x06002D0D RID: 11533 RVA: 0x000A1B13 File Offset: 0x0009FD13
		protected SecurityAttribute(SecurityAction action)
		{
			this.Action = action;
		}

		// Token: 0x06002D0E RID: 11534
		public abstract IPermission CreatePermission();

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06002D0F RID: 11535 RVA: 0x000A1B22 File Offset: 0x0009FD22
		// (set) Token: 0x06002D10 RID: 11536 RVA: 0x000A1B2A File Offset: 0x0009FD2A
		public bool Unrestricted
		{
			get
			{
				return this.m_Unrestricted;
			}
			set
			{
				this.m_Unrestricted = value;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06002D11 RID: 11537 RVA: 0x000A1B33 File Offset: 0x0009FD33
		// (set) Token: 0x06002D12 RID: 11538 RVA: 0x000A1B3B File Offset: 0x0009FD3B
		public SecurityAction Action
		{
			get
			{
				return this.m_Action;
			}
			set
			{
				this.m_Action = value;
			}
		}

		// Token: 0x04002093 RID: 8339
		private SecurityAction m_Action;

		// Token: 0x04002094 RID: 8340
		private bool m_Unrestricted;
	}
}
