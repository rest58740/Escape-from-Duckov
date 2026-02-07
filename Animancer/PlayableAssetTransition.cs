using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	public class PlayableAssetTransition : AnimancerTransition<PlayableAssetState>, PlayableAssetState.ITransition, ITransition<PlayableAssetState>, ITransition, IHasKey, IPolymorphic, IAnimationClipCollection, ICopyable<PlayableAssetTransition>
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0000ECDE File Offset: 0x0000CEDE
		public ref PlayableAsset Asset
		{
			get
			{
				return ref this._Asset;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0000ECE6 File Offset: 0x0000CEE6
		public override UnityEngine.Object MainObject
		{
			get
			{
				return this._Asset;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0000ECEE File Offset: 0x0000CEEE
		public override object Key
		{
			get
			{
				return this._Asset;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0000ECF6 File Offset: 0x0000CEF6
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0000ECFE File Offset: 0x0000CEFE
		public override float Speed
		{
			get
			{
				return this._Speed;
			}
			set
			{
				this._Speed = value;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0000ED07 File Offset: 0x0000CF07
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x0000ED0F File Offset: 0x0000CF0F
		public override float NormalizedStartTime
		{
			get
			{
				return this._NormalizedStartTime;
			}
			set
			{
				this._NormalizedStartTime = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0000ED18 File Offset: 0x0000CF18
		public ref UnityEngine.Object[] Bindings
		{
			get
			{
				return ref this._Bindings;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0000ED20 File Offset: 0x0000CF20
		public override float MaximumDuration
		{
			get
			{
				if (!(this._Asset != null))
				{
					return 0f;
				}
				return (float)this._Asset.duration;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0000ED42 File Offset: 0x0000CF42
		public override bool IsValid
		{
			get
			{
				return this._Asset != null;
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000ED50 File Offset: 0x0000CF50
		public override PlayableAssetState CreateState()
		{
			base.State = new PlayableAssetState(this._Asset);
			base.State.SetBindings(this._Bindings);
			return base.State;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000ED7A File Offset: 0x0000CF7A
		public override void Apply(AnimancerState state)
		{
			AnimancerTransition<PlayableAssetState>.ApplyDetails(state, this._Speed, this._NormalizedStartTime);
			base.Apply(state);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0000ED95 File Offset: 0x0000CF95
		void IAnimationClipCollection.GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			clips.GatherFromAsset(this._Asset);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000EDA4 File Offset: 0x0000CFA4
		public virtual void CopyFrom(PlayableAssetTransition copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._Asset = null;
				this._Speed = 1f;
				this._NormalizedStartTime = float.NaN;
				this._Bindings = null;
				return;
			}
			this._Asset = copyFrom._Asset;
			this._Speed = copyFrom._Speed;
			this._NormalizedStartTime = copyFrom._NormalizedStartTime;
			AnimancerUtilities.CopyExactArray<UnityEngine.Object>(copyFrom._Bindings, ref this._Bindings);
		}

		// Token: 0x040000F7 RID: 247
		[SerializeField]
		[Tooltip("The asset to play")]
		private PlayableAsset _Asset;

		// Token: 0x040000F8 RID: 248
		[SerializeField]
		[Tooltip("How fast the animation will play, e.g:\n• 0x = paused\n• 1x = normal speed\n• -2x = double speed backwards\n• Disabled = keep previous speed\n• Middle Click = reset to default value")]
		private float _Speed = 1f;

		// Token: 0x040000F9 RID: 249
		[SerializeField]
		[Tooltip("• Enabled = use FadeMode.FromStart and always restart at this time.\n• Disabled = use FadeMode.FixedSpeed and continue from the current time if already playing.\n• x = Normalized, s = Seconds, f = Frame")]
		private float _NormalizedStartTime = float.NaN;

		// Token: 0x040000FA RID: 250
		[SerializeField]
		[Tooltip("The objects controlled by each of the tracks in the Asset")]
		[NonReorderable]
		private UnityEngine.Object[] _Bindings;
	}
}
