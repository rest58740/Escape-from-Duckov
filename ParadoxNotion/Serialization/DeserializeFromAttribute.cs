using System;

namespace ParadoxNotion.Serialization
{
	// Token: 0x02000085 RID: 133
	public class DeserializeFromAttribute : Attribute
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x00010044 File Offset: 0x0000E244
		public DeserializeFromAttribute(string previousTypeFullName)
		{
			this.previousTypeFullName = previousTypeFullName;
		}

		// Token: 0x040001BA RID: 442
		public readonly string previousTypeFullName;
	}
}
