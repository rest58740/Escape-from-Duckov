using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000314 RID: 788
	public class DeathLottery : MonoBehaviour
	{
		// Token: 0x140000AB RID: 171
		// (add) Token: 0x060019C6 RID: 6598 RVA: 0x0005E71C File Offset: 0x0005C91C
		// (remove) Token: 0x060019C7 RID: 6599 RVA: 0x0005E750 File Offset: 0x0005C950
		public static event Action<DeathLottery> OnRequestUI;

		// Token: 0x060019C8 RID: 6600 RVA: 0x0005E783 File Offset: 0x0005C983
		public void RequestUI()
		{
			Action<DeathLottery> onRequestUI = DeathLottery.OnRequestUI;
			if (onRequestUI == null)
			{
				return;
			}
			onRequestUI(this);
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0005E795 File Offset: 0x0005C995
		public int MaxChances
		{
			get
			{
				return this.costs.Length;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x0005E79F File Offset: 0x0005C99F
		public static uint CurrentDeadCharacterToken
		{
			get
			{
				return SavesSystem.Load<uint>("DeadCharacterToken");
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x0005E7AB File Offset: 0x0005C9AB
		private string SelectNotificationFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.selectNotificationFormatKey);
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060019CC RID: 6604 RVA: 0x0005E7B8 File Offset: 0x0005C9B8
		public DeathLottery.OptionalCosts[] Costs
		{
			get
			{
				return this.costs;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0005E7C0 File Offset: 0x0005C9C0
		public List<Item> ItemInstances
		{
			get
			{
				return this.itemInstances;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x0005E7C8 File Offset: 0x0005C9C8
		public DeathLottery.Status CurrentStatus
		{
			get
			{
				if (this.loading)
				{
					return default(DeathLottery.Status);
				}
				if (!this.status.valid)
				{
					return default(DeathLottery.Status);
				}
				return this.status;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x0005E804 File Offset: 0x0005CA04
		public int RemainingChances
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x0005E812 File Offset: 0x0005CA12
		public bool Loading
		{
			get
			{
				return this.loading;
			}
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0005E81C File Offset: 0x0005CA1C
		private UniTask Load()
		{
			DeathLottery.<Load>d__31 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<DeathLottery.<Load>d__31>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0005E860 File Offset: 0x0005CA60
		private UniTask LoadItemInstances()
		{
			DeathLottery.<LoadItemInstances>d__32 <LoadItemInstances>d__;
			<LoadItemInstances>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadItemInstances>d__.<>4__this = this;
			<LoadItemInstances>d__.<>1__state = -1;
			<LoadItemInstances>d__.<>t__builder.Start<DeathLottery.<LoadItemInstances>d__32>(ref <LoadItemInstances>d__);
			return <LoadItemInstances>d__.<>t__builder.Task;
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0005E8A4 File Offset: 0x0005CAA4
		private void ClearItemInstances()
		{
			for (int i = 0; i < this.itemInstances.Count; i++)
			{
				Item item = this.itemInstances[i];
				if (!(item.ParentItem != null))
				{
					ItemTreeExtensions.DestroyTree(item);
				}
			}
			this.itemInstances.Clear();
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x0005E8F3 File Offset: 0x0005CAF3
		[ContextMenu("ForceCreateNewStatus")]
		private void ForceCreateNewStatus()
		{
			if (this.Loading)
			{
				return;
			}
			this.ForceCreateNewStatusTask().Forget();
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0005E90C File Offset: 0x0005CB0C
		private UniTask ForceCreateNewStatusTask()
		{
			DeathLottery.<ForceCreateNewStatusTask>d__35 <ForceCreateNewStatusTask>d__;
			<ForceCreateNewStatusTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForceCreateNewStatusTask>d__.<>4__this = this;
			<ForceCreateNewStatusTask>d__.<>1__state = -1;
			<ForceCreateNewStatusTask>d__.<>t__builder.Start<DeathLottery.<ForceCreateNewStatusTask>d__35>(ref <ForceCreateNewStatusTask>d__);
			return <ForceCreateNewStatusTask>d__.<>t__builder.Task;
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0005E950 File Offset: 0x0005CB50
		private UniTask CreateNewStatus()
		{
			DeathLottery.<CreateNewStatus>d__36 <CreateNewStatus>d__;
			<CreateNewStatus>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateNewStatus>d__.<>4__this = this;
			<CreateNewStatus>d__.<>1__state = -1;
			<CreateNewStatus>d__.<>t__builder.Start<DeathLottery.<CreateNewStatus>d__36>(ref <CreateNewStatus>d__);
			return <CreateNewStatus>d__.<>t__builder.Task;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x0005E993 File Offset: 0x0005CB93
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0005E9A6 File Offset: 0x0005CBA6
		private void Start()
		{
			this.Load().Forget();
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0005E9B3 File Offset: 0x0005CBB3
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0005E9C6 File Offset: 0x0005CBC6
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			SavesSystem.Save<DeathLottery.Status>("DeathLottery/status", this.status);
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0005E9E4 File Offset: 0x0005CBE4
		private UniTask<List<Item>> SelectCandidates(Item deadCharacter)
		{
			DeathLottery.<SelectCandidates>d__42 <SelectCandidates>d__;
			<SelectCandidates>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
			<SelectCandidates>d__.<>4__this = this;
			<SelectCandidates>d__.deadCharacter = deadCharacter;
			<SelectCandidates>d__.<>1__state = -1;
			<SelectCandidates>d__.<>t__builder.Start<DeathLottery.<SelectCandidates>d__42>(ref <SelectCandidates>d__);
			return <SelectCandidates>d__.<>t__builder.Task;
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0005EA30 File Offset: 0x0005CC30
		private bool CanBeACandidate(Item item)
		{
			if (item == null)
			{
				return false;
			}
			foreach (Tag item2 in this.excludeTags)
			{
				if (item.Tags.Contains(item2))
				{
					return false;
				}
			}
			foreach (Tag item3 in this.requireTags)
			{
				if (item.Tags.Contains(item3))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0005EA9C File Offset: 0x0005CC9C
		public UniTask<bool> Select(int index, Cost payWhenSucceed)
		{
			DeathLottery.<Select>d__44 <Select>d__;
			<Select>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Select>d__.<>4__this = this;
			<Select>d__.index = index;
			<Select>d__.payWhenSucceed = payWhenSucceed;
			<Select>d__.<>1__state = -1;
			<Select>d__.<>t__builder.Start<DeathLottery.<Select>d__44>(ref <Select>d__);
			return <Select>d__.<>t__builder.Task;
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0005EAF0 File Offset: 0x0005CCF0
		internal DeathLottery.OptionalCosts GetCost()
		{
			if (!this.status.valid)
			{
				return default(DeathLottery.OptionalCosts);
			}
			if (this.status.SelectedCount >= this.Costs.Length)
			{
				return default(DeathLottery.OptionalCosts);
			}
			return this.Costs[this.status.SelectedCount];
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0005EB67 File Offset: 0x0005CD67
		[CompilerGenerated]
		internal static void <Select>g__SendToPlayer|44_0(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (!ItemUtilities.SendToPlayerCharacter(item, false))
			{
				ItemUtilities.SendToPlayerStorage(item, false);
			}
		}

		// Token: 0x040012C5 RID: 4805
		public const int MaxCandidateCount = 8;

		// Token: 0x040012C6 RID: 4806
		[SerializeField]
		[LocalizationKey("Default")]
		private string selectNotificationFormatKey = "DeathLottery_SelectNotification";

		// Token: 0x040012C7 RID: 4807
		[SerializeField]
		private Tag[] requireTags;

		// Token: 0x040012C8 RID: 4808
		[SerializeField]
		private Tag[] excludeTags;

		// Token: 0x040012C9 RID: 4809
		[SerializeField]
		private RandomContainer<DeathLottery.dummyItemEntry> dummyItems;

		// Token: 0x040012CA RID: 4810
		[SerializeField]
		private DeathLottery.OptionalCosts[] costs;

		// Token: 0x040012CB RID: 4811
		private DeathLottery.Status status;

		// Token: 0x040012CC RID: 4812
		private List<Item> itemInstances = new List<Item>();

		// Token: 0x040012CD RID: 4813
		private bool loading;

		// Token: 0x020005B3 RID: 1459
		[Serializable]
		private struct dummyItemEntry
		{
			// Token: 0x040020D0 RID: 8400
			[ItemTypeID]
			public int typeID;
		}

		// Token: 0x020005B4 RID: 1460
		[Serializable]
		public struct OptionalCosts
		{
			// Token: 0x040020D1 RID: 8401
			[SerializeField]
			public Cost costA;

			// Token: 0x040020D2 RID: 8402
			[SerializeField]
			public bool useCostB;

			// Token: 0x040020D3 RID: 8403
			[SerializeField]
			public Cost costB;
		}

		// Token: 0x020005B5 RID: 1461
		[Serializable]
		public struct Status
		{
			// Token: 0x170007A1 RID: 1953
			// (get) Token: 0x060029BE RID: 10686 RVA: 0x0009AC8F File Offset: 0x00098E8F
			public int SelectedCount
			{
				get
				{
					return this.selectedItems.Count;
				}
			}

			// Token: 0x040020D4 RID: 8404
			public bool valid;

			// Token: 0x040020D5 RID: 8405
			public uint deadCharacterToken;

			// Token: 0x040020D6 RID: 8406
			public List<int> selectedItems;

			// Token: 0x040020D7 RID: 8407
			public List<ItemTreeData> candidates;
		}
	}
}
