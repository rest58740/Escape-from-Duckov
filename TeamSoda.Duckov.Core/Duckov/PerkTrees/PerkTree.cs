using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200025D RID: 605
	public class PerkTree : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x00048D11 File Offset: 0x00046F11
		// (set) Token: 0x0600131F RID: 4895 RVA: 0x00048D0F File Offset: 0x00046F0F
		[LocalizationKey("Perks")]
		private string perkTreeName
		{
			get
			{
				return this.displayNameKey;
			}
			set
			{
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x00048D19 File Offset: 0x00046F19
		public string ID
		{
			get
			{
				return this.perkTreeID;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x00048D21 File Offset: 0x00046F21
		private string displayNameKey
		{
			get
			{
				return "PerkTree_" + this.ID;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001323 RID: 4899 RVA: 0x00048D33 File Offset: 0x00046F33
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayNameKey);
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x00048D40 File Offset: 0x00046F40
		public bool Horizontal
		{
			get
			{
				return this.horizontal;
			}
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06001325 RID: 4901 RVA: 0x00048D48 File Offset: 0x00046F48
		// (remove) Token: 0x06001326 RID: 4902 RVA: 0x00048D80 File Offset: 0x00046F80
		public event Action<PerkTree> onPerkTreeStatusChanged;

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x00048DB5 File Offset: 0x00046FB5
		public ReadOnlyCollection<Perk> Perks
		{
			get
			{
				if (this.perks_ReadOnly == null)
				{
					this.perks_ReadOnly = this.perks.AsReadOnly();
				}
				return this.perks_ReadOnly;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x00048DD6 File Offset: 0x00046FD6
		public PerkTreeRelationGraphOwner RelationGraphOwner
		{
			get
			{
				return this.relationGraphOwner;
			}
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00048DDE File Offset: 0x00046FDE
		private void Awake()
		{
			this.Load();
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00048E08 File Offset: 0x00047008
		private void Start()
		{
			foreach (Perk perk in this.perks)
			{
				if (!(perk == null) && perk.DefaultUnlocked)
				{
					perk.ForceUnlock();
				}
			}
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00048E6C File Offset: 0x0004706C
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00048E90 File Offset: 0x00047090
		public object GenerateSaveData()
		{
			return new PerkTree.SaveData(this);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00048E98 File Offset: 0x00047098
		public void SetupSaveData(object data)
		{
			foreach (Perk perk in this.perks)
			{
				perk.Unlocked = false;
			}
			PerkTree.SaveData saveData = data as PerkTree.SaveData;
			if (saveData == null)
			{
				return;
			}
			using (List<Perk>.Enumerator enumerator = this.perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Perk cur = enumerator.Current;
					if (!(cur == null))
					{
						PerkTree.SaveData.Entry entry = saveData.entries.Find((PerkTree.SaveData.Entry e) => e != null && e.perkName == cur.name);
						if (entry != null)
						{
							cur.Unlocked = entry.unlocked;
							cur.unlocking = entry.unlocking;
							cur.unlockingBeginTimeRaw = entry.unlockingBeginTime;
						}
					}
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x00048F98 File Offset: 0x00047198
		private string SaveKey
		{
			get
			{
				return "PerkTree_" + this.perkTreeID;
			}
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00048FAA File Offset: 0x000471AA
		public void Save()
		{
			SavesSystem.Save<PerkTree.SaveData>(this.SaveKey, this.GenerateSaveData() as PerkTree.SaveData);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00048FC4 File Offset: 0x000471C4
		public void Load()
		{
			if (!SavesSystem.KeyExisits(this.SaveKey))
			{
				return;
			}
			PerkTree.SaveData data = SavesSystem.Load<PerkTree.SaveData>(this.SaveKey);
			this.SetupSaveData(data);
			this.loaded = true;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00048FFC File Offset: 0x000471FC
		public void ReapplyPerks()
		{
			foreach (Perk perk in this.perks)
			{
				perk.Unlocked = false;
			}
			foreach (Perk perk2 in this.perks)
			{
				perk2.Unlocked = perk2.Unlocked;
			}
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00049094 File Offset: 0x00047294
		internal bool AreAllParentsUnlocked(Perk perk)
		{
			PerkRelationNode relatedNode = this.RelationGraphOwner.GetRelatedNode(perk);
			if (relatedNode == null)
			{
				return false;
			}
			foreach (PerkRelationNode perkRelationNode in this.relationGraphOwner.RelationGraph.GetIncomingNodes(relatedNode))
			{
				Perk relatedNode2 = perkRelationNode.relatedNode;
				if (!(relatedNode2 == null) && !relatedNode2.Unlocked)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0004911C File Offset: 0x0004731C
		internal void NotifyChildStateChanged(Perk perk)
		{
			PerkRelationNode relatedNode = this.RelationGraphOwner.GetRelatedNode(perk);
			if (relatedNode == null)
			{
				return;
			}
			foreach (PerkRelationNode perkRelationNode in this.relationGraphOwner.RelationGraph.GetOutgoingNodes(relatedNode))
			{
				perkRelationNode.NotifyIncomingStateChanged();
			}
			Action<PerkTree> action = this.onPerkTreeStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0004919C File Offset: 0x0004739C
		private void Collect()
		{
			this.perks.Clear();
			Perk[] componentsInChildren = base.transform.GetComponentsInChildren<Perk>();
			Perk[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Master = this;
			}
			this.perks.AddRange(componentsInChildren);
		}

		// Token: 0x04000E95 RID: 3733
		[SerializeField]
		private string perkTreeID = "DefaultPerkTree";

		// Token: 0x04000E96 RID: 3734
		[SerializeField]
		private bool horizontal;

		// Token: 0x04000E97 RID: 3735
		[SerializeField]
		private PerkTreeRelationGraphOwner relationGraphOwner;

		// Token: 0x04000E98 RID: 3736
		[SerializeField]
		internal List<Perk> perks = new List<Perk>();

		// Token: 0x04000E9A RID: 3738
		private ReadOnlyCollection<Perk> perks_ReadOnly;

		// Token: 0x04000E9B RID: 3739
		private bool loaded;

		// Token: 0x02000556 RID: 1366
		[Serializable]
		private class SaveData
		{
			// Token: 0x060028F6 RID: 10486 RVA: 0x00096B10 File Offset: 0x00094D10
			public SaveData(PerkTree perkTree)
			{
				this.entries = new List<PerkTree.SaveData.Entry>();
				for (int i = 0; i < perkTree.perks.Count; i++)
				{
					Perk perk = perkTree.perks[i];
					if (!(perk == null))
					{
						this.entries.Add(new PerkTree.SaveData.Entry(perk));
					}
				}
			}

			// Token: 0x04001F6B RID: 8043
			public List<PerkTree.SaveData.Entry> entries;

			// Token: 0x0200069E RID: 1694
			[Serializable]
			public class Entry
			{
				// Token: 0x06002BF4 RID: 11252 RVA: 0x000A6D59 File Offset: 0x000A4F59
				public Entry(Perk perk)
				{
					this.perkName = perk.name;
					this.unlocked = perk.Unlocked;
					this.unlocking = perk.Unlocking;
					this.unlockingBeginTime = perk.unlockingBeginTimeRaw;
				}

				// Token: 0x0400243F RID: 9279
				public string perkName;

				// Token: 0x04002440 RID: 9280
				public bool unlocking;

				// Token: 0x04002441 RID: 9281
				public long unlockingBeginTime;

				// Token: 0x04002442 RID: 9282
				public bool unlocked;
			}
		}
	}
}
