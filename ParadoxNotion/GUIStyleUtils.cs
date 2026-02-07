using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000079 RID: 121
	public static class GUIStyleUtils
	{
		// Token: 0x06000493 RID: 1171 RVA: 0x0000CF82 File Offset: 0x0000B182
		public static GUIStyle Margin(this GUIStyle style, int left, int right, int top, int bottom)
		{
			style.margin = new RectOffset(left, right, top, bottom);
			return style;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000CF95 File Offset: 0x0000B195
		public static GUIStyle Padding(this GUIStyle style, int left, int right, int top, int bottom)
		{
			style.padding = new RectOffset(left, right, top, bottom);
			return style;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000CFA8 File Offset: 0x0000B1A8
		public static GUIStyle Border(this GUIStyle style, int left, int right, int top, int bottom)
		{
			style.border = new RectOffset(left, right, top, bottom);
			return style;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000CFBB File Offset: 0x0000B1BB
		public static GUIStyle Overflow(this GUIStyle style, int left, int right, int top, int bottom)
		{
			style.overflow = new RectOffset(left, right, top, bottom);
			return style;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000CFCE File Offset: 0x0000B1CE
		public static GUIStyle TextAlignment(this GUIStyle style, TextAnchor anchor)
		{
			style.alignment = anchor;
			return style;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000CFD8 File Offset: 0x0000B1D8
		public static GUIStyle RichText(this GUIStyle style, bool rich)
		{
			style.richText = rich;
			return style;
		}
	}
}
