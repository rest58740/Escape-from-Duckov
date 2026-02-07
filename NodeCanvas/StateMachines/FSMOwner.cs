using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000DE RID: 222
	[AddComponentMenu("NodeCanvas/FSM Owner")]
	public class FSMOwner : GraphOwner<FSM>
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000F327 File Offset: 0x0000D527
		public string currentRootStateName
		{
			get
			{
				FSM behaviour = base.behaviour;
				if (behaviour == null)
				{
					return null;
				}
				return behaviour.currentStateName;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000F33A File Offset: 0x0000D53A
		public string previousRootStateName
		{
			get
			{
				FSM behaviour = base.behaviour;
				if (behaviour == null)
				{
					return null;
				}
				return behaviour.previousStateName;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000F34D File Offset: 0x0000D54D
		public string currentDeepStateName
		{
			get
			{
				IState currentState = this.GetCurrentState(true);
				if (currentState == null)
				{
					return null;
				}
				return currentState.name;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0000F361 File Offset: 0x0000D561
		public string previousDeepStateName
		{
			get
			{
				IState previousState = this.GetPreviousState(true);
				if (previousState == null)
				{
					return null;
				}
				return previousState.name;
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000F378 File Offset: 0x0000D578
		public IState GetCurrentState(bool includeSubFSMs = true)
		{
			if (base.behaviour == null)
			{
				return null;
			}
			FSMState fsmstate = base.behaviour.currentState;
			if (includeSubFSMs)
			{
				for (;;)
				{
					NestedFSMState nestedFSMState = fsmstate as NestedFSMState;
					if (nestedFSMState == null)
					{
						break;
					}
					FSM currentInstance = nestedFSMState.currentInstance;
					fsmstate = ((currentInstance != null) ? currentInstance.currentState : null);
				}
			}
			return fsmstate;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000F3C4 File Offset: 0x0000D5C4
		public IState GetPreviousState(bool includeSubFSMs = true)
		{
			if (base.behaviour == null)
			{
				return null;
			}
			FSMState fsmstate = base.behaviour.currentState;
			FSMState result = base.behaviour.previousState;
			if (includeSubFSMs)
			{
				for (;;)
				{
					NestedFSMState nestedFSMState = fsmstate as NestedFSMState;
					if (nestedFSMState == null)
					{
						break;
					}
					FSM currentInstance = nestedFSMState.currentInstance;
					fsmstate = ((currentInstance != null) ? currentInstance.currentState : null);
					FSM currentInstance2 = nestedFSMState.currentInstance;
					result = ((currentInstance2 != null) ? currentInstance2.previousState : null);
				}
			}
			return result;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000F42F File Offset: 0x0000D62F
		public IState TriggerState(string stateName)
		{
			return this.TriggerState(stateName, FSM.TransitionCallMode.Normal);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000F439 File Offset: 0x0000D639
		public IState TriggerState(string stateName, FSM.TransitionCallMode callMode)
		{
			FSM behaviour = base.behaviour;
			if (behaviour == null)
			{
				return null;
			}
			return behaviour.TriggerState(stateName, callMode);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000F44E File Offset: 0x0000D64E
		public string[] GetStateNames()
		{
			FSM behaviour = base.behaviour;
			if (behaviour == null)
			{
				return null;
			}
			return behaviour.GetStateNames();
		}
	}
}
