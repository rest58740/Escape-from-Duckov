using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x0200040E RID: 1038
	[Serializable]
	public class Ruleset
	{
		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600258F RID: 9615 RVA: 0x00083171 File Offset: 0x00081371
		// (set) Token: 0x06002590 RID: 9616 RVA: 0x00083183 File Offset: 0x00081383
		[LocalizationKey("UIText")]
		internal string descriptionKey
		{
			get
			{
				return this.displayNameKey + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002591 RID: 9617 RVA: 0x00083185 File Offset: 0x00081385
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayNameKey);
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002592 RID: 9618 RVA: 0x00083192 File Offset: 0x00081392
		public string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionKey);
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002593 RID: 9619 RVA: 0x0008319F File Offset: 0x0008139F
		public bool SpawnDeadBody
		{
			get
			{
				return this.spawnDeadBody;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002594 RID: 9620 RVA: 0x000831A7 File Offset: 0x000813A7
		public int SaveDeadbodyCount
		{
			get
			{
				return this.saveDeadbodyCount;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002595 RID: 9621 RVA: 0x000831AF File Offset: 0x000813AF
		public bool FogOfWar
		{
			get
			{
				return this.fogOfWar;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002596 RID: 9622 RVA: 0x000831B7 File Offset: 0x000813B7
		public bool AdvancedDebuffMode
		{
			get
			{
				return this.advancedDebuffMode;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002597 RID: 9623 RVA: 0x000831BF File Offset: 0x000813BF
		public float RecoilMultiplier
		{
			get
			{
				return this.recoilMultiplier;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002598 RID: 9624 RVA: 0x000831C7 File Offset: 0x000813C7
		public float DamageFactor_ToPlayer
		{
			get
			{
				return this.damageFactor_ToPlayer;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002599 RID: 9625 RVA: 0x000831CF File Offset: 0x000813CF
		public float EnemyHealthFactor
		{
			get
			{
				return this.enemyHealthFactor;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x000831D7 File Offset: 0x000813D7
		public float EnemyReactionTimeFactor
		{
			get
			{
				return this.enemyReactionTimeFactor;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600259B RID: 9627 RVA: 0x000831DF File Offset: 0x000813DF
		public float EnemyAttackTimeSpaceFactor
		{
			get
			{
				return this.enemyAttackTimeSpaceFactor;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x000831E7 File Offset: 0x000813E7
		public float EnemyAttackTimeFactor
		{
			get
			{
				return this.enemyAttackTimeFactor;
			}
		}

		// Token: 0x04001996 RID: 6550
		[LocalizationKey("UIText")]
		[SerializeField]
		internal string displayNameKey;

		// Token: 0x04001997 RID: 6551
		[SerializeField]
		private float damageFactor_ToPlayer = 1f;

		// Token: 0x04001998 RID: 6552
		[SerializeField]
		private float enemyHealthFactor = 1f;

		// Token: 0x04001999 RID: 6553
		[SerializeField]
		private bool spawnDeadBody = true;

		// Token: 0x0400199A RID: 6554
		[SerializeField]
		private bool fogOfWar = true;

		// Token: 0x0400199B RID: 6555
		[SerializeField]
		private bool advancedDebuffMode;

		// Token: 0x0400199C RID: 6556
		[SerializeField]
		private int saveDeadbodyCount = 1;

		// Token: 0x0400199D RID: 6557
		[Range(0f, 1f)]
		[SerializeField]
		private float recoilMultiplier = 1f;

		// Token: 0x0400199E RID: 6558
		[SerializeField]
		internal float enemyReactionTimeFactor = 1f;

		// Token: 0x0400199F RID: 6559
		[SerializeField]
		internal float enemyAttackTimeSpaceFactor = 1f;

		// Token: 0x040019A0 RID: 6560
		[SerializeField]
		internal float enemyAttackTimeFactor = 1f;
	}
}
