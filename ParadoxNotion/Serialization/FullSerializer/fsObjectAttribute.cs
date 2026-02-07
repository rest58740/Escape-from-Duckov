using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x0200009F RID: 159
	[AttributeUsage(12)]
	public sealed class fsObjectAttribute : Attribute
	{
		// Token: 0x040001D8 RID: 472
		public Type Converter;

		// Token: 0x040001D9 RID: 473
		public Type Processor;
	}
}
