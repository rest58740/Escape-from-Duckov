using System;

namespace System.Reflection
{
	// Token: 0x02000882 RID: 2178
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCultureAttribute : Attribute
	{
		// Token: 0x06004857 RID: 18519 RVA: 0x000EE041 File Offset: 0x000EC241
		public AssemblyCultureAttribute(string culture)
		{
			this.Culture = culture;
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06004858 RID: 18520 RVA: 0x000EE050 File Offset: 0x000EC250
		public string Culture { get; }
	}
}
