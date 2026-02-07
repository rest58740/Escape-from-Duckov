using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000065 RID: 101
	public static class RectPivotExtensions
	{
		// Token: 0x06000CA6 RID: 3238 RVA: 0x000195AA File Offset: 0x000177AA
		public static Rect GetRect(this RectPivot pivot, Vector2 size)
		{
			return pivot.GetRect(size.x, size.y);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000195BE File Offset: 0x000177BE
		public static Rect GetRect(this RectPivot pivot, float w, float h)
		{
			if (pivot != RectPivot.Corner)
			{
				return new Rect(-w / 2f, -h / 2f, w, h);
			}
			return new Rect(0f, 0f, w, h);
		}
	}
}
