using System;
using System.Text;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000037 RID: 55
	public class DirectionalMixerState : MixerState<Vector2>, ICopyable<DirectionalMixerState>
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000948E File Offset: 0x0000768E
		// (set) Token: 0x06000375 RID: 885 RVA: 0x0000949B File Offset: 0x0000769B
		public float ParameterX
		{
			get
			{
				return base.Parameter.x;
			}
			set
			{
				base.Parameter = new Vector2(value, base.Parameter.y);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000376 RID: 886 RVA: 0x000094B4 File Offset: 0x000076B4
		// (set) Token: 0x06000377 RID: 887 RVA: 0x000094C1 File Offset: 0x000076C1
		public float ParameterY
		{
			get
			{
				return base.Parameter.y;
			}
			set
			{
				base.Parameter = new Vector2(base.Parameter.x, value);
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000094DA File Offset: 0x000076DA
		public override string GetParameterError(Vector2 value)
		{
			if (!value.IsFinite())
			{
				return "value.x and value.y must not be NaN or Infinity";
			}
			return null;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000094EB File Offset: 0x000076EB
		public override void OnThresholdsChanged()
		{
			this._BlendFactorsDirty = true;
			base.OnThresholdsChanged();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000094FC File Offset: 0x000076FC
		protected override void ForceRecalculateWeights()
		{
			base.WeightsAreDirty = false;
			int childCount = this.ChildCount;
			if (childCount == 0)
			{
				return;
			}
			if (childCount == 1)
			{
				this.GetChild(0).Weight = 1f;
				return;
			}
			this.CalculateBlendFactors(childCount);
			float magnitude = base.Parameter.magnitude;
			float num = 0f;
			for (int i = 0; i < childCount; i++)
			{
				AnimancerState child = this.GetChild(i);
				if (child != null)
				{
					Vector2[] array = this._BlendFactors[i];
					Vector2 threshold = base.GetThreshold(i);
					float num2 = this._ThresholdMagnitudes[i];
					float num3 = magnitude - num2;
					float y = DirectionalMixerState.SignedAngle(threshold, base.Parameter) * 2f;
					float num4 = 1f;
					for (int j = 0; j < childCount; j++)
					{
						if (j != i && this.GetChild(j) != null)
						{
							float num5 = (this._ThresholdMagnitudes[j] + num2) * 0.5f;
							Vector2 lhs = new Vector2(num3 / num5, y);
							float num6 = 1f - Vector2.Dot(lhs, array[j]);
							if (num4 > num6)
							{
								num4 = num6;
							}
						}
					}
					if (num4 < 0.01f)
					{
						num4 = 0f;
					}
					child.Weight = num4;
					num += num4;
				}
			}
			base.NormalizeWeights(num);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000963C File Offset: 0x0000783C
		private void CalculateBlendFactors(int childCount)
		{
			if (!this._BlendFactorsDirty)
			{
				return;
			}
			this._BlendFactorsDirty = false;
			if (this._BlendFactors == null || this._BlendFactors.Length != childCount)
			{
				this._ThresholdMagnitudes = new float[childCount];
				this._BlendFactors = new Vector2[childCount][];
				for (int i = 0; i < childCount; i++)
				{
					this._BlendFactors[i] = new Vector2[childCount];
				}
			}
			for (int j = 0; j < childCount; j++)
			{
				this._ThresholdMagnitudes[j] = base.GetThreshold(j).magnitude;
			}
			for (int k = 0; k < childCount; k++)
			{
				Vector2[] array = this._BlendFactors[k];
				Vector2 threshold = base.GetThreshold(k);
				float num = this._ThresholdMagnitudes[k];
				for (int l = 0; l < childCount; l++)
				{
					if (k != l)
					{
						Vector2 threshold2 = base.GetThreshold(l);
						float num2 = this._ThresholdMagnitudes[l];
						float num3 = (num + num2) * 0.5f;
						float num4 = num2 - num;
						float num5 = DirectionalMixerState.SignedAngle(threshold, threshold2);
						Vector2 vector = new Vector2(num4 / num3, num5 * 2f);
						vector *= 1f / vector.sqrMagnitude;
						array[l] = vector;
						this._BlendFactors[l][k] = -vector;
					}
				}
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000978C File Offset: 0x0000798C
		private static float SignedAngle(Vector2 a, Vector2 b)
		{
			if ((a.x == 0f && a.y == 0f) || (b.x == 0f && b.y == 0f))
			{
				return 0f;
			}
			return Mathf.Atan2(a.x * b.y - a.y * b.x, a.x * b.x + a.y * b.y);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000980E File Offset: 0x00007A0E
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			DirectionalMixerState directionalMixerState = new DirectionalMixerState();
			directionalMixerState.SetNewCloneRoot(root);
			((ICopyable<DirectionalMixerState>)directionalMixerState).CopyFrom(this);
			return directionalMixerState;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00009823 File Offset: 0x00007A23
		void ICopyable<DirectionalMixerState>.CopyFrom(DirectionalMixerState copyFrom)
		{
			this._ThresholdMagnitudes = copyFrom._ThresholdMagnitudes;
			this._BlendFactorsDirty = copyFrom._BlendFactorsDirty;
			if (!this._BlendFactorsDirty)
			{
				this._BlendFactors = copyFrom._BlendFactors;
			}
			((ICopyable<MixerState<Vector2>>)this).CopyFrom(copyFrom);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00009858 File Offset: 0x00007A58
		public override void AppendParameter(StringBuilder text, Vector2 parameter)
		{
			text.Append('(').Append(parameter.x).Append(", ").Append(parameter.y).Append(')');
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000988A File Offset: 0x00007A8A
		protected override int ParameterCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000988D File Offset: 0x00007A8D
		protected override string GetParameterName(int index)
		{
			if (index == 0)
			{
				return "Parameter X";
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return "Parameter Y";
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000098AE File Offset: 0x00007AAE
		protected override AnimatorControllerParameterType GetParameterType(int index)
		{
			return AnimatorControllerParameterType.Float;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000098B1 File Offset: 0x00007AB1
		protected override object GetParameterValue(int index)
		{
			if (index == 0)
			{
				return this.ParameterX;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.ParameterY;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000098DE File Offset: 0x00007ADE
		protected override void SetParameterValue(int index, object value)
		{
			if (index == 0)
			{
				this.ParameterX = (float)value;
				return;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.ParameterY = (float)value;
		}

		// Token: 0x04000096 RID: 150
		private float[] _ThresholdMagnitudes;

		// Token: 0x04000097 RID: 151
		private Vector2[][] _BlendFactors;

		// Token: 0x04000098 RID: 152
		private bool _BlendFactorsDirty = true;

		// Token: 0x04000099 RID: 153
		private const float AngleFactor = 2f;
	}
}
