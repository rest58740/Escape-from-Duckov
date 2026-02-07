using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	public class PathsD : List<PathD>
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002A4C File Offset: 0x00000C4C
		public PathsD()
		{
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002A54 File Offset: 0x00000C54
		public PathsD(int capacity = 0) : base(capacity)
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002A5D File Offset: 0x00000C5D
		public PathsD(IEnumerable<PathD> paths) : base(paths)
		{
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002A68 File Offset: 0x00000C68
		public string ToString(int precision = 2)
		{
			string text = "";
			foreach (PathD pathD in this)
			{
				text = text + pathD.ToString(precision) + "\n";
			}
			return text;
		}
	}
}
