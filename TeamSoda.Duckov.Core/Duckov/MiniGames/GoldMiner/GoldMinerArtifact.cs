using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029A RID: 666
	public class GoldMinerArtifact : MiniGameBehaviour
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x000514F9 File Offset: 0x0004F6F9
		// (set) Token: 0x060015C7 RID: 5575 RVA: 0x0005150B File Offset: 0x0004F70B
		[LocalizationKey("Default")]
		private string displayNameKey
		{
			get
			{
				return "GoldMiner_" + this.id;
			}
			set
			{
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x0005150D File Offset: 0x0004F70D
		// (set) Token: 0x060015C9 RID: 5577 RVA: 0x00051524 File Offset: 0x0004F724
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "GoldMiner_" + this.id + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00051526 File Offset: 0x0004F726
		public bool AllowMultiple
		{
			get
			{
				return this.allowMultiple;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x0005152E File Offset: 0x0004F72E
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayNameKey);
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x0005153B File Offset: 0x0004F73B
		public string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionKey);
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x00051548 File Offset: 0x0004F748
		public int Quality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x00051550 File Offset: 0x0004F750
		public int BasePrice
		{
			get
			{
				return this.basePrice;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x00051558 File Offset: 0x0004F758
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x00051560 File Offset: 0x0004F760
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x00051568 File Offset: 0x0004F768
		public GoldMiner Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00051570 File Offset: 0x0004F770
		public void Attach(GoldMiner master)
		{
			this.master = master;
			base.transform.SetParent(master.transform);
			Action<GoldMinerArtifact> onAttached = this.OnAttached;
			if (onAttached == null)
			{
				return;
			}
			onAttached(this);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0005159B File Offset: 0x0004F79B
		public void Detatch(GoldMiner master)
		{
			Action<GoldMinerArtifact> onDetached = this.OnDetached;
			if (onDetached != null)
			{
				onDetached(this);
			}
			if (master != this.master)
			{
				Debug.LogError("Artifact is being notified detach by a different GoldMiner instance.", master.gameObject);
			}
			this.master = null;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x000515D4 File Offset: 0x0004F7D4
		private void OnDestroy()
		{
			this.Detatch(this.master);
		}

		// Token: 0x04001001 RID: 4097
		[SerializeField]
		private string id;

		// Token: 0x04001002 RID: 4098
		[SerializeField]
		private Sprite icon;

		// Token: 0x04001003 RID: 4099
		[SerializeField]
		private bool allowMultiple;

		// Token: 0x04001004 RID: 4100
		[SerializeField]
		private int basePrice;

		// Token: 0x04001005 RID: 4101
		[SerializeField]
		private int quality;

		// Token: 0x04001006 RID: 4102
		private GoldMiner master;

		// Token: 0x04001007 RID: 4103
		public Action<GoldMinerArtifact> OnAttached;

		// Token: 0x04001008 RID: 4104
		public Action<GoldMinerArtifact> OnDetached;
	}
}
