using System;
using ItemStatsSystem;

// Token: 0x020000F9 RID: 249
public class ItemSetting_NightVision : ItemSettingBase
{
	// Token: 0x0600084C RID: 2124 RVA: 0x00025882 File Offset: 0x00023A82
	public override void OnInit()
	{
		if (this._item)
		{
			this._item.onPluggedIntoSlot += this.OnplugedIntoSlot;
		}
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x000258A8 File Offset: 0x00023AA8
	private void OnplugedIntoSlot(Item item)
	{
		this.nightVisionOn = true;
		this.SyncModifiers();
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x000258B7 File Offset: 0x00023AB7
	private void OnDestroy()
	{
		if (this._item)
		{
			this._item.onPluggedIntoSlot -= this.OnplugedIntoSlot;
		}
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x000258DD File Offset: 0x00023ADD
	public void ToggleNightVison()
	{
		this.nightVisionOn = !this.nightVisionOn;
		this.SyncModifiers();
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x000258F4 File Offset: 0x00023AF4
	private void SyncModifiers()
	{
		if (!this._item)
		{
			return;
		}
		this._item.Modifiers.ModifierEnable = this.nightVisionOn;
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0002591A File Offset: 0x00023B1A
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsNightVision", true, true);
	}

	// Token: 0x040007C8 RID: 1992
	private bool nightVisionOn = true;
}
