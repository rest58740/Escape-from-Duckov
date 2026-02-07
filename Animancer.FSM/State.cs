using System;

namespace Animancer.FSM
{
	// Token: 0x02000006 RID: 6
	public abstract class State : IState
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000211A File Offset: 0x0000031A
		public virtual bool CanEnterState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000211D File Offset: 0x0000031D
		public virtual bool CanExitState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002120 File Offset: 0x00000320
		public virtual void OnEnterState()
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002122 File Offset: 0x00000322
		public virtual void OnExitState()
		{
		}
	}
}
