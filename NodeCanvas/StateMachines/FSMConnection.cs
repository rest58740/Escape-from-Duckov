using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000DB RID: 219
	public class FSMConnection : Connection, ITaskAssignable<ConditionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000F1F3 File Offset: 0x0000D3F3
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0000F1FB File Offset: 0x0000D3FB
		public ConditionTask condition
		{
			get
			{
				return this._condition;
			}
			set
			{
				this._condition = value;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000F204 File Offset: 0x0000D404
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000F20C File Offset: 0x0000D40C
		public Task task
		{
			get
			{
				return this.condition;
			}
			set
			{
				this.condition = (ConditionTask)value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000F21A File Offset: 0x0000D41A
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0000F222 File Offset: 0x0000D422
		public FSM.TransitionCallMode transitionCallMode
		{
			get
			{
				return this._transitionCallMode;
			}
			private set
			{
				this._transitionCallMode = value;
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000F22B File Offset: 0x0000D42B
		public void EnableCondition(Component agent, IBlackboard blackboard)
		{
			if (this.condition != null)
			{
				this.condition.Enable(agent, blackboard);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000F242 File Offset: 0x0000D442
		public void DisableCondition()
		{
			if (this.condition != null)
			{
				this.condition.Disable();
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000F257 File Offset: 0x0000D457
		public void PerformTransition()
		{
			(base.graph as FSM).EnterState((FSMState)base.targetNode, this.transitionCallMode);
		}

		// Token: 0x04000292 RID: 658
		[SerializeField]
		private FSM.TransitionCallMode _transitionCallMode;

		// Token: 0x04000293 RID: 659
		[SerializeField]
		private ConditionTask _condition;
	}
}
