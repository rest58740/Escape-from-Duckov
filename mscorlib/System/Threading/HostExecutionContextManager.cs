using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x020002EF RID: 751
	public class HostExecutionContextManager
	{
		// Token: 0x060020B0 RID: 8368 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public virtual HostExecutionContext Capture()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000479FC File Offset: 0x00045BFC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MonoTODO]
		public virtual void Revert(object previousState)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
		public virtual object SetHostExecutionContext(HostExecutionContext hostExecutionContext)
		{
			throw new NotImplementedException();
		}
	}
}
