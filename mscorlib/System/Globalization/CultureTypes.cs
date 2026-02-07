using System;

namespace System.Globalization
{
	// Token: 0x02000958 RID: 2392
	[Flags]
	public enum CultureTypes
	{
		// Token: 0x040033D5 RID: 13269
		NeutralCultures = 1,
		// Token: 0x040033D6 RID: 13270
		SpecificCultures = 2,
		// Token: 0x040033D7 RID: 13271
		InstalledWin32Cultures = 4,
		// Token: 0x040033D8 RID: 13272
		AllCultures = 7,
		// Token: 0x040033D9 RID: 13273
		UserCustomCulture = 8,
		// Token: 0x040033DA RID: 13274
		ReplacementCultures = 16,
		// Token: 0x040033DB RID: 13275
		[Obsolete("This value has been deprecated.  Please use other values in CultureTypes.")]
		WindowsOnlyCultures = 32,
		// Token: 0x040033DC RID: 13276
		[Obsolete("This value has been deprecated.  Please use other values in CultureTypes.")]
		FrameworkCultures = 64
	}
}
