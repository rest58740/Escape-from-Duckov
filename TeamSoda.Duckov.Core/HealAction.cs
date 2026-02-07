using System;
using ItemStatsSystem;

// Token: 0x02000084 RID: 132
public class HealAction : EffectAction
{
	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x00016552 File Offset: 0x00014752
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0001658C File Offset: 0x0001478C
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.Health.AddHealth((float)this.healValue);
	}

	// Token: 0x0400041C RID: 1052
	private CharacterMainControl _mainControl;

	// Token: 0x0400041D RID: 1053
	public int healValue = 10;
}
