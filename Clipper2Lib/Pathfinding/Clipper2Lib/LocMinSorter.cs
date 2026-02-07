using System;
using System.Collections.Generic;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000018 RID: 24
	internal struct LocMinSorter : IComparer<LocalMinima>
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00005836 File Offset: 0x00003A36
		public readonly int Compare(LocalMinima locMin1, LocalMinima locMin2)
		{
			return locMin2.vertex.pt.Y.CompareTo(locMin1.vertex.pt.Y);
		}
	}
}
