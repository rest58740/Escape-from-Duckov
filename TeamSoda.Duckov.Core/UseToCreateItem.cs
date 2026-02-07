using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class UseToCreateItem : UsageBehavior
{
	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00026F74 File Offset: 0x00025174
	public override UsageBehavior.DisplaySettingsData DisplaySettings
	{
		get
		{
			UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
			result.display = true;
			result.description = LocalizationManager.ToPlainText(this.descKey);
			return result;
		}
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00026FA4 File Offset: 0x000251A4
	public override bool CanBeUsed(Item item, object user)
	{
		return user as CharacterMainControl;
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00026FB8 File Offset: 0x000251B8
	protected override void OnUse(Item item, object user)
	{
		CharacterMainControl characterMainControl = user as CharacterMainControl;
		if (!characterMainControl)
		{
			return;
		}
		if (this.entries.entries.Count == 0)
		{
			return;
		}
		UseToCreateItem.Entry random = this.entries.GetRandom(0f);
		this.Generate(random.itemTypeID, characterMainControl).Forget();
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0002700C File Offset: 0x0002520C
	private UniTask Generate(int typeID, CharacterMainControl character)
	{
		UseToCreateItem.<Generate>d__9 <Generate>d__;
		<Generate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Generate>d__.<>4__this = this;
		<Generate>d__.typeID = typeID;
		<Generate>d__.character = character;
		<Generate>d__.<>1__state = -1;
		<Generate>d__.<>t__builder.Start<UseToCreateItem.<Generate>d__9>(ref <Generate>d__);
		return <Generate>d__.<>t__builder.Task;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0002705F File Offset: 0x0002525F
	private void OnValidate()
	{
		this.entries.RefreshPercent();
	}

	// Token: 0x040007EB RID: 2027
	[SerializeField]
	private RandomContainer<UseToCreateItem.Entry> entries;

	// Token: 0x040007EC RID: 2028
	[LocalizationKey("Items")]
	public string descKey;

	// Token: 0x040007ED RID: 2029
	[LocalizationKey("Default")]
	public string notificationKey;

	// Token: 0x040007EE RID: 2030
	private bool running;

	// Token: 0x020004A1 RID: 1185
	[Serializable]
	private struct Entry
	{
		// Token: 0x04001C68 RID: 7272
		[ItemTypeID]
		[SerializeField]
		public int itemTypeID;
	}
}
