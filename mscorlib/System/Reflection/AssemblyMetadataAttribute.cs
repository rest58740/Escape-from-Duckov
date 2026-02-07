using System;

namespace System.Reflection
{
	// Token: 0x0200088B RID: 2187
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public sealed class AssemblyMetadataAttribute : Attribute
	{
		// Token: 0x0600486C RID: 18540 RVA: 0x000EE11E File Offset: 0x000EC31E
		public AssemblyMetadataAttribute(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600486D RID: 18541 RVA: 0x000EE134 File Offset: 0x000EC334
		public string Key { get; }

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x0600486E RID: 18542 RVA: 0x000EE13C File Offset: 0x000EC33C
		public string Value { get; }
	}
}
