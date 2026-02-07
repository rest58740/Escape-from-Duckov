using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000524 RID: 1316
	public sealed class EventWaitHandleAuditRule : AuditRule
	{
		// Token: 0x06003421 RID: 13345 RVA: 0x000BE9E2 File Offset: 0x000BCBE2
		public EventWaitHandleAuditRule(IdentityReference identity, EventWaitHandleRights eventRights, AuditFlags flags) : base(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
			if (eventRights < EventWaitHandleRights.Modify || eventRights > EventWaitHandleRights.FullControl)
			{
				throw new ArgumentOutOfRangeException("eventRights");
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x000BC72E File Offset: 0x000BA92E
		public EventWaitHandleRights EventWaitHandleRights
		{
			get
			{
				return (EventWaitHandleRights)base.AccessMask;
			}
		}
	}
}
