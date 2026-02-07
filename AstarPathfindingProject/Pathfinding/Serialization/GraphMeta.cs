using System;
using System.Collections.Generic;

namespace Pathfinding.Serialization
{
	// Token: 0x0200023A RID: 570
	public class GraphMeta
	{
		// Token: 0x06000D8C RID: 3468 RVA: 0x0005538C File Offset: 0x0005358C
		public Type GetGraphType(int index, Type[] availableGraphTypes)
		{
			if (string.IsNullOrEmpty(this.typeNames[index]))
			{
				return null;
			}
			for (int i = 0; i < availableGraphTypes.Length; i++)
			{
				if (availableGraphTypes[i].FullName == this.typeNames[index])
				{
					return availableGraphTypes[i];
				}
			}
			throw new Exception("No graph of type '" + this.typeNames[index] + "' could be created, type does not exist");
		}

		// Token: 0x04000A77 RID: 2679
		public Version version;

		// Token: 0x04000A78 RID: 2680
		public int graphs;

		// Token: 0x04000A79 RID: 2681
		public List<string> guids;

		// Token: 0x04000A7A RID: 2682
		public List<string> typeNames;
	}
}
