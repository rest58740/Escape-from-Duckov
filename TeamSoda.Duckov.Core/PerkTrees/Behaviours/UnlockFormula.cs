using System;
using System.Collections.Generic;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x02000268 RID: 616
	public class UnlockFormula : PerkBehaviour
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x000498EF File Offset: 0x00047AEF
		private IEnumerable<string> FormulasToUnlock
		{
			get
			{
				if (!CraftingFormulaCollection.Instance)
				{
					yield break;
				}
				string matchKey = base.Master.Master.ID + "/" + base.Master.name;
				foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
				{
					if (craftingFormula.requirePerk == matchKey)
					{
						yield return craftingFormula.id;
					}
				}
				IEnumerator<CraftingFormula> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00049900 File Offset: 0x00047B00
		protected override void OnUnlocked()
		{
			foreach (string formulaID in this.FormulasToUnlock)
			{
				CraftingManager.UnlockFormula(formulaID);
			}
		}
	}
}
