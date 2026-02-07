using System;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000130 RID: 304
	public class FleePath : RandomPath
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x000334F8 File Offset: 0x000316F8
		public static FleePath Construct(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback = null)
		{
			FleePath path = PathPool.GetPath<FleePath>();
			path.Setup(start, avoid, searchLength, callback);
			return path;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00033509 File Offset: 0x00031709
		protected void Setup(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback)
		{
			base.Setup(start, searchLength, callback);
			this.aim = start - (avoid - start) * 10f;
		}
	}
}
