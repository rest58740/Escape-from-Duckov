using System;

namespace System.Reflection
{
	// Token: 0x02000884 RID: 2180
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDelaySignAttribute : Attribute
	{
		// Token: 0x0600485B RID: 18523 RVA: 0x000EE06F File Offset: 0x000EC26F
		public AssemblyDelaySignAttribute(bool delaySign)
		{
			this.DelaySign = delaySign;
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x0600485C RID: 18524 RVA: 0x000EE07E File Offset: 0x000EC27E
		public bool DelaySign { get; }
	}
}
