using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000068 RID: 104
	[Serializable]
	public abstract class MixerTransition<TMixer, TParameter> : ManualMixerTransition<TMixer>, ICopyable<MixerTransition<TMixer, TParameter>> where TMixer : MixerState<TParameter>
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0000EBC6 File Offset: 0x0000CDC6
		public ref TParameter[] Thresholds
		{
			get
			{
				return ref this._Thresholds;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0000EBCE File Offset: 0x0000CDCE
		public ref TParameter DefaultParameter
		{
			get
			{
				return ref this._DefaultParameter;
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0000EBD6 File Offset: 0x0000CDD6
		public override void InitializeState()
		{
			base.InitializeState();
			base.State.SetThresholds(this._Thresholds);
			base.State.Parameter = this._DefaultParameter;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0000EC0A File Offset: 0x0000CE0A
		public virtual void CopyFrom(MixerTransition<TMixer, TParameter> copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._DefaultParameter = default(TParameter);
				this._Thresholds = null;
				return;
			}
			this._DefaultParameter = copyFrom._DefaultParameter;
			AnimancerUtilities.CopyExactArray<TParameter>(copyFrom._Thresholds, ref this._Thresholds);
		}

		// Token: 0x040000F2 RID: 242
		[SerializeField]
		private TParameter[] _Thresholds;

		// Token: 0x040000F3 RID: 243
		public const string ThresholdsField = "_Thresholds";

		// Token: 0x040000F4 RID: 244
		[SerializeField]
		private TParameter _DefaultParameter;

		// Token: 0x040000F5 RID: 245
		public const string DefaultParameterField = "_DefaultParameter";
	}
}
