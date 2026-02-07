using System;

namespace Pathfinding
{
	// Token: 0x020000AE RID: 174
	internal interface IPathInternals
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600057F RID: 1407
		PathHandler PathHandler { get; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000580 RID: 1408
		// (set) Token: 0x06000581 RID: 1409
		bool Pooled { get; set; }

		// Token: 0x06000582 RID: 1410
		void AdvanceState(PathState s);

		// Token: 0x06000583 RID: 1411
		void OnEnterPool();

		// Token: 0x06000584 RID: 1412
		void Reset();

		// Token: 0x06000585 RID: 1413
		void ReturnPath();

		// Token: 0x06000586 RID: 1414
		void PrepareBase(PathHandler handler);

		// Token: 0x06000587 RID: 1415
		void Prepare();

		// Token: 0x06000588 RID: 1416
		void Cleanup();

		// Token: 0x06000589 RID: 1417
		void CalculateStep(long targetTick);

		// Token: 0x0600058A RID: 1418
		string DebugString(PathLog logMode);
	}
}
