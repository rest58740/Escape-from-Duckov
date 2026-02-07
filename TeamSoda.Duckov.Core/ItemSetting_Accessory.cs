using System;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class ItemSetting_Accessory : ItemSettingBase
{
	// Token: 0x0600081C RID: 2076 RVA: 0x00024D57 File Offset: 0x00022F57
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsAccessory", true, true);
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00024D66 File Offset: 0x00022F66
	public override void OnInit()
	{
		base.Item.onPluggedIntoSlot += this.OnPluggedIntoSlot;
		base.Item.onUnpluggedFromSlot += this.OnUnpluggedIntoSlot;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00024D98 File Offset: 0x00022F98
	private void OnPluggedIntoSlot(Item selfItem)
	{
		Slot pluggedIntoSlot = selfItem.PluggedIntoSlot;
		if (pluggedIntoSlot == null)
		{
			return;
		}
		this.masterItem = pluggedIntoSlot.Master;
		if (!this.masterItem)
		{
			return;
		}
		this.masterItem.AgentUtilities.onCreateAgent += this.OnMasterCreateAgent;
		this.CreateAccessory(this.masterItem.AgentUtilities.ActiveAgent as DuckovItemAgent);
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00024E01 File Offset: 0x00023001
	private void OnUnpluggedIntoSlot(Item selfItem)
	{
		if (this.masterItem)
		{
			this.masterItem.AgentUtilities.onCreateAgent -= this.OnMasterCreateAgent;
		}
		this.DestroyAccessory();
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00024E32 File Offset: 0x00023032
	private void OnDestroy()
	{
		if (this.masterItem)
		{
			this.masterItem.AgentUtilities.onCreateAgent -= this.OnMasterCreateAgent;
		}
		this.DestroyAccessory();
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00024E63 File Offset: 0x00023063
	private void OnMasterCreateAgent(Item _masterItem, ItemAgent newAgnet)
	{
		if (this.masterItem != _masterItem)
		{
			Debug.LogError("缓存了错误的Item");
		}
		if (newAgnet.AgentType != 2)
		{
			return;
		}
		this.CreateAccessory(newAgnet as DuckovItemAgent);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00024E94 File Offset: 0x00023094
	private void CreateAccessory(DuckovItemAgent parentAgent)
	{
		this.DestroyAccessory();
		if (this.accessoryPfb == null || parentAgent == null || parentAgent.AgentType != 2)
		{
			return;
		}
		this.accessoryInstance = Object.Instantiate<AccessoryBase>(this.accessoryPfb);
		this.accessoryInstance.Init(parentAgent, base.Item);
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00024EEB File Offset: 0x000230EB
	private void DestroyAccessory()
	{
		if (this.accessoryInstance)
		{
			Object.Destroy(this.accessoryInstance.gameObject);
		}
	}

	// Token: 0x040007A1 RID: 1953
	[SerializeField]
	private AccessoryBase accessoryPfb;

	// Token: 0x040007A2 RID: 1954
	public ADSAimMarker overrideAdsAimMarker;

	// Token: 0x040007A3 RID: 1955
	private AccessoryBase accessoryInstance;

	// Token: 0x040007A4 RID: 1956
	private bool created;

	// Token: 0x040007A5 RID: 1957
	private Item masterItem;
}
