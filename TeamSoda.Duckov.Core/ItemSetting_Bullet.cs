using System;
using ItemStatsSystem;

// Token: 0x020000F2 RID: 242
public class ItemSetting_Bullet : ItemSettingBase
{
	// Token: 0x06000825 RID: 2085 RVA: 0x00024F12 File Offset: 0x00023112
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsBullet", true, true);
	}
}
