using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000452 RID: 1106
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class ReflectionPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002CDB RID: 11483 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public ReflectionPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002CDC RID: 11484 RVA: 0x000A0F88 File Offset: 0x0009F188
		// (set) Token: 0x06002CDD RID: 11485 RVA: 0x000A0F90 File Offset: 0x0009F190
		public ReflectionPermissionFlag Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
				this.memberAccess = ((this.flags & ReflectionPermissionFlag.MemberAccess) == ReflectionPermissionFlag.MemberAccess);
				this.reflectionEmit = ((this.flags & ReflectionPermissionFlag.ReflectionEmit) == ReflectionPermissionFlag.ReflectionEmit);
				this.typeInfo = ((this.flags & ReflectionPermissionFlag.TypeInformation) == ReflectionPermissionFlag.TypeInformation);
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002CDE RID: 11486 RVA: 0x000A0FCC File Offset: 0x0009F1CC
		// (set) Token: 0x06002CDF RID: 11487 RVA: 0x000A0FD4 File Offset: 0x0009F1D4
		public bool MemberAccess
		{
			get
			{
				return this.memberAccess;
			}
			set
			{
				if (value)
				{
					this.flags |= ReflectionPermissionFlag.MemberAccess;
				}
				else
				{
					this.flags -= 2;
				}
				this.memberAccess = value;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06002CE0 RID: 11488 RVA: 0x000A0FFE File Offset: 0x0009F1FE
		// (set) Token: 0x06002CE1 RID: 11489 RVA: 0x000A1006 File Offset: 0x0009F206
		[Obsolete]
		public bool ReflectionEmit
		{
			get
			{
				return this.reflectionEmit;
			}
			set
			{
				if (value)
				{
					this.flags |= ReflectionPermissionFlag.ReflectionEmit;
				}
				else
				{
					this.flags -= 4;
				}
				this.reflectionEmit = value;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x000A1030 File Offset: 0x0009F230
		// (set) Token: 0x06002CE3 RID: 11491 RVA: 0x000A103D File Offset: 0x0009F23D
		public bool RestrictedMemberAccess
		{
			get
			{
				return (this.flags & ReflectionPermissionFlag.RestrictedMemberAccess) == ReflectionPermissionFlag.RestrictedMemberAccess;
			}
			set
			{
				if (value)
				{
					this.flags |= ReflectionPermissionFlag.RestrictedMemberAccess;
					return;
				}
				this.flags -= 8;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06002CE4 RID: 11492 RVA: 0x000A105F File Offset: 0x0009F25F
		// (set) Token: 0x06002CE5 RID: 11493 RVA: 0x000A1067 File Offset: 0x0009F267
		[Obsolete("not enforced in 2.0+")]
		public bool TypeInformation
		{
			get
			{
				return this.typeInfo;
			}
			set
			{
				if (value)
				{
					this.flags |= ReflectionPermissionFlag.TypeInformation;
				}
				else
				{
					this.flags--;
				}
				this.typeInfo = value;
			}
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000A1094 File Offset: 0x0009F294
		public override IPermission CreatePermission()
		{
			ReflectionPermission result;
			if (base.Unrestricted)
			{
				result = new ReflectionPermission(PermissionState.Unrestricted);
			}
			else
			{
				result = new ReflectionPermission(this.flags);
			}
			return result;
		}

		// Token: 0x0400207B RID: 8315
		private ReflectionPermissionFlag flags;

		// Token: 0x0400207C RID: 8316
		private bool memberAccess;

		// Token: 0x0400207D RID: 8317
		private bool reflectionEmit;

		// Token: 0x0400207E RID: 8318
		private bool typeInfo;
	}
}
