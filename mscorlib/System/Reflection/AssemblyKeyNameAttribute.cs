using System;

namespace System.Reflection
{
	// Token: 0x0200088A RID: 2186
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyKeyNameAttribute : Attribute
	{
		// Token: 0x0600486A RID: 18538 RVA: 0x000EE107 File Offset: 0x000EC307
		public AssemblyKeyNameAttribute(string keyName)
		{
			this.KeyName = keyName;
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600486B RID: 18539 RVA: 0x000EE116 File Offset: 0x000EC316
		public string KeyName { get; }
	}
}
