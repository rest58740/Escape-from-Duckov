using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000051 RID: 81
	public class TimeSynchronizationGroup : HashSet<object>
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0000D6D2 File Offset: 0x0000B8D2
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x0000D6DC File Offset: 0x0000B8DC
		public AnimancerComponent Animancer
		{
			get
			{
				return this._Animancer;
			}
			set
			{
				this._Animancer = value;
				this.NormalizedTime = null;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x0000D6FF File Offset: 0x0000B8FF
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x0000D707 File Offset: 0x0000B907
		public float? NormalizedTime { get; set; }

		// Token: 0x060004CB RID: 1227 RVA: 0x0000D710 File Offset: 0x0000B910
		public TimeSynchronizationGroup(AnimancerComponent animancer)
		{
			this.Animancer = animancer;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000D71F File Offset: 0x0000B91F
		public bool StoreTime(object key)
		{
			return this.StoreTime(key, this.Animancer.States.Current);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000D738 File Offset: 0x0000B938
		public bool StoreTime(object key, AnimancerState state)
		{
			if (state != null && base.Contains(key))
			{
				this.NormalizedTime = new float?(state.NormalizedTime);
				return true;
			}
			this.NormalizedTime = null;
			return false;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0000D774 File Offset: 0x0000B974
		public bool SyncTime(object key)
		{
			return this.SyncTime(key, Time.deltaTime);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0000D782 File Offset: 0x0000B982
		public bool SyncTime(object key, float deltaTime)
		{
			return this.SyncTime(key, this.Animancer.States.Current, deltaTime);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000D79C File Offset: 0x0000B99C
		public bool SyncTime(object key, AnimancerState state)
		{
			return this.SyncTime(key, state, Time.deltaTime);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		public bool SyncTime(object key, AnimancerState state, float deltaTime)
		{
			if (this.NormalizedTime == null || state == null || !base.Contains(key))
			{
				return false;
			}
			state.Time = this.NormalizedTime.Value * state.Length + deltaTime * state.EffectiveSpeed;
			return true;
		}

		// Token: 0x040000D1 RID: 209
		private AnimancerComponent _Animancer;
	}
}
