using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000061 RID: 97
	public static class Vector2IntExtensions
	{
		// Token: 0x0600036F RID: 879 RVA: 0x00010555 File Offset: 0x0000E755
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 ToInt2(this Vector2Int v)
		{
			return new int2(v.x, v.y);
		}
	}
}
