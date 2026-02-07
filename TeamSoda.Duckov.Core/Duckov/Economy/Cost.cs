using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x0200033C RID: 828
	[Serializable]
	public struct Cost
	{
		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001BDE RID: 7134 RVA: 0x00065371 File Offset: 0x00063571
		public bool Enough
		{
			get
			{
				return EconomyManager.IsEnough(this, true, true);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x00065380 File Offset: 0x00063580
		public bool IsFree
		{
			get
			{
				return this.money <= 0L && (this.items == null || this.items.Length == 0);
			}
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000653A2 File Offset: 0x000635A2
		public bool Pay(bool accountAvaliable = true, bool cashAvaliable = true)
		{
			return EconomyManager.Pay(this, accountAvaliable, cashAvaliable);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000653B4 File Offset: 0x000635B4
		public static Cost FromString(string costDescription)
		{
			int num = 0;
			List<Cost.ItemEntry> list = new List<Cost.ItemEntry>();
			foreach (string text in costDescription.Split(',', StringSplitOptions.None))
			{
				string[] array2 = text.Split(":", StringSplitOptions.None);
				if (array2.Length != 2)
				{
					Debug.LogError("Invalid cost description: " + text + "\n" + costDescription);
				}
				else
				{
					string text2 = array2[0].Trim();
					int num2;
					if (!int.TryParse(array2[1].Trim(), out num2))
					{
						Debug.LogError("Invalid cost description: " + text);
					}
					else if (text2 == "money")
					{
						num = num2;
					}
					else
					{
						int num3 = ItemAssetsCollection.TryGetIDByName(text2, false);
						if (num3 <= 0)
						{
							Debug.LogError("Invalid item name " + text2);
						}
						else
						{
							list.Add(new Cost.ItemEntry
							{
								id = num3,
								amount = (long)num2
							});
						}
					}
				}
			}
			return new Cost
			{
				money = (long)num,
				items = list.ToArray()
			};
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x000654C6 File Offset: 0x000636C6
		public static bool TaskPending
		{
			get
			{
				return Cost.ReturnTaskLocks.Count > 0;
			}
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000654D8 File Offset: 0x000636D8
		internal UniTask Return(bool directToBuffer = false, bool toPlayerInventory = false, int amountFactor = 1, List<Item> generatedItemsBuffer = null)
		{
			Cost.<Return>d__12 <Return>d__;
			<Return>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Return>d__.<>4__this = this;
			<Return>d__.directToBuffer = directToBuffer;
			<Return>d__.toPlayerInventory = toPlayerInventory;
			<Return>d__.amountFactor = amountFactor;
			<Return>d__.generatedItemsBuffer = generatedItemsBuffer;
			<Return>d__.<>1__state = -1;
			<Return>d__.<>t__builder.Start<Cost.<Return>d__12>(ref <Return>d__);
			return <Return>d__.<>t__builder.Task;
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x00065544 File Offset: 0x00063744
		public Cost(long money, [TupleElementNames(new string[]
		{
			"id",
			"amount"
		})] ValueTuple<int, long>[] items)
		{
			this.money = money;
			this.items = new Cost.ItemEntry[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				ValueTuple<int, long> valueTuple = items[i];
				this.items[i] = new Cost.ItemEntry
				{
					id = valueTuple.Item1,
					amount = valueTuple.Item2
				};
			}
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000655AB File Offset: 0x000637AB
		public Cost(long money)
		{
			this.money = money;
			this.items = new Cost.ItemEntry[0];
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000655C0 File Offset: 0x000637C0
		public Cost([TupleElementNames(new string[]
		{
			"id",
			"amount"
		})] params ValueTuple<int, long>[] items)
		{
			this.money = 0L;
			this.items = new Cost.ItemEntry[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				ValueTuple<int, long> valueTuple = items[i];
				this.items[i] = new Cost.ItemEntry
				{
					id = valueTuple.Item1,
					amount = valueTuple.Item2
				};
			}
		}

		// Token: 0x040013D3 RID: 5075
		public long money;

		// Token: 0x040013D4 RID: 5076
		public Cost.ItemEntry[] items;

		// Token: 0x040013D5 RID: 5077
		private static List<object> ReturnTaskLocks = new List<object>();

		// Token: 0x020005E8 RID: 1512
		[Serializable]
		public struct ItemEntry
		{
			// Token: 0x04002179 RID: 8569
			[ItemTypeID]
			public int id;

			// Token: 0x0400217A RID: 8570
			public long amount;
		}
	}
}
