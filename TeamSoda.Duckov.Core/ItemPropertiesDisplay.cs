using System;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class ItemPropertiesDisplay : MonoBehaviour
{
	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x00030094 File Offset: 0x0002E294
	private PrefabPool<LabelAndValue> EntryPool
	{
		get
		{
			if (this._entryPool == null)
			{
				this._entryPool = new PrefabPool<LabelAndValue>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._entryPool;
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x000300CD File Offset: 0x0002E2CD
	private void Awake()
	{
		this.entryTemplate.gameObject.SetActive(false);
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x000300E0 File Offset: 0x0002E2E0
	internal void Setup(Item targetItem)
	{
		this.EntryPool.ReleaseAll();
		if (targetItem == null)
		{
			return;
		}
		foreach (ValueTuple<string, string, Polarity> valueTuple in targetItem.GetPropertyValueTextPair())
		{
			this.EntryPool.Get(null).Setup(valueTuple.Item1, valueTuple.Item2, valueTuple.Item3);
		}
	}

	// Token: 0x04000991 RID: 2449
	[SerializeField]
	private LabelAndValue entryTemplate;

	// Token: 0x04000992 RID: 2450
	private PrefabPool<LabelAndValue> _entryPool;
}
