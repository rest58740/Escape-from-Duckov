using System;

namespace System.Resources
{
	// Token: 0x0200085C RID: 2140
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public sealed class NeutralResourcesLanguageAttribute : Attribute
	{
		// Token: 0x06004740 RID: 18240 RVA: 0x000E80FC File Offset: 0x000E62FC
		public NeutralResourcesLanguageAttribute(string cultureName)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName");
			}
			this.CultureName = cultureName;
			this.Location = 0;
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x000E8120 File Offset: 0x000E6320
		public NeutralResourcesLanguageAttribute(string cultureName, UltimateResourceFallbackLocation location)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName");
			}
			if (!Enum.IsDefined(typeof(UltimateResourceFallbackLocation), location))
			{
				throw new ArgumentException(SR.Format("The NeutralResourcesLanguageAttribute specifies an invalid or unrecognized ultimate resource fallback location: \"{0}\".", location));
			}
			this.CultureName = cultureName;
			this.Location = location;
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06004742 RID: 18242 RVA: 0x000E817C File Offset: 0x000E637C
		public string CultureName { get; }

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06004743 RID: 18243 RVA: 0x000E8184 File Offset: 0x000E6384
		public UltimateResourceFallbackLocation Location { get; }
	}
}
