using System;
using Duckov;
using Duckov.Quests;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class Condition_CharacterLevel : Condition
{
	// Token: 0x17000209 RID: 521
	// (get) Token: 0x060009BB RID: 2491 RVA: 0x0002B27C File Offset: 0x0002947C
	[LocalizationKey("Default")]
	private string DisplayTextFormatKey
	{
		get
		{
			switch (this.relation)
			{
			case Condition_CharacterLevel.Relation.LessThan:
				return "Condition_CharacterLevel_LessThan";
			case Condition_CharacterLevel.Relation.Equals:
				return "Condition_CharacterLevel_Equals";
			case Condition_CharacterLevel.Relation.GreaterThan:
				return "Condition_CharacterLevel_GreaterThan";
			}
			return "";
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x060009BC RID: 2492 RVA: 0x0002B2C1 File Offset: 0x000294C1
	private string DisplayTextFormat
	{
		get
		{
			return LocalizationManager.ToPlainText(this.DisplayTextFormatKey);
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x060009BD RID: 2493 RVA: 0x0002B2CE File Offset: 0x000294CE
	public override string DisplayText
	{
		get
		{
			return StringExtensions.Format(this.DisplayTextFormat, new
			{
				this.level
			});
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0002B2E8 File Offset: 0x000294E8
	public override bool Evaluate()
	{
		int num = EXPManager.Level;
		switch (this.relation)
		{
		case Condition_CharacterLevel.Relation.LessThan:
			return num <= this.level;
		case Condition_CharacterLevel.Relation.Equals:
			return num == this.level;
		case Condition_CharacterLevel.Relation.GreaterThan:
			return num >= this.level;
		}
		return false;
	}

	// Token: 0x040008B3 RID: 2227
	[SerializeField]
	private Condition_CharacterLevel.Relation relation;

	// Token: 0x040008B4 RID: 2228
	[SerializeField]
	private int level;

	// Token: 0x020004B6 RID: 1206
	private enum Relation
	{
		// Token: 0x04001CC1 RID: 7361
		LessThan = 1,
		// Token: 0x04001CC2 RID: 7362
		Equals,
		// Token: 0x04001CC3 RID: 7363
		GreaterThan = 4
	}
}
