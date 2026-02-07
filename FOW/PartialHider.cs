using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200000C RID: 12
	public class PartialHider
	{
		// Token: 0x06000055 RID: 85 RVA: 0x000045A1 File Offset: 0x000027A1
		public PartialHider(Material mat)
		{
			this.HiderMaterial = mat;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000045B0 File Offset: 0x000027B0
		public void Register()
		{
			if (!FogOfWarWorld.PartialHiders.Contains(this))
			{
				FogOfWarWorld.PartialHiders.Add(this);
			}
			if (!this._initialized)
			{
				this.InitializeMaterial();
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000045D8 File Offset: 0x000027D8
		public void Deregister()
		{
			if (FogOfWarWorld.PartialHiders.Contains(this))
			{
				FogOfWarWorld.PartialHiders.Remove(this);
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000045F3 File Offset: 0x000027F3
		private void InitializeMaterial()
		{
			this._initialized = true;
			FogOfWarWorld.instance.InitializeFogProperties(this.HiderMaterial);
			FogOfWarWorld.instance.UpdateMaterialProperties(this.HiderMaterial);
			FogOfWarWorld.instance.SetNumRevealers(this.HiderMaterial);
		}

		// Token: 0x04000081 RID: 129
		public Material HiderMaterial;

		// Token: 0x04000082 RID: 130
		private bool _initialized;
	}
}
