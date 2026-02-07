using System;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x0200040D RID: 1037
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	[Serializable]
	public abstract class EvidenceBase
	{
		// Token: 0x06002A74 RID: 10868 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecurityPermission(SecurityAction.Assert, SerializationFormatter = true)]
		public virtual EvidenceBase Clone()
		{
			throw new NotImplementedException();
		}
	}
}
