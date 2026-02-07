using System;
using ItemStatsSystem;

// Token: 0x02000086 RID: 134
public class RemoveBuffAction : EffectAction
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00016776 File Offset: 0x00014976
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

	// Token: 0x060004E7 RID: 1255 RVA: 0x00016794 File Offset: 0x00014994
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.RemoveBuff(this.buffID, this.removeOneLayer);
	}

	// Token: 0x04000427 RID: 1063
	public int buffID;

	// Token: 0x04000428 RID: 1064
	public bool removeOneLayer;
}
