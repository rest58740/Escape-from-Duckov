using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000FE RID: 254
[Serializable]
public struct RandomItemGenerateDescription
{
	// Token: 0x060008A3 RID: 2211 RVA: 0x00026E94 File Offset: 0x00025094
	public UniTask<List<Item>> Generate(int count = -1)
	{
		RandomItemGenerateDescription.<Generate>d__12 <Generate>d__;
		<Generate>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Generate>d__.<>4__this = this;
		<Generate>d__.count = count;
		<Generate>d__.<>1__state = -1;
		<Generate>d__.<>t__builder.Start<RandomItemGenerateDescription.<Generate>d__12>(ref <Generate>d__);
		return <Generate>d__.<>t__builder.Task;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00026EE4 File Offset: 0x000250E4
	private void SetDurabilityIfNeeded(Item targetItem)
	{
		if (targetItem == null)
		{
			return;
		}
		if (this.controlDurability && targetItem.UseDurability)
		{
			float num = Random.Range(this.durabilityIntegrity.x, this.durabilityIntegrity.y);
			targetItem.DurabilityLoss = 1f - num;
			float num2 = Random.Range(this.durability.x, this.durability.y);
			if (num2 > num)
			{
				num2 = num;
			}
			targetItem.Durability = targetItem.MaxDurability * num2;
		}
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00026F64 File Offset: 0x00025164
	private void RefreshPercent()
	{
		this.itemPool.RefreshPercent();
	}

	// Token: 0x040007DF RID: 2015
	[TextArea]
	[SerializeField]
	private string comment;

	// Token: 0x040007E0 RID: 2016
	[Range(0f, 1f)]
	public float chance;

	// Token: 0x040007E1 RID: 2017
	public Vector2Int randomCount;

	// Token: 0x040007E2 RID: 2018
	public bool controlDurability;

	// Token: 0x040007E3 RID: 2019
	public Vector2 durability;

	// Token: 0x040007E4 RID: 2020
	public Vector2 durabilityIntegrity;

	// Token: 0x040007E5 RID: 2021
	public bool randomFromPool;

	// Token: 0x040007E6 RID: 2022
	[SerializeField]
	public RandomContainer<RandomItemGenerateDescription.Entry> itemPool;

	// Token: 0x040007E7 RID: 2023
	public RandomContainer<Tag> tags;

	// Token: 0x040007E8 RID: 2024
	public List<Tag> addtionalRequireTags;

	// Token: 0x040007E9 RID: 2025
	public List<Tag> excludeTags;

	// Token: 0x040007EA RID: 2026
	public RandomContainer<int> qualities;

	// Token: 0x0200049F RID: 1183
	[Serializable]
	public struct Entry
	{
		// Token: 0x04001C5F RID: 7263
		[ItemTypeID]
		[SerializeField]
		public int itemTypeID;
	}
}
