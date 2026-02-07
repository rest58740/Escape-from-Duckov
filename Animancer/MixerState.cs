using System;
using System.Text;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200003A RID: 58
	public abstract class MixerState<TParameter> : ManualMixerState, ICopyable<MixerState<TParameter>>
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000B176 File Offset: 0x00009376
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0000B17E File Offset: 0x0000937E
		public TParameter Parameter
		{
			get
			{
				return this._Parameter;
			}
			set
			{
				this._Parameter = value;
				base.WeightsAreDirty = true;
				base.RequireUpdate();
			}
		}

		// Token: 0x060003F4 RID: 1012
		public abstract string GetParameterError(TParameter parameter);

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000B194 File Offset: 0x00009394
		public bool HasThresholds
		{
			get
			{
				return this._Thresholds.Length >= this.ChildCount;
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000B1A9 File Offset: 0x000093A9
		public TParameter GetThreshold(int index)
		{
			return this._Thresholds[index];
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000B1B7 File Offset: 0x000093B7
		public void SetThreshold(int index, TParameter threshold)
		{
			this._Thresholds[index] = threshold;
			this.OnThresholdsChanged();
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000B1CC File Offset: 0x000093CC
		public void SetThresholds(params TParameter[] thresholds)
		{
			if (thresholds.Length < this.ChildCount)
			{
				throw new ArgumentOutOfRangeException("thresholds", string.Format("Threshold count ({0}) must not be less than child count ({1}).", thresholds.Length, this.ChildCount));
			}
			this._Thresholds = thresholds;
			this.OnThresholdsChanged();
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000B219 File Offset: 0x00009419
		public bool ValidateThresholdCount()
		{
			if (this._Thresholds.Length >= this.ChildCount)
			{
				return false;
			}
			this._Thresholds = new TParameter[base.ChildCapacity];
			return true;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000B23F File Offset: 0x0000943F
		public virtual void OnThresholdsChanged()
		{
			base.WeightsAreDirty = true;
			base.RequireUpdate();
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000B250 File Offset: 0x00009450
		public void CalculateThresholds(Func<AnimancerState, TParameter> calculate)
		{
			this.ValidateThresholdCount();
			for (int i = this.ChildCount - 1; i >= 0; i--)
			{
				this._Thresholds[i] = calculate(this.GetChild(i));
			}
			this.OnThresholdsChanged();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000B296 File Offset: 0x00009496
		public override void RecreatePlayable()
		{
			base.RecreatePlayable();
			base.WeightsAreDirty = true;
			base.RequireUpdate();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000B2AB File Offset: 0x000094AB
		protected override void OnChildCapacityChanged()
		{
			Array.Resize<TParameter>(ref this._Thresholds, base.ChildCapacity);
			this.OnThresholdsChanged();
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000B2C4 File Offset: 0x000094C4
		public void Add(AnimancerState state, TParameter threshold)
		{
			base.Add(state);
			this.SetThreshold(state.Index, threshold);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000B2DC File Offset: 0x000094DC
		public ClipState Add(AnimationClip clip, TParameter threshold)
		{
			ClipState clipState = base.Add(clip);
			this.SetThreshold(clipState.Index, threshold);
			return clipState;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000B300 File Offset: 0x00009500
		public AnimancerState Add(Animancer.ITransition transition, TParameter threshold)
		{
			AnimancerState animancerState = base.Add(transition);
			this.SetThreshold(animancerState.Index, threshold);
			return animancerState;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000B324 File Offset: 0x00009524
		public AnimancerState Add(object child, TParameter threshold)
		{
			AnimationClip animationClip = child as AnimationClip;
			if (animationClip != null)
			{
				return this.Add(animationClip, threshold);
			}
			ManualMixerState.ITransition transition = child as ManualMixerState.ITransition;
			if (transition != null)
			{
				return this.Add(transition, threshold);
			}
			AnimancerState animancerState = child as AnimancerState;
			if (animancerState != null)
			{
				this.Add(animancerState, threshold);
				return animancerState;
			}
			throw new ArgumentException(string.Format("Unable to add '{0}' as child of '{1}'.", AnimancerUtilities.ToStringOrNull(child), this));
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000B384 File Offset: 0x00009584
		void ICopyable<MixerState<!0>>.CopyFrom(MixerState<TParameter> copyFrom)
		{
			((ICopyable<ManualMixerState>)this).CopyFrom(copyFrom);
			int childCount = copyFrom.ChildCount;
			if (copyFrom._Thresholds != null)
			{
				this._Thresholds = new TParameter[childCount];
				int length = Math.Min(childCount, copyFrom._Thresholds.Length);
				Array.Copy(copyFrom._Thresholds, this._Thresholds, length);
			}
			this.Parameter = copyFrom.Parameter;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000B3E0 File Offset: 0x000095E0
		public override string GetDisplayKey(AnimancerState state)
		{
			return string.Format("[{0}] {1}", state.Index, this._Thresholds[state.Index]);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000B410 File Offset: 0x00009610
		protected override void AppendDetails(StringBuilder text, string separator)
		{
			text.Append(separator);
			text.Append("Parameter: ");
			this.AppendParameter(text, this.Parameter);
			text.Append(separator).Append("Thresholds: ");
			int num = Math.Min(base.ChildCapacity, this._Thresholds.Length);
			for (int i = 0; i < num; i++)
			{
				if (i > 0)
				{
					text.Append(", ");
				}
				this.AppendParameter(text, this._Thresholds[i]);
			}
			base.AppendDetails(text, separator);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000B49B File Offset: 0x0000969B
		public virtual void AppendParameter(StringBuilder description, TParameter parameter)
		{
			description.Append(parameter);
		}

		// Token: 0x040000A4 RID: 164
		private TParameter[] _Thresholds = Array.Empty<TParameter>();

		// Token: 0x040000A5 RID: 165
		private TParameter _Parameter;
	}
}
