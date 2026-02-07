using System;
using System.Linq;
using Sirenix.Utilities;

// Token: 0x020001B2 RID: 434
public class InteractCrafter : InteractableBase
{
	// Token: 0x06000D04 RID: 3332 RVA: 0x000373D5 File Offset: 0x000355D5
	protected override void Awake()
	{
		base.Awake();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x000373E4 File Offset: 0x000355E4
	protected override void OnInteractFinished()
	{
		base.OnInteractFinished();
		CraftView.SetupAndOpenView(new Predicate<CraftingFormula>(this.FilterCraft));
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x000373FD File Offset: 0x000355FD
	private bool FilterCraft(CraftingFormula formula)
	{
		return StringExtensions.IsNullOrWhitespace(this.requireTag) || formula.tags.Contains(this.requireTag);
	}

	// Token: 0x04000B55 RID: 2901
	public string requireTag;
}
