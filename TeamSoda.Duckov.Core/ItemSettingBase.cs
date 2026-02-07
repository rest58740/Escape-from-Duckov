using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public abstract class ItemSettingBase : MonoBehaviour
{
	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000816 RID: 2070 RVA: 0x00024D08 File Offset: 0x00022F08
	public Item Item
	{
		get
		{
			if (this._item == null)
			{
				this._item = base.GetComponent<Item>();
			}
			return this._item;
		}
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00024D2A File Offset: 0x00022F2A
	public void Awake()
	{
		if (this.Item)
		{
			this.SetMarkerParam(this.Item);
			this.OnInit();
		}
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00024D4B File Offset: 0x00022F4B
	public virtual void OnInit()
	{
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00024D4D File Offset: 0x00022F4D
	public virtual void Start()
	{
	}

	// Token: 0x0600081A RID: 2074
	public abstract void SetMarkerParam(Item selfItem);

	// Token: 0x040007A0 RID: 1952
	protected Item _item;
}
