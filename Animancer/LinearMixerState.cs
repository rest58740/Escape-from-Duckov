using System;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000038 RID: 56
	public class LinearMixerState : MixerState<float>, ICopyable<LinearMixerState>
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000991C File Offset: 0x00007B1C
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00009924 File Offset: 0x00007B24
		public bool ExtrapolateSpeed
		{
			get
			{
				return this._ExtrapolateSpeed;
			}
			set
			{
				if (this._ExtrapolateSpeed == value)
				{
					return;
				}
				this._ExtrapolateSpeed = value;
				if (!this._Playable.IsValid<Playable>())
				{
					return;
				}
				float num = base.Speed;
				int childCount = this.ChildCount;
				if (value && childCount > 0)
				{
					float threshold = base.GetThreshold(childCount - 1);
					if (base.Parameter > threshold)
					{
						num *= base.Parameter / threshold;
					}
				}
				this._Playable.SetSpeed((double)num);
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00009990 File Offset: 0x00007B90
		public override string GetParameterError(float value)
		{
			if (!value.IsFinite())
			{
				return "must not be NaN or Infinity";
			}
			return null;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000099A1 File Offset: 0x00007BA1
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			LinearMixerState linearMixerState = new LinearMixerState();
			linearMixerState.SetNewCloneRoot(root);
			((ICopyable<LinearMixerState>)linearMixerState).CopyFrom(this);
			return linearMixerState;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000099B6 File Offset: 0x00007BB6
		void ICopyable<LinearMixerState>.CopyFrom(LinearMixerState copyFrom)
		{
			this._ExtrapolateSpeed = copyFrom._ExtrapolateSpeed;
			((ICopyable<MixerState<float>>)this).CopyFrom(copyFrom);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000099CC File Offset: 0x00007BCC
		public void AssertThresholdsSorted()
		{
			if (!base.HasThresholds)
			{
				throw new InvalidOperationException("Thresholds have not been initialized");
			}
			float num = float.NegativeInfinity;
			int childCount = this.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				if (base.ChildStates[i] != null)
				{
					float threshold = base.GetThreshold(i);
					if (threshold <= num)
					{
						throw new ArgumentException(((threshold == num) ? "Mixer has multiple identical thresholds." : "Mixer has thresholds out of order.") + " They must be sorted from lowest to highest with no equal values.\n" + base.GetDescription("\n"));
					}
					num = threshold;
				}
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00009A4C File Offset: 0x00007C4C
		protected override void ForceRecalculateWeights()
		{
			base.WeightsAreDirty = false;
			int childCount = this.ChildCount;
			if (childCount != 0)
			{
				int num = 0;
				AnimancerState animancerState = base.ChildStates[num];
				float parameter = base.Parameter;
				float num2 = base.GetThreshold(num);
				if (parameter <= num2)
				{
					base.DisableRemainingStates(num);
					if (num2 >= 0f)
					{
						animancerState.Weight = 1f;
						goto IL_E2;
					}
				}
				else
				{
					while (++num < childCount)
					{
						AnimancerState animancerState2 = base.ChildStates[num];
						float threshold = base.GetThreshold(num);
						if (parameter > num2 && parameter <= threshold)
						{
							float num3 = (parameter - num2) / (threshold - num2);
							animancerState.Weight = 1f - num3;
							animancerState2.Weight = num3;
							base.DisableRemainingStates(num);
							goto IL_E2;
						}
						animancerState.Weight = 0f;
						animancerState = animancerState2;
						num2 = threshold;
					}
				}
				animancerState.Weight = 1f;
				if (this.ExtrapolateSpeed)
				{
					this._Playable.SetSpeed((double)(base.Speed * (parameter / num2)));
				}
				return;
			}
			IL_E2:
			if (this.ExtrapolateSpeed && this._Playable.IsValid<Playable>())
			{
				this._Playable.SetSpeed((double)base.Speed);
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00009B64 File Offset: 0x00007D64
		public LinearMixerState AssignLinearThresholds(float min = 0f, float max = 1f)
		{
			int childCount = this.ChildCount;
			float[] array = new float[childCount];
			float num = (max - min) / (float)(childCount - 1);
			for (int i = 0; i < childCount; i++)
			{
				array[i] = ((i < childCount - 1) ? (min + (float)i * num) : max);
			}
			base.SetThresholds(array);
			return this;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00009BAE File Offset: 0x00007DAE
		protected override void AppendDetails(StringBuilder text, string separator)
		{
			text.Append(separator).Append("ExtrapolateSpeed: ").Append(this.ExtrapolateSpeed);
			base.AppendDetails(text, separator);
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00009BD5 File Offset: 0x00007DD5
		protected override int ParameterCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00009BD8 File Offset: 0x00007DD8
		protected override string GetParameterName(int index)
		{
			return "Parameter";
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00009BDF File Offset: 0x00007DDF
		protected override AnimatorControllerParameterType GetParameterType(int index)
		{
			return AnimatorControllerParameterType.Float;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00009BE2 File Offset: 0x00007DE2
		protected override object GetParameterValue(int index)
		{
			return base.Parameter;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00009BEF File Offset: 0x00007DEF
		protected override void SetParameterValue(int index, object value)
		{
			base.Parameter = (float)value;
		}

		// Token: 0x0400009A RID: 154
		private bool _ExtrapolateSpeed = true;

		// Token: 0x02000090 RID: 144
		public new interface ITransition : ITransition<LinearMixerState>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
