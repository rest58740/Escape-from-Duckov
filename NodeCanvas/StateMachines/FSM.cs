using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000DA RID: 218
	[GraphInfo(packageName = "NodeCanvas", docsURL = "https://nodecanvas.paradoxnotion.com/documentation/", resourcesURL = "https://nodecanvas.paradoxnotion.com/downloads/", forumsURL = "https://nodecanvas.paradoxnotion.com/forums-page/")]
	[CreateAssetMenu(menuName = "ParadoxNotion/NodeCanvas/FSM Asset")]
	public class FSM : Graph
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003B5 RID: 949 RVA: 0x0000EAF8 File Offset: 0x0000CCF8
		// (remove) Token: 0x060003B6 RID: 950 RVA: 0x0000EB30 File Offset: 0x0000CD30
		public event Action<IState> onStateEnter;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060003B7 RID: 951 RVA: 0x0000EB68 File Offset: 0x0000CD68
		// (remove) Token: 0x060003B8 RID: 952 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
		public event Action<IState> onStateUpdate;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060003B9 RID: 953 RVA: 0x0000EBD8 File Offset: 0x0000CDD8
		// (remove) Token: 0x060003BA RID: 954 RVA: 0x0000EC10 File Offset: 0x0000CE10
		public event Action<IState> onStateExit;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060003BB RID: 955 RVA: 0x0000EC48 File Offset: 0x0000CE48
		// (remove) Token: 0x060003BC RID: 956 RVA: 0x0000EC80 File Offset: 0x0000CE80
		public event Action<IState> onStateTransition;

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0000ECB5 File Offset: 0x0000CEB5
		// (set) Token: 0x060003BE RID: 958 RVA: 0x0000ECBD File Offset: 0x0000CEBD
		public FSMState currentState { get; private set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0000ECC6 File Offset: 0x0000CEC6
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x0000ECCE File Offset: 0x0000CECE
		public FSMState previousState { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000ECD7 File Offset: 0x0000CED7
		public string currentStateName
		{
			get
			{
				if (this.currentState == null)
				{
					return null;
				}
				return this.currentState.name;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000ECEE File Offset: 0x0000CEEE
		public string previousStateName
		{
			get
			{
				if (this.previousState == null)
				{
					return null;
				}
				return this.previousState.name;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0000ED05 File Offset: 0x0000CF05
		public override Type baseNodeType
		{
			get
			{
				return typeof(FSMNode);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000ED11 File Offset: 0x0000CF11
		public override bool requiresAgent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0000ED14 File Offset: 0x0000CF14
		public override bool requiresPrimeNode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000ED17 File Offset: 0x0000CF17
		public override bool isTree
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000ED1A File Offset: 0x0000CF1A
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000ED1D File Offset: 0x0000CF1D
		public sealed override bool canAcceptVariableDrops
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0000ED20 File Offset: 0x0000CF20
		public sealed override PlanarDirection flowDirection
		{
			get
			{
				return PlanarDirection.Auto;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000ED24 File Offset: 0x0000CF24
		protected override void OnGraphInitialize()
		{
			base.ThreadSafeInitCall(new Action(this.GatherCallbackReceivers));
			this.updatableNodes = new List<IUpdatable>();
			for (int i = 0; i < base.allNodes.Count; i++)
			{
				if (base.allNodes[i] is IUpdatable)
				{
					this.updatableNodes.Add((IUpdatable)base.allNodes[i]);
				}
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000ED93 File Offset: 0x0000CF93
		protected override void OnGraphStarted()
		{
			this.stateStack = new Stack<FSMState>();
			this.enterStartStateFlag = true;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
		protected override void OnGraphUpdate()
		{
			if (this.enterStartStateFlag)
			{
				this.enterStartStateFlag = false;
				this.EnterState((FSMState)base.primeNode, FSM.TransitionCallMode.Normal);
			}
			if (this.currentState != null)
			{
				for (int i = 0; i < this.updatableNodes.Count; i++)
				{
					this.updatableNodes[i].Update();
				}
				if (this.currentState == null)
				{
					base.Stop(false);
					return;
				}
				this.currentState.Execute(base.agent, base.blackboard);
				if (this.currentState == null)
				{
					base.Stop(false);
					return;
				}
				if (this.onStateUpdate != null && this.currentState.status == Status.Running)
				{
					this.onStateUpdate.Invoke(this.currentState);
				}
				if (this.currentState == null)
				{
					base.Stop(false);
					return;
				}
				if (this.currentState.status != Status.Running && this.currentState.outConnections.Count == 0)
				{
					if (this.stateStack.Count > 0)
					{
						FSMState newState = this.stateStack.Pop();
						this.EnterState(newState, FSM.TransitionCallMode.Normal);
						return;
					}
					if (!this.updatableNodes.Any((IUpdatable n) => n.status == Status.Running))
					{
						base.Stop(true);
						return;
					}
				}
			}
			if (this.currentState == null)
			{
				base.Stop(false);
				return;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000EF00 File Offset: 0x0000D100
		protected override void OnGraphStoped()
		{
			if (this.currentState != null && this.onStateExit != null)
			{
				this.onStateExit.Invoke(this.currentState);
			}
			this.previousState = null;
			this.currentState = null;
			this.stateStack = null;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000EF38 File Offset: 0x0000D138
		public bool EnterState(FSMState newState, FSM.TransitionCallMode callMode)
		{
			if (!base.isRunning)
			{
				return false;
			}
			if (newState == null)
			{
				return false;
			}
			if (this.currentState != null)
			{
				if (this.onStateExit != null)
				{
					this.onStateExit.Invoke(this.currentState);
				}
				this.currentState.Reset(false);
				if (callMode == FSM.TransitionCallMode.Stacked)
				{
					this.stateStack.Push(this.currentState);
					int count = this.stateStack.Count;
				}
			}
			if (callMode == FSM.TransitionCallMode.Clean)
			{
				this.stateStack.Clear();
			}
			this.previousState = this.currentState;
			this.currentState = newState;
			if (this.onStateTransition != null)
			{
				this.onStateTransition.Invoke(this.currentState);
			}
			if (this.onStateEnter != null)
			{
				this.onStateEnter.Invoke(this.currentState);
			}
			this.currentState.Execute(base.agent, base.blackboard);
			return true;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000F014 File Offset: 0x0000D214
		public FSMState TriggerState(string stateName, FSM.TransitionCallMode callMode)
		{
			FSMState stateWithName = this.GetStateWithName(stateName);
			if (stateWithName != null)
			{
				this.EnterState(stateWithName, callMode);
				return stateWithName;
			}
			return null;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000F038 File Offset: 0x0000D238
		public string[] GetStateNames()
		{
			return (from n in base.allNodes
			where n is FSMState
			select n.name).ToArray<string>();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000F098 File Offset: 0x0000D298
		public FSMState GetStateWithName(string name)
		{
			return (FSMState)base.allNodes.Find((Node n) => n is FSMState && n.name == name);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000F0D0 File Offset: 0x0000D2D0
		private void GatherCallbackReceivers()
		{
			if (base.agent == null)
			{
				return;
			}
			this.callbackReceivers = base.agent.gameObject.GetComponents<IStateCallbackReceiver>();
			if (this.callbackReceivers.Length != 0)
			{
				this.onStateEnter += delegate(IState x)
				{
					IStateCallbackReceiver[] array = this.callbackReceivers;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].OnStateEnter(x);
					}
				};
				this.onStateUpdate += delegate(IState x)
				{
					IStateCallbackReceiver[] array = this.callbackReceivers;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].OnStateUpdate(x);
					}
				};
				this.onStateExit += delegate(IState x)
				{
					IStateCallbackReceiver[] array = this.callbackReceivers;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].OnStateExit(x);
					}
				};
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000F141 File Offset: 0x0000D341
		public FSMState PeekStack()
		{
			if (this.stateStack == null || this.stateStack.Count <= 0)
			{
				return null;
			}
			return this.stateStack.Peek();
		}

		// Token: 0x04000288 RID: 648
		private List<IUpdatable> updatableNodes;

		// Token: 0x04000289 RID: 649
		private IStateCallbackReceiver[] callbackReceivers;

		// Token: 0x0400028A RID: 650
		private Stack<FSMState> stateStack;

		// Token: 0x0400028B RID: 651
		private bool enterStartStateFlag;

		// Token: 0x0200014A RID: 330
		public enum TransitionCallMode
		{
			// Token: 0x040003BB RID: 955
			Normal,
			// Token: 0x040003BC RID: 956
			Stacked,
			// Token: 0x040003BD RID: 957
			Clean
		}
	}
}
