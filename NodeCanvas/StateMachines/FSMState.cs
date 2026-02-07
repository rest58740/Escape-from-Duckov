using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000DF RID: 223
	public abstract class FSMState : FSMNode, IState
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000F469 File Offset: 0x0000D669
		public override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000F46C File Offset: 0x0000D66C
		public override bool canSelfConnect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000F46F File Offset: 0x0000D66F
		public override int maxInConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000F472 File Offset: 0x0000D672
		public override int maxOutConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000F475 File Offset: 0x0000D675
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000F47D File Offset: 0x0000D67D
		public FSMState.TransitionEvaluationMode transitionEvaluation
		{
			get
			{
				return this._transitionEvaluation;
			}
			set
			{
				this._transitionEvaluation = value;
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000F488 File Offset: 0x0000D688
		public FSMConnection[] GetTransitions()
		{
			FSMConnection[] array = new FSMConnection[base.outConnections.Count];
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				array[i] = (FSMConnection)base.outConnections[i];
			}
			return array;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000F4D1 File Offset: 0x0000D6D1
		public void Finish()
		{
			this.Finish(Status.Success);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000F4DA File Offset: 0x0000D6DA
		public void Finish(bool inSuccess)
		{
			this.Finish(inSuccess ? Status.Success : Status.Failure);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000F4E9 File Offset: 0x0000D6E9
		public void Finish(Status status)
		{
			base.status = status;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000F4F2 File Offset: 0x0000D6F2
		public override void OnGraphPaused()
		{
			if (base.status == Status.Running)
			{
				this.OnPause();
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000F503 File Offset: 0x0000D703
		protected override bool CanConnectFromSource(Node sourceNode)
		{
			return !base.IsChildOf(sourceNode);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000F511 File Offset: 0x0000D711
		protected override bool CanConnectToTarget(Node targetNode)
		{
			return !base.IsParentOf(targetNode);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000F520 File Offset: 0x0000D720
		protected sealed override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (!this._hasInit)
			{
				this._hasInit = true;
				this.OnInit();
			}
			if (base.status == Status.Resting)
			{
				base.status = Status.Running;
				for (int i = 0; i < base.outConnections.Count; i++)
				{
					((FSMConnection)base.outConnections[i]).EnableCondition(agent, bb);
				}
				this.OnEnter();
			}
			else
			{
				bool flag = this.transitionEvaluation == FSMState.TransitionEvaluationMode.CheckContinuously;
				bool flag2 = this.transitionEvaluation == FSMState.TransitionEvaluationMode.CheckAfterStateFinished && base.status != Status.Running;
				if (flag || flag2)
				{
					this.CheckTransitions();
				}
				if (base.status == Status.Running)
				{
					this.OnUpdate();
				}
			}
			return base.status;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000F5CC File Offset: 0x0000D7CC
		public bool CheckTransitions()
		{
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				FSMConnection fsmconnection = (FSMConnection)base.outConnections[i];
				ConditionTask condition = fsmconnection.condition;
				if (fsmconnection.isActive)
				{
					if ((condition != null && condition.Check(base.graphAgent, base.graphBlackboard)) || (condition == null && base.status != Status.Running))
					{
						base.FSM.EnterState((FSMState)fsmconnection.targetNode, fsmconnection.transitionCallMode);
						fsmconnection.status = Status.Success;
						return true;
					}
					fsmconnection.status = Status.Failure;
				}
			}
			return false;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000F664 File Offset: 0x0000D864
		protected sealed override void OnReset()
		{
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				((FSMConnection)base.outConnections[i]).DisableCondition();
			}
			this.OnExit();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000F6A3 File Offset: 0x0000D8A3
		protected virtual void OnInit()
		{
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000F6A5 File Offset: 0x0000D8A5
		protected virtual void OnEnter()
		{
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000F6A7 File Offset: 0x0000D8A7
		protected virtual void OnUpdate()
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000F6A9 File Offset: 0x0000D8A9
		protected virtual void OnExit()
		{
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000F6AB File Offset: 0x0000D8AB
		protected virtual void OnPause()
		{
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000F6B5 File Offset: 0x0000D8B5
		string IState.get_tag()
		{
			return base.tag;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000F6BD File Offset: 0x0000D8BD
		float IState.get_elapsedTime()
		{
			return base.elapsedTime;
		}

		// Token: 0x04000297 RID: 663
		[SerializeField]
		private FSMState.TransitionEvaluationMode _transitionEvaluation;

		// Token: 0x04000298 RID: 664
		private bool _hasInit;

		// Token: 0x0200014D RID: 333
		public enum TransitionEvaluationMode
		{
			// Token: 0x040003C4 RID: 964
			CheckContinuously,
			// Token: 0x040003C5 RID: 965
			CheckAfterStateFinished,
			// Token: 0x040003C6 RID: 966
			CheckManually
		}
	}
}
