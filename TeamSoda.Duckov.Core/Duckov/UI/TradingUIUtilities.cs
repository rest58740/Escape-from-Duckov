using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003CC RID: 972
	public static class TradingUIUtilities
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x0007A70A File Offset: 0x0007890A
		// (set) Token: 0x06002307 RID: 8967 RVA: 0x0007A716 File Offset: 0x00078916
		public static IMerchant ActiveMerchant
		{
			get
			{
				return TradingUIUtilities.activeMerchant as IMerchant;
			}
			set
			{
				TradingUIUtilities.activeMerchant = (value as Object);
				Action<IMerchant> onActiveMerchantChanged = TradingUIUtilities.OnActiveMerchantChanged;
				if (onActiveMerchantChanged == null)
				{
					return;
				}
				onActiveMerchantChanged(value);
			}
		}

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x06002308 RID: 8968 RVA: 0x0007A734 File Offset: 0x00078934
		// (remove) Token: 0x06002309 RID: 8969 RVA: 0x0007A768 File Offset: 0x00078968
		public static event Action<IMerchant> OnActiveMerchantChanged;

		// Token: 0x040017B0 RID: 6064
		private static Object activeMerchant;
	}
}
