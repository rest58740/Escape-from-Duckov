using System;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000079 RID: 121
	public class UnitAttribute : Attribute
	{
		// Token: 0x06000191 RID: 401 RVA: 0x00003F22 File Offset: 0x00002122
		public UnitAttribute(Units unit)
		{
			this.Base = unit;
			this.Display = unit;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00003F46 File Offset: 0x00002146
		public UnitAttribute(string unit)
		{
			this.BaseName = unit;
			this.DisplayName = unit;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00003F6A File Offset: 0x0000216A
		public UnitAttribute(Units @base, Units display)
		{
			this.Base = @base;
			this.Display = display;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00003F8E File Offset: 0x0000218E
		public UnitAttribute(Units @base, string display)
		{
			this.Base = @base;
			this.DisplayName = display;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00003FB2 File Offset: 0x000021B2
		public UnitAttribute(string @base, Units display)
		{
			this.BaseName = @base;
			this.Display = display;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00003FD6 File Offset: 0x000021D6
		public UnitAttribute(string @base, string display)
		{
			this.BaseName = @base;
			this.DisplayName = display;
		}

		// Token: 0x0400015A RID: 346
		public Units Base = Units.Unset;

		// Token: 0x0400015B RID: 347
		public Units Display = Units.Unset;

		// Token: 0x0400015C RID: 348
		public string BaseName;

		// Token: 0x0400015D RID: 349
		public string DisplayName;

		// Token: 0x0400015E RID: 350
		public bool DisplayAsString;

		// Token: 0x0400015F RID: 351
		public bool ForceDisplayUnit;
	}
}
