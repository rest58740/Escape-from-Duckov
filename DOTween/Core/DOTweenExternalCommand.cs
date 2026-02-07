using System;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x02000052 RID: 82
	public static class DOTweenExternalCommand
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002D6 RID: 726 RVA: 0x0000FCB8 File Offset: 0x0000DEB8
		// (remove) Token: 0x060002D7 RID: 727 RVA: 0x0000FCEC File Offset: 0x0000DEEC
		public static event Action<PathOptions, Tween, Quaternion, Transform> SetOrientationOnPath;

		// Token: 0x060002D8 RID: 728 RVA: 0x0000FD1F File Offset: 0x0000DF1F
		internal static void Dispatch_SetOrientationOnPath(PathOptions options, Tween t, Quaternion newRot, Transform trans)
		{
			if (DOTweenExternalCommand.SetOrientationOnPath != null)
			{
				DOTweenExternalCommand.SetOrientationOnPath(options, t, newRot, trans);
			}
		}
	}
}
