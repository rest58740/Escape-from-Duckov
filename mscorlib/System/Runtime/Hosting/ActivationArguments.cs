using System;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace System.Runtime.Hosting
{
	// Token: 0x02000555 RID: 1365
	[ComVisible(true)]
	[Serializable]
	public sealed class ActivationArguments : EvidenceBase
	{
		// Token: 0x060035C1 RID: 13761 RVA: 0x000C207A File Offset: 0x000C027A
		public ActivationArguments(ActivationContext activationData)
		{
			if (activationData == null)
			{
				throw new ArgumentNullException("activationData");
			}
			this._context = activationData;
			this._identity = activationData.Identity;
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000C20A3 File Offset: 0x000C02A3
		public ActivationArguments(ApplicationIdentity applicationIdentity)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			this._identity = applicationIdentity;
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x000C20C0 File Offset: 0x000C02C0
		public ActivationArguments(ActivationContext activationContext, string[] activationData)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			this._context = activationContext;
			this._identity = activationContext.Identity;
			this._data = activationData;
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000C20F0 File Offset: 0x000C02F0
		public ActivationArguments(ApplicationIdentity applicationIdentity, string[] activationData)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			this._identity = applicationIdentity;
			this._data = activationData;
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x000C2114 File Offset: 0x000C0314
		public ActivationContext ActivationContext
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060035C6 RID: 13766 RVA: 0x000C211C File Offset: 0x000C031C
		public string[] ActivationData
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060035C7 RID: 13767 RVA: 0x000C2124 File Offset: 0x000C0324
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x0400250F RID: 9487
		private ActivationContext _context;

		// Token: 0x04002510 RID: 9488
		private ApplicationIdentity _identity;

		// Token: 0x04002511 RID: 9489
		private string[] _data;
	}
}
