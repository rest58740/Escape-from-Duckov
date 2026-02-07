using System;

namespace System.Security
{
	// Token: 0x020003D3 RID: 979
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class SecurityCriticalAttribute : Attribute
	{
		// Token: 0x06002866 RID: 10342 RVA: 0x00002050 File Offset: 0x00000250
		public SecurityCriticalAttribute()
		{
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x00092A6E File Offset: 0x00090C6E
		public SecurityCriticalAttribute(SecurityCriticalScope scope)
		{
			this._val = scope;
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06002868 RID: 10344 RVA: 0x00092A7D File Offset: 0x00090C7D
		[Obsolete("SecurityCriticalScope is only used for .NET 2.0 transparency compatibility.")]
		public SecurityCriticalScope Scope
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001EA1 RID: 7841
		private SecurityCriticalScope _val;
	}
}
