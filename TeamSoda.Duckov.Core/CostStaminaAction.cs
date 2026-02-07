using System;
using ItemStatsSystem;

// Token: 0x02000082 RID: 130
public class CostStaminaAction : EffectAction
{
	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0001640C File Offset: 0x0001460C
	private CharacterMainControl MainControl
	{
		get
		{
			Effect master = base.Master;
			if (master == null)
			{
				return null;
			}
			Item item = master.Item;
			if (item == null)
			{
				return null;
			}
			return item.GetCharacterMainControl();
		}
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0001642A File Offset: 0x0001462A
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.UseStamina(this.staminaCost);
	}

	// Token: 0x04000418 RID: 1048
	public float staminaCost;
}
