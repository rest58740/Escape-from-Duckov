using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003ED RID: 1005
	public class BarDisplayController_Thurst : BarDisplayController
	{
		// Token: 0x06002494 RID: 9364 RVA: 0x000805B0 File Offset: 0x0007E7B0
		private void Update()
		{
			float num = this.Current;
			float max = this.Max;
			if (this.displayingCurrent != num || this.displayingMax != max)
			{
				base.Refresh();
				this.displayingCurrent = num;
				this.displayingMax = max;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002495 RID: 9365 RVA: 0x000805F1 File Offset: 0x0007E7F1
		private CharacterMainControl Target
		{
			get
			{
				if (this._target == null)
				{
					this._target = CharacterMainControl.Main;
				}
				return this._target;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x00080612 File Offset: 0x0007E812
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentWater;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002497 RID: 9367 RVA: 0x00080634 File Offset: 0x0007E834
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxWater;
			}
		}

		// Token: 0x040018DC RID: 6364
		private CharacterMainControl _target;

		// Token: 0x040018DD RID: 6365
		private float displayingCurrent = -1f;

		// Token: 0x040018DE RID: 6366
		private float displayingMax = -1f;
	}
}
