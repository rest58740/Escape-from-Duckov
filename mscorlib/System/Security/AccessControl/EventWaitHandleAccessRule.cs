using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000523 RID: 1315
	public sealed class EventWaitHandleAccessRule : AccessRule
	{
		// Token: 0x0600341E RID: 13342 RVA: 0x000BE078 File Offset: 0x000BC278
		public EventWaitHandleAccessRule(IdentityReference identity, EventWaitHandleRights eventRights, AccessControlType type) : base(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
		{
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x000BE9D2 File Offset: 0x000BCBD2
		public EventWaitHandleAccessRule(string identity, EventWaitHandleRights eventRights, AccessControlType type) : this(new NTAccount(identity), eventRights, type)
		{
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x000BC72E File Offset: 0x000BA92E
		public EventWaitHandleRights EventWaitHandleRights
		{
			get
			{
				return (EventWaitHandleRights)base.AccessMask;
			}
		}
	}
}
