using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x02000046 RID: 70
	public static class GizmoContext
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000AD75 File Offset: 0x00008F75
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0000AD81 File Offset: 0x00008F81
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

		// Token: 0x0600024F RID: 591 RVA: 0x0000AD89 File Offset: 0x00008F89
		internal static void SetDirty()
		{
			GizmoContext.dirty = true;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00002104 File Offset: 0x00000304
		private static void Refresh()
		{
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000AD91 File Offset: 0x00008F91
		public static bool InSelection(Component c)
		{
			return GizmoContext.InSelection(c.transform);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000ADA0 File Offset: 0x00008FA0
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

		// Token: 0x06000253 RID: 595 RVA: 0x0000ADE3 File Offset: 0x00008FE3
		public static bool InActiveSelection(Component c)
		{
			return GizmoContext.InActiveSelection(c.transform);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public static bool InActiveSelection(Transform tr)
		{
			return false;
		}

		// Token: 0x04000100 RID: 256
		private static HashSet<Transform> selectedTransforms = new HashSet<Transform>();

		// Token: 0x04000101 RID: 257
		internal static bool drawingGizmos;

		// Token: 0x04000102 RID: 258
		internal static bool dirty;

		// Token: 0x04000103 RID: 259
		private static int selectionSizeInternal;
	}
}
