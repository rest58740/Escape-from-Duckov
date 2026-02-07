using System;
using System.Collections.Generic;

namespace Pathfinding
{
	// Token: 0x0200002D RID: 45
	public interface IUpdatableGraph
	{
		// Token: 0x060001FA RID: 506
		IGraphUpdatePromise ScheduleGraphUpdates(List<GraphUpdateObject> graphUpdates);
	}
}
