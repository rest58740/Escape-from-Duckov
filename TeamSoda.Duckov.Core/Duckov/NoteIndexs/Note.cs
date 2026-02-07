using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000273 RID: 627
	[Serializable]
	public class Note
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x0004A5F8 File Offset: 0x000487F8
		// (set) Token: 0x060013C6 RID: 5062 RVA: 0x0004A60F File Offset: 0x0004880F
		[LocalizationKey("Default")]
		public string titleKey
		{
			get
			{
				return "Note_" + this.key + "_Title";
			}
			set
			{
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x0004A611 File Offset: 0x00048811
		// (set) Token: 0x060013C8 RID: 5064 RVA: 0x0004A628 File Offset: 0x00048828
		[LocalizationKey("Default")]
		public string contentKey
		{
			get
			{
				return "Note_" + this.key + "_Content";
			}
			set
			{
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x0004A62A File Offset: 0x0004882A
		public string Title
		{
			get
			{
				return LocalizationManager.ToPlainText(this.titleKey);
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x0004A637 File Offset: 0x00048837
		private Sprite previewSprite
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x0004A63F File Offset: 0x0004883F
		public string Content
		{
			get
			{
				return LocalizationManager.ToPlainText(this.contentKey);
			}
		}

		// Token: 0x04000ECB RID: 3787
		[SerializeField]
		public string key;

		// Token: 0x04000ECC RID: 3788
		[SerializeField]
		public Sprite image;

		// Token: 0x04000ECD RID: 3789
		[SerializeField]
		public bool hide;
	}
}
