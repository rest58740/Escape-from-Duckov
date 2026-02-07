using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000055 RID: 85
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/AnimancerTransitionAssetBase")]
	public abstract class AnimancerTransitionAssetBase : ScriptableObject, ITransition, IHasKey, IPolymorphic, IWrapper, IAnimationClipSource
	{
		// Token: 0x060004FB RID: 1275
		public abstract ITransition GetTransition();

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0000DB10 File Offset: 0x0000BD10
		object IWrapper.WrappedObject
		{
			get
			{
				return this.GetTransition();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0000DB18 File Offset: 0x0000BD18
		public virtual bool IsValid
		{
			get
			{
				return this.GetTransition().IsValid();
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000DB25 File Offset: 0x0000BD25
		public virtual float FadeDuration
		{
			get
			{
				return this.GetTransition().FadeDuration;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0000DB32 File Offset: 0x0000BD32
		public virtual object Key
		{
			get
			{
				return this.GetTransition().Key;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0000DB3F File Offset: 0x0000BD3F
		public virtual FadeMode FadeMode
		{
			get
			{
				return this.GetTransition().FadeMode;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		public virtual AnimancerState CreateState()
		{
			return this.GetTransition().CreateState();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0000DB59 File Offset: 0x0000BD59
		public virtual void Apply(AnimancerState state)
		{
			this.GetTransition().Apply(state);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000DB67 File Offset: 0x0000BD67
		public virtual void GetAnimationClips(List<AnimationClip> clips)
		{
			clips.GatherFromSource(this.GetTransition());
		}

		// Token: 0x020000A8 RID: 168
		[Serializable]
		public class UnShared : AnimancerTransitionAssetBase.UnShared<AnimancerTransitionAssetBase>
		{
		}

		// Token: 0x020000A9 RID: 169
		[Serializable]
		public class UnShared<TAsset> : ITransition, IHasKey, IPolymorphic, ITransitionWithEvents, IHasEvents, IWrapper where TAsset : AnimancerTransitionAssetBase
		{
			// Token: 0x17000195 RID: 405
			// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00012516 File Offset: 0x00010716
			// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0001251E File Offset: 0x0001071E
			public TAsset Asset
			{
				get
				{
					return this._Asset;
				}
				set
				{
					this._Asset = value;
					this.BaseState = null;
					this.ClearCachedEvents();
				}
			}

			// Token: 0x17000196 RID: 406
			// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00012534 File Offset: 0x00010734
			object IWrapper.WrappedObject
			{
				get
				{
					return this._Asset;
				}
			}

			// Token: 0x17000197 RID: 407
			// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00012541 File Offset: 0x00010741
			public ITransition BaseTransition
			{
				get
				{
					return this._Asset.GetTransition();
				}
			}

			// Token: 0x17000198 RID: 408
			// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00012553 File Offset: 0x00010753
			public virtual bool IsValid
			{
				get
				{
					return this._Asset.IsValid();
				}
			}

			// Token: 0x17000199 RID: 409
			// (get) Token: 0x060006FA RID: 1786 RVA: 0x00012565 File Offset: 0x00010765
			public bool HasAsset
			{
				get
				{
					return this._Asset != null;
				}
			}

			// Token: 0x060006FB RID: 1787 RVA: 0x00012578 File Offset: 0x00010778
			[Conditional("UNITY_ASSERTIONS")]
			private void AssertAsset()
			{
				if (this._Asset == null)
				{
					UnityEngine.Debug.LogError(base.GetType().Name + ".Asset is not assigned. HasAsset can be used to check without triggering this error.");
				}
			}

			// Token: 0x1700019A RID: 410
			// (get) Token: 0x060006FC RID: 1788 RVA: 0x000125A7 File Offset: 0x000107A7
			// (set) Token: 0x060006FD RID: 1789 RVA: 0x000125AF File Offset: 0x000107AF
			public AnimancerState BaseState
			{
				get
				{
					return this._BaseState;
				}
				protected set
				{
					this._BaseState = value;
					this.OnSetBaseState();
				}
			}

			// Token: 0x060006FE RID: 1790 RVA: 0x000125BE File Offset: 0x000107BE
			protected virtual void OnSetBaseState()
			{
			}

			// Token: 0x1700019B RID: 411
			// (get) Token: 0x060006FF RID: 1791 RVA: 0x000125C0 File Offset: 0x000107C0
			public unsafe virtual AnimancerEvent.Sequence Events
			{
				get
				{
					if (this._Events == null)
					{
						this._Events = new AnimancerEvent.Sequence(this.SerializedEvents->GetEventsOptional());
					}
					return this._Events;
				}
			}

			// Token: 0x1700019C RID: 412
			// (get) Token: 0x06000700 RID: 1792 RVA: 0x000125E7 File Offset: 0x000107E7
			public virtual ref AnimancerEvent.Sequence.Serializable SerializedEvents
			{
				get
				{
					return ((ITransitionWithEvents)this._Asset.GetTransition()).SerializedEvents;
				}
			}

			// Token: 0x06000701 RID: 1793 RVA: 0x00012603 File Offset: 0x00010803
			public void ClearCachedEvents()
			{
				this._Events = null;
			}

			// Token: 0x06000702 RID: 1794 RVA: 0x0001260C File Offset: 0x0001080C
			public unsafe virtual void Apply(AnimancerState state)
			{
				this.BaseState = state;
				this._Asset.Apply(state);
				if (this._Events == null)
				{
					this._Events = this.SerializedEvents->GetEventsOptional();
					if (this._Events == null)
					{
						return;
					}
					this._Events = new AnimancerEvent.Sequence(this._Events);
				}
				state.Events = this._Events;
			}

			// Token: 0x1700019D RID: 413
			// (get) Token: 0x06000703 RID: 1795 RVA: 0x00012671 File Offset: 0x00010871
			public virtual object Key
			{
				get
				{
					return this._Asset.Key;
				}
			}

			// Token: 0x1700019E RID: 414
			// (get) Token: 0x06000704 RID: 1796 RVA: 0x00012683 File Offset: 0x00010883
			public virtual float FadeDuration
			{
				get
				{
					return this._Asset.FadeDuration;
				}
			}

			// Token: 0x1700019F RID: 415
			// (get) Token: 0x06000705 RID: 1797 RVA: 0x00012695 File Offset: 0x00010895
			public virtual FadeMode FadeMode
			{
				get
				{
					return this._Asset.FadeMode;
				}
			}

			// Token: 0x06000706 RID: 1798 RVA: 0x000126A8 File Offset: 0x000108A8
			AnimancerState ITransition.CreateState()
			{
				return this.BaseState = this._Asset.CreateState();
			}

			// Token: 0x04000188 RID: 392
			[SerializeField]
			private TAsset _Asset;

			// Token: 0x04000189 RID: 393
			private AnimancerState _BaseState;

			// Token: 0x0400018A RID: 394
			private AnimancerEvent.Sequence _Events;
		}

		// Token: 0x020000AA RID: 170
		[Serializable]
		public class UnShared<TAsset, TTransition, TState> : AnimancerTransitionAssetBase.UnShared<TAsset>, ITransition<TState>, ITransition, IHasKey, IPolymorphic where TAsset : AnimancerTransitionAsset<TTransition> where TTransition : ITransition<TState>, IHasEvents where TState : AnimancerState
		{
			// Token: 0x170001A0 RID: 416
			// (get) Token: 0x06000708 RID: 1800 RVA: 0x000126D6 File Offset: 0x000108D6
			// (set) Token: 0x06000709 RID: 1801 RVA: 0x000126E8 File Offset: 0x000108E8
			public TTransition Transition
			{
				get
				{
					return base.Asset.Transition;
				}
				set
				{
					base.Asset.Transition = value;
				}
			}

			// Token: 0x0600070A RID: 1802 RVA: 0x000126FB File Offset: 0x000108FB
			protected override void OnSetBaseState()
			{
				base.OnSetBaseState();
				if (this._State != base.BaseState)
				{
					this._State = default(TState);
				}
			}

			// Token: 0x170001A1 RID: 417
			// (get) Token: 0x0600070B RID: 1803 RVA: 0x00012722 File Offset: 0x00010922
			// (set) Token: 0x0600070C RID: 1804 RVA: 0x00012748 File Offset: 0x00010948
			public TState State
			{
				get
				{
					if (this._State == null)
					{
						this._State = (TState)((object)base.BaseState);
					}
					return this._State;
				}
				protected set
				{
					this._State = value;
					base.BaseState = value;
				}
			}

			// Token: 0x170001A2 RID: 418
			// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001276C File Offset: 0x0001096C
			public override ref AnimancerEvent.Sequence.Serializable SerializedEvents
			{
				get
				{
					TTransition transition = base.Asset.Transition;
					return transition.SerializedEvents;
				}
			}

			// Token: 0x0600070E RID: 1806 RVA: 0x00012798 File Offset: 0x00010998
			public virtual TState CreateState()
			{
				return this.State = (TState)((object)base.Asset.CreateState());
			}

			// Token: 0x0400018B RID: 395
			private TState _State;
		}
	}
}
