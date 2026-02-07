using System;

namespace Pathfinding.Serialization
{
	// Token: 0x0200023F RID: 575
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class JsonDynamicTypeAliasAttribute : Attribute
	{
		// Token: 0x06000D94 RID: 3476 RVA: 0x0005541F File Offset: 0x0005361F
		public JsonDynamicTypeAliasAttribute(string alias, Type type)
		{
			this.alias = alias;
			this.type = type;
		}

		// Token: 0x04000A7C RID: 2684
		public string alias;

		// Token: 0x04000A7D RID: 2685
		public Type type;
	}
}
