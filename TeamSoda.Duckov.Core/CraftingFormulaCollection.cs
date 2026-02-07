using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020001AE RID: 430
[CreateAssetMenu]
public class CraftingFormulaCollection : ScriptableObject
{
	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00036F11 File Offset: 0x00035111
	public static CraftingFormulaCollection Instance
	{
		get
		{
			return GameplayDataSettings.CraftingFormulas;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00036F18 File Offset: 0x00035118
	public ReadOnlyCollection<CraftingFormula> Entries
	{
		get
		{
			if (this._entries_ReadOnly == null)
			{
				this._entries_ReadOnly = new ReadOnlyCollection<CraftingFormula>(this.list);
			}
			return this._entries_ReadOnly;
		}
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x00036F3C File Offset: 0x0003513C
	public static bool TryGetFormula(string id, out CraftingFormula formula)
	{
		if (!(CraftingFormulaCollection.Instance == null))
		{
			CraftingFormula craftingFormula = CraftingFormulaCollection.Instance.list.FirstOrDefault((CraftingFormula e) => e.id == id);
			if (!string.IsNullOrEmpty(craftingFormula.id))
			{
				formula = craftingFormula;
				return true;
			}
		}
		formula = default(CraftingFormula);
		return false;
	}

	// Token: 0x04000B49 RID: 2889
	[SerializeField]
	private List<CraftingFormula> list;

	// Token: 0x04000B4A RID: 2890
	private ReadOnlyCollection<CraftingFormula> _entries_ReadOnly;
}
