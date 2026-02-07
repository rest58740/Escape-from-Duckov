using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200005B RID: 91
	[Serializable]
	public abstract class ControllerTransition<TState> : AnimancerTransition<TState>, IAnimationClipCollection, ICopyable<ControllerTransition<TState>> where TState : ControllerState
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0000E26A File Offset: 0x0000C46A
		public ref RuntimeAnimatorController Controller
		{
			get
			{
				return ref this._Controller;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0000E272 File Offset: 0x0000C472
		public override UnityEngine.Object MainObject
		{
			get
			{
				return this._Controller;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0000E27A File Offset: 0x0000C47A
		public ref ControllerState.ActionOnStop[] ActionsOnStop
		{
			get
			{
				return ref this._ActionsOnStop;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0000E284 File Offset: 0x0000C484
		public override float MaximumDuration
		{
			get
			{
				if (this._Controller == null)
				{
					return 0f;
				}
				float num = 0f;
				AnimationClip[] animationClips = this._Controller.animationClips;
				for (int i = 0; i < animationClips.Length; i++)
				{
					float length = animationClips[i].length;
					if (num < length)
					{
						num = length;
					}
				}
				return num;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0000E2D5 File Offset: 0x0000C4D5
		public override bool IsValid
		{
			get
			{
				return this._Controller != null;
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000E2E3 File Offset: 0x0000C4E3
		public static implicit operator RuntimeAnimatorController(ControllerTransition<TState> transition)
		{
			if (transition == null)
			{
				return null;
			}
			return transition._Controller;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000E2F0 File Offset: 0x0000C4F0
		public override void Apply(AnimancerState state)
		{
			ControllerState controllerState = state as ControllerState;
			if (controllerState != null)
			{
				controllerState.ActionsOnStop = this._ActionsOnStop;
			}
			base.Apply(state);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000E31A File Offset: 0x0000C51A
		void IAnimationClipCollection.GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			if (this._Controller != null)
			{
				clips.Gather(this._Controller.animationClips);
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000E33B File Offset: 0x0000C53B
		public virtual void CopyFrom(ControllerTransition<TState> copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._Controller = null;
				this._ActionsOnStop = Array.Empty<ControllerState.ActionOnStop>();
				return;
			}
			this._Controller = copyFrom._Controller;
			this._ActionsOnStop = copyFrom._ActionsOnStop;
		}

		// Token: 0x040000E2 RID: 226
		[SerializeField]
		private RuntimeAnimatorController _Controller;

		// Token: 0x040000E3 RID: 227
		[SerializeField]
		[Tooltip("Determines what each layer does when ControllerState.Stop is called.\n• If empty, all layers will reset to their default state.\n• If this array is smaller than the layer count, any additional layers will use the last value in this array.")]
		private ControllerState.ActionOnStop[] _ActionsOnStop;
	}
}
