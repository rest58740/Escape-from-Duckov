using System;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x020001FE RID: 510
	public class CustomGridGraphRuleEditorAttribute : Attribute
	{
		// Token: 0x06000CC5 RID: 3269 RVA: 0x000501D5 File Offset: 0x0004E3D5
		public CustomGridGraphRuleEditorAttribute(Type type, string name)
		{
			this.type = type;
			this.name = name;
		}

		// Token: 0x04000958 RID: 2392
		public Type type;

		// Token: 0x04000959 RID: 2393
		public string name;
	}
}
