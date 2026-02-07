using System;
using System.Collections.Generic;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200036B RID: 875
	public class QuestTask_KillCount : Task
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001E7C RID: 7804 RVA: 0x0006D56F File Offset: 0x0006B76F
		// (set) Token: 0x06001E7D RID: 7805 RVA: 0x0006D576 File Offset: 0x0006B776
		[LocalizationKey("TasksAndRewards")]
		private string defaultEnemyNameKey
		{
			get
			{
				return "Task_Desc_AnyEnemy";
			}
			set
			{
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x0006D578 File Offset: 0x0006B778
		// (set) Token: 0x06001E7F RID: 7807 RVA: 0x0006D57F File Offset: 0x0006B77F
		[LocalizationKey("TasksAndRewards")]
		private string defaultWeaponNameKey
		{
			get
			{
				return "Task_Desc_AnyWeapon";
			}
			set
			{
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x0006D584 File Offset: 0x0006B784
		private string weaponName
		{
			get
			{
				if (this.withWeapon)
				{
					return ItemAssetsCollection.GetMetaData(this.weaponTypeID).DisplayName;
				}
				return LocalizationManager.ToPlainText(this.defaultWeaponNameKey);
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x0006D5B8 File Offset: 0x0006B7B8
		private string enemyName
		{
			get
			{
				if (this.requireEnemyType == null)
				{
					return LocalizationManager.ToPlainText(this.defaultEnemyNameKey);
				}
				return this.requireEnemyType.DisplayName;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x0006D5DF File Offset: 0x0006B7DF
		// (set) Token: 0x06001E83 RID: 7811 RVA: 0x0006D5E6 File Offset: 0x0006B7E6
		[LocalizationKey("TasksAndRewards")]
		private string descriptionFormatKey
		{
			get
			{
				return "Task_KillCount";
			}
			set
			{
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x0006D5E8 File Offset: 0x0006B7E8
		// (set) Token: 0x06001E85 RID: 7813 RVA: 0x0006D5EF File Offset: 0x0006B7EF
		[LocalizationKey("TasksAndRewards")]
		private string withWeaponDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_WithWeapon";
			}
			set
			{
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x0006D5F1 File Offset: 0x0006B7F1
		// (set) Token: 0x06001E87 RID: 7815 RVA: 0x0006D5F8 File Offset: 0x0006B7F8
		[LocalizationKey("TasksAndRewards")]
		private string requireSceneDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_RequireScene";
			}
			set
			{
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x0006D5FA File Offset: 0x0006B7FA
		// (set) Token: 0x06001E89 RID: 7817 RVA: 0x0006D601 File Offset: 0x0006B801
		[LocalizationKey("TasksAndRewards")]
		private string RequireHeadShotDescriptionKey
		{
			get
			{
				return "Task_Desc_RequireHeadShot";
			}
			set
			{
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x0006D603 File Offset: 0x0006B803
		// (set) Token: 0x06001E8B RID: 7819 RVA: 0x0006D60A File Offset: 0x0006B80A
		[LocalizationKey("TasksAndRewards")]
		private string WithoutHeadShotDescriptionKey
		{
			get
			{
				return "Task_Desc_WithoutHeadShot";
			}
			set
			{
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x0006D60C File Offset: 0x0006B80C
		// (set) Token: 0x06001E8D RID: 7821 RVA: 0x0006D613 File Offset: 0x0006B813
		[LocalizationKey("TasksAndRewards")]
		private string RequireBuffDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_WithBuff";
			}
			set
			{
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x0006D615 File Offset: 0x0006B815
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x0006D624 File Offset: 0x0006B824
		public override string[] ExtraDescriptsions
		{
			get
			{
				List<string> list = new List<string>();
				if (this.withWeapon)
				{
					list.Add(this.WithWeaponDescription);
				}
				if (!string.IsNullOrEmpty(this.requireSceneID))
				{
					list.Add(this.RequireSceneDescription);
				}
				if (this.requireHeadShot)
				{
					list.Add(this.RequireHeadShotDescription);
				}
				if (this.withoutHeadShot)
				{
					list.Add(this.WithoutHeadShotDescription);
				}
				if (this.requireBuff)
				{
					list.Add(this.RequireBuffDescription);
				}
				return list.ToArray();
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x0006D6A6 File Offset: 0x0006B8A6
		private string WithWeaponDescription
		{
			get
			{
				return StringExtensions.Format(LocalizationManager.ToPlainText(this.withWeaponDescriptionFormatKey), new
				{
					this.weaponName
				});
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x0006D6C3 File Offset: 0x0006B8C3
		private string RequireSceneDescription
		{
			get
			{
				return StringExtensions.Format(LocalizationManager.ToPlainText(this.requireSceneDescriptionFormatKey), new
				{
					this.requireSceneName
				});
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x0006D6E0 File Offset: 0x0006B8E0
		private string RequireHeadShotDescription
		{
			get
			{
				return LocalizationManager.ToPlainText(this.RequireHeadShotDescriptionKey);
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x0006D6ED File Offset: 0x0006B8ED
		private string WithoutHeadShotDescription
		{
			get
			{
				return LocalizationManager.ToPlainText(this.WithoutHeadShotDescriptionKey);
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x0006D6FC File Offset: 0x0006B8FC
		private string RequireBuffDescription
		{
			get
			{
				string buffDisplayName = GameplayDataSettings.Buffs.GetBuffDisplayName(this.requireBuffID);
				return StringExtensions.Format(LocalizationManager.ToPlainText(this.RequireBuffDescriptionFormatKey), new
				{
					buffName = buffDisplayName
				});
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x0006D730 File Offset: 0x0006B930
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					this.weaponName,
					this.enemyName,
					this.requireAmount,
					this.amount,
					this.requireSceneName
				});
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001E96 RID: 7830 RVA: 0x0006D760 File Offset: 0x0006B960
		public SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x0006D770 File Offset: 0x0006B970
		public SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001E98 RID: 7832 RVA: 0x0006D78F File Offset: 0x0006B98F
		public string requireSceneName
		{
			get
			{
				if (string.IsNullOrEmpty(this.requireSceneID))
				{
					return LocalizationManager.ToPlainText("Task_Desc_AnyScene");
				}
				return this.RequireSceneInfo.DisplayName;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x0006D7B4 File Offset: 0x0006B9B4
		public bool SceneRequirementSatisfied
		{
			get
			{
				if (string.IsNullOrEmpty(this.requireSceneID))
				{
					return true;
				}
				SceneReference requireScene = this.RequireScene;
				return requireScene == null || requireScene.UnsafeReason == 1 || requireScene.UnsafeReason != null || requireScene.LoadedScene.isLoaded;
			}
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0006D7FF File Offset: 0x0006B9FF
		private void OnEnable()
		{
			Health.OnDead += this.Health_OnDead;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0006D823 File Offset: 0x0006BA23
		private void OnDisable()
		{
			Health.OnDead -= this.Health_OnDead;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0006D847 File Offset: 0x0006BA47
		private void OnLevelInitialized()
		{
			if (this.resetOnLevelInitialized)
			{
				this.amount = 0;
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0006D858 File Offset: 0x0006BA58
		private void Health_OnDead(Health health, DamageInfo info)
		{
			if (health.team == Teams.player)
			{
				return;
			}
			bool flag = false;
			CharacterMainControl fromCharacter = info.fromCharacter;
			if (fromCharacter != null && info.fromCharacter.IsMainCharacter())
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			if (this.withWeapon && info.fromWeaponItemID != this.weaponTypeID)
			{
				return;
			}
			if (!this.SceneRequirementSatisfied)
			{
				return;
			}
			if (this.requireHeadShot && info.crit <= 0)
			{
				return;
			}
			if (this.withoutHeadShot && info.crit > 0)
			{
				return;
			}
			if (this.requireBuff && !fromCharacter.HasBuff(this.requireBuffID))
			{
				return;
			}
			if (this.requireEnemyType != null)
			{
				CharacterMainControl characterMainControl = health.TryGetCharacter();
				if (characterMainControl == null)
				{
					return;
				}
				CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
				if (characterPreset == null)
				{
					return;
				}
				if (characterPreset.nameKey != this.requireEnemyType.nameKey)
				{
					return;
				}
			}
			this.AddCount();
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0006D93D File Offset: 0x0006BB3D
		private void AddCount()
		{
			if (this.amount < this.requireAmount)
			{
				this.amount++;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0006D961 File Offset: 0x0006BB61
		public override object GenerateSaveData()
		{
			return this.amount;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0006D96E File Offset: 0x0006BB6E
		protected override bool CheckFinished()
		{
			return this.amount >= this.requireAmount;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0006D984 File Offset: 0x0006BB84
		public override void SetupSaveData(object data)
		{
			if (data is int)
			{
				int num = (int)data;
				this.amount = num;
			}
		}

		// Token: 0x04001521 RID: 5409
		[SerializeField]
		private int requireAmount = 1;

		// Token: 0x04001522 RID: 5410
		[SerializeField]
		private bool resetOnLevelInitialized;

		// Token: 0x04001523 RID: 5411
		[SerializeField]
		private int amount;

		// Token: 0x04001524 RID: 5412
		[SerializeField]
		private bool withWeapon;

		// Token: 0x04001525 RID: 5413
		[SerializeField]
		[ItemTypeID]
		private int weaponTypeID;

		// Token: 0x04001526 RID: 5414
		[SerializeField]
		private bool requireHeadShot;

		// Token: 0x04001527 RID: 5415
		[SerializeField]
		private bool withoutHeadShot;

		// Token: 0x04001528 RID: 5416
		[SerializeField]
		private bool requireBuff;

		// Token: 0x04001529 RID: 5417
		[SerializeField]
		private int requireBuffID;

		// Token: 0x0400152A RID: 5418
		[SerializeField]
		private CharacterRandomPreset requireEnemyType;

		// Token: 0x0400152B RID: 5419
		[SceneID]
		[SerializeField]
		private string requireSceneID;
	}
}
