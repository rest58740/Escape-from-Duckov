using System;

namespace Animancer.FSM
{
	// Token: 0x02000003 RID: 3
	public class DelegateState : IState
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public virtual bool CanEnterState
		{
			get
			{
				return this.canEnter == null || this.canEnter();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020D7 File Offset: 0x000002D7
		public virtual bool CanExitState
		{
			get
			{
				return this.canExit == null || this.canExit();
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020EE File Offset: 0x000002EE
		public virtual void OnEnterState()
		{
			Action action = this.onEnter;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002100 File Offset: 0x00000300
		public virtual void OnExitState()
		{
			Action action = this.onExit;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04000001 RID: 1
		public Func<bool> canEnter;

		// Token: 0x04000002 RID: 2
		public Func<bool> canExit;

		// Token: 0x04000003 RID: 3
		public Action onEnter;

		// Token: 0x04000004 RID: 4
		public Action onExit;
	}
}
