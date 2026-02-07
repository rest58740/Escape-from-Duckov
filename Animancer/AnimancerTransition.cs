using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000053 RID: 83
	[Serializable]
	public abstract class AnimancerTransition<TState> : ITransition<TState>, ITransition, IHasKey, IPolymorphic, ITransitionDetailed, ITransitionWithEvents, IHasEvents, ICopyable<AnimancerTransition<TState>> where TState : AnimancerState
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0000D915 File Offset: 0x0000BB15
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public float FadeDuration
		{
			get
			{
				return this._FadeDuration;
			}
			set
			{
				if (value < 0f)
				{
					throw new ArgumentOutOfRangeException("value", "FadeDuration must not be negative");
				}
				this._FadeDuration = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0000D93E File Offset: 0x0000BB3E
		public virtual bool IsLooping
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0000D941 File Offset: 0x0000BB41
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0000D948 File Offset: 0x0000BB48
		public virtual float NormalizedStartTime
		{
			get
			{
				return float.NaN;
			}
			set
			{
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0000D94A File Offset: 0x0000BB4A
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0000D951 File Offset: 0x0000BB51
		public virtual float Speed
		{
			get
			{
				return 1f;
			}
			set
			{
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004E7 RID: 1255
		public abstract float MaximumDuration { get; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0000D953 File Offset: 0x0000BB53
		public AnimancerEvent.Sequence Events
		{
			get
			{
				if (this._Events == null)
				{
					this._Events = new AnimancerEvent.Sequence.Serializable();
				}
				return this._Events.Events;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0000D973 File Offset: 0x0000BB73
		public ref AnimancerEvent.Sequence.Serializable SerializedEvents
		{
			get
			{
				return ref this._Events;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x0000D97B File Offset: 0x0000BB7B
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x0000D983 File Offset: 0x0000BB83
		public AnimancerState BaseState { get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0000D98C File Offset: 0x0000BB8C
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		public TState State
		{
			get
			{
				if (this._State == null)
				{
					this._State = (TState)((object)this.BaseState);
				}
				return this._State;
			}
			protected set
			{
				this._State = value;
				this.BaseState = value;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x0000D9D6 File Offset: 0x0000BBD6
		public virtual bool IsValid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0000D9D9 File Offset: 0x0000BBD9
		public virtual object Key
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		public virtual FadeMode FadeMode
		{
			get
			{
				return FadeMode.FixedSpeed;
			}
		}

		// Token: 0x060004F1 RID: 1265
		public abstract TState CreateState();

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000D9DF File Offset: 0x0000BBDF
		AnimancerState ITransition.CreateState()
		{
			return this.CreateState();
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000D9EC File Offset: 0x0000BBEC
		public virtual void Apply(AnimancerState state)
		{
			state.Events = this._Events;
			this.BaseState = state;
			if (this._State != state)
			{
				this._State = default(TState);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0000DA20 File Offset: 0x0000BC20
		public virtual UnityEngine.Object MainObject { get; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0000DA28 File Offset: 0x0000BC28
		public virtual string Name
		{
			get
			{
				UnityEngine.Object mainObject = this.MainObject;
				if (!(mainObject != null))
				{
					return null;
				}
				return mainObject.name;
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000DA50 File Offset: 0x0000BC50
		public override string ToString()
		{
			string fullName = base.GetType().FullName;
			string name = this.Name;
			if (name != null)
			{
				return name + " (" + fullName + ")";
			}
			return fullName;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000DA86 File Offset: 0x0000BC86
		public virtual void CopyFrom(AnimancerTransition<TState> copyFrom)
		{
			if (copyFrom == null)
			{
				this._FadeDuration = AnimancerPlayable.DefaultFadeDuration;
				this._Events = null;
				return;
			}
			this._FadeDuration = copyFrom._FadeDuration;
			this._Events = copyFrom._Events.Clone<AnimancerEvent.Sequence.Serializable>();
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000DABB File Offset: 0x0000BCBB
		public static void ApplyDetails(AnimancerState state, float speed, float normalizedStartTime)
		{
			if (!float.IsNaN(speed))
			{
				state.Speed = speed;
			}
			if (!float.IsNaN(normalizedStartTime))
			{
				state.NormalizedTime = normalizedStartTime;
				return;
			}
			if (state.Weight == 0f)
			{
				state.NormalizedTime = AnimancerEvent.Sequence.GetDefaultNormalizedStartTime(speed);
			}
		}

		// Token: 0x040000D6 RID: 214
		[SerializeField]
		[Tooltip("The amount of time the transition will take, e.g:\n• 0s = Instant\n• 0.25s = quarter of a second (Default)\n• 0.25x = quarter of the animation length\n• x = Normalized, s = Seconds, f = Frame\n• Middle Click = reset to default value")]
		private float _FadeDuration = AnimancerPlayable.DefaultFadeDuration;

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		[Tooltip("Events which will be triggered as the animation plays")]
		private AnimancerEvent.Sequence.Serializable _Events;

		// Token: 0x040000D9 RID: 217
		private TState _State;
	}
}
