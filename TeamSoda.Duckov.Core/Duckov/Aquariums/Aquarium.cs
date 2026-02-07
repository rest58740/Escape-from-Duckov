using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.Aquariums
{
	// Token: 0x02000336 RID: 822
	public class Aquarium : MonoBehaviour
	{
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001B83 RID: 7043 RVA: 0x00063FF9 File Offset: 0x000621F9
		private string ItemSaveKey
		{
			get
			{
				return "Aquarium/Item/" + this.id;
			}
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x0006400B File Offset: 0x0006220B
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0006401E File Offset: 0x0006221E
		private void Start()
		{
			this.Load().Forget();
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x0006402B File Offset: 0x0006222B
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x00064040 File Offset: 0x00062240
		private UniTask Load()
		{
			Aquarium.<Load>d__14 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<Aquarium.<Load>d__14>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x00064083 File Offset: 0x00062283
		private void OnChildChanged(Item item)
		{
			this.dirty = true;
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x0006408C File Offset: 0x0006228C
		private void FixedUpdate()
		{
			if (this.loading)
			{
				return;
			}
			if (this.dirty)
			{
				this.Refresh();
				this.dirty = false;
			}
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000640AC File Offset: 0x000622AC
		private void Refresh()
		{
			if (this.aquariumItem == null)
			{
				return;
			}
			foreach (Item item in ItemTreeExtensions.GetAllChildren(this.aquariumItem, false, true))
			{
				if (!(item == null) && item.Tags.Contains("Fish"))
				{
					this.GetOrCreateGraphic(item) == null;
				}
			}
			this.graphicRecords.RemoveAll((Aquarium.ItemGraphicPair e) => e == null || e.graphic == null);
			for (int i = 0; i < this.graphicRecords.Count; i++)
			{
				Aquarium.ItemGraphicPair itemGraphicPair = this.graphicRecords[i];
				if (itemGraphicPair.item == null || itemGraphicPair.item.ParentItem != this.aquariumItem)
				{
					if (itemGraphicPair.graphic != null)
					{
						Object.Destroy(itemGraphicPair.graphic);
					}
					this.graphicRecords.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000641D4 File Offset: 0x000623D4
		private ItemGraphicInfo GetOrCreateGraphic(Item item)
		{
			if (item == null)
			{
				return null;
			}
			Aquarium.ItemGraphicPair itemGraphicPair = this.graphicRecords.Find((Aquarium.ItemGraphicPair e) => e != null && e.item == item);
			if (itemGraphicPair != null && itemGraphicPair.graphic != null)
			{
				return itemGraphicPair.graphic;
			}
			ItemGraphicInfo itemGraphicInfo = ItemGraphicInfo.CreateAGraphic(item, this.graphicsParent);
			if (itemGraphicPair != null)
			{
				this.graphicRecords.Remove(itemGraphicPair);
			}
			if (itemGraphicInfo == null)
			{
				return null;
			}
			IAquariumContent component = itemGraphicInfo.GetComponent<IAquariumContent>();
			if (component != null)
			{
				component.Setup(this);
			}
			this.graphicRecords.Add(new Aquarium.ItemGraphicPair
			{
				item = item,
				graphic = itemGraphicInfo
			});
			return itemGraphicInfo;
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x00064290 File Offset: 0x00062490
		public void Loot()
		{
			LootView.LootItem(this.aquariumItem);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0006429D File Offset: 0x0006249D
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			if (!this.loaded)
			{
				return;
			}
			this.aquariumItem.Save(this.ItemSaveKey);
		}

		// Token: 0x040013B4 RID: 5044
		[SerializeField]
		private string id = "Default";

		// Token: 0x040013B5 RID: 5045
		[SerializeField]
		private Transform graphicsParent;

		// Token: 0x040013B6 RID: 5046
		[ItemTypeID]
		private int aquariumItemTypeID = 1158;

		// Token: 0x040013B7 RID: 5047
		private Item aquariumItem;

		// Token: 0x040013B8 RID: 5048
		private List<Aquarium.ItemGraphicPair> graphicRecords = new List<Aquarium.ItemGraphicPair>();

		// Token: 0x040013B9 RID: 5049
		private bool loading;

		// Token: 0x040013BA RID: 5050
		private bool loaded;

		// Token: 0x040013BB RID: 5051
		private int loadToken;

		// Token: 0x040013BC RID: 5052
		private bool dirty = true;

		// Token: 0x020005E1 RID: 1505
		private class ItemGraphicPair
		{
			// Token: 0x04002167 RID: 8551
			public Item item;

			// Token: 0x04002168 RID: 8552
			public ItemGraphicInfo graphic;
		}
	}
}
