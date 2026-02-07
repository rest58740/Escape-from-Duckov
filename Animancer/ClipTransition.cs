using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	public class ClipTransition : AnimancerTransition<ClipState>, ClipState.ITransition, ITransition<ClipState>, ITransition, IHasKey, IPolymorphic, IMotion, IAnimationClipCollection, ICopyable<ClipTransition>
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x0000DBE6 File Offset: 0x0000BDE6
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x0000DBEE File Offset: 0x0000BDEE
		public AnimationClip Clip
		{
			get
			{
				return this._Clip;
			}
			set
			{
				this._Clip = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0000DBF7 File Offset: 0x0000BDF7
		public override UnityEngine.Object MainObject
		{
			get
			{
				return this._Clip;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0000DBFF File Offset: 0x0000BDFF
		public override object Key
		{
			get
			{
				return this._Clip;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0000DC07 File Offset: 0x0000BE07
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x0000DC0F File Offset: 0x0000BE0F
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

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0000DC18 File Offset: 0x0000BE18
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x0000DC20 File Offset: 0x0000BE20
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

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0000DC29 File Offset: 0x0000BE29
		public override FadeMode FadeMode
		{
			get
			{
				if (!float.IsNaN(this._NormalizedStartTime))
				{
					return FadeMode.FromStart;
				}
				return FadeMode.FixedSpeed;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0000DC3C File Offset: 0x0000BE3C
		public virtual float Length
		{
			get
			{
				if (!this.IsValid)
				{
					return 0f;
				}
				float num = base.Events.NormalizedEndTime;
				num = ((!float.IsNaN(num)) ? num : AnimancerEvent.Sequence.GetDefaultNormalizedEndTime(this._Speed));
				float num2 = (!float.IsNaN(this._NormalizedStartTime)) ? this._NormalizedStartTime : AnimancerEvent.Sequence.GetDefaultNormalizedStartTime(this._Speed);
				return this._Clip.length * (num - num2);
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0000DCAA File Offset: 0x0000BEAA
		public override bool IsValid
		{
			get
			{
				return this._Clip != null && !this._Clip.legacy;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0000DCCA File Offset: 0x0000BECA
		public override bool IsLooping
		{
			get
			{
				return this._Clip != null && this._Clip.isLooping;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0000DCE7 File Offset: 0x0000BEE7
		public override float MaximumDuration
		{
			get
			{
				if (!(this._Clip != null))
				{
					return 0f;
				}
				return this._Clip.length;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0000DD08 File Offset: 0x0000BF08
		public virtual float AverageAngularSpeed
		{
			get
			{
				if (!(this._Clip != null))
				{
					return 0f;
				}
				return this._Clip.averageAngularSpeed;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0000DD2C File Offset: 0x0000BF2C
		public virtual Vector3 AverageVelocity
		{
			get
			{
				if (!(this._Clip != null))
				{
					return default(Vector3);
				}
				return this._Clip.averageSpeed;
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000DD5C File Offset: 0x0000BF5C
		public override ClipState CreateState()
		{
			return base.State = new ClipState(this._Clip);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0000DD7D File Offset: 0x0000BF7D
		public override void Apply(AnimancerState state)
		{
			AnimancerTransition<ClipState>.ApplyDetails(state, this._Speed, this._NormalizedStartTime);
			base.Apply(state);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000DD98 File Offset: 0x0000BF98
		public virtual void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			clips.Gather(this._Clip);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000DDA8 File Offset: 0x0000BFA8
		public virtual void CopyFrom(ClipTransition copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._Clip = null;
				this._Speed = 1f;
				this._NormalizedStartTime = float.NaN;
				return;
			}
			this._Clip = copyFrom._Clip;
			this._Speed = copyFrom._Speed;
			this._NormalizedStartTime = copyFrom._NormalizedStartTime;
		}

		// Token: 0x040000DC RID: 220
		public const string ClipFieldName = "_Clip";

		// Token: 0x040000DD RID: 221
		[SerializeField]
		[Tooltip("The animation to play")]
		private AnimationClip _Clip;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		[Tooltip("How fast the animation will play, e.g:\n• 0x = paused\n• 1x = normal speed\n• -2x = double speed backwards\n• Disabled = keep previous speed\n• Middle Click = reset to default value")]
		private float _Speed = 1f;

		// Token: 0x040000DF RID: 223
		[SerializeField]
		[Tooltip("• Enabled = use FadeMode.FromStart and always restart at this time.\n• Disabled = use FadeMode.FixedSpeed and continue from the current time if already playing.\n• x = Normalized, s = Seconds, f = Frame")]
		private float _NormalizedStartTime = float.NaN;
	}
}
