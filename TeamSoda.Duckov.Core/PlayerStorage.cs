using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class PlayerStorage : MonoBehaviour, IInitializedQueryHandler
{
	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000875 RID: 2165 RVA: 0x00026752 File Offset: 0x00024952
	// (set) Token: 0x06000876 RID: 2166 RVA: 0x00026759 File Offset: 0x00024959
	public static PlayerStorage Instance { get; private set; }

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06000877 RID: 2167 RVA: 0x00026764 File Offset: 0x00024964
	// (remove) Token: 0x06000878 RID: 2168 RVA: 0x00026798 File Offset: 0x00024998
	public static event Action<PlayerStorage, Inventory, int> OnPlayerStorageChange;

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x000267CB File Offset: 0x000249CB
	public static Inventory Inventory
	{
		get
		{
			if (PlayerStorage.Instance == null)
			{
				return null;
			}
			return PlayerStorage.Instance.inventory;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x000267E6 File Offset: 0x000249E6
	public static List<ItemTreeData> IncomingItemBuffer
	{
		get
		{
			return PlayerStorageBuffer.Buffer;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x0600087B RID: 2171 RVA: 0x000267ED File Offset: 0x000249ED
	public InteractableLootbox InteractableLootBox
	{
		get
		{
			return this.interactable;
		}
	}

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x0600087C RID: 2172 RVA: 0x000267F8 File Offset: 0x000249F8
	// (remove) Token: 0x0600087D RID: 2173 RVA: 0x0002682C File Offset: 0x00024A2C
	public static event Action<PlayerStorage.StorageCapacityCalculationHolder> OnRecalculateStorageCapacity;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x0600087E RID: 2174 RVA: 0x00026860 File Offset: 0x00024A60
	// (remove) Token: 0x0600087F RID: 2175 RVA: 0x00026894 File Offset: 0x00024A94
	public static event Action OnTakeBufferItem;

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06000880 RID: 2176 RVA: 0x000268C8 File Offset: 0x00024AC8
	// (remove) Token: 0x06000881 RID: 2177 RVA: 0x000268FC File Offset: 0x00024AFC
	public static event Action<Item> OnItemAddedToBuffer;

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06000882 RID: 2178 RVA: 0x00026930 File Offset: 0x00024B30
	// (remove) Token: 0x06000883 RID: 2179 RVA: 0x00026964 File Offset: 0x00024B64
	public static event Action OnLoadingFinished;

	// Token: 0x06000884 RID: 2180 RVA: 0x00026997 File Offset: 0x00024B97
	public static bool IsAccessableAndNotFull()
	{
		return !(PlayerStorage.Instance == null) && !(PlayerStorage.Inventory == null) && PlayerStorage.Inventory.GetFirstEmptyPosition(0) >= 0;
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x000269C8 File Offset: 0x00024BC8
	public int DefaultCapacity
	{
		get
		{
			return this.defaultCapacity;
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x000269D0 File Offset: 0x00024BD0
	public static void NotifyCapacityDirty()
	{
		PlayerStorage.needRecalculateCapacity = true;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x000269D8 File Offset: 0x00024BD8
	private void Awake()
	{
		if (PlayerStorage.Instance == null)
		{
			PlayerStorage.Instance = this;
		}
		if (PlayerStorage.Instance != this)
		{
			Debug.LogError("发现了多个Player Storage!");
			return;
		}
		if (this.interactable == null)
		{
			this.interactable = base.GetComponent<InteractableLootbox>();
		}
		this.inventory.onContentChanged += this.OnInventoryContentChanged;
		SavesSystem.OnCollectSaveData += this.SavesSystem_OnCollectSaveData;
		LevelManager.RegisterWaitForInitialization<PlayerStorage>(this);
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00026A58 File Offset: 0x00024C58
	private void Start()
	{
		this.Load().Forget();
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00026A65 File Offset: 0x00024C65
	private void OnDestroy()
	{
		this.inventory.onContentChanged -= this.OnInventoryContentChanged;
		SavesSystem.OnCollectSaveData -= this.SavesSystem_OnCollectSaveData;
		LevelManager.UnregisterWaitForInitialization<PlayerStorage>(this);
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00026A96 File Offset: 0x00024C96
	private void SavesSystem_OnSetFile()
	{
		this.Load().Forget();
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00026AA3 File Offset: 0x00024CA3
	private void SavesSystem_OnCollectSaveData()
	{
		this.Save();
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00026AAB File Offset: 0x00024CAB
	private void OnInventoryContentChanged(Inventory inventory, int index)
	{
		Action<PlayerStorage, Inventory, int> onPlayerStorageChange = PlayerStorage.OnPlayerStorageChange;
		if (onPlayerStorageChange == null)
		{
			return;
		}
		onPlayerStorageChange(this, inventory, index);
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x00026AC0 File Offset: 0x00024CC0
	public static void Push(Item item, bool toBufferDirectly = false)
	{
		if (item == null)
		{
			return;
		}
		if (!toBufferDirectly && PlayerStorage.Inventory != null)
		{
			if (item.Stackable)
			{
				Func<Item, bool> <>9__0;
				while (item.StackCount > 0)
				{
					IEnumerable<Item> source = PlayerStorage.Inventory;
					Func<Item, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Item e) => e.TypeID == item.TypeID && e.MaxStackCount > e.StackCount));
					}
					Item item2 = source.FirstOrDefault(predicate);
					if (item2 == null)
					{
						break;
					}
					item2.Combine(item);
				}
			}
			if (item != null && item.StackCount > 0)
			{
				int firstEmptyPosition = PlayerStorage.Inventory.GetFirstEmptyPosition(0);
				if (firstEmptyPosition >= 0)
				{
					PlayerStorage.Inventory.AddAt(item, firstEmptyPosition);
					return;
				}
			}
		}
		NotificationText.Push(StringExtensions.Format(LocalizationManager.ToPlainText("PlayerStorage_Notification_ItemAddedToBuffer"), new
		{
			displayName = item.DisplayName
		}));
		PlayerStorage.IncomingItemBuffer.Add(ItemTreeData.FromItem(item));
		item.Detach();
		ItemTreeExtensions.DestroyTree(item);
		Action<Item> onItemAddedToBuffer = PlayerStorage.OnItemAddedToBuffer;
		if (onItemAddedToBuffer == null)
		{
			return;
		}
		onItemAddedToBuffer(item);
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x00026BFE File Offset: 0x00024DFE
	private void Save()
	{
		if (PlayerStorage.Loading)
		{
			return;
		}
		this.inventory.Save("PlayerStorage");
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x0600088F RID: 2191 RVA: 0x00026C18 File Offset: 0x00024E18
	// (set) Token: 0x06000890 RID: 2192 RVA: 0x00026C1F File Offset: 0x00024E1F
	public static bool Loading { get; private set; }

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000891 RID: 2193 RVA: 0x00026C27 File Offset: 0x00024E27
	// (set) Token: 0x06000892 RID: 2194 RVA: 0x00026C2E File Offset: 0x00024E2E
	public static bool TakingItem { get; private set; }

	// Token: 0x06000893 RID: 2195 RVA: 0x00026C38 File Offset: 0x00024E38
	private UniTask Load()
	{
		PlayerStorage.<Load>d__52 <Load>d__;
		<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Load>d__.<>4__this = this;
		<Load>d__.<>1__state = -1;
		<Load>d__.<>t__builder.Start<PlayerStorage.<Load>d__52>(ref <Load>d__);
		return <Load>d__.<>t__builder.Task;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00026C7B File Offset: 0x00024E7B
	private void Update()
	{
		if (PlayerStorage.needRecalculateCapacity)
		{
			PlayerStorage.RecalculateStorageCapacity();
		}
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00026C8C File Offset: 0x00024E8C
	public static int RecalculateStorageCapacity()
	{
		if (PlayerStorage.Instance == null)
		{
			return 0;
		}
		PlayerStorage.StorageCapacityCalculationHolder storageCapacityCalculationHolder = new PlayerStorage.StorageCapacityCalculationHolder();
		storageCapacityCalculationHolder.capacity = PlayerStorage.Instance.DefaultCapacity;
		Action<PlayerStorage.StorageCapacityCalculationHolder> onRecalculateStorageCapacity = PlayerStorage.OnRecalculateStorageCapacity;
		if (onRecalculateStorageCapacity != null)
		{
			onRecalculateStorageCapacity(storageCapacityCalculationHolder);
		}
		int capacity = storageCapacityCalculationHolder.capacity;
		PlayerStorage.Instance.SetCapacity(capacity);
		PlayerStorage.needRecalculateCapacity = false;
		return capacity;
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00026CE8 File Offset: 0x00024EE8
	private void SetCapacity(int capacity)
	{
		this.inventory.SetCapacity(capacity);
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x00026CF8 File Offset: 0x00024EF8
	public static UniTask TakeBufferItem(int index)
	{
		PlayerStorage.<TakeBufferItem>d__56 <TakeBufferItem>d__;
		<TakeBufferItem>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<TakeBufferItem>d__.index = index;
		<TakeBufferItem>d__.<>1__state = -1;
		<TakeBufferItem>d__.<>t__builder.Start<PlayerStorage.<TakeBufferItem>d__56>(ref <TakeBufferItem>d__);
		return <TakeBufferItem>d__.<>t__builder.Task;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00026D3B File Offset: 0x00024F3B
	public bool HasInitialized()
	{
		return this.initialized;
	}

	// Token: 0x040007D0 RID: 2000
	[SerializeField]
	private Inventory inventory;

	// Token: 0x040007D1 RID: 2001
	[SerializeField]
	private InteractableLootbox interactable;

	// Token: 0x040007D6 RID: 2006
	[SerializeField]
	private int defaultCapacity = 32;

	// Token: 0x040007D7 RID: 2007
	private static bool needRecalculateCapacity;

	// Token: 0x040007D8 RID: 2008
	private const string inventorySaveKey = "PlayerStorage";

	// Token: 0x040007DB RID: 2011
	private bool initialized;

	// Token: 0x0200049B RID: 1179
	public class StorageCapacityCalculationHolder
	{
		// Token: 0x04001C54 RID: 7252
		public int capacity;
	}
}
