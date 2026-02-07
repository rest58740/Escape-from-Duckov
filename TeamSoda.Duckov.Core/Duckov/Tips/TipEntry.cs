using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Tips
{
	// Token: 0x02000254 RID: 596
	[Serializable]
	internal struct TipEntry
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x000480F7 File Offset: 0x000462F7
		public string TipID
		{
			get
			{
				return this.tipID;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x000480FF File Offset: 0x000462FF
		// (set) Token: 0x060012CB RID: 4811 RVA: 0x00048111 File Offset: 0x00046311
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Tips_" + this.tipID;
			}
			set
			{
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x00048113 File Offset: 0x00046313
		public string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.DescriptionKey);
			}
		}

		// Token: 0x04000E6F RID: 3695
		[SerializeField]
		private string tipID;
	}
}
