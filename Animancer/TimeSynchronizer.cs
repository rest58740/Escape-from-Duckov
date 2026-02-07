using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000052 RID: 82
	public class TimeSynchronizer<T>
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0000D7FC File Offset: 0x0000B9FC
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x0000D804 File Offset: 0x0000BA04
		public T CurrentGroup { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0000D80D File Offset: 0x0000BA0D
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x0000D815 File Offset: 0x0000BA15
		public bool SynchronizeDefaultGroup { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0000D81E File Offset: 0x0000BA1E
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x0000D826 File Offset: 0x0000BA26
		public double NormalizedTime { get; set; }

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000D82F File Offset: 0x0000BA2F
		public TimeSynchronizer()
		{
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000D837 File Offset: 0x0000BA37
		public TimeSynchronizer(T group, bool synchronizeDefaultGroup = false)
		{
			this.CurrentGroup = group;
			this.SynchronizeDefaultGroup = synchronizeDefaultGroup;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000D84D File Offset: 0x0000BA4D
		public void StoreTime(AnimancerLayer layer)
		{
			this.StoreTime(layer.CurrentState);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000D85B File Offset: 0x0000BA5B
		public void StoreTime(AnimancerState state)
		{
			this.NormalizedTime = ((state != null) ? state.NormalizedTimeD : 0.0);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000D877 File Offset: 0x0000BA77
		public bool SyncTime(AnimancerLayer layer, T group)
		{
			return this.SyncTime(layer.CurrentState, group, Time.deltaTime);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000D88B File Offset: 0x0000BA8B
		public bool SyncTime(AnimancerLayer layer, T group, float deltaTime)
		{
			return this.SyncTime(layer.CurrentState, group, deltaTime);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0000D89B File Offset: 0x0000BA9B
		public bool SyncTime(AnimancerState state, T group)
		{
			return this.SyncTime(state, group, Time.deltaTime);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		public bool SyncTime(AnimancerState state, T group, float deltaTime)
		{
			if (state == null || !EqualityComparer<T>.Default.Equals(this.CurrentGroup, group) || (!this.SynchronizeDefaultGroup && EqualityComparer<T>.Default.Equals(default(T), group)))
			{
				this.CurrentGroup = group;
				return false;
			}
			state.TimeD = this.NormalizedTime * (double)state.Length + (double)(deltaTime * state.EffectiveSpeed);
			return true;
		}
	}
}
