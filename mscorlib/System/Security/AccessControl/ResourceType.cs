using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000548 RID: 1352
	public enum ResourceType
	{
		// Token: 0x040024ED RID: 9453
		Unknown,
		// Token: 0x040024EE RID: 9454
		FileObject,
		// Token: 0x040024EF RID: 9455
		Service,
		// Token: 0x040024F0 RID: 9456
		Printer,
		// Token: 0x040024F1 RID: 9457
		RegistryKey,
		// Token: 0x040024F2 RID: 9458
		LMShare,
		// Token: 0x040024F3 RID: 9459
		KernelObject,
		// Token: 0x040024F4 RID: 9460
		WindowObject,
		// Token: 0x040024F5 RID: 9461
		DSObject,
		// Token: 0x040024F6 RID: 9462
		DSObjectAll,
		// Token: 0x040024F7 RID: 9463
		ProviderDefined,
		// Token: 0x040024F8 RID: 9464
		WmiGuidObject,
		// Token: 0x040024F9 RID: 9465
		RegistryWow6432Key
	}
}
