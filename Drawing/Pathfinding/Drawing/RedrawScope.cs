using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x0200002B RID: 43
	public struct RedrawScope : IDisposable
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000849D File Offset: 0x0000669D
		public bool isValid
		{
			get
			{
				return this.id != 0;
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000084A8 File Offset: 0x000066A8
		internal RedrawScope(DrawingData gizmos, int id)
		{
			this.gizmos = gizmos.gizmosHandle;
			this.id = id;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000084BD File Offset: 0x000066BD
		internal RedrawScope(DrawingData gizmos)
		{
			this.gizmos = gizmos.gizmosHandle;
			this.id = RedrawScope.idCounter++;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000084E0 File Offset: 0x000066E0
		internal void Draw()
		{
			if (this.gizmos.IsAllocated)
			{
				DrawingData drawingData = this.gizmos.Target as DrawingData;
				if (drawingData != null)
				{
					drawingData.Draw(this);
				}
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000851C File Offset: 0x0000671C
		public void Rewind()
		{
			GameObject associatedGameObject = null;
			if (this.gizmos.IsAllocated)
			{
				DrawingData drawingData = this.gizmos.Target as DrawingData;
				if (drawingData != null)
				{
					associatedGameObject = drawingData.GetAssociatedGameObject(this);
				}
			}
			this.Dispose();
			this = DrawingManager.GetRedrawScope(associatedGameObject);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000856C File Offset: 0x0000676C
		internal void DrawUntilDispose(GameObject associatedGameObject)
		{
			DrawingData drawingData = this.gizmos.Target as DrawingData;
			if (drawingData != null)
			{
				drawingData.DrawUntilDisposed(this, associatedGameObject);
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000859C File Offset: 0x0000679C
		public void Dispose()
		{
			if (this.gizmos.IsAllocated)
			{
				DrawingData drawingData = this.gizmos.Target as DrawingData;
				if (drawingData != null)
				{
					drawingData.DisposeRedrawScope(this);
				}
			}
			this.gizmos = default(GCHandle);
			this.id = 0;
		}

		// Token: 0x04000084 RID: 132
		internal GCHandle gizmos;

		// Token: 0x04000085 RID: 133
		internal int id;

		// Token: 0x04000086 RID: 134
		private static int idCounter = 1;
	}
}
