using System;
using System.Collections.Generic;
using System.Linq;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023A RID: 570
	public class ItemShortcut : MonoBehaviour
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x00046084 File Offset: 0x00044284
		private static CharacterMainControl Master
		{
			get
			{
				return CharacterMainControl.Main;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x0004608B File Offset: 0x0004428B
		private static Inventory MainInventory
		{
			get
			{
				if (ItemShortcut.Master == null)
				{
					return null;
				}
				if (!ItemShortcut.Master.CharacterItem)
				{
					return null;
				}
				return ItemShortcut.Master.CharacterItem.Inventory;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x000460BE File Offset: 0x000442BE
		public static int MaxIndex
		{
			get
			{
				if (ItemShortcut.Instance == null)
				{
					return 0;
				}
				return ItemShortcut.Instance.maxIndex;
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x000460DC File Offset: 0x000442DC
		private void Awake()
		{
			if (ItemShortcut.Instance == null)
			{
				ItemShortcut.Instance = this;
			}
			else
			{
				Debug.LogError("检测到多个ItemShortcut");
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004613B File Offset: 0x0004433B
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00046170 File Offset: 0x00044370
		private void Start()
		{
			this.Load();
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00046178 File Offset: 0x00044378
		private void OnLevelInitialized()
		{
			this.Load();
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00046180 File Offset: 0x00044380
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00046188 File Offset: 0x00044388
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00046190 File Offset: 0x00044390
		private void Load()
		{
			ItemShortcut.SaveData saveData = SavesSystem.Load<ItemShortcut.SaveData>("ItemShortcut_Data");
			if (saveData == null)
			{
				return;
			}
			saveData.ApplyTo(this);
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x000461A8 File Offset: 0x000443A8
		private void Save()
		{
			ItemShortcut.SaveData saveData = new ItemShortcut.SaveData();
			saveData.Generate(this);
			SavesSystem.Save<ItemShortcut.SaveData>("ItemShortcut_Data", saveData);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x000461D0 File Offset: 0x000443D0
		public static bool IsItemValid(Item item)
		{
			return !(item == null) && !(ItemShortcut.MainInventory == null) && !(ItemShortcut.MainInventory != item.InInventory) && !item.Tags.Contains("Weapon");
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00046220 File Offset: 0x00044420
		private bool Set_Local(int index, Item item)
		{
			if (ItemShortcut.Master == null)
			{
				return false;
			}
			if (index < 0 || index > this.maxIndex)
			{
				return false;
			}
			if (!ItemShortcut.IsItemValid(item))
			{
				return false;
			}
			while (this.items.Count <= index)
			{
				this.items.Add(null);
			}
			while (this.itemTypes.Count <= index)
			{
				this.itemTypes.Add(-1);
			}
			this.items[index] = item;
			this.itemTypes[index] = item.TypeID;
			Action<int> onSetItem = ItemShortcut.OnSetItem;
			if (onSetItem != null)
			{
				onSetItem(index);
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (i != index)
				{
					bool flag = false;
					if (this.items[i] == item)
					{
						this.items[i] = null;
						flag = true;
					}
					if (this.itemTypes[i] == item.TypeID)
					{
						this.itemTypes[i] = -1;
						this.items[i] = null;
						flag = true;
					}
					if (flag)
					{
						ItemShortcut.OnSetItem(i);
					}
				}
			}
			return true;
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0004633C File Offset: 0x0004453C
		private Item Get_Local(int index)
		{
			if (index >= this.items.Count)
			{
				return null;
			}
			Item item = this.items[index];
			if (item == null)
			{
				item = ItemShortcut.MainInventory.Find(this.itemTypes[index]);
				if (item != null)
				{
					this.items[index] = item;
				}
			}
			if (!ItemShortcut.IsItemValid(item))
			{
				this.SetDirty(index);
				return null;
			}
			return item;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x000463AE File Offset: 0x000445AE
		private void SetDirty(int index)
		{
			this.dirtyIndexes.Add(index);
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x000463C0 File Offset: 0x000445C0
		private void Update()
		{
			if (this.dirtyIndexes.Count > 0)
			{
				foreach (int num in this.dirtyIndexes.ToArray<int>())
				{
					if (num < this.items.Count && !ItemShortcut.IsItemValid(this.items[num]))
					{
						this.items[num] = null;
						Action<int> onSetItem = ItemShortcut.OnSetItem;
						if (onSetItem != null)
						{
							onSetItem(num);
						}
					}
				}
				this.dirtyIndexes.Clear();
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x060011F5 RID: 4597 RVA: 0x00046444 File Offset: 0x00044644
		// (remove) Token: 0x060011F6 RID: 4598 RVA: 0x00046478 File Offset: 0x00044678
		public static event Action<int> OnSetItem;

		// Token: 0x060011F7 RID: 4599 RVA: 0x000464AB File Offset: 0x000446AB
		public static Item Get(int index)
		{
			if (ItemShortcut.Instance == null)
			{
				return null;
			}
			return ItemShortcut.Instance.Get_Local(index);
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000464C7 File Offset: 0x000446C7
		public static bool Set(int index, Item item)
		{
			return !(ItemShortcut.Instance == null) && ItemShortcut.Instance.Set_Local(index, item);
		}

		// Token: 0x04000DF4 RID: 3572
		public static ItemShortcut Instance;

		// Token: 0x04000DF5 RID: 3573
		[SerializeField]
		private int maxIndex = 3;

		// Token: 0x04000DF6 RID: 3574
		[SerializeField]
		private List<Item> items = new List<Item>();

		// Token: 0x04000DF7 RID: 3575
		[SerializeField]
		private List<int> itemTypes = new List<int>();

		// Token: 0x04000DF8 RID: 3576
		private const string SaveKey = "ItemShortcut_Data";

		// Token: 0x04000DF9 RID: 3577
		private HashSet<int> dirtyIndexes = new HashSet<int>();

		// Token: 0x02000549 RID: 1353
		[Serializable]
		private class SaveData
		{
			// Token: 0x17000789 RID: 1929
			// (get) Token: 0x060028D8 RID: 10456 RVA: 0x00095EAB File Offset: 0x000940AB
			public int Count
			{
				get
				{
					return this.inventoryIndexes.Count;
				}
			}

			// Token: 0x060028D9 RID: 10457 RVA: 0x00095EB8 File Offset: 0x000940B8
			public void Generate(ItemShortcut shortcut)
			{
				this.inventoryIndexes.Clear();
				Inventory mainInventory = ItemShortcut.MainInventory;
				if (mainInventory == null)
				{
					return;
				}
				for (int i = 0; i < shortcut.items.Count; i++)
				{
					Item item = shortcut.items[i];
					int index = mainInventory.GetIndex(item);
					this.inventoryIndexes.Add(index);
				}
			}

			// Token: 0x060028DA RID: 10458 RVA: 0x00095F18 File Offset: 0x00094118
			public void ApplyTo(ItemShortcut shortcut)
			{
				Inventory mainInventory = ItemShortcut.MainInventory;
				if (mainInventory == null)
				{
					return;
				}
				for (int i = 0; i < this.inventoryIndexes.Count; i++)
				{
					int num = this.inventoryIndexes[i];
					if (num >= 0)
					{
						Item itemAt = mainInventory.GetItemAt(num);
						shortcut.Set_Local(i, itemAt);
					}
				}
			}

			// Token: 0x04001F41 RID: 8001
			[SerializeField]
			internal List<int> inventoryIndexes = new List<int>();
		}
	}
}
