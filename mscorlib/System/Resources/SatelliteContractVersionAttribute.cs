using System;

namespace System.Resources
{
	// Token: 0x02000861 RID: 2145
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public sealed class SatelliteContractVersionAttribute : Attribute
	{
		// Token: 0x06004759 RID: 18265 RVA: 0x000E87B0 File Offset: 0x000E69B0
		public SatelliteContractVersionAttribute(string version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this.Version = version;
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x0600475A RID: 18266 RVA: 0x000E87CD File Offset: 0x000E69CD
		public string Version { get; }
	}
}
