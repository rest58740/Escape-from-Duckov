using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer.FSM
{
	// Token: 0x02000010 RID: 16
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer.FSM/StateMachine_2")]
	[Serializable]
	public class StateMachine<TKey, TState> : StateMachine<TState>, IKeyedStateMachine<TKey>, IDictionary<TKey, TState>, ICollection<KeyValuePair<TKey, TState>>, IEnumerable<KeyValuePair<TKey, TState>>, IEnumerable where TState : class, IState
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005C RID: 92 RVA: 0x0000272A File Offset: 0x0000092A
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002732 File Offset: 0x00000932
		public IDictionary<TKey, TState> Dictionary { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000273B File Offset: 0x0000093B
		public TKey CurrentKey
		{
			get
			{
				return this._CurrentKey;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002743 File Offset: 0x00000943
		public TKey PreviousKey
		{
			get
			{
				return KeyChange<TKey>.PreviousKey;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000060 RID: 96 RVA: 0x0000274A File Offset: 0x0000094A
		public TKey NextKey
		{
			get
			{
				return KeyChange<TKey>.NextKey;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002751 File Offset: 0x00000951
		public StateMachine()
		{
			this.Dictionary = new Dictionary<TKey, TState>();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002764 File Offset: 0x00000964
		public StateMachine(IDictionary<TKey, TState> dictionary)
		{
			this.Dictionary = dictionary;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002773 File Offset: 0x00000973
		public StateMachine(TKey defaultKey, TState defaultState)
		{
			this.Dictionary = new Dictionary<TKey, TState>
			{
				{
					defaultKey,
					defaultState
				}
			};
			this.ForceSetState(defaultKey, defaultState);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002796 File Offset: 0x00000996
		public StateMachine(IDictionary<TKey, TState> dictionary, TKey defaultKey, TState defaultState)
		{
			this.Dictionary = dictionary;
			dictionary.Add(defaultKey, defaultState);
			this.ForceSetState(defaultKey, defaultState);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000027B8 File Offset: 0x000009B8
		public override void InitializeAfterDeserialize()
		{
			if (base.CurrentState != null)
			{
				using (new KeyChange<TKey>(this, default(TKey), this._CurrentKey))
				{
					using (new StateChange<TState>(this, default(TState), base.CurrentState))
					{
						base.CurrentState.OnEnterState();
						return;
					}
				}
			}
			TState state;
			if (this.Dictionary.TryGetValue(this._CurrentKey, out state))
			{
				this.ForceSetState(this._CurrentKey, state);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000286C File Offset: 0x00000A6C
		public bool TrySetState(TKey key, TState state)
		{
			return base.CurrentState == state || this.TryResetState(key, state);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000288B File Offset: 0x00000A8B
		public TState TrySetState(TKey key)
		{
			if (EqualityComparer<TKey>.Default.Equals(this._CurrentKey, key))
			{
				return base.CurrentState;
			}
			return this.TryResetState(key);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000028AE File Offset: 0x00000AAE
		object IKeyedStateMachine<!0>.TrySetState(TKey key)
		{
			return this.TrySetState(key);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000028BC File Offset: 0x00000ABC
		public bool TryResetState(TKey key, TState state)
		{
			bool result;
			using (new KeyChange<TKey>(this, this._CurrentKey, key))
			{
				if (!base.CanSetState(state))
				{
					result = false;
				}
				else
				{
					this._CurrentKey = key;
					base.ForceSetState(state);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002918 File Offset: 0x00000B18
		public TState TryResetState(TKey key)
		{
			TState tstate;
			if (this.Dictionary.TryGetValue(key, out tstate) && this.TryResetState(key, tstate))
			{
				return tstate;
			}
			return default(TState);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000294A File Offset: 0x00000B4A
		object IKeyedStateMachine<!0>.TryResetState(TKey key)
		{
			return this.TryResetState(key);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002958 File Offset: 0x00000B58
		public void ForceSetState(TKey key, TState state)
		{
			using (new KeyChange<TKey>(this, this._CurrentKey, key))
			{
				this._CurrentKey = key;
				base.ForceSetState(state);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000029A4 File Offset: 0x00000BA4
		public TState ForceSetState(TKey key)
		{
			TState tstate;
			this.Dictionary.TryGetValue(key, out tstate);
			this.ForceSetState(key, tstate);
			return tstate;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000029C9 File Offset: 0x00000BC9
		object IKeyedStateMachine<!0>.ForceSetState(TKey key)
		{
			return this.ForceSetState(key);
		}

		// Token: 0x17000023 RID: 35
		public TState this[TKey key]
		{
			get
			{
				return this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000029F4 File Offset: 0x00000BF4
		public bool TryGetValue(TKey key, out TState state)
		{
			return this.Dictionary.TryGetValue(key, out state);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002A03 File Offset: 0x00000C03
		public ICollection<TKey> Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002A10 File Offset: 0x00000C10
		public ICollection<TState> Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002A1D File Offset: 0x00000C1D
		public int Count
		{
			get
			{
				return this.Dictionary.Count;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002A2A File Offset: 0x00000C2A
		public void Add(TKey key, TState state)
		{
			this.Dictionary.Add(key, state);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002A39 File Offset: 0x00000C39
		public void Add(KeyValuePair<TKey, TState> item)
		{
			this.Dictionary.Add(item);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002A47 File Offset: 0x00000C47
		public bool Remove(TKey key)
		{
			return this.Dictionary.Remove(key);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002A55 File Offset: 0x00000C55
		public bool Remove(KeyValuePair<TKey, TState> item)
		{
			return this.Dictionary.Remove(item);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002A63 File Offset: 0x00000C63
		public void Clear()
		{
			this.Dictionary.Clear();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002A70 File Offset: 0x00000C70
		public bool Contains(KeyValuePair<TKey, TState> item)
		{
			return this.Dictionary.Contains(item);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002A7E File Offset: 0x00000C7E
		public bool ContainsKey(TKey key)
		{
			return this.Dictionary.ContainsKey(key);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002A8C File Offset: 0x00000C8C
		public IEnumerator<KeyValuePair<TKey, TState>> GetEnumerator()
		{
			return this.Dictionary.GetEnumerator();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002A99 File Offset: 0x00000C99
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002AA1 File Offset: 0x00000CA1
		public void CopyTo(KeyValuePair<TKey, TState>[] array, int arrayIndex)
		{
			this.Dictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002AB0 File Offset: 0x00000CB0
		bool ICollection<KeyValuePair<!0, !1>>.IsReadOnly
		{
			get
			{
				return this.Dictionary.IsReadOnly;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002AC0 File Offset: 0x00000CC0
		public TState GetState(TKey key)
		{
			TState result;
			this.TryGetValue(key, out result);
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public void AddRange(TKey[] keys, TState[] states)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				this.Dictionary.Add(keys[i], states[i]);
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002B0C File Offset: 0x00000D0C
		public void SetFakeKey(TKey key)
		{
			this._CurrentKey = key;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002B18 File Offset: 0x00000D18
		public override string ToString()
		{
			return string.Format("{0} -> {1} -> {2}", base.GetType().FullName, this._CurrentKey, (base.CurrentState != null) ? base.CurrentState.ToString() : "null");
		}

		// Token: 0x04000011 RID: 17
		[SerializeField]
		private TKey _CurrentKey;

		// Token: 0x02000017 RID: 23
		public new class InputBuffer : StateMachine<TState>.InputBuffer<StateMachine<TKey, TState>>
		{
			// Token: 0x1700002D RID: 45
			// (get) Token: 0x0600009E RID: 158 RVA: 0x00002DEA File Offset: 0x00000FEA
			// (set) Token: 0x0600009F RID: 159 RVA: 0x00002DF2 File Offset: 0x00000FF2
			public TKey Key { get; set; }

			// Token: 0x060000A0 RID: 160 RVA: 0x00002DFB File Offset: 0x00000FFB
			public InputBuffer()
			{
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x00002E03 File Offset: 0x00001003
			public InputBuffer(StateMachine<TKey, TState> stateMachine) : base(stateMachine)
			{
			}

			// Token: 0x060000A2 RID: 162 RVA: 0x00002E0C File Offset: 0x0000100C
			public bool Buffer(TKey key, float timeOut)
			{
				TState state;
				if (base.StateMachine.TryGetValue(key, out state))
				{
					this.Buffer(key, state, timeOut);
					return true;
				}
				return false;
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x00002E35 File Offset: 0x00001035
			public void Buffer(TKey key, TState state, float timeOut)
			{
				this.Key = key;
				base.Buffer(state, timeOut);
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x00002E46 File Offset: 0x00001046
			protected override bool TryEnterState()
			{
				return base.StateMachine.TryResetState(this.Key, base.State);
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x00002E60 File Offset: 0x00001060
			public override void Clear()
			{
				base.Clear();
				this.Key = default(TKey);
			}
		}

		// Token: 0x02000018 RID: 24
		[Serializable]
		public new class WithDefault : StateMachine<TKey, TState>
		{
			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000A6 RID: 166 RVA: 0x00002E82 File Offset: 0x00001082
			// (set) Token: 0x060000A7 RID: 167 RVA: 0x00002E8A File Offset: 0x0000108A
			public TKey DefaultKey
			{
				get
				{
					return this._DefaultKey;
				}
				set
				{
					this._DefaultKey = value;
					if (base.CurrentState == null && value != null)
					{
						base.ForceSetState(value);
					}
				}
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x00002EB0 File Offset: 0x000010B0
			public WithDefault()
			{
				this.ForceSetDefaultState = delegate()
				{
					base.ForceSetState(this._DefaultKey);
				};
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00002ECA File Offset: 0x000010CA
			public WithDefault(TKey defaultKey) : this()
			{
				this._DefaultKey = defaultKey;
				base.ForceSetState(defaultKey);
			}

			// Token: 0x060000AA RID: 170 RVA: 0x00002EE4 File Offset: 0x000010E4
			public override void InitializeAfterDeserialize()
			{
				if (base.CurrentState != null)
				{
					using (new KeyChange<TKey>(this, default(TKey), this._DefaultKey))
					{
						using (new StateChange<TState>(this, default(TState), base.CurrentState))
						{
							base.CurrentState.OnEnterState();
							return;
						}
					}
				}
				base.ForceSetState(this._DefaultKey);
			}

			// Token: 0x060000AB RID: 171 RVA: 0x00002F84 File Offset: 0x00001184
			public TState TrySetDefaultState()
			{
				return base.TrySetState(this._DefaultKey);
			}

			// Token: 0x060000AC RID: 172 RVA: 0x00002F92 File Offset: 0x00001192
			public TState TryResetDefaultState()
			{
				return base.TryResetState(this._DefaultKey);
			}

			// Token: 0x0400001F RID: 31
			[SerializeField]
			private TKey _DefaultKey;

			// Token: 0x04000020 RID: 32
			public readonly Action ForceSetDefaultState;
		}
	}
}
