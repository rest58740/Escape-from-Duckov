using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000458 RID: 1112
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SecurityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002D21 RID: 11553 RVA: 0x000A1DB3 File Offset: 0x0009FFB3
		public SecurityPermissionAttribute(SecurityAction action) : base(action)
		{
			this.m_Flags = SecurityPermissionFlag.NoFlags;
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06002D22 RID: 11554 RVA: 0x000A1DC3 File Offset: 0x0009FFC3
		// (set) Token: 0x06002D23 RID: 11555 RVA: 0x000A1DD0 File Offset: 0x0009FFD0
		public bool Assertion
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Assertion) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Assertion;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.Assertion;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002D24 RID: 11556 RVA: 0x000A1DF3 File Offset: 0x0009FFF3
		// (set) Token: 0x06002D25 RID: 11557 RVA: 0x000A1E04 File Offset: 0x000A0004
		public bool BindingRedirects
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.BindingRedirects) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.BindingRedirects;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.BindingRedirects;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002D26 RID: 11558 RVA: 0x000A1E2E File Offset: 0x000A002E
		// (set) Token: 0x06002D27 RID: 11559 RVA: 0x000A1E3F File Offset: 0x000A003F
		public bool ControlAppDomain
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlAppDomain) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlAppDomain;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlAppDomain;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002D28 RID: 11560 RVA: 0x000A1E69 File Offset: 0x000A0069
		// (set) Token: 0x06002D29 RID: 11561 RVA: 0x000A1E7A File Offset: 0x000A007A
		public bool ControlDomainPolicy
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlDomainPolicy) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlDomainPolicy;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlDomainPolicy;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06002D2A RID: 11562 RVA: 0x000A1EA4 File Offset: 0x000A00A4
		// (set) Token: 0x06002D2B RID: 11563 RVA: 0x000A1EB2 File Offset: 0x000A00B2
		public bool ControlEvidence
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlEvidence) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlEvidence;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlEvidence;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06002D2C RID: 11564 RVA: 0x000A1ED6 File Offset: 0x000A00D6
		// (set) Token: 0x06002D2D RID: 11565 RVA: 0x000A1EE4 File Offset: 0x000A00E4
		public bool ControlPolicy
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlPolicy) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlPolicy;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlPolicy;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000A1F08 File Offset: 0x000A0108
		// (set) Token: 0x06002D2F RID: 11567 RVA: 0x000A1F19 File Offset: 0x000A0119
		public bool ControlPrincipal
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlPrincipal) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlPrincipal;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlPrincipal;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002D30 RID: 11568 RVA: 0x000A1F43 File Offset: 0x000A0143
		// (set) Token: 0x06002D31 RID: 11569 RVA: 0x000A1F51 File Offset: 0x000A0151
		public bool ControlThread
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlThread) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlThread;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlThread;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002D32 RID: 11570 RVA: 0x000A1F75 File Offset: 0x000A0175
		// (set) Token: 0x06002D33 RID: 11571 RVA: 0x000A1F82 File Offset: 0x000A0182
		public bool Execution
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Execution) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Execution;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.Execution;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002D34 RID: 11572 RVA: 0x000A1FA5 File Offset: 0x000A01A5
		// (set) Token: 0x06002D35 RID: 11573 RVA: 0x000A1FB6 File Offset: 0x000A01B6
		[ComVisible(true)]
		public bool Infrastructure
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Infrastructure) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Infrastructure;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.Infrastructure;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002D36 RID: 11574 RVA: 0x000A1FE0 File Offset: 0x000A01E0
		// (set) Token: 0x06002D37 RID: 11575 RVA: 0x000A1FF1 File Offset: 0x000A01F1
		public bool RemotingConfiguration
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.RemotingConfiguration) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.RemotingConfiguration;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.RemotingConfiguration;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06002D38 RID: 11576 RVA: 0x000A201B File Offset: 0x000A021B
		// (set) Token: 0x06002D39 RID: 11577 RVA: 0x000A202C File Offset: 0x000A022C
		public bool SerializationFormatter
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.SerializationFormatter) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.SerializationFormatter;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.SerializationFormatter;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06002D3A RID: 11578 RVA: 0x000A2056 File Offset: 0x000A0256
		// (set) Token: 0x06002D3B RID: 11579 RVA: 0x000A2063 File Offset: 0x000A0263
		public bool SkipVerification
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.SkipVerification) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.SkipVerification;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.SkipVerification;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002D3C RID: 11580 RVA: 0x000A2086 File Offset: 0x000A0286
		// (set) Token: 0x06002D3D RID: 11581 RVA: 0x000A2093 File Offset: 0x000A0293
		public bool UnmanagedCode
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.UnmanagedCode) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.UnmanagedCode;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.UnmanagedCode;
			}
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000A20B8 File Offset: 0x000A02B8
		public override IPermission CreatePermission()
		{
			SecurityPermission result;
			if (base.Unrestricted)
			{
				result = new SecurityPermission(PermissionState.Unrestricted);
			}
			else
			{
				result = new SecurityPermission(this.m_Flags);
			}
			return result;
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000A20E5 File Offset: 0x000A02E5
		// (set) Token: 0x06002D40 RID: 11584 RVA: 0x000A20ED File Offset: 0x000A02ED
		public SecurityPermissionFlag Flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x04002097 RID: 8343
		private SecurityPermissionFlag m_Flags;
	}
}
