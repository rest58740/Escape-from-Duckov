using System;

namespace Shapes
{
	// Token: 0x0200004F RID: 79
	public static class DashTypeExtensions
	{
		// Token: 0x06000C7F RID: 3199 RVA: 0x00018FD4 File Offset: 0x000171D4
		public static bool HasModifier(this DashType type)
		{
			return type == DashType.Angled || type == DashType.Chevron;
		}
	}
}
