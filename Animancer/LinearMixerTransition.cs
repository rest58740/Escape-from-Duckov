using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000064 RID: 100
	[Serializable]
	public class LinearMixerTransition : MixerTransition<LinearMixerState, float>, LinearMixerState.ITransition, ITransition<LinearMixerState>, ITransition, IHasKey, IPolymorphic, ICopyable<LinearMixerTransition>
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0000E5F9 File Offset: 0x0000C7F9
		public ref bool ExtrapolateSpeed
		{
			get
			{
				return ref this._ExtrapolateSpeed;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x0000E604 File Offset: 0x0000C804
		public unsafe override bool IsValid
		{
			get
			{
				if (!base.IsValid)
				{
					return false;
				}
				float num = float.NegativeInfinity;
				foreach (float num2 in *base.Thresholds)
				{
					if (num2 < num)
					{
						return false;
					}
					num = num2;
				}
				return true;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0000E644 File Offset: 0x0000C844
		public override LinearMixerState CreateState()
		{
			base.State = new LinearMixerState();
			this.InitializeState();
			return base.State;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0000E65D File Offset: 0x0000C85D
		public override void Apply(AnimancerState state)
		{
			base.Apply(state);
			base.State.ExtrapolateSpeed = this._ExtrapolateSpeed;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0000E678 File Offset: 0x0000C878
		public unsafe void SortByThresholds()
		{
			int num = base.Thresholds->Length;
			if (num <= 1)
			{
				return;
			}
			int num2 = base.Speeds->Length;
			int num3 = base.SynchronizeChildren->Length;
			float num4 = (*base.Thresholds)[0];
			for (int i = 1; i < num; i++)
			{
				float num5 = (*base.Thresholds)[i];
				if (num5 >= num4)
				{
					num4 = num5;
				}
				else
				{
					(*base.Thresholds).Swap(i, i - 1);
					(*base.Animations).Swap(i, i - 1);
					if (i < num2)
					{
						(*base.Speeds).Swap(i, i - 1);
					}
					if (i == num3 && !(*base.SynchronizeChildren)[i - 1])
					{
						bool[] array = *base.SynchronizeChildren;
						Array.Resize<bool>(ref array, ++num3);
						array[i - 1] = true;
						array[i] = false;
						*base.SynchronizeChildren = array;
					}
					else if (i < num3)
					{
						(*base.SynchronizeChildren).Swap(i, i - 1);
					}
					if (i == 1)
					{
						i = 0;
						num4 = float.NegativeInfinity;
					}
					else
					{
						i -= 2;
						num4 = (*base.Thresholds)[i];
					}
				}
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0000E795 File Offset: 0x0000C995
		public virtual void CopyFrom(LinearMixerTransition copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._ExtrapolateSpeed = true;
				return;
			}
			this._ExtrapolateSpeed = copyFrom._ExtrapolateSpeed;
		}

		// Token: 0x040000EA RID: 234
		[SerializeField]
		[Tooltip("Should setting the Parameter above the highest threshold increase the Speed of the mixer proportionally?")]
		private bool _ExtrapolateSpeed = true;
	}
}
