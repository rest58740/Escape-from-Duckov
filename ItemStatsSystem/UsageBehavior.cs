using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000025 RID: 37
	public abstract class UsageBehavior : MonoBehaviour
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00008024 File Offset: 0x00006224
		public virtual UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				return default(UsageBehavior.DisplaySettingsData);
			}
		}

		// Token: 0x060001FF RID: 511
		public abstract bool CanBeUsed(Item item, object user);

		// Token: 0x06000200 RID: 512
		protected abstract void OnUse(Item item, object user);

		// Token: 0x06000201 RID: 513 RVA: 0x0000803A File Offset: 0x0000623A
		public void Use(Item item, object user)
		{
			this.OnUse(item, user);
		}

		// Token: 0x0200004A RID: 74
		public struct DisplaySettingsData
		{
			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x06000290 RID: 656 RVA: 0x00009973 File Offset: 0x00007B73
			public string Description
			{
				get
				{
					return this.description;
				}
			}

			// Token: 0x04000127 RID: 295
			public bool display;

			// Token: 0x04000128 RID: 296
			public string description;
		}
	}
}
