using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001B0 RID: 432
[CreateAssetMenu]
public class DecomposeDatabase : ScriptableObject
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000CFA RID: 3322 RVA: 0x0003727D File Offset: 0x0003547D
	public static DecomposeDatabase Instance
	{
		get
		{
			return GameplayDataSettings.DecomposeDatabase;
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00037284 File Offset: 0x00035484
	private Dictionary<int, DecomposeFormula> Dic
	{
		get
		{
			if (this._dic == null)
			{
				this.RebuildDictionary();
			}
			return this._dic;
		}
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x0003729C File Offset: 0x0003549C
	public void RebuildDictionary()
	{
		this._dic = new Dictionary<int, DecomposeFormula>();
		foreach (DecomposeFormula decomposeFormula in this.entries)
		{
			this._dic[decomposeFormula.item] = decomposeFormula;
		}
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x000372E4 File Offset: 0x000354E4
	public DecomposeFormula GetFormula(int itemTypeID)
	{
		DecomposeFormula result;
		if (!this.Dic.TryGetValue(itemTypeID, out result))
		{
			return default(DecomposeFormula);
		}
		return result;
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x0003730C File Offset: 0x0003550C
	public static UniTask<bool> Decompose(Item item, int count)
	{
		DecomposeDatabase.<Decompose>d__8 <Decompose>d__;
		<Decompose>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Decompose>d__.item = item;
		<Decompose>d__.count = count;
		<Decompose>d__.<>1__state = -1;
		<Decompose>d__.<>t__builder.Start<DecomposeDatabase.<Decompose>d__8>(ref <Decompose>d__);
		return <Decompose>d__.<>t__builder.Task;
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x00037357 File Offset: 0x00035557
	public static bool CanDecompose(int itemTypeID)
	{
		return !(DecomposeDatabase.Instance == null) && DecomposeDatabase.Instance.GetFormula(itemTypeID).valid;
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x00037378 File Offset: 0x00035578
	public static bool CanDecompose(Item item)
	{
		return !(item == null) && DecomposeDatabase.CanDecompose(item.TypeID);
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x00037390 File Offset: 0x00035590
	public static DecomposeFormula GetDecomposeFormula(int itemTypeID)
	{
		if (DecomposeDatabase.Instance == null)
		{
			return default(DecomposeFormula);
		}
		return DecomposeDatabase.Instance.GetFormula(itemTypeID);
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x000373BF File Offset: 0x000355BF
	public void SetData(List<DecomposeFormula> formulas)
	{
		this.entries = formulas.ToArray();
	}

	// Token: 0x04000B50 RID: 2896
	[SerializeField]
	private DecomposeFormula[] entries;

	// Token: 0x04000B51 RID: 2897
	private Dictionary<int, DecomposeFormula> _dic;
}
