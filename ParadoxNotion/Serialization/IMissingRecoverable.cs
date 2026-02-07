using System;

namespace ParadoxNotion.Serialization
{
	// Token: 0x02000088 RID: 136
	public interface IMissingRecoverable
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000594 RID: 1428
		// (set) Token: 0x06000595 RID: 1429
		string missingType { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000596 RID: 1430
		// (set) Token: 0x06000597 RID: 1431
		string recoveryState { get; set; }
	}
}
