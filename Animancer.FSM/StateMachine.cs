using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Animancer.FSM
{
	// Token: 0x0200000C RID: 12
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer.FSM/StateMachine_1")]
	[Serializable]
	public class StateMachine<TState> : IStateMachine where TState : class, IState
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000023BA File Offset: 0x000005BA
		public TState CurrentState
		{
			get
			{
				return this._CurrentState;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000023C2 File Offset: 0x000005C2
		public TState PreviousState
		{
			get
			{
				return StateChange<TState>.PreviousState;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000023C9 File Offset: 0x000005C9
		public TState NextState
		{
			get
			{
				return StateChange<TState>.NextState;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000023D0 File Offset: 0x000005D0
		public StateMachine()
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000023D8 File Offset: 0x000005D8
		public StateMachine(TState state)
		{
			using (new StateChange<TState>(this, default(TState), state))
			{
				this._CurrentState = state;
				state.OnEnterState();
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002430 File Offset: 0x00000630
		public virtual void InitializeAfterDeserialize()
		{
			if (this._CurrentState != null)
			{
				using (new StateChange<TState>(this, default(TState), this._CurrentState))
				{
					this._CurrentState.OnEnterState();
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002494 File Offset: 0x00000694
		public bool CanSetState(TState state)
		{
			bool result;
			using (new StateChange<TState>(this, this._CurrentState, state))
			{
				if (this._CurrentState != null && !this._CurrentState.CanExitState)
				{
					result = false;
				}
				else if (state != null && !state.CanEnterState)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002510 File Offset: 0x00000710
		public TState CanSetState(IList<TState> states)
		{
			int count = states.Count;
			for (int i = 0; i < count; i++)
			{
				TState tstate = states[i];
				if (this.CanSetState(tstate))
				{
					return tstate;
				}
			}
			return default(TState);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000254C File Offset: 0x0000074C
		public bool TrySetState(TState state)
		{
			return this._CurrentState == state || this.TryResetState(state);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000256C File Offset: 0x0000076C
		public bool TrySetState(IList<TState> states)
		{
			int count = states.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.TrySetState(states[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000259E File Offset: 0x0000079E
		public bool TryResetState(TState state)
		{
			if (!this.CanSetState(state))
			{
				return false;
			}
			this.ForceSetState(state);
			return true;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000025B4 File Offset: 0x000007B4
		public bool TryResetState(IList<TState> states)
		{
			int count = states.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.TryResetState(states[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000025E8 File Offset: 0x000007E8
		public void ForceSetState(TState state)
		{
			using (new StateChange<TState>(this, this._CurrentState, state))
			{
				TState tstate = this._CurrentState;
				if (tstate != null)
				{
					tstate.OnExitState();
				}
				this._CurrentState = state;
				TState tstate2 = state;
				if (tstate2 != null)
				{
					tstate2.OnEnterState();
				}
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002654 File Offset: 0x00000854
		public override string ToString()
		{
			return string.Format("{0} -> {1}", base.GetType().Name, this._CurrentState);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002676 File Offset: 0x00000876
		[Conditional("UNITY_ASSERTIONS")]
		public void SetAllowNullStates(bool allow = true)
		{
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002678 File Offset: 0x00000878
		object IStateMachine.CurrentState
		{
			get
			{
				return this._CurrentState;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002685 File Offset: 0x00000885
		object IStateMachine.PreviousState
		{
			get
			{
				return this.PreviousState;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002692 File Offset: 0x00000892
		object IStateMachine.NextState
		{
			get
			{
				return this.NextState;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000269F File Offset: 0x0000089F
		object IStateMachine.CanSetState(IList states)
		{
			return this.CanSetState((List<TState>)states);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000026B2 File Offset: 0x000008B2
		bool IStateMachine.CanSetState(object state)
		{
			return this.CanSetState((TState)((object)state));
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000026C0 File Offset: 0x000008C0
		void IStateMachine.ForceSetState(object state)
		{
			this.ForceSetState((TState)((object)state));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000026CE File Offset: 0x000008CE
		bool IStateMachine.TryResetState(IList states)
		{
			return this.TryResetState((List<TState>)states);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000026DC File Offset: 0x000008DC
		bool IStateMachine.TryResetState(object state)
		{
			return this.TryResetState((TState)((object)state));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000026EA File Offset: 0x000008EA
		bool IStateMachine.TrySetState(IList states)
		{
			return this.TrySetState((List<TState>)states);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000026F8 File Offset: 0x000008F8
		bool IStateMachine.TrySetState(object state)
		{
			return this.TrySetState((TState)((object)state));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002706 File Offset: 0x00000906
		void IStateMachine.SetAllowNullStates(bool allow)
		{
		}

		// Token: 0x0400000E RID: 14
		[SerializeField]
		private TState _CurrentState;

		// Token: 0x02000013 RID: 19
		public class InputBuffer : StateMachine<TState>.InputBuffer<StateMachine<TState>>
		{
			// Token: 0x06000084 RID: 132 RVA: 0x00002B69 File Offset: 0x00000D69
			public InputBuffer()
			{
			}

			// Token: 0x06000085 RID: 133 RVA: 0x00002B71 File Offset: 0x00000D71
			public InputBuffer(StateMachine<TState> stateMachine) : base(stateMachine)
			{
			}
		}

		// Token: 0x02000014 RID: 20
		public class InputBuffer<TStateMachine> where TStateMachine : StateMachine<TState>
		{
			// Token: 0x17000028 RID: 40
			// (get) Token: 0x06000086 RID: 134 RVA: 0x00002B7A File Offset: 0x00000D7A
			// (set) Token: 0x06000087 RID: 135 RVA: 0x00002B82 File Offset: 0x00000D82
			public TStateMachine StateMachine
			{
				get
				{
					return this._StateMachine;
				}
				set
				{
					this._StateMachine = value;
					this.Clear();
				}
			}

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x06000088 RID: 136 RVA: 0x00002B91 File Offset: 0x00000D91
			// (set) Token: 0x06000089 RID: 137 RVA: 0x00002B99 File Offset: 0x00000D99
			public TState State { get; set; }

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x0600008A RID: 138 RVA: 0x00002BA2 File Offset: 0x00000DA2
			// (set) Token: 0x0600008B RID: 139 RVA: 0x00002BAA File Offset: 0x00000DAA
			public float TimeOut { get; set; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x0600008C RID: 140 RVA: 0x00002BB3 File Offset: 0x00000DB3
			public bool IsActive
			{
				get
				{
					return this.State != null;
				}
			}

			// Token: 0x0600008D RID: 141 RVA: 0x00002BC3 File Offset: 0x00000DC3
			public InputBuffer()
			{
			}

			// Token: 0x0600008E RID: 142 RVA: 0x00002BCB File Offset: 0x00000DCB
			public InputBuffer(TStateMachine stateMachine)
			{
				this._StateMachine = stateMachine;
			}

			// Token: 0x0600008F RID: 143 RVA: 0x00002BDA File Offset: 0x00000DDA
			public void Buffer(TState state, float timeOut)
			{
				this.State = state;
				this.TimeOut = timeOut;
			}

			// Token: 0x06000090 RID: 144 RVA: 0x00002BEA File Offset: 0x00000DEA
			protected virtual bool TryEnterState()
			{
				return this.StateMachine.TryResetState(this.State);
			}

			// Token: 0x06000091 RID: 145 RVA: 0x00002C02 File Offset: 0x00000E02
			public bool Update()
			{
				return this.Update(Time.deltaTime);
			}

			// Token: 0x06000092 RID: 146 RVA: 0x00002C0F File Offset: 0x00000E0F
			public bool Update(float deltaTime)
			{
				if (this.IsActive)
				{
					if (this.TryEnterState())
					{
						this.Clear();
						return true;
					}
					this.TimeOut -= deltaTime;
					if (this.TimeOut < 0f)
					{
						this.Clear();
					}
				}
				return false;
			}

			// Token: 0x06000093 RID: 147 RVA: 0x00002C4C File Offset: 0x00000E4C
			public virtual void Clear()
			{
				this.State = default(TState);
				this.TimeOut = 0f;
			}

			// Token: 0x04000019 RID: 25
			private TStateMachine _StateMachine;
		}

		// Token: 0x02000015 RID: 21
		public class StateSelector : SortedList<float, TState>
		{
			// Token: 0x06000094 RID: 148 RVA: 0x00002C73 File Offset: 0x00000E73
			public StateSelector() : base(ReverseComparer<float>.Instance)
			{
			}

			// Token: 0x06000095 RID: 149 RVA: 0x00002C80 File Offset: 0x00000E80
			public void Add<TPrioritizable>(TPrioritizable state) where TPrioritizable : TState, IPrioritizable
			{
				base.Add(state.Priority, (TState)((object)state));
			}
		}

		// Token: 0x02000016 RID: 22
		[Serializable]
		public class WithDefault : StateMachine<TState>
		{
			// Token: 0x1700002C RID: 44
			// (get) Token: 0x06000096 RID: 150 RVA: 0x00002CA0 File Offset: 0x00000EA0
			// (set) Token: 0x06000097 RID: 151 RVA: 0x00002CA8 File Offset: 0x00000EA8
			public TState DefaultState
			{
				get
				{
					return this._DefaultState;
				}
				set
				{
					this._DefaultState = value;
					if (this._CurrentState == null && value != null)
					{
						base.ForceSetState(value);
					}
				}
			}

			// Token: 0x06000098 RID: 152 RVA: 0x00002CCD File Offset: 0x00000ECD
			public WithDefault()
			{
				this.ForceSetDefaultState = delegate()
				{
					base.ForceSetState(this._DefaultState);
				};
			}

			// Token: 0x06000099 RID: 153 RVA: 0x00002CE7 File Offset: 0x00000EE7
			public WithDefault(TState defaultState) : this()
			{
				this._DefaultState = defaultState;
				base.ForceSetState(defaultState);
			}

			// Token: 0x0600009A RID: 154 RVA: 0x00002D00 File Offset: 0x00000F00
			public override void InitializeAfterDeserialize()
			{
				if (this._CurrentState != null)
				{
					using (new StateChange<TState>(this, default(TState), this._CurrentState))
					{
						this._CurrentState.OnEnterState();
						return;
					}
				}
				if (this._DefaultState != null)
				{
					using (new StateChange<TState>(this, default(TState), base.CurrentState))
					{
						this._CurrentState = this._DefaultState;
						this._CurrentState.OnEnterState();
					}
				}
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00002DC0 File Offset: 0x00000FC0
			public bool TrySetDefaultState()
			{
				return base.TrySetState(this.DefaultState);
			}

			// Token: 0x0600009C RID: 156 RVA: 0x00002DCE File Offset: 0x00000FCE
			public bool TryResetDefaultState()
			{
				return base.TryResetState(this.DefaultState);
			}

			// Token: 0x0400001C RID: 28
			[SerializeField]
			private TState _DefaultState;

			// Token: 0x0400001D RID: 29
			public readonly Action ForceSetDefaultState;
		}
	}
}
