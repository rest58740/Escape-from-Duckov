using System;

namespace System.Reflection
{
	// Token: 0x02000885 RID: 2181
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDescriptionAttribute : Attribute
	{
		// Token: 0x0600485D RID: 18525 RVA: 0x000EE086 File Offset: 0x000EC286
		public AssemblyDescriptionAttribute(string description)
		{
			this.Description = description;
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x0600485E RID: 18526 RVA: 0x000EE095 File Offset: 0x000EC295
		public string Description { get; }
	}
}
