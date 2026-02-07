using System;
using System.Collections.Generic;

namespace FMODUnity
{
	// Token: 0x0200010E RID: 270
	public class PlatformDefault : Platform
	{
		// Token: 0x060006F1 RID: 1777 RVA: 0x0000842C File Offset: 0x0000662C
		public PlatformDefault()
		{
			base.Identifier = "default";
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0000843F File Offset: 0x0000663F
		internal override string DisplayName
		{
			get
			{
				return "Default";
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00008446 File Offset: 0x00006646
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00008448 File Offset: 0x00006648
		internal override bool IsIntrinsic
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0000844C File Offset: 0x0000664C
		internal override void InitializeProperties()
		{
			base.InitializeProperties();
			Platform.PropertyAccessors.Plugins.Set(this, new List<string>());
			Platform.PropertyAccessors.StaticPlugins.Set(this, new List<string>());
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00008488 File Offset: 0x00006688
		internal override void EnsurePropertiesAreValid()
		{
			base.EnsurePropertiesAreValid();
			if (base.StaticPlugins == null)
			{
				Platform.PropertyAccessors.StaticPlugins.Set(this, new List<string>());
			}
		}

		// Token: 0x04000599 RID: 1433
		public const string ConstIdentifier = "default";
	}
}
