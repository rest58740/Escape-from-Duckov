using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Animancer
{
	// Token: 0x02000067 RID: 103
	[Serializable]
	public abstract class ManualMixerTransition<TMixer> : AnimancerTransition<TMixer>, IMotion, IAnimationClipCollection, ICopyable<ManualMixerTransition<TMixer>> where TMixer : ManualMixerState
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0000E7F6 File Offset: 0x0000C9F6
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x0000E7FE File Offset: 0x0000C9FE
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

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0000E807 File Offset: 0x0000CA07
		public ref UnityEngine.Object[] Animations
		{
			get
			{
				return ref this._Animations;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0000E80F File Offset: 0x0000CA0F
		public ref float[] Speeds
		{
			get
			{
				return ref this._Speeds;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0000E817 File Offset: 0x0000CA17
		public bool HasSpeeds
		{
			get
			{
				return this._Speeds != null && this._Speeds.Length >= this._Animations.Length;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0000E838 File Offset: 0x0000CA38
		public ref bool[] SynchronizeChildren
		{
			get
			{
				return ref this._SynchronizeChildren;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0000E840 File Offset: 0x0000CA40
		public override bool IsLooping
		{
			get
			{
				for (int i = this._Animations.Length - 1; i >= 0; i--)
				{
					bool flag;
					if (AnimancerUtilities.TryGetIsLooping(this._Animations[i], out flag) && flag)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0000E878 File Offset: 0x0000CA78
		public override float MaximumDuration
		{
			get
			{
				if (this._Animations == null)
				{
					return 0f;
				}
				float num = 0f;
				bool hasSpeeds = this.HasSpeeds;
				for (int i = this._Animations.Length - 1; i >= 0; i--)
				{
					float num2;
					if (AnimancerUtilities.TryGetLength(this._Animations[i], out num2))
					{
						if (hasSpeeds)
						{
							num2 *= this._Speeds[i];
						}
						if (num < num2)
						{
							num = num2;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000E8DC File Offset: 0x0000CADC
		public virtual float AverageAngularSpeed
		{
			get
			{
				if (this._Animations == null)
				{
					return 0f;
				}
				float num = 0f;
				bool hasSpeeds = this.HasSpeeds;
				int num2 = 0;
				for (int i = this._Animations.Length - 1; i >= 0; i--)
				{
					float num3;
					if (AnimancerUtilities.TryGetAverageAngularSpeed(this._Animations[i], out num3))
					{
						if (hasSpeeds)
						{
							num3 *= this._Speeds[i];
						}
						num += num3;
						num2++;
					}
				}
				return num / (float)num2;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0000E94C File Offset: 0x0000CB4C
		public virtual Vector3 AverageVelocity
		{
			get
			{
				if (this._Animations == null)
				{
					return default(Vector3);
				}
				Vector3 a = default(Vector3);
				bool hasSpeeds = this.HasSpeeds;
				int num = 0;
				for (int i = this._Animations.Length - 1; i >= 0; i--)
				{
					Vector3 vector;
					if (AnimancerUtilities.TryGetAverageVelocity(this._Animations[i], out vector))
					{
						if (hasSpeeds)
						{
							vector *= this._Speeds[i];
						}
						a += vector;
						num++;
					}
				}
				return a / (float)num;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0000E9D4 File Offset: 0x0000CBD4
		public override bool IsValid
		{
			get
			{
				if (this._Animations == null || this._Animations.Length == 0)
				{
					return false;
				}
				for (int i = this._Animations.Length - 1; i >= 0; i--)
				{
					if (this._Animations[i] == null)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0000EA1C File Offset: 0x0000CC1C
		public virtual void InitializeState()
		{
			TMixer state = base.State;
			int childCount = state.ChildCount;
			bool synchronizeNewChildren = ManualMixerState.SynchronizeNewChildren;
			try
			{
				ManualMixerState.SynchronizeNewChildren = false;
				ManualMixerState manualMixerState = state;
				object[] animations = this._Animations;
				manualMixerState.AddRange(animations);
			}
			finally
			{
				ManualMixerState.SynchronizeNewChildren = synchronizeNewChildren;
			}
			state.InitializeSynchronizedChildren(this._SynchronizeChildren);
			if (this._Speeds != null)
			{
				for (int i = Math.Min(this._Animations.Length, this._Speeds.Length) - 1; i >= 0; i--)
				{
					state.GetChild(childCount + i).Speed = this._Speeds[i];
				}
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0000EAD0 File Offset: 0x0000CCD0
		public override void Apply(AnimancerState state)
		{
			base.Apply(state);
			if (!float.IsNaN(this._Speed))
			{
				state.Speed = this._Speed;
			}
			for (int i = 0; i < this._Animations.Length; i++)
			{
				ITransition transition = this._Animations[i] as ITransition;
				if (transition != null)
				{
					transition.Apply(state.GetChild(i));
				}
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000EB2E File Offset: 0x0000CD2E
		void IAnimationClipCollection.GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			clips.GatherFromSource(this._Animations);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0000EB3C File Offset: 0x0000CD3C
		public virtual void CopyFrom(ManualMixerTransition<TMixer> copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._Speed = 1f;
				this._Animations = null;
				this._Speeds = null;
				this._SynchronizeChildren = null;
				return;
			}
			this._Speed = copyFrom._Speed;
			AnimancerUtilities.CopyExactArray<UnityEngine.Object>(copyFrom._Animations, ref this._Animations);
			AnimancerUtilities.CopyExactArray<float>(copyFrom._Speeds, ref this._Speeds);
			AnimancerUtilities.CopyExactArray<bool>(copyFrom._SynchronizeChildren, ref this._SynchronizeChildren);
		}

		// Token: 0x040000EB RID: 235
		[SerializeField]
		[Tooltip("How fast the animation will play, e.g:\n• 0x = paused\n• 1x = normal speed\n• -2x = double speed backwards\n• Disabled = keep previous speed\n• Middle Click = reset to default value")]
		private float _Speed = 1f;

		// Token: 0x040000EC RID: 236
		[SerializeField]
		[FormerlySerializedAs("_Clips")]
		[FormerlySerializedAs("_States")]
		private UnityEngine.Object[] _Animations;

		// Token: 0x040000ED RID: 237
		public const string AnimationsField = "_Animations";

		// Token: 0x040000EE RID: 238
		[SerializeField]
		private float[] _Speeds;

		// Token: 0x040000EF RID: 239
		public const string SpeedsField = "_Speeds";

		// Token: 0x040000F0 RID: 240
		[SerializeField]
		private bool[] _SynchronizeChildren;

		// Token: 0x040000F1 RID: 241
		public const string SynchronizeChildrenField = "_SynchronizeChildren";
	}
}
