using System;
using UnityEngine;

namespace JoshH.Extensions
{
	// Token: 0x0200004F RID: 79
	public static class Vector2Extension
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000BEE0 File Offset: 0x0000A0E0
		public static Vector2 Rotate(this Vector2 v, float degrees)
		{
			float num = Mathf.Sin(degrees * 0.017453292f);
			float num2 = Mathf.Cos(degrees * 0.017453292f);
			float x = v.x;
			float y = v.y;
			v.x = num2 * x - num * y;
			v.y = num * x + num2 * y;
			return v;
		}
	}
}
