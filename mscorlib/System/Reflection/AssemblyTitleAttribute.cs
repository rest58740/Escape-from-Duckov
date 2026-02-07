using System;

namespace System.Reflection
{
	// Token: 0x0200088F RID: 2191
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyTitleAttribute : Attribute
	{
		// Token: 0x06004874 RID: 18548 RVA: 0x000EE181 File Offset: 0x000EC381
		public AssemblyTitleAttribute(string title)
		{
			this.Title = title;
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06004875 RID: 18549 RVA: 0x000EE190 File Offset: 0x000EC390
		public string Title { get; }
	}
}
