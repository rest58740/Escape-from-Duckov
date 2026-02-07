using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x02000093 RID: 147
	[AttributeUsage(1036)]
	public sealed class fsForwardAttribute : Attribute
	{
		// Token: 0x060005E6 RID: 1510 RVA: 0x000111AD File Offset: 0x0000F3AD
		public fsForwardAttribute(string memberName)
		{
			this.MemberName = memberName;
		}

		// Token: 0x040001D3 RID: 467
		public string MemberName;
	}
}
