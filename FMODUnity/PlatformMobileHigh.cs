using System;

namespace FMODUnity
{
	// Token: 0x02000110 RID: 272
	public class PlatformMobileHigh : PlatformMobileLow
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x000084C8 File Offset: 0x000066C8
		static PlatformMobileHigh()
		{
			Settings.AddPlatformTemplate<PlatformMobileHigh>("fd7c55dab0fce234b8c25f6ffca523c1");
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x000084D4 File Offset: 0x000066D4
		internal override string DisplayName
		{
			get
			{
				return "High-End Mobile";
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x000084DB File Offset: 0x000066DB
		internal override float Priority
		{
			get
			{
				return base.Priority + 1f;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x000084E9 File Offset: 0x000066E9
		internal override bool MatchesCurrentEnvironment
		{
			get
			{
				bool active = base.Active;
				return false;
			}
		}
	}
}
