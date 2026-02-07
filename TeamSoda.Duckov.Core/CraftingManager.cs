using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class CraftingManager : MonoBehaviour
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00036FA5 File Offset: 0x000351A5
	private static CraftingFormulaCollection FormulaCollection
	{
		get
		{
			return CraftingFormulaCollection.Instance;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000CED RID: 3309 RVA: 0x00036FAC File Offset: 0x000351AC
	// (set) Token: 0x06000CEE RID: 3310 RVA: 0x00036FB3 File Offset: 0x000351B3
	public static CraftingManager Instance { get; private set; }

	// Token: 0x06000CEF RID: 3311 RVA: 0x00036FBB File Offset: 0x000351BB
	private void Awake()
	{
		CraftingManager.Instance = this;
		this.Load();
		SavesSystem.OnCollectSaveData += this.Save;
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x00036FDA File Offset: 0x000351DA
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00036FED File Offset: 0x000351ED
	private void Save()
	{
		SavesSystem.Save<List<string>>("Crafting/UnlockedFormulaIDs", this.unlockedFormulaIDs);
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x00037000 File Offset: 0x00035200
	private void Load()
	{
		this.unlockedFormulaIDs = SavesSystem.Load<List<string>>("Crafting/UnlockedFormulaIDs");
		if (this.unlockedFormulaIDs == null)
		{
			this.unlockedFormulaIDs = new List<string>();
		}
		foreach (CraftingFormula craftingFormula in CraftingManager.FormulaCollection.Entries)
		{
			if (craftingFormula.unlockByDefault && !this.unlockedFormulaIDs.Contains(craftingFormula.id))
			{
				this.unlockedFormulaIDs.Add(craftingFormula.id);
			}
		}
		this.unlockedFormulaIDs.Sort();
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x000370A4 File Offset: 0x000352A4
	public static IEnumerable<string> UnlockedFormulaIDs
	{
		get
		{
			if (!(CraftingManager.Instance == null))
			{
				foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
				{
					if (CraftingManager.IsFormulaUnlocked(craftingFormula.id))
					{
						yield return craftingFormula.id;
					}
				}
				IEnumerator<CraftingFormula> enumerator = null;
			}
			yield break;
			yield break;
		}
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x000370B0 File Offset: 0x000352B0
	public static void UnlockFormula(string formulaID)
	{
		if (CraftingManager.Instance == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(formulaID))
		{
			Debug.LogError("Invalid formula ID");
			return;
		}
		CraftingFormula craftingFormula = CraftingManager.FormulaCollection.Entries.FirstOrDefault((CraftingFormula e) => e.id == formulaID);
		if (!craftingFormula.IDValid)
		{
			Debug.LogError("Invalid formula ID: " + formulaID);
			return;
		}
		if (craftingFormula.unlockByDefault)
		{
			Debug.LogError("Formula is unlocked by default: " + formulaID);
			return;
		}
		if (CraftingManager.Instance.unlockedFormulaIDs.Contains(formulaID))
		{
			return;
		}
		CraftingManager.Instance.unlockedFormulaIDs.Add(formulaID);
		Action<string> onFormulaUnlocked = CraftingManager.OnFormulaUnlocked;
		if (onFormulaUnlocked == null)
		{
			return;
		}
		onFormulaUnlocked(formulaID);
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x0003718C File Offset: 0x0003538C
	private UniTask<List<Item>> Craft(CraftingFormula formula)
	{
		CraftingManager.<Craft>d__17 <Craft>d__;
		<Craft>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Craft>d__.formula = formula;
		<Craft>d__.<>1__state = -1;
		<Craft>d__.<>t__builder.Start<CraftingManager.<Craft>d__17>(ref <Craft>d__);
		return <Craft>d__.<>t__builder.Task;
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x000371D0 File Offset: 0x000353D0
	public UniTask<List<Item>> Craft(string id)
	{
		CraftingManager.<Craft>d__18 <Craft>d__;
		<Craft>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Craft>d__.<>4__this = this;
		<Craft>d__.id = id;
		<Craft>d__.<>1__state = -1;
		<Craft>d__.<>t__builder.Start<CraftingManager.<Craft>d__18>(ref <Craft>d__);
		return <Craft>d__.<>t__builder.Task;
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x0003721B File Offset: 0x0003541B
	internal static bool IsFormulaUnlocked(string value)
	{
		return !(CraftingManager.Instance == null) && !string.IsNullOrEmpty(value) && CraftingManager.Instance.unlockedFormulaIDs.Contains(value);
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00037248 File Offset: 0x00035448
	internal static CraftingFormula GetFormula(string id)
	{
		CraftingFormula result;
		if (CraftingFormulaCollection.TryGetFormula(id, out result))
		{
			return result;
		}
		return default(CraftingFormula);
	}

	// Token: 0x04000B4B RID: 2891
	public static Action<CraftingFormula, Item> OnItemCrafted;

	// Token: 0x04000B4C RID: 2892
	public static Action<string> OnFormulaUnlocked;

	// Token: 0x04000B4E RID: 2894
	private const string SaveKey = "Crafting/UnlockedFormulaIDs";

	// Token: 0x04000B4F RID: 2895
	private List<string> unlockedFormulaIDs = new List<string>();
}
