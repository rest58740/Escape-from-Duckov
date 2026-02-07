using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000059 RID: 89
	[Serializable]
	public class ClipTransitionSequence : ClipTransition, ISerializationCallbackReceiver, ICopyable<ClipTransitionSequence>
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0000DE1F File Offset: 0x0000C01F
		public ref ClipTransition[] Others
		{
			get
			{
				return ref this._Others;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0000DE27 File Offset: 0x0000C027
		public ClipTransition LastTransition
		{
			get
			{
				if (this._Others.Length == 0)
				{
					return this;
				}
				return this._Others[this._Others.Length - 1];
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000DE45 File Offset: 0x0000C045
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000DE48 File Offset: 0x0000C048
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this._Others.Length <= 1)
			{
				return;
			}
			ClipTransition clipTransition = this._Others[0];
			for (int i = 1; i < this._Others.Length; i++)
			{
				ClipTransition next = this._Others[i];
				clipTransition.Events.OnEnd = delegate()
				{
					AnimancerEvent.CurrentState.Layer.Play(next);
				};
				clipTransition = next;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000DEB0 File Offset: 0x0000C0B0
		public override void Apply(AnimancerState state)
		{
			if (this._Others.Length != 0)
			{
				if (this._OnEnd == null)
				{
					this._OnEnd = delegate()
					{
						AnimancerEvent.CurrentState.Layer.Play(this._Others[0]);
					};
				}
				Action action = base.Events.OnEnd;
				if (action != this._OnEnd)
				{
					base.Events.OnEnd = this._OnEnd;
					action = (Action)Delegate.Remove(action, this._OnEnd);
					this._Others[this._Others.Length - 1].Events.OnEnd = action;
				}
			}
			base.Apply(state);
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0000DF40 File Offset: 0x0000C140
		public override bool IsValid
		{
			get
			{
				if (!base.IsValid)
				{
					return false;
				}
				for (int i = 0; i < this._Others.Length; i++)
				{
					if (!this._Others[i].IsValid)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0000DF7C File Offset: 0x0000C17C
		public override bool IsLooping
		{
			get
			{
				if (this._Others.Length == 0)
				{
					return base.IsLooping;
				}
				return this.LastTransition.IsLooping;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x0000DF9C File Offset: 0x0000C19C
		public override float Length
		{
			get
			{
				float num = base.Length;
				for (int i = 0; i < this._Others.Length; i++)
				{
					num += this._Others[i].Length;
				}
				return num;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0000DFD4 File Offset: 0x0000C1D4
		public override float MaximumDuration
		{
			get
			{
				float num = base.MaximumDuration;
				for (int i = 0; i < this._Others.Length; i++)
				{
					num += this._Others[i].MaximumDuration;
				}
				return num;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0000E00C File Offset: 0x0000C20C
		public override float AverageAngularSpeed
		{
			get
			{
				float num = base.AverageAngularSpeed;
				if (this._Others.Length == 0)
				{
					return num;
				}
				float num2 = base.MaximumDuration;
				num *= num2;
				for (int i = 0; i < this._Others.Length; i++)
				{
					ClipTransition clipTransition = this._Others[i];
					float averageAngularSpeed = clipTransition.AverageAngularSpeed;
					float maximumDuration = clipTransition.MaximumDuration;
					num += averageAngularSpeed * maximumDuration;
					num2 += maximumDuration;
				}
				return num / num2;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0000E070 File Offset: 0x0000C270
		public override Vector3 AverageVelocity
		{
			get
			{
				Vector3 vector = base.AverageVelocity;
				if (this._Others.Length == 0)
				{
					return vector;
				}
				float num = base.MaximumDuration;
				vector *= num;
				for (int i = 0; i < this._Others.Length; i++)
				{
					ClipTransition clipTransition = this._Others[i];
					Vector3 averageVelocity = clipTransition.AverageVelocity;
					float maximumDuration = clipTransition.MaximumDuration;
					vector += averageVelocity * maximumDuration;
					num += maximumDuration;
				}
				return vector / num;
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
		public override void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			base.GatherAnimationClips(clips);
			for (int i = 0; i < this._Others.Length; i++)
			{
				this._Others[i].GatherAnimationClips(clips);
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000E119 File Offset: 0x0000C319
		public virtual void CopyFrom(ClipTransitionSequence copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._Others = Array.Empty<ClipTransition>();
				return;
			}
			AnimancerUtilities.CopyExactArray<ClipTransition>(copyFrom._Others, ref this._Others);
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0000E142 File Offset: 0x0000C342
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x0000E154 File Offset: 0x0000C354
		public AnimancerEvent EndEvent
		{
			get
			{
				return this.LastTransition.Events.EndEvent;
			}
			set
			{
				this.LastTransition.Events.EndEvent = value;
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000E168 File Offset: 0x0000C368
		public void AddEvent(float time, bool normalized, Action callback)
		{
			if (normalized)
			{
				time *= this.Length;
			}
			if (ClipTransitionSequence.TryAddEvent(this, base.Length, ref time, callback))
			{
				return;
			}
			for (int i = 0; i < this._Others.Length - 1; i++)
			{
				ClipTransition clipTransition = this._Others[i];
				if (ClipTransitionSequence.TryAddEvent(clipTransition, clipTransition.Length, ref time, callback))
				{
					return;
				}
			}
			ClipTransitionSequence.AddEvent(this.LastTransition, time, callback);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000E1CF File Offset: 0x0000C3CF
		private static bool TryAddEvent(ClipTransition transition, float length, ref float time, Action callback)
		{
			if (time > length)
			{
				time -= length;
				return false;
			}
			ClipTransitionSequence.AddEvent(transition, time, callback);
			return true;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
		private static void AddEvent(ClipTransition transition, float time, Action callback)
		{
			float num = transition.NormalizedStartTime;
			if (float.IsNaN(num))
			{
				num = AnimancerEvent.Sequence.GetDefaultNormalizedStartTime(num);
			}
			time /= transition.Clip.length * (1f - num);
			time += num;
			transition.Events.Add(time, callback);
		}

		// Token: 0x040000E0 RID: 224
		[SerializeField]
		[Tooltip("The other transitions to play in order after the first one.")]
		private ClipTransition[] _Others = Array.Empty<ClipTransition>();

		// Token: 0x040000E1 RID: 225
		private Action _OnEnd;
	}
}
