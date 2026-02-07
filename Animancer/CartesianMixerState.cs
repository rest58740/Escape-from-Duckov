using System;
using System.Text;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000036 RID: 54
	public class CartesianMixerState : MixerState<Vector2>, ICopyable<CartesianMixerState>
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00009179 File Offset: 0x00007379
		// (set) Token: 0x06000364 RID: 868 RVA: 0x00009186 File Offset: 0x00007386
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

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000919F File Offset: 0x0000739F
		// (set) Token: 0x06000366 RID: 870 RVA: 0x000091AC File Offset: 0x000073AC
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

		// Token: 0x06000367 RID: 871 RVA: 0x000091C5 File Offset: 0x000073C5
		public override string GetParameterError(Vector2 value)
		{
			if (!value.IsFinite())
			{
				return "value.x and value.y must not be NaN or Infinity";
			}
			return null;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000091D6 File Offset: 0x000073D6
		public override void OnThresholdsChanged()
		{
			this._BlendFactorsDirty = true;
			base.OnThresholdsChanged();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000091E8 File Offset: 0x000073E8
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
			float num = 0f;
			for (int i = 0; i < childCount; i++)
			{
				AnimancerState child = this.GetChild(i);
				if (child != null)
				{
					Vector2[] array = this._BlendFactors[i];
					Vector2 threshold = base.GetThreshold(i);
					Vector2 lhs = base.Parameter - threshold;
					float num2 = 1f;
					for (int j = 0; j < childCount; j++)
					{
						if (j != i && this.GetChild(j) != null)
						{
							float num3 = 1f - Vector2.Dot(lhs, array[j]);
							if (num2 > num3)
							{
								num2 = num3;
							}
						}
					}
					if (num2 < 0.01f)
					{
						num2 = 0f;
					}
					child.Weight = num2;
					num += num2;
				}
			}
			base.NormalizeWeights(num);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000092D8 File Offset: 0x000074D8
		private void CalculateBlendFactors(int childCount)
		{
			if (!this._BlendFactorsDirty)
			{
				return;
			}
			this._BlendFactorsDirty = false;
			if (AnimancerUtilities.SetLength<Vector2[]>(ref this._BlendFactors, childCount))
			{
				for (int i = 0; i < childCount; i++)
				{
					this._BlendFactors[i] = new Vector2[childCount];
				}
			}
			for (int j = 0; j < childCount; j++)
			{
				Vector2[] array = this._BlendFactors[j];
				Vector2 threshold = base.GetThreshold(j);
				for (int k = j + 1; k < childCount; k++)
				{
					Vector2 vector = base.GetThreshold(k) - threshold;
					vector /= vector.sqrMagnitude;
					array[k] = vector;
					this._BlendFactors[k][j] = -vector;
				}
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000938C File Offset: 0x0000758C
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			CartesianMixerState cartesianMixerState = new CartesianMixerState();
			cartesianMixerState.SetNewCloneRoot(root);
			((ICopyable<CartesianMixerState>)cartesianMixerState).CopyFrom(this);
			return cartesianMixerState;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x000093A1 File Offset: 0x000075A1
		void ICopyable<CartesianMixerState>.CopyFrom(CartesianMixerState copyFrom)
		{
			this._BlendFactorsDirty = copyFrom._BlendFactorsDirty;
			if (!this._BlendFactorsDirty)
			{
				this._BlendFactors = copyFrom._BlendFactors;
			}
			((ICopyable<MixerState<Vector2>>)this).CopyFrom(copyFrom);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000093CA File Offset: 0x000075CA
		public override void AppendParameter(StringBuilder text, Vector2 parameter)
		{
			text.Append('(').Append(parameter.x).Append(", ").Append(parameter.y).Append(')');
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000093FC File Offset: 0x000075FC
		protected override int ParameterCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000093FF File Offset: 0x000075FF
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

		// Token: 0x06000370 RID: 880 RVA: 0x00009420 File Offset: 0x00007620
		protected override AnimatorControllerParameterType GetParameterType(int index)
		{
			return AnimatorControllerParameterType.Float;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00009423 File Offset: 0x00007623
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

		// Token: 0x06000372 RID: 882 RVA: 0x00009450 File Offset: 0x00007650
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

		// Token: 0x04000094 RID: 148
		private Vector2[][] _BlendFactors;

		// Token: 0x04000095 RID: 149
		private bool _BlendFactorsDirty = true;
	}
}
