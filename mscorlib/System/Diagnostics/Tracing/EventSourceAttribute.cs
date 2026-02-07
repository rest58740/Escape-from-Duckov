using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009F9 RID: 2553
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class EventSourceAttribute : Attribute
	{
		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06005B15 RID: 23317 RVA: 0x001347EE File Offset: 0x001329EE
		// (set) Token: 0x06005B16 RID: 23318 RVA: 0x001347F6 File Offset: 0x001329F6
		public string Guid { get; set; }

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06005B17 RID: 23319 RVA: 0x001347FF File Offset: 0x001329FF
		// (set) Token: 0x06005B18 RID: 23320 RVA: 0x00134807 File Offset: 0x00132A07
		public string LocalizationResources { get; set; }

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06005B19 RID: 23321 RVA: 0x00134810 File Offset: 0x00132A10
		// (set) Token: 0x06005B1A RID: 23322 RVA: 0x00134818 File Offset: 0x00132A18
		public string Name { get; set; }
	}
}
