using System;

namespace System.Reflection
{
	// Token: 0x02000889 RID: 2185
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyKeyFileAttribute : Attribute
	{
		// Token: 0x06004868 RID: 18536 RVA: 0x000EE0F0 File Offset: 0x000EC2F0
		public AssemblyKeyFileAttribute(string keyFile)
		{
			this.KeyFile = keyFile;
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06004869 RID: 18537 RVA: 0x000EE0FF File Offset: 0x000EC2FF
		public string KeyFile { get; }
	}
}
