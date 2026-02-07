using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200006A RID: 106
	[Serializable]
	public class MixerTransition2D : MixerTransition<MixerState<Vector2>, Vector2>, ManualMixerState.ITransition2D, ITransition<MixerState<Vector2>>, ITransition, IHasKey, IPolymorphic, ICopyable<MixerTransition2D>
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0000EC57 File Offset: 0x0000CE57
		public ref MixerTransition2D.MixerType Type
		{
			get
			{
				return ref this._Type;
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0000EC60 File Offset: 0x0000CE60
		public override MixerState<Vector2> CreateState()
		{
			MixerTransition2D.MixerType type = this._Type;
			if (type != MixerTransition2D.MixerType.Cartesian)
			{
				if (type != MixerTransition2D.MixerType.Directional)
				{
					throw new ArgumentOutOfRangeException("_Type");
				}
				base.State = new DirectionalMixerState();
			}
			else
			{
				base.State = new CartesianMixerState();
			}
			this.InitializeState();
			return base.State;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0000ECAE File Offset: 0x0000CEAE
		public virtual void CopyFrom(MixerTransition2D copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._Type = MixerTransition2D.MixerType.Cartesian;
				return;
			}
			this._Type = copyFrom._Type;
		}

		// Token: 0x040000F6 RID: 246
		[SerializeField]
		private MixerTransition2D.MixerType _Type;

		// Token: 0x020000B4 RID: 180
		public enum MixerType
		{
			// Token: 0x0400018E RID: 398
			Cartesian,
			// Token: 0x0400018F RID: 399
			Directional
		}
	}
}
