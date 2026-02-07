using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200025B RID: 603
	public class AddPlayerStorage : PerkBehaviour
	{
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001313 RID: 4883 RVA: 0x00048C45 File Offset: 0x00046E45
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText("PerkBehaviour_AddPlayerStorage");
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x00048C51 File Offset: 0x00046E51
		public override string Description
		{
			get
			{
				return StringExtensions.Format(this.DescriptionFormat, new
				{
					this.addCapacity
				});
			}
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00048C69 File Offset: 0x00046E69
		protected override void OnAwake()
		{
			PlayerStorage.OnRecalculateStorageCapacity += this.OnRecalculatePlayerStorage;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00048C7C File Offset: 0x00046E7C
		protected override void OnOnDestroy()
		{
			PlayerStorage.OnRecalculateStorageCapacity -= this.OnRecalculatePlayerStorage;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00048C8F File Offset: 0x00046E8F
		private void OnRecalculatePlayerStorage(PlayerStorage.StorageCapacityCalculationHolder holder)
		{
			if (base.Master.Unlocked)
			{
				holder.capacity += this.addCapacity;
			}
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00048CB1 File Offset: 0x00046EB1
		protected override void OnUnlocked()
		{
			base.OnUnlocked();
			PlayerStorage.NotifyCapacityDirty();
		}

		// Token: 0x04000E92 RID: 3730
		[SerializeField]
		private int addCapacity;
	}
}
