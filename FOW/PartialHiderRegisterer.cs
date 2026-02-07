using System;
using System.Collections.Generic;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200000D RID: 13
	public class PartialHiderRegisterer : MonoBehaviour
	{
		// Token: 0x06000059 RID: 89 RVA: 0x0000462C File Offset: 0x0000282C
		private void OnEnable()
		{
			this.RegisterMaterials();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004634 File Offset: 0x00002834
		private void OnDisable()
		{
			this.DeRegisterMaterials();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000463C File Offset: 0x0000283C
		public void RegisterMaterials()
		{
			if (this.InitializedMaterials == null)
			{
				this.InitializedMaterials = new Dictionary<Material, PartialHider>();
			}
			foreach (Material material in this.MaterialsToInitialize)
			{
				if (!this.InitializedMaterials.ContainsKey(material))
				{
					this.InitializedMaterials.Add(material, new PartialHider(material));
				}
				this.InitializedMaterials[material].Register();
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000046A8 File Offset: 0x000028A8
		public void DeRegisterMaterials()
		{
			if (this.InitializedMaterials == null)
			{
				this.InitializedMaterials = new Dictionary<Material, PartialHider>();
			}
			foreach (Material material in this.MaterialsToInitialize)
			{
				if (!this.InitializedMaterials.ContainsKey(material))
				{
					this.InitializedMaterials.Add(material, new PartialHider(material));
				}
				this.InitializedMaterials[material].Deregister();
			}
		}

		// Token: 0x04000083 RID: 131
		public Material[] MaterialsToInitialize;

		// Token: 0x04000084 RID: 132
		private Dictionary<Material, PartialHider> InitializedMaterials;
	}
}
