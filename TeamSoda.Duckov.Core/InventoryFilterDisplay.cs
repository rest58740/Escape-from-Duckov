using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001F9 RID: 505
public class InventoryFilterDisplay : MonoBehaviour, ISingleSelectionMenu<InventoryFilterDisplayEntry>
{
	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0003CDA8 File Offset: 0x0003AFA8
	private PrefabPool<InventoryFilterDisplayEntry> Pool
	{
		get
		{
			if (this._pool == null)
			{
				this._pool = new PrefabPool<InventoryFilterDisplayEntry>(this.template, null, null, null, null, true, 10, 10000, null);
			}
			return this._pool;
		}
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0003CDE1 File Offset: 0x0003AFE1
	private void Awake()
	{
		this.template.gameObject.SetActive(false);
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0003CDF4 File Offset: 0x0003AFF4
	public void Setup(InventoryDisplay target)
	{
		this.Pool.ReleaseAll();
		this.entries.Clear();
		if (target == null)
		{
			return;
		}
		this.targetDisplay = target;
		this.provider = target.Target.GetComponent<InventoryFilterProvider>();
		if (this.provider == null)
		{
			return;
		}
		foreach (InventoryFilterProvider.FilterEntry filter in this.provider.entries)
		{
			InventoryFilterDisplayEntry inventoryFilterDisplayEntry = this.Pool.Get(null);
			inventoryFilterDisplayEntry.Setup(new Action<InventoryFilterDisplayEntry, PointerEventData>(this.OnEntryClicked), filter);
			this.entries.Add(inventoryFilterDisplayEntry);
		}
		this.selection = null;
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0003CE9D File Offset: 0x0003B09D
	private void OnEntryClicked(InventoryFilterDisplayEntry entry, PointerEventData data)
	{
		this.SetSelection(entry);
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0003CEA7 File Offset: 0x0003B0A7
	internal void Select(int i)
	{
		if (i < 0 || i >= this.entries.Count)
		{
			return;
		}
		this.SetSelection(this.entries[i]);
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x0003CECF File Offset: 0x0003B0CF
	public InventoryFilterDisplayEntry GetSelection()
	{
		return this.selection;
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x0003CED8 File Offset: 0x0003B0D8
	public bool SetSelection(InventoryFilterDisplayEntry selection)
	{
		if (selection == null)
		{
			return false;
		}
		this.selection = selection;
		InventoryFilterProvider.FilterEntry filter = selection.Filter;
		this.targetDisplay.SetFilter(filter.GetFunction());
		foreach (InventoryFilterDisplayEntry inventoryFilterDisplayEntry in this.entries)
		{
			inventoryFilterDisplayEntry.NotifySelectionChanged(inventoryFilterDisplayEntry == selection);
		}
		return true;
	}

	// Token: 0x04000C82 RID: 3202
	[SerializeField]
	private InventoryFilterDisplayEntry template;

	// Token: 0x04000C83 RID: 3203
	private PrefabPool<InventoryFilterDisplayEntry> _pool;

	// Token: 0x04000C84 RID: 3204
	private InventoryDisplay targetDisplay;

	// Token: 0x04000C85 RID: 3205
	private InventoryFilterProvider provider;

	// Token: 0x04000C86 RID: 3206
	private List<InventoryFilterDisplayEntry> entries = new List<InventoryFilterDisplayEntry>();

	// Token: 0x04000C87 RID: 3207
	private InventoryFilterDisplayEntry selection;
}
