using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.MasterKeys
{
	// Token: 0x020002ED RID: 749
	public class MasterKeysManager : MonoBehaviour
	{
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x0600182A RID: 6186 RVA: 0x000594FC File Offset: 0x000576FC
		// (remove) Token: 0x0600182B RID: 6187 RVA: 0x00059530 File Offset: 0x00057730
		public static event Action<int> OnMasterKeyUnlocked;

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600182C RID: 6188 RVA: 0x00059563 File Offset: 0x00057763
		// (set) Token: 0x0600182D RID: 6189 RVA: 0x0005956A File Offset: 0x0005776A
		public static MasterKeysManager Instance { get; private set; }

		// Token: 0x0600182E RID: 6190 RVA: 0x00059574 File Offset: 0x00057774
		public static bool SubmitAndActivate(Item item)
		{
			if (MasterKeysManager.Instance == null)
			{
				return false;
			}
			if (item == null)
			{
				return false;
			}
			int typeID = item.TypeID;
			if (MasterKeysManager.IsActive(typeID))
			{
				return false;
			}
			if (item.StackCount > 1)
			{
				int stackCount = item.StackCount;
				item.StackCount = stackCount - 1;
			}
			else
			{
				item.Detach();
				ItemTreeExtensions.DestroyTree(item);
			}
			MasterKeysManager.Activate(typeID);
			return true;
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x000595DA File Offset: 0x000577DA
		public static bool IsActive(int id)
		{
			return !(MasterKeysManager.Instance == null) && MasterKeysManager.Instance.IsActive_Local(id);
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x000595F6 File Offset: 0x000577F6
		internal static void Activate(int id)
		{
			if (MasterKeysManager.Instance == null)
			{
				return;
			}
			MasterKeysManager.Instance.Activate_Local(id);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x00059611 File Offset: 0x00057811
		internal static MasterKeysManager.Status GetStatus(int id)
		{
			if (MasterKeysManager.Instance == null)
			{
				return null;
			}
			return MasterKeysManager.Instance.GetStatus_Local(id);
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001832 RID: 6194 RVA: 0x0005962D File Offset: 0x0005782D
		public int Count
		{
			get
			{
				return this.keysStatus.Count;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001833 RID: 6195 RVA: 0x0005963C File Offset: 0x0005783C
		public static List<int> AllPossibleKeys
		{
			get
			{
				if (MasterKeysManager._cachedKeyItemIds == null)
				{
					MasterKeysManager._cachedKeyItemIds = new List<int>();
					foreach (ItemAssetsCollection.Entry entry in ItemAssetsCollection.Instance.entries)
					{
						Tag[] tags = entry.metaData.tags;
						if (tags.Any((Tag e) => Tag.Match(e, "Key")))
						{
							if (GameMetaData.Instance.IsDemo)
							{
								if (tags.Any((Tag e) => e.name == GameplayDataSettings.Tags.LockInDemoTag.name))
								{
									continue;
								}
							}
							if (!tags.Any((Tag e) => MasterKeysManager.excludeTags.Contains(e.name)))
							{
								MasterKeysManager._cachedKeyItemIds.Add(entry.typeID);
							}
						}
					}
				}
				return MasterKeysManager._cachedKeyItemIds;
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00059748 File Offset: 0x00057948
		private void Awake()
		{
			if (MasterKeysManager.Instance == null)
			{
				MasterKeysManager.Instance = this;
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			this.Load();
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x00059774 File Offset: 0x00057974
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x00059787 File Offset: 0x00057987
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00059790 File Offset: 0x00057990
		public bool IsActive_Local(int id)
		{
			MasterKeysManager.Status status = MasterKeysManager.GetStatus(id);
			return status != null && status.active;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x000597AF File Offset: 0x000579AF
		private void Activate_Local(int id)
		{
			if (id < 0)
			{
				return;
			}
			if (!MasterKeysManager.AllPossibleKeys.Contains(id))
			{
				return;
			}
			this.GetOrCreateStatus(id).active = true;
			Action<int> onMasterKeyUnlocked = MasterKeysManager.OnMasterKeyUnlocked;
			if (onMasterKeyUnlocked == null)
			{
				return;
			}
			onMasterKeyUnlocked(id);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x000597E4 File Offset: 0x000579E4
		public MasterKeysManager.Status GetStatus_Local(int id)
		{
			return this.keysStatus.Find((MasterKeysManager.Status e) => e.id == id);
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00059818 File Offset: 0x00057A18
		public MasterKeysManager.Status GetOrCreateStatus(int id)
		{
			MasterKeysManager.Status status_Local = this.GetStatus_Local(id);
			if (status_Local != null)
			{
				return status_Local;
			}
			MasterKeysManager.Status status = new MasterKeysManager.Status();
			status.id = id;
			this.keysStatus.Add(status);
			return status;
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x0005984C File Offset: 0x00057A4C
		private void Save()
		{
			SavesSystem.Save<List<MasterKeysManager.Status>>("MasterKeys", this.keysStatus);
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0005985E File Offset: 0x00057A5E
		private void Load()
		{
			if (SavesSystem.KeyExisits("MasterKeys"))
			{
				this.keysStatus = SavesSystem.Load<List<MasterKeysManager.Status>>("MasterKeys");
				return;
			}
			this.keysStatus = new List<MasterKeysManager.Status>();
		}

		// Token: 0x040011B4 RID: 4532
		[SerializeField]
		private List<MasterKeysManager.Status> keysStatus = new List<MasterKeysManager.Status>();

		// Token: 0x040011B5 RID: 4533
		private static List<int> _cachedKeyItemIds;

		// Token: 0x040011B6 RID: 4534
		private static string[] excludeTags = new string[]
		{
			"SpecialKey"
		};

		// Token: 0x040011B7 RID: 4535
		private const string SaveKey = "MasterKeys";

		// Token: 0x0200059E RID: 1438
		[Serializable]
		public class Status
		{
			// Token: 0x04002094 RID: 8340
			[ItemTypeID]
			public int id;

			// Token: 0x04002095 RID: 8341
			public bool active;
		}
	}
}
