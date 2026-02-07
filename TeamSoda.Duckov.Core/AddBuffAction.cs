using System;
using Duckov.Buffs;
using ItemStatsSystem;

// Token: 0x02000081 RID: 129
public class AddBuffAction : EffectAction
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x060004D5 RID: 1237 RVA: 0x000163BE File Offset: 0x000145BE
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

	// Token: 0x060004D6 RID: 1238 RVA: 0x000163DC File Offset: 0x000145DC
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.AddBuff(this.buffPfb, this.MainControl, 0);
	}

	// Token: 0x04000417 RID: 1047
	public Buff buffPfb;
}
