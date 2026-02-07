using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003EC RID: 1004
	public class BarDisplayController_Stemina : BarDisplayController
	{
		// Token: 0x0600248F RID: 9359 RVA: 0x000804EC File Offset: 0x0007E6EC
		private void Update()
		{
			float num = this.Current;
			float max = this.Max;
			if (this.displayingStemina != num || this.displayingMaxStemina != max)
			{
				base.Refresh();
				this.displayingStemina = num;
				this.displayingMaxStemina = max;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x0008052D File Offset: 0x0007E72D
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

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x0008054E File Offset: 0x0007E74E
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentStamina;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x00080570 File Offset: 0x0007E770
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxStamina;
			}
		}

		// Token: 0x040018D9 RID: 6361
		private CharacterMainControl _target;

		// Token: 0x040018DA RID: 6362
		private float displayingStemina = -1f;

		// Token: 0x040018DB RID: 6363
		private float displayingMaxStemina = -1f;
	}
}
