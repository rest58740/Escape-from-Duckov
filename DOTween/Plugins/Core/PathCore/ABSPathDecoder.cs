using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000046 RID: 70
	internal abstract class ABSPathDecoder
	{
		// Token: 0x06000278 RID: 632
		internal abstract void FinalizePath(Path p, Vector3[] wps, bool isClosedPath);

		// Token: 0x06000279 RID: 633
		internal abstract Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints);

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600027A RID: 634
		internal abstract int minInputWaypoints { get; }
	}
}
