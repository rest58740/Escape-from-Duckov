using System;

namespace Duckov.UI
{
	// Token: 0x020003BA RID: 954
	public struct SlotDisplayOperationContext
	{
		// Token: 0x06002225 RID: 8741 RVA: 0x00077B74 File Offset: 0x00075D74
		public SlotDisplayOperationContext(SlotDisplay slotDisplay, SlotDisplayOperationContext.Operation operation, bool succeed)
		{
			this.slotDisplay = slotDisplay;
			this.operation = operation;
			this.succeed = succeed;
		}

		// Token: 0x04001732 RID: 5938
		public SlotDisplay slotDisplay;

		// Token: 0x04001733 RID: 5939
		public SlotDisplayOperationContext.Operation operation;

		// Token: 0x04001734 RID: 5940
		public bool succeed;

		// Token: 0x02000644 RID: 1604
		public enum Operation
		{
			// Token: 0x040022B0 RID: 8880
			None,
			// Token: 0x040022B1 RID: 8881
			Equip,
			// Token: 0x040022B2 RID: 8882
			Unequip,
			// Token: 0x040022B3 RID: 8883
			Deny
		}
	}
}
