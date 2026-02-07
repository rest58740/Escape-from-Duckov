using System;
using System.Collections.Generic;
using Duckov.PerkTrees;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200036F RID: 879
	public class QuestTask_UnlockPerk : Task
	{
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x0006DD9E File Offset: 0x0006BF9E
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x0006DDAB File Offset: 0x0006BFAB
		private string PerkDisplayName
		{
			get
			{
				if (this.perk == null)
				{
					this.BindPerk();
				}
				if (this.perk == null)
				{
					return LocalizationManager.ToPlainText(this.perkObjectName);
				}
				return this.perk.DisplayName;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x0006DDE7 File Offset: 0x0006BFE7
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					this.PerkDisplayName
				});
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x0006DDFF File Offset: 0x0006BFFF
		public override Sprite Icon
		{
			get
			{
				if (this.perk != null)
				{
					return this.perk.Icon;
				}
				return null;
			}
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0006DE1C File Offset: 0x0006C01C
		protected override void OnInit()
		{
			if (LevelManager.LevelInited)
			{
				this.BindPerk();
				return;
			}
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0006DE40 File Offset: 0x0006C040
		private bool BindPerk()
		{
			if (this.perk)
			{
				if (!this.unlocked && this.perk.Unlocked)
				{
					this.OnPerkUnlockStateChanged(this.perk, true);
				}
				return false;
			}
			PerkTree perkTree = PerkTreeManager.GetPerkTree(this.perkTreeID);
			if (perkTree)
			{
				using (List<Perk>.Enumerator enumerator = perkTree.perks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Perk perk = enumerator.Current;
						if (perk.gameObject.name == this.perkObjectName)
						{
							this.perk = perk;
							if (this.perk.Unlocked)
							{
								this.OnPerkUnlockStateChanged(this.perk, true);
							}
							this.perk.onUnlockStateChanged += this.OnPerkUnlockStateChanged;
							return true;
						}
					}
					goto IL_E6;
				}
			}
			Debug.LogError("PerkTree Not Found " + this.perkTreeID, base.gameObject);
			IL_E6:
			Debug.LogError("Perk Not Found: " + this.perkTreeID + "/" + this.perkObjectName, base.gameObject);
			return false;
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0006DF6C File Offset: 0x0006C16C
		private void OnPerkUnlockStateChanged(Perk _perk, bool _unlocked)
		{
			if (base.Master.Complete)
			{
				return;
			}
			if (_unlocked)
			{
				this.unlocked = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0006DF8C File Offset: 0x0006C18C
		private void OnDestroy()
		{
			if (this.perk)
			{
				this.perk.onUnlockStateChanged -= this.OnPerkUnlockStateChanged;
			}
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0006DFC3 File Offset: 0x0006C1C3
		private void OnLevelInitialized()
		{
			this.BindPerk();
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0006DFCC File Offset: 0x0006C1CC
		public override object GenerateSaveData()
		{
			return this.unlocked;
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0006DFD9 File Offset: 0x0006C1D9
		protected override bool CheckFinished()
		{
			return this.unlocked;
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0006DFE4 File Offset: 0x0006C1E4
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.unlocked = flag;
			}
		}

		// Token: 0x04001539 RID: 5433
		[SerializeField]
		private string perkTreeID;

		// Token: 0x0400153A RID: 5434
		[SerializeField]
		private string perkObjectName;

		// Token: 0x0400153B RID: 5435
		private Perk perk;

		// Token: 0x0400153C RID: 5436
		[NonSerialized]
		private bool unlocked;

		// Token: 0x0400153D RID: 5437
		private string descriptionFormatKey = "Task_UnlockPerk";
	}
}
