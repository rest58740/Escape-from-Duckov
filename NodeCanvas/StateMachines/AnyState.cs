using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E4 RID: 228
	[Name("Any State", 0)]
	[Description("The transitions of this node will be constantly checked. If any becomes true, that transition will take place. This is not a state.")]
	[Color("b3ff7f")]
	public class AnyState : FSMNode, IUpdatable, IGraphElement
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0000F816 File Offset: 0x0000DA16
		public override string name
		{
			get
			{
				return "FROM ANY STATE";
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000F81D File Offset: 0x0000DA1D
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x0000F820 File Offset: 0x0000DA20
		public override int maxOutConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000F823 File Offset: 0x0000DA23
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000F828 File Offset: 0x0000DA28
		public override void OnGraphStarted()
		{
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				(base.outConnections[i] as FSMConnection).EnableCondition(base.graphAgent, base.graphBlackboard);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000F870 File Offset: 0x0000DA70
		public override void OnGraphStoped()
		{
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				(base.outConnections[i] as FSMConnection).DisableCondition();
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000F8AC File Offset: 0x0000DAAC
		void IUpdatable.Update()
		{
			if (base.outConnections.Count == 0)
			{
				return;
			}
			base.status = Status.Running;
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				FSMConnection fsmconnection = (FSMConnection)base.outConnections[i];
				ConditionTask condition = fsmconnection.condition;
				if (fsmconnection.isActive && condition != null && (!this.dontRetriggerStates || base.FSM.currentState != (FSMState)fsmconnection.targetNode || base.FSM.currentState.status != Status.Running))
				{
					if (condition.Check(base.graphAgent, base.graphBlackboard))
					{
						base.FSM.EnterState((FSMState)fsmconnection.targetNode, fsmconnection.transitionCallMode);
						fsmconnection.status = Status.Success;
						return;
					}
					fsmconnection.status = Status.Failure;
				}
			}
		}

		// Token: 0x0400029E RID: 670
		[Tooltip("If enabled, a transition to an already running state will not happen.")]
		public bool dontRetriggerStates;
	}
}
