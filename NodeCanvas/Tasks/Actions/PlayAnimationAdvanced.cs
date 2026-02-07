using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000059 RID: 89
	[Category("Animation")]
	public class PlayAnimationAdvanced : ActionTask<Animation>
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00007DE0 File Offset: 0x00005FE0
		protected override string info
		{
			get
			{
				return "Anim " + this.animationClip.ToString();
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00007DF7 File Offset: 0x00005FF7
		protected override string OnInit()
		{
			base.agent.AddClip(this.animationClip.value, this.animationClip.value.name);
			this.animationClip.value.legacy = true;
			return null;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00007E34 File Offset: 0x00006034
		protected override void OnExecute()
		{
			if (this.playDirection == PlayDirections.Toggle)
			{
				this.dir = -this.dir;
			}
			if (this.playDirection == PlayDirections.Backward)
			{
				this.dir = -1;
			}
			if (this.playDirection == PlayDirections.Forward)
			{
				this.dir = 1;
			}
			base.agent.AddClip(this.animationClip.value, this.animationClip.value.name);
			this.animationToPlay = this.animationClip.value.name;
			if (!string.IsNullOrEmpty(this.mixTransformName.value))
			{
				this.mixTransform = this.FindTransform(base.agent.transform, this.mixTransformName.value);
				if (!this.mixTransform)
				{
				}
			}
			else
			{
				this.mixTransform = null;
			}
			this.animationToPlay = this.animationClip.value.name;
			if (this.mixTransform)
			{
				base.agent[this.animationToPlay].AddMixingTransform(this.mixTransform, true);
			}
			base.agent[this.animationToPlay].layer = this.animationLayer.value;
			base.agent[this.animationToPlay].speed = (float)this.dir * this.playbackSpeed;
			base.agent[this.animationToPlay].normalizedTime = Mathf.Clamp01((float)(-(float)this.dir));
			base.agent[this.animationToPlay].wrapMode = this.animationWrap;
			base.agent[this.animationToPlay].blendMode = this.blendMode;
			if (this.queueAnimation)
			{
				base.agent.CrossFadeQueued(this.animationToPlay, this.crossFadeTime);
			}
			else
			{
				base.agent.CrossFade(this.animationToPlay, this.crossFadeTime);
			}
			if (!this.waitActionFinish)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008024 File Offset: 0x00006224
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= base.agent[this.animationToPlay].length / this.playbackSpeed - this.crossFadeTime)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000805C File Offset: 0x0000625C
		private Transform FindTransform(Transform parent, string name)
		{
			if (parent.name == name)
			{
				return parent;
			}
			foreach (Transform transform in parent.GetComponentsInChildren<Transform>())
			{
				if (transform.name == name)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x04000106 RID: 262
		[RequiredField]
		public BBParameter<AnimationClip> animationClip;

		// Token: 0x04000107 RID: 263
		public WrapMode animationWrap;

		// Token: 0x04000108 RID: 264
		public AnimationBlendMode blendMode;

		// Token: 0x04000109 RID: 265
		[SliderField(0, 2)]
		public float playbackSpeed = 1f;

		// Token: 0x0400010A RID: 266
		[SliderField(0, 1)]
		public float crossFadeTime = 0.25f;

		// Token: 0x0400010B RID: 267
		public PlayDirections playDirection;

		// Token: 0x0400010C RID: 268
		public BBParameter<string> mixTransformName;

		// Token: 0x0400010D RID: 269
		public BBParameter<int> animationLayer;

		// Token: 0x0400010E RID: 270
		public bool queueAnimation;

		// Token: 0x0400010F RID: 271
		public bool waitActionFinish = true;

		// Token: 0x04000110 RID: 272
		private string animationToPlay = string.Empty;

		// Token: 0x04000111 RID: 273
		private int dir = -1;

		// Token: 0x04000112 RID: 274
		private Transform mixTransform;
	}
}
