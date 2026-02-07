using System;
using System.Runtime.InteropServices;

namespace Drawing
{
	// Token: 0x0200002A RID: 42
	public struct RedrawScope : IDisposable
	{
		// Token: 0x060002CC RID: 716 RVA: 0x0000A2E1 File Offset: 0x000084E1
		internal RedrawScope(DrawingData gizmos, int id)
		{
			this.gizmos = gizmos.gizmosHandle;
			this.id = id;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000A2F6 File Offset: 0x000084F6
		internal RedrawScope(DrawingData gizmos)
		{
			this.gizmos = gizmos.gizmosHandle;
			this.id = RedrawScope.idCounter++;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000A318 File Offset: 0x00008518
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

		// Token: 0x060002CF RID: 719 RVA: 0x0000A352 File Offset: 0x00008552
		public void Rewind()
		{
			this.Dispose();
			this = DrawingManager.GetRedrawScope();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000A368 File Offset: 0x00008568
		internal void DrawUntilDispose()
		{
			DrawingData drawingData = this.gizmos.Target as DrawingData;
			if (drawingData != null)
			{
				drawingData.DrawUntilDisposed(this);
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000A398 File Offset: 0x00008598
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
		}

		// Token: 0x0400007D RID: 125
		internal GCHandle gizmos;

		// Token: 0x0400007E RID: 126
		internal int id;

		// Token: 0x0400007F RID: 127
		private static int idCounter = 1;
	}
}
