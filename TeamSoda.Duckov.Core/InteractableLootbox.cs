using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000DE RID: 222
public class InteractableLootbox : InteractableBase
{
	// Token: 0x1700014E RID: 334
	// (get) Token: 0x0600071C RID: 1820 RVA: 0x000202EB File Offset: 0x0001E4EB
	public bool ShowSortButton
	{
		get
		{
			return this.showSortButton;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x0600071D RID: 1821 RVA: 0x000202F3 File Offset: 0x0001E4F3
	public bool UsePages
	{
		get
		{
			return this.usePages;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x0600071E RID: 1822 RVA: 0x000202FB File Offset: 0x0001E4FB
	public static Transform LootBoxInventoriesParent
	{
		get
		{
			return LevelManager.LootBoxInventoriesParent;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x0600071F RID: 1823 RVA: 0x00020302 File Offset: 0x0001E502
	public static Dictionary<int, Inventory> Inventories
	{
		get
		{
			return LevelManager.LootBoxInventories;
		}
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0002030C File Offset: 0x0001E50C
	public static Inventory GetOrCreateInventory(InteractableLootbox lootBox)
	{
		if (lootBox == null)
		{
			if (CharacterMainControl.Main != null)
			{
				CharacterMainControl.Main.PopText("ERROR:尝试创建Inventory, 但lootbox是null", -1f);
			}
			Debug.LogError("尝试创建Inventory, 但lootbox是null");
			return null;
		}
		int key = lootBox.GetKey();
		Inventory inventory;
		if (InteractableLootbox.Inventories.TryGetValue(key, out inventory))
		{
			if (!(inventory == null))
			{
				return inventory;
			}
			CharacterMainControl.Main.PopText(string.Format("Inventory缓存字典里有Key: {0}, 但其对应值为null.重新创建Inventory。", key), -1f);
			Debug.LogError(string.Format("Inventory缓存字典里有Key: {0}, 但其对应值为null.重新创建Inventory。", key));
		}
		GameObject gameObject = new GameObject(string.Format("Inventory_{0}", key));
		gameObject.transform.SetParent(InteractableLootbox.LootBoxInventoriesParent);
		gameObject.transform.position = lootBox.transform.position;
		inventory = gameObject.AddComponent<Inventory>();
		inventory.NeedInspection = lootBox.needInspect;
		InteractableLootbox.Inventories.Add(key, inventory);
		LootBoxLoader component = lootBox.GetComponent<LootBoxLoader>();
		if (component && component.autoSetup)
		{
			component.Setup().Forget();
		}
		return inventory;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00020424 File Offset: 0x0001E624
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int num = Mathf.RoundToInt(vector.x);
		int num2 = Mathf.RoundToInt(vector.y);
		int num3 = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int;
		vector3Int..ctor(num, num2, num3);
		return vector3Int.GetHashCode();
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000722 RID: 1826 RVA: 0x00020480 File Offset: 0x0001E680
	public Inventory Inventory
	{
		get
		{
			Inventory orCreateInventory;
			if (this.inventoryReference)
			{
				orCreateInventory = this.inventoryReference;
			}
			else
			{
				orCreateInventory = InteractableLootbox.GetOrCreateInventory(this);
				if (orCreateInventory == null)
				{
					if (LevelManager.Instance == null)
					{
						Debug.Log("LevelManager.Instance 不存在，取消创建inventory");
						return null;
					}
					if (this == null)
					{
						Debug.Log("Cannot create inventory now. Lootbox is probably destroyed.");
						return null;
					}
					LevelManager.Instance.MainCharacter.PopText("空的Inventory", -1f);
					Debug.LogError("未能成功创建Inventory," + base.gameObject.name, this);
				}
				this.inventoryReference = orCreateInventory;
			}
			if (this.inventoryReference && this.inventoryReference.hasBeenInspectedInLootBox)
			{
				base.SetMarkerUsed();
			}
			orCreateInventory.DisplayNameKey = this.displayNameKey;
			return orCreateInventory;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000723 RID: 1827 RVA: 0x00020545 File Offset: 0x0001E745
	public bool Looted
	{
		get
		{
			return LootView.HasInventoryEverBeenLooted(this.Inventory);
		}
	}

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06000724 RID: 1828 RVA: 0x00020554 File Offset: 0x0001E754
	// (remove) Token: 0x06000725 RID: 1829 RVA: 0x00020588 File Offset: 0x0001E788
	public static event Action<InteractableLootbox> OnStartLoot;

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06000726 RID: 1830 RVA: 0x000205BC File Offset: 0x0001E7BC
	// (remove) Token: 0x06000727 RID: 1831 RVA: 0x000205F0 File Offset: 0x0001E7F0
	public static event Action<InteractableLootbox> OnStopLoot;

	// Token: 0x06000728 RID: 1832 RVA: 0x00020624 File Offset: 0x0001E824
	protected override void Start()
	{
		base.Start();
		if (this.inventoryReference == null)
		{
			InteractableLootbox.GetOrCreateInventory(this);
		}
		if (this.Inventory && this.Inventory.hasBeenInspectedInLootBox)
		{
			base.SetMarkerUsed();
		}
		this.overrideInteractName = true;
		base.InteractName = this.displayNameKey;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x0002067F File Offset: 0x0001E87F
	protected override bool IsInteractable()
	{
		if (this.Inventory == null)
		{
			if (CharacterMainControl.Main)
			{
				CharacterMainControl.Main.PopText("ERROR :( 存在不包含Inventory的Lootbox。", -1f);
			}
			return false;
		}
		return this.lootState == InteractableLootbox.LootBoxStates.closed;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x000206BC File Offset: 0x0001E8BC
	protected override void OnUpdate(CharacterMainControl interactCharacter, float deltaTime)
	{
		if (this.Inventory == null)
		{
			base.StopInteract();
			if (LootView.Instance && LootView.Instance.open)
			{
				LootView.Instance.Close();
			}
			return;
		}
		switch (this.lootState)
		{
		case InteractableLootbox.LootBoxStates.closed:
			base.StopInteract();
			return;
		case InteractableLootbox.LootBoxStates.openning:
			if (interactCharacter.CurrentAction.ActionTimer >= base.InteractTime && !this.Inventory.Loading)
			{
				if (this.StartLoot())
				{
					this.lootState = InteractableLootbox.LootBoxStates.looting;
					return;
				}
				CharacterMainControl.Main.PopText("ERROR :Start loot失败，终止交互。", -1f);
				base.StopInteract();
				this.lootState = InteractableLootbox.LootBoxStates.closed;
				return;
			}
			break;
		case InteractableLootbox.LootBoxStates.looting:
			if (!LootView.Instance || !LootView.Instance.open)
			{
				CharacterMainControl.Main.PopText("ERROR :打开Loot界面失败，终止交互。", -1f);
				base.StopInteract();
				return;
			}
			if (this.inspectingItem != null)
			{
				this.inspectTimer += deltaTime;
				if (this.inspectTimer >= this.inspectTime)
				{
					this.inspectingItem.Inspected = true;
					this.inspectingItem.Inspecting = false;
				}
				if (!this.inspectingItem.Inspecting)
				{
					this.inspectingItem = null;
					return;
				}
			}
			else
			{
				Item item = this.FindFistUninspectedItem();
				if (!item)
				{
					base.StopInteract();
					return;
				}
				this.StartInspectItem(item);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00020820 File Offset: 0x0001EA20
	private void StartInspectItem(Item item)
	{
		if (item == null)
		{
			return;
		}
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		this.inspectingItem = item;
		this.inspectingItem.Inspecting = true;
		this.inspectTimer = 0f;
		this.inspectTime = GameplayDataSettings.LootingData.GetInspectingTime(item);
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0002087B File Offset: 0x0001EA7B
	private void UpdateInspect()
	{
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00020880 File Offset: 0x0001EA80
	private Item FindFistUninspectedItem()
	{
		if (!this.Inventory)
		{
			return null;
		}
		if (!this.Inventory.NeedInspection)
		{
			return null;
		}
		return this.Inventory.FirstOrDefault((Item e) => !e.Inspected);
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x000208D5 File Offset: 0x0001EAD5
	protected override void OnInteractStart(CharacterMainControl interactCharacter)
	{
		this.lootState = InteractableLootbox.LootBoxStates.openning;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x000208E0 File Offset: 0x0001EAE0
	protected override void OnInteractStop()
	{
		this.lootState = InteractableLootbox.LootBoxStates.closed;
		Action<InteractableLootbox> onStopLoot = InteractableLootbox.OnStopLoot;
		if (onStopLoot != null)
		{
			onStopLoot(this);
		}
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		if (this.Inventory)
		{
			this.Inventory.hasBeenInspectedInLootBox = true;
		}
		base.SetMarkerUsed();
		this.CheckHideIfEmpty();
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00020944 File Offset: 0x0001EB44
	protected override void OnInteractFinished()
	{
		base.OnInteractFinished();
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		this.CheckHideIfEmpty();
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002096C File Offset: 0x0001EB6C
	public void CheckHideIfEmpty()
	{
		if (!this.hideIfEmpty)
		{
			return;
		}
		if (this.Inventory.IsEmpty())
		{
			this.hideIfEmpty.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x0002099A File Offset: 0x0001EB9A
	private bool StartLoot()
	{
		if (this.Inventory == null)
		{
			base.StopInteract();
			Debug.LogError("开始loot失败，缺少inventory。");
			return false;
		}
		Action<InteractableLootbox> onStartLoot = InteractableLootbox.OnStartLoot;
		if (onStartLoot != null)
		{
			onStartLoot(this);
		}
		return true;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x000209D0 File Offset: 0x0001EBD0
	private void CreateLocalInventory()
	{
		Inventory inventory = base.gameObject.AddComponent<Inventory>();
		this.inventoryReference = inventory;
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000734 RID: 1844 RVA: 0x000209F0 File Offset: 0x0001EBF0
	public static InteractableLootbox Prefab
	{
		get
		{
			GameplayDataSettings.PrefabsData prefabs = GameplayDataSettings.Prefabs;
			if (prefabs == null)
			{
				return null;
			}
			return prefabs.LootBoxPrefab;
		}
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00020A04 File Offset: 0x0001EC04
	public static InteractableLootbox CreateFromItem(Item item, Vector3 position, Quaternion rotation, bool moveToMainScene = true, InteractableLootbox prefab = null, bool filterDontDropOnDead = false)
	{
		if (item == null)
		{
			Debug.LogError("正在尝试给一个不存在的Item创建LootBox，已取消。");
			return null;
		}
		if (prefab == null)
		{
			prefab = InteractableLootbox.Prefab;
		}
		if (prefab == null)
		{
			Debug.LogError("未配置LootBox的Prefab");
			return null;
		}
		InteractableLootbox interactableLootbox = Object.Instantiate<InteractableLootbox>(prefab, position, rotation);
		interactableLootbox.CreateLocalInventory();
		if (moveToMainScene)
		{
			MultiSceneCore.MoveToActiveWithScene(interactableLootbox.gameObject, SceneManager.GetActiveScene().buildIndex);
		}
		Inventory inventory = interactableLootbox.Inventory;
		if (inventory == null)
		{
			Debug.LogError("LootBox未配置Inventory");
			return interactableLootbox;
		}
		inventory.SetCapacity(512);
		List<Item> list = new List<Item>();
		if (item.Slots != null)
		{
			foreach (Slot slot in item.Slots)
			{
				Item content = slot.Content;
				if (!(content == null))
				{
					content.Inspected = true;
					if (content.Tags.Contains(GameplayDataSettings.Tags.DestroyOnLootBox))
					{
						ItemTreeExtensions.DestroyTree(content);
					}
					if (!filterDontDropOnDead || (!content.Tags.Contains(GameplayDataSettings.Tags.DontDropOnDeadInSlot) && !content.Sticky))
					{
						list.Add(content);
					}
				}
			}
		}
		if (item.Inventory != null)
		{
			foreach (Item item2 in item.Inventory)
			{
				if (!(item2 == null) && !item2.Tags.Contains(GameplayDataSettings.Tags.DestroyOnLootBox))
				{
					list.Add(item2);
				}
			}
		}
		foreach (Item item3 in list)
		{
			item3.Detach();
			inventory.AddAndMerge(item3, 0);
		}
		int capacity = Mathf.Max(8, inventory.GetLastItemPosition() + 1);
		inventory.SetCapacity(capacity);
		inventory.NeedInspection = prefab.needInspect;
		return interactableLootbox;
	}

	// Token: 0x040006DF RID: 1759
	public bool useDefaultInteractName;

	// Token: 0x040006E0 RID: 1760
	[SerializeField]
	private bool showSortButton;

	// Token: 0x040006E1 RID: 1761
	[SerializeField]
	private bool usePages;

	// Token: 0x040006E2 RID: 1762
	public bool needInspect = true;

	// Token: 0x040006E3 RID: 1763
	public bool showPickAllButton = true;

	// Token: 0x040006E4 RID: 1764
	public Transform hideIfEmpty;

	// Token: 0x040006E5 RID: 1765
	[LocalizationKey("Default")]
	[SerializeField]
	private string displayNameKey;

	// Token: 0x040006E6 RID: 1766
	[SerializeField]
	private Inventory inventoryReference;

	// Token: 0x040006E7 RID: 1767
	private Item inspectingItem;

	// Token: 0x040006E8 RID: 1768
	private float inspectTime = 1f;

	// Token: 0x040006E9 RID: 1769
	private float inspectTimer;

	// Token: 0x040006EA RID: 1770
	private InteractableLootbox.LootBoxStates lootState;

	// Token: 0x02000482 RID: 1154
	public enum LootBoxStates
	{
		// Token: 0x04001C06 RID: 7174
		closed,
		// Token: 0x04001C07 RID: 7175
		openning,
		// Token: 0x04001C08 RID: 7176
		looting
	}
}
