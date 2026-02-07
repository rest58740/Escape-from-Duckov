using System;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x02000111 RID: 273
	public class PlatformMobileLow : Platform
	{
		// Token: 0x060006FF RID: 1791 RVA: 0x000084FB File Offset: 0x000066FB
		static PlatformMobileLow()
		{
			Settings.AddPlatformTemplate<PlatformMobileLow>("c88d16e5272a4e241b0ef0ac2e53b73d");
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00008507 File Offset: 0x00006707
		internal override string DisplayName
		{
			get
			{
				return "Low-End Mobile";
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0000850E File Offset: 0x0000670E
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.IPhonePlayer, this);
			settings.DeclareRuntimePlatform(RuntimePlatform.Android, this);
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00008521 File Offset: 0x00006721
		internal override float Priority
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00008528 File Offset: 0x00006728
		internal override bool MatchesCurrentEnvironment
		{
			get
			{
				return base.Active;
			}
		}
	}
}
