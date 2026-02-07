using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200005A RID: 90
	[Category("Animation")]
	public class PlayAnimationSimple : ActionTask<Animation>
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000080DA File Offset: 0x000062DA
		protected override string info
		{
			get
			{
				return "Anim " + this.animationClip.ToString();
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000080F1 File Offset: 0x000062F1
		protected override string OnInit()
		{
			base.agent.AddClip(this.animationClip.value, this.animationClip.value.name);
			this.animationClip.value.legacy = true;
			return null;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000812C File Offset: 0x0000632C
		protected override void OnExecute()
		{
			AnimationClip x = null;
			if (PlayAnimationSimple.lastPlayedClips.TryGetValue(base.agent, ref x) && x == this.animationClip.value)
			{
				base.EndAction(true);
				return;
			}
			PlayAnimationSimple.lastPlayedClips[base.agent] = this.animationClip.value;
			base.agent[this.animationClip.value.name].wrapMode = this.animationWrap;
			base.agent.CrossFade(this.animationClip.value.name, this.crossFadeTime);
			if (!this.waitActionFinish)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000081DB File Offset: 0x000063DB
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.animationClip.value.length - this.crossFadeTime)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x04000113 RID: 275
		[RequiredField]
		public BBParameter<AnimationClip> animationClip;

		// Token: 0x04000114 RID: 276
		[SliderField(0, 1)]
		public float crossFadeTime = 0.25f;

		// Token: 0x04000115 RID: 277
		public WrapMode animationWrap = WrapMode.Loop;

		// Token: 0x04000116 RID: 278
		public bool waitActionFinish = true;

		// Token: 0x04000117 RID: 279
		private static Dictionary<Animation, AnimationClip> lastPlayedClips = new Dictionary<Animation, AnimationClip>();
	}
}
