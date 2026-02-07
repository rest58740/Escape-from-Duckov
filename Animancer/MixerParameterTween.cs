using System;

namespace Animancer
{
	// Token: 0x02000046 RID: 70
	public abstract class MixerParameterTween<TParameter> : Key, IUpdatable, Key.IListItem
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0000C8DB File Offset: 0x0000AADB
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0000C8E3 File Offset: 0x0000AAE3
		public MixerState<TParameter> Mixer { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		public TParameter StartValue { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000C8FD File Offset: 0x0000AAFD
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x0000C905 File Offset: 0x0000AB05
		public TParameter EndValue { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000C90E File Offset: 0x0000AB0E
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0000C916 File Offset: 0x0000AB16
		public float Duration { get; set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0000C91F File Offset: 0x0000AB1F
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x0000C927 File Offset: 0x0000AB27
		public float Time { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0000C930 File Offset: 0x0000AB30
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0000C93F File Offset: 0x0000AB3F
		public float Progress
		{
			get
			{
				return this.Time / this.Duration;
			}
			set
			{
				this.Time = value * this.Duration;
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000C94F File Offset: 0x0000AB4F
		public MixerParameterTween()
		{
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000C957 File Offset: 0x0000AB57
		public MixerParameterTween(MixerState<TParameter> mixer)
		{
			this.Mixer = mixer;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000C966 File Offset: 0x0000AB66
		public void Start(TParameter endValue, float duration)
		{
			this.StartValue = this.Mixer.Parameter;
			this.EndValue = endValue;
			this.Duration = duration;
			this.Time = 0f;
			this.Mixer.Root.RequirePreUpdate(this);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000C9A3 File Offset: 0x0000ABA3
		public void Stop()
		{
			MixerState<TParameter> mixer = this.Mixer;
			if (mixer == null)
			{
				return;
			}
			AnimancerPlayable root = mixer.Root;
			if (root == null)
			{
				return;
			}
			root.CancelPreUpdate(this);
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
		public bool IsActive
		{
			get
			{
				return Key.IsInList(this);
			}
		}

		// Token: 0x06000468 RID: 1128
		protected abstract TParameter CalculateCurrentValue();

		// Token: 0x06000469 RID: 1129 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		void IUpdatable.Update()
		{
			this.Time += AnimancerPlayable.DeltaTime;
			if (this.Time < this.Duration)
			{
				this.Mixer.Parameter = this.CalculateCurrentValue();
				return;
			}
			this.Time = this.Duration;
			this.Mixer.Parameter = this.EndValue;
			this.Stop();
		}
	}
}
