using System;

namespace Shapes
{
	// Token: 0x02000054 RID: 84
	internal static class DiscTypeExtensions
	{
		// Token: 0x06000C81 RID: 3201 RVA: 0x00018FED File Offset: 0x000171ED
		public static bool HasThickness(this DiscType type)
		{
			return type == DiscType.Ring || type == DiscType.Arc;
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00018FF9 File Offset: 0x000171F9
		public static bool HasSector(this DiscType type)
		{
			return type == DiscType.Pie || type == DiscType.Arc;
		}
	}
}
