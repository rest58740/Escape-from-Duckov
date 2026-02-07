using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000136 RID: 310
	[Obsolete("Use an ABPath with the ABPath.endingCondition field instead")]
	public class XPath : ABPath
	{
		// Token: 0x0600098D RID: 2445 RVA: 0x00034819 File Offset: 0x00032A19
		[Obsolete("Use ABPath.Construct instead")]
		public new static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
		{
			return ABPath.Construct(start, end, callback);
		}
	}
}
