using System;
using Duckov.Quests;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class Condition_XiaoHeiZi : Condition
{
	// Token: 0x1700020C RID: 524
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0002B3D9 File Offset: 0x000295D9
	public override string DisplayText
	{
		get
		{
			return "看看你是不是小黑子";
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0002B3E0 File Offset: 0x000295E0
	public override bool Evaluate()
	{
		if (CharacterMainControl.Main == null)
		{
			return false;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		CharacterModel characterModel = main.characterModel;
		if (!characterModel)
		{
			return false;
		}
		CustomFaceInstance customFace = characterModel.CustomFace;
		if (!customFace)
		{
			return false;
		}
		if (customFace.ConvertToSaveData().hairID != this.hairID)
		{
			return false;
		}
		Item armorItem = main.GetArmorItem();
		return !(armorItem == null) && armorItem.TypeID == this.armorID;
	}

	// Token: 0x040008B8 RID: 2232
	[SerializeField]
	private int hairID = 6;

	// Token: 0x040008B9 RID: 2233
	[ItemTypeID]
	[SerializeField]
	private int armorID = 379;
}
