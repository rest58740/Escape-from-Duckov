using System;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x0200010F RID: 271
	public class PlatformGroup : Platform
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x000084B6 File Offset: 0x000066B6
		internal override string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000084BE File Offset: 0x000066BE
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
		}

		// Token: 0x0400059A RID: 1434
		[SerializeField]
		private string displayName;

		// Token: 0x0400059B RID: 1435
		[SerializeField]
		private Legacy.Platform legacyIdentifier;
	}
}
