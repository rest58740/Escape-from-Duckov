using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x0200001A RID: 26
	[AddComponentMenu("Animancer/Solo Animation")]
	[DefaultExecutionOrder(-5000)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/SoloAnimation")]
	public class SoloAnimation : MonoBehaviour, IAnimationClipSource
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00008BC4 File Offset: 0x00006DC4
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00008BCC File Offset: 0x00006DCC
		public Animator Animator
		{
			get
			{
				return this._Animator;
			}
			set
			{
				this._Animator = value;
				if (this.IsInitialized)
				{
					this.Play();
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00008BE3 File Offset: 0x00006DE3
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00008BEB File Offset: 0x00006DEB
		public AnimationClip Clip
		{
			get
			{
				return this._Clip;
			}
			set
			{
				this._Clip = value;
				if (this.IsInitialized)
				{
					this.Play();
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00008C02 File Offset: 0x00006E02
		// (set) Token: 0x06000300 RID: 768 RVA: 0x00008C12 File Offset: 0x00006E12
		public bool StopOnDisable
		{
			get
			{
				return !this._Animator.keepAnimatorStateOnDisable;
			}
			set
			{
				this._Animator.keepAnimatorStateOnDisable = !value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00008C23 File Offset: 0x00006E23
		// (set) Token: 0x06000302 RID: 770 RVA: 0x00008C2B File Offset: 0x00006E2B
		public bool IsPlaying
		{
			get
			{
				return this._IsPlaying;
			}
			set
			{
				this._IsPlaying = value;
				if (!value)
				{
					if (this.IsInitialized)
					{
						this._Graph.Stop();
					}
					return;
				}
				if (!this.IsInitialized)
				{
					this.Play();
					return;
				}
				this._Graph.Play();
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00008C65 File Offset: 0x00006E65
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00008C6D File Offset: 0x00006E6D
		public float Speed
		{
			get
			{
				return this._Speed;
			}
			set
			{
				this._Speed = value;
				this._Playable.SetSpeed((double)value);
				this.IsPlaying = (value != 0f);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00008C94 File Offset: 0x00006E94
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00008C9C File Offset: 0x00006E9C
		public bool FootIK
		{
			get
			{
				return this._FootIK;
			}
			set
			{
				this._FootIK = value;
				this._Playable.SetApplyFootIK(value);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00008CB1 File Offset: 0x00006EB1
		// (set) Token: 0x06000308 RID: 776 RVA: 0x00008CBF File Offset: 0x00006EBF
		public float Time
		{
			get
			{
				return (float)this._Playable.GetTime<AnimationClipPlayable>();
			}
			set
			{
				this._Playable.SetTime((double)value);
				this._Playable.SetTime((double)value);
				this.IsPlaying = true;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00008CE2 File Offset: 0x00006EE2
		// (set) Token: 0x0600030A RID: 778 RVA: 0x00008CF6 File Offset: 0x00006EF6
		public float NormalizedTime
		{
			get
			{
				return this.Time / this._Clip.length;
			}
			set
			{
				this.Time = value * this._Clip.length;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00008D0B File Offset: 0x00006F0B
		public bool IsInitialized
		{
			get
			{
				return this._Graph.IsValid();
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00008D18 File Offset: 0x00006F18
		public void Play()
		{
			this.Play(this._Clip);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00008D28 File Offset: 0x00006F28
		public void Play(AnimationClip clip)
		{
			if (clip == null || this._Animator == null)
			{
				return;
			}
			if (this._Graph.IsValid())
			{
				this._Graph.Destroy();
			}
			this._Playable = AnimationPlayableUtilities.PlayClip(this._Animator, clip, out this._Graph);
			this._Playable.SetSpeed((double)this._Speed);
			if (!this._FootIK)
			{
				this._Playable.SetApplyFootIK(false);
			}
			if (!clip.isLooping)
			{
				this._Playable.SetDuration((double)clip.length);
			}
			this._IsPlaying = true;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00008DC4 File Offset: 0x00006FC4
		protected virtual void OnEnable()
		{
			this.IsPlaying = true;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00008DD0 File Offset: 0x00006FD0
		protected virtual void Update()
		{
			if (!this.IsPlaying)
			{
				return;
			}
			if (this._Graph.IsDone())
			{
				this.IsPlaying = false;
				return;
			}
			if (this._Speed < 0f && this.Time <= 0f)
			{
				this.IsPlaying = false;
				this.Time = 0f;
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00008E28 File Offset: 0x00007028
		protected virtual void OnDisable()
		{
			this.IsPlaying = false;
			if (this.IsInitialized && this.StopOnDisable)
			{
				this._Playable.SetTime(0.0);
				this._Playable.SetTime(0.0);
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00008E74 File Offset: 0x00007074
		protected virtual void OnDestroy()
		{
			if (this.IsInitialized)
			{
				this._Graph.Destroy();
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00008E89 File Offset: 0x00007089
		public void GetAnimationClips(List<AnimationClip> clips)
		{
			if (this._Clip != null)
			{
				clips.Add(this._Clip);
			}
		}

		// Token: 0x04000063 RID: 99
		public const int DefaultExecutionOrder = -5000;

		// Token: 0x04000064 RID: 100
		[SerializeField]
		[Tooltip("The Animator component which this script controls")]
		private Animator _Animator;

		// Token: 0x04000065 RID: 101
		[SerializeField]
		[Tooltip("The animation that will be played")]
		private AnimationClip _Clip;

		// Token: 0x04000066 RID: 102
		private PlayableGraph _Graph;

		// Token: 0x04000067 RID: 103
		private AnimationClipPlayable _Playable;

		// Token: 0x04000068 RID: 104
		private bool _IsPlaying;

		// Token: 0x04000069 RID: 105
		[SerializeField]
		[Tooltip("The speed at which the animation plays (default 1)")]
		private float _Speed = 1f;

		// Token: 0x0400006A RID: 106
		[SerializeField]
		[Tooltip("Determines whether Foot IK will be applied to the model (if it is Humanoid)")]
		private bool _FootIK;
	}
}
