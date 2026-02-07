using System;

namespace Pathfinding.Util
{
	// Token: 0x0200027F RID: 639
	public interface IGraphSnapshot : IDisposable
	{
		// Token: 0x06000F29 RID: 3881
		void Restore(IGraphUpdateContext ctx);
	}
}
