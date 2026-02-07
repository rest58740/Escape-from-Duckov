using System;
using System.Diagnostics;

namespace Animancer
{
	// Token: 0x0200001E RID: 30
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class EventNamesAttribute : Attribute
	{
		// Token: 0x0600031D RID: 797 RVA: 0x00008F24 File Offset: 0x00007124
		public EventNamesAttribute(params string[] names)
		{
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00008F2C File Offset: 0x0000712C
		public EventNamesAttribute(Type type)
		{
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00008F34 File Offset: 0x00007134
		public EventNamesAttribute(Type type, string name)
		{
		}
	}
}
