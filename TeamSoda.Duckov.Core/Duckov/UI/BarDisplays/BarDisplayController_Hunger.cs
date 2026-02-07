using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003EB RID: 1003
	public class BarDisplayController_Hunger : BarDisplayController
	{
		// Token: 0x0600248A RID: 9354 RVA: 0x00080428 File Offset: 0x0007E628
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

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x00080469 File Offset: 0x0007E669
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

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x0600248C RID: 9356 RVA: 0x0008048A File Offset: 0x0007E68A
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentEnergy;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x000804AC File Offset: 0x0007E6AC
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxEnergy;
			}
		}

		// Token: 0x040018D6 RID: 6358
		private CharacterMainControl _target;

		// Token: 0x040018D7 RID: 6359
		private float displayingCurrent = -1f;

		// Token: 0x040018D8 RID: 6360
		private float displayingMax = -1f;
	}
}
