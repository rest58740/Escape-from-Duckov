using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000017 RID: 23
	public class ClipState : AnimancerState
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x000085A6 File Offset: 0x000067A6
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x000085AE File Offset: 0x000067AE
		public override AnimationClip Clip
		{
			get
			{
				return this._Clip;
			}
			set
			{
				base.ChangeMainObject<AnimationClip>(ref this._Clip, value);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x000085BD File Offset: 0x000067BD
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x000085C5 File Offset: 0x000067C5
		public override UnityEngine.Object MainObject
		{
			get
			{
				return this._Clip;
			}
			set
			{
				this.Clip = (AnimationClip)value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002DA RID: 730 RVA: 0x000085D3 File Offset: 0x000067D3
		public override float Length
		{
			get
			{
				return this._Clip.length;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002DB RID: 731 RVA: 0x000085E0 File Offset: 0x000067E0
		public override bool IsLooping
		{
			get
			{
				return this._Clip.isLooping;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002DC RID: 732 RVA: 0x000085ED File Offset: 0x000067ED
		public override Vector3 AverageVelocity
		{
			get
			{
				return this._Clip.averageSpeed;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002DD RID: 733 RVA: 0x000085FC File Offset: 0x000067FC
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000862C File Offset: 0x0000682C
		public override bool ApplyAnimatorIK
		{
			get
			{
				return this._Playable.IsValid<Playable>() && ((AnimationClipPlayable)this._Playable).GetApplyPlayableIK();
			}
			set
			{
				((AnimationClipPlayable)this._Playable).SetApplyPlayableIK(value);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00008650 File Offset: 0x00006850
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x00008680 File Offset: 0x00006880
		public override bool ApplyFootIK
		{
			get
			{
				return this._Playable.IsValid<Playable>() && ((AnimationClipPlayable)this._Playable).GetApplyFootIK();
			}
			set
			{
				((AnimationClipPlayable)this._Playable).SetApplyFootIK(value);
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000086A1 File Offset: 0x000068A1
		public ClipState(AnimationClip clip)
		{
			this._Clip = clip;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000086B0 File Offset: 0x000068B0
		protected override void CreatePlayable(out Playable playable)
		{
			playable = AnimationClipPlayable.Create(base.Root._Graph, this._Clip);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000086D3 File Offset: 0x000068D3
		public override void Destroy()
		{
			this._Clip = null;
			base.Destroy();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000086E2 File Offset: 0x000068E2
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			ClipState clipState = new ClipState(this._Clip);
			clipState.SetNewCloneRoot(root);
			((ICopyable<AnimancerState>)clipState).CopyFrom(this);
			return clipState;
		}

		// Token: 0x04000057 RID: 87
		private AnimationClip _Clip;

		// Token: 0x0200008B RID: 139
		public interface ITransition : ITransition<ClipState>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
