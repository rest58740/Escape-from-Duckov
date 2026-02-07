using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000AA RID: 170
public class KunEvents : MonoBehaviour
{
	// Token: 0x060005DB RID: 1499 RVA: 0x0001A4EE File Offset: 0x000186EE
	private void Awake()
	{
		this.setActiveObject.SetActive(false);
		if (!this.dialogueBubbleProxy)
		{
			this.dialogueBubbleProxy.GetComponent<DialogueBubbleProxy>();
		}
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0001A518 File Offset: 0x00018718
	public void Check()
	{
		bool flag = false;
		if (CharacterMainControl.Main == null)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		CharacterModel characterModel = main.characterModel;
		if (!characterModel)
		{
			return;
		}
		CustomFaceInstance customFace = characterModel.CustomFace;
		if (!customFace)
		{
			return;
		}
		bool flag2 = customFace.ConvertToSaveData().hairID == this.hairID;
		Item armorItem = main.GetArmorItem();
		if (armorItem != null && armorItem.TypeID == this.armorID)
		{
			flag = true;
		}
		if (!flag2 && !flag)
		{
			this.dialogueBubbleProxy.textKey = this.notRight;
		}
		else if (flag2 && !flag)
		{
			this.dialogueBubbleProxy.textKey = this.onlyRightFace;
		}
		else if (!flag2 && flag)
		{
			this.dialogueBubbleProxy.textKey = this.onlyRightCloth;
		}
		else
		{
			this.dialogueBubbleProxy.textKey = this.allRight;
			this.setActiveObject.SetActive(true);
		}
		this.dialogueBubbleProxy.Pop();
	}

	// Token: 0x04000561 RID: 1377
	[SerializeField]
	private int hairID = 6;

	// Token: 0x04000562 RID: 1378
	[ItemTypeID]
	[SerializeField]
	private int armorID;

	// Token: 0x04000563 RID: 1379
	public DialogueBubbleProxy dialogueBubbleProxy;

	// Token: 0x04000564 RID: 1380
	[LocalizationKey("Dialogues")]
	public string notRight;

	// Token: 0x04000565 RID: 1381
	[LocalizationKey("Dialogues")]
	public string onlyRightFace;

	// Token: 0x04000566 RID: 1382
	[LocalizationKey("Dialogues")]
	public string onlyRightCloth;

	// Token: 0x04000567 RID: 1383
	[LocalizationKey("Dialogues")]
	public string allRight;

	// Token: 0x04000568 RID: 1384
	[FormerlySerializedAs("SetActiveObject")]
	public GameObject setActiveObject;
}
