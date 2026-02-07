using System;
using Duckov;
using Duckov.Quests;
using Duckov.UI;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class SpaceShipInstaller : MonoBehaviour
{
	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000613 RID: 1555 RVA: 0x0001B505 File Offset: 0x00019705
	// (set) Token: 0x06000614 RID: 1556 RVA: 0x0001B512 File Offset: 0x00019712
	private bool Installed
	{
		get
		{
			return SavesSystem.Load<bool>(this.saveDataKey);
		}
		set
		{
			SavesSystem.Save<bool>(this.saveDataKey, value);
		}
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0001B520 File Offset: 0x00019720
	private void Awake()
	{
		if (this.buildFx)
		{
			this.buildFx.SetActive(false);
		}
		this.interactable.overrideInteractName = true;
		this.interactable._overrideInteractNameKey = this.interactKey;
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0001B558 File Offset: 0x00019758
	public void Install()
	{
		if (this.buildFx)
		{
			this.buildFx.SetActive(true);
		}
		AudioManager.Post("Archived/Building/Default/Constructed", base.gameObject);
		this.Installed = true;
		this.SyncGraphic(true);
		this.interactable.gameObject.SetActive(false);
		NotificationText.Push(LocalizationManager.ToPlainText(this.notificationKey));
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0001B5BE File Offset: 0x000197BE
	private void SyncGraphic(bool _installed)
	{
		if (this.builtGraphic)
		{
			this.builtGraphic.SetActive(_installed);
		}
		if (this.unbuiltGraphic)
		{
			this.unbuiltGraphic.SetActive(!_installed);
		}
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0001B5F8 File Offset: 0x000197F8
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.inited)
		{
			bool flag = this.Installed;
			if (flag)
			{
				TaskEvent.EmitTaskEvent(this.saveDataKey);
			}
			else if (QuestManager.IsQuestFinished(this.questID))
			{
				flag = true;
				this.Installed = true;
			}
			this.interactable.gameObject.SetActive(!flag && QuestManager.IsQuestActive(this.questID));
			this.SyncGraphic(flag);
			this.inited = true;
		}
		if (!this.Installed && !this.interactable.gameObject.activeSelf && QuestManager.IsQuestActive(this.questID))
		{
			this.interactable.gameObject.SetActive(true);
		}
	}

	// Token: 0x040005A7 RID: 1447
	[SerializeField]
	private string saveDataKey;

	// Token: 0x040005A8 RID: 1448
	[SerializeField]
	private int questID;

	// Token: 0x040005A9 RID: 1449
	[SerializeField]
	private InteractableBase interactable;

	// Token: 0x040005AA RID: 1450
	[SerializeField]
	[LocalizationKey("Default")]
	private string notificationKey;

	// Token: 0x040005AB RID: 1451
	[SerializeField]
	[LocalizationKey("Default")]
	private string interactKey;

	// Token: 0x040005AC RID: 1452
	private bool inited;

	// Token: 0x040005AD RID: 1453
	public GameObject builtGraphic;

	// Token: 0x040005AE RID: 1454
	public GameObject unbuiltGraphic;

	// Token: 0x040005AF RID: 1455
	public GameObject buildFx;
}
