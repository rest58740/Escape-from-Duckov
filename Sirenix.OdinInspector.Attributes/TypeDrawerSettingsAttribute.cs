using System;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000074 RID: 116
	[AttributeUsage(384)]
	public class TypeDrawerSettingsAttribute : Attribute
	{
		// Token: 0x04000149 RID: 329
		public Type BaseType;

		// Token: 0x0400014A RID: 330
		public TypeInclusionFilter Filter = TypeInclusionFilter.IncludeAll;
	}
}
