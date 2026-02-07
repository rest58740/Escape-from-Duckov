using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200000B RID: 11
	[NullableContext(1)]
	[Nullable(0)]
	public class PathD : List<PointD>
	{
		// Token: 0x0600003B RID: 59 RVA: 0x000029AF File Offset: 0x00000BAF
		public PathD()
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000029B7 File Offset: 0x00000BB7
		public PathD(int capacity = 0) : base(capacity)
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000029C0 File Offset: 0x00000BC0
		public PathD(IEnumerable<PointD> path) : base(path)
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000029CC File Offset: 0x00000BCC
		public string ToString(int precision = 2)
		{
			string text = "";
			foreach (PointD pointD in this)
			{
				text = text + pointD.ToString(precision) + ", ";
			}
			if (text != "")
			{
				text = text.Remove(text.Length - 2);
			}
			return text;
		}
	}
}
