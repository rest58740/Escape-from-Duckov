using System;

namespace System.Reflection
{
	// Token: 0x02000881 RID: 2177
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCopyrightAttribute : Attribute
	{
		// Token: 0x06004855 RID: 18517 RVA: 0x000EE02A File Offset: 0x000EC22A
		public AssemblyCopyrightAttribute(string copyright)
		{
			this.Copyright = copyright;
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06004856 RID: 18518 RVA: 0x000EE039 File Offset: 0x000EC239
		public string Copyright { get; }
	}
}
