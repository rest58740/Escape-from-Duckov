using System;

namespace Pathfinding
{
	// Token: 0x02000114 RID: 276
	public interface IPathModifier
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060008CF RID: 2255
		int Order { get; }

		// Token: 0x060008D0 RID: 2256
		void Apply(Path path);

		// Token: 0x060008D1 RID: 2257
		void PreProcess(Path path);
	}
}
