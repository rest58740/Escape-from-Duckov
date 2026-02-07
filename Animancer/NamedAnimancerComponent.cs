using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200003B RID: 59
	[AddComponentMenu("Animancer/Named Animancer Component")]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/NamedAnimancerComponent")]
	public class NamedAnimancerComponent : AnimancerComponent
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000B4BD File Offset: 0x000096BD
		public ref bool PlayAutomatically
		{
			get
			{
				return ref this._PlayAutomatically;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000B4C5 File Offset: 0x000096C5
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0000B4CD File Offset: 0x000096CD
		public AnimationClip[] Animations
		{
			get
			{
				return this._Animations;
			}
			set
			{
				this._Animations = value;
				base.States.CreateIfNew(value);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0000B4E2 File Offset: 0x000096E2
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0000B4FB File Offset: 0x000096FB
		public AnimationClip DefaultAnimation
		{
			get
			{
				if (!this._Animations.IsNullOrEmpty<AnimationClip>())
				{
					return this._Animations[0];
				}
				return null;
			}
			set
			{
				if (this._Animations.IsNullOrEmpty<AnimationClip>())
				{
					this._Animations = new AnimationClip[]
					{
						value
					};
					return;
				}
				this._Animations[0] = value;
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000B524 File Offset: 0x00009724
		protected virtual void Awake()
		{
			if (!base.TryGetAnimator())
			{
				return;
			}
			base.States.CreateIfNew(this._Animations);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000B540 File Offset: 0x00009740
		protected override void OnEnable()
		{
			if (!base.TryGetAnimator())
			{
				return;
			}
			base.OnEnable();
			if (this._PlayAutomatically && !this._Animations.IsNullOrEmpty<AnimationClip>())
			{
				AnimationClip animationClip = this._Animations[0];
				if (animationClip != null)
				{
					base.Play(animationClip);
				}
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000B58B File Offset: 0x0000978B
		public override object GetKey(AnimationClip clip)
		{
			return clip.name;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000B593 File Offset: 0x00009793
		public override void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			base.GatherAnimationClips(clips);
			clips.Gather(this._Animations);
		}

		// Token: 0x040000A6 RID: 166
		[SerializeField]
		[Tooltip("If true, the 'Default Animation' will be automatically played by OnEnable")]
		private bool _PlayAutomatically = true;

		// Token: 0x040000A7 RID: 167
		[SerializeField]
		[Tooltip("Animations in this array will be automatically registered by Awake as states that can be retrieved using their name")]
		private AnimationClip[] _Animations;
	}
}
