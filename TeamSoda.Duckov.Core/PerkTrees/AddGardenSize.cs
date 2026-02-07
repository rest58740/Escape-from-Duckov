using System;
using Duckov.Crops;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200025A RID: 602
	public class AddGardenSize : PerkBehaviour, IGardenSizeAdder
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x00048BBF File Offset: 0x00046DBF
		public override string Description
		{
			get
			{
				return StringExtensions.Format(LocalizationManager.ToPlainText(this.descriptionFormatKey), new
				{
					addX = this.add.x,
					addY = this.add.y
				});
			}
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00048BEC File Offset: 0x00046DEC
		protected override void OnUnlocked()
		{
			Garden.Register(this);
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00048BF4 File Offset: 0x00046DF4
		protected override void OnOnDestroy()
		{
			Garden.Unregister(this);
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x00048BFC File Offset: 0x00046DFC
		public Vector2Int GetValue(string gardenID)
		{
			if (gardenID != this.gardenID)
			{
				return default(Vector2Int);
			}
			return this.add;
		}

		// Token: 0x04000E8F RID: 3727
		[LocalizationKey("Default")]
		[SerializeField]
		private string descriptionFormatKey = "PerkBehaviour_AddGardenSize";

		// Token: 0x04000E90 RID: 3728
		[SerializeField]
		private string gardenID = "Default";

		// Token: 0x04000E91 RID: 3729
		[SerializeField]
		private Vector2Int add;
	}
}
