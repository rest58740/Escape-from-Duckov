using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x02000240 RID: 576
	[Preserve]
	internal class SerializableAnimationCurve
	{
		// Token: 0x04000A7E RID: 2686
		public WrapMode preWrapMode;

		// Token: 0x04000A7F RID: 2687
		public WrapMode postWrapMode;

		// Token: 0x04000A80 RID: 2688
		public Keyframe[] keys;
	}
}
