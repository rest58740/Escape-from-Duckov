using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drawing
{
	// Token: 0x02000045 RID: 69
	public static class GizmoContext
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000CA61 File Offset: 0x0000AC61
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0000CA6D File Offset: 0x0000AC6D
		public static int selectionSize
		{
			get
			{
				GizmoContext.Refresh();
				return GizmoContext.selectionSizeInternal;
			}
			private set
			{
				GizmoContext.selectionSizeInternal = value;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000CA75 File Offset: 0x0000AC75
		internal static void SetDirty()
		{
			GizmoContext.dirty = true;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00002104 File Offset: 0x00000304
		private static void Refresh()
		{
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000CA7D File Offset: 0x0000AC7D
		public static bool InSelection(Component c)
		{
			return GizmoContext.InSelection(c.transform);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000CA8C File Offset: 0x0000AC8C
		public static bool InSelection(Transform tr)
		{
			GizmoContext.Refresh();
			Transform item = tr;
			while (tr != null)
			{
				if (GizmoContext.selectedTransforms.Contains(tr))
				{
					GizmoContext.selectedTransforms.Add(item);
					return true;
				}
				tr = tr.parent;
			}
			return false;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000CACF File Offset: 0x0000ACCF
		public static bool InActiveSelection(Component c)
		{
			return GizmoContext.InActiveSelection(c.transform);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000CADC File Offset: 0x0000ACDC
		public static bool InActiveSelection(Transform tr)
		{
			return false;
		}

		// Token: 0x040000F9 RID: 249
		private static HashSet<Transform> selectedTransforms = new HashSet<Transform>();

		// Token: 0x040000FA RID: 250
		internal static bool drawingGizmos;

		// Token: 0x040000FB RID: 251
		internal static bool dirty;

		// Token: 0x040000FC RID: 252
		private static int selectionSizeInternal;
	}
}
