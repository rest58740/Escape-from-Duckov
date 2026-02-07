using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	public struct ControlPoint
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0000E35E File Offset: 0x0000C55E
		public ControlPoint(Vector3 a, Vector3 b)
		{
			this.a = a;
			this.b = b;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000E36E File Offset: 0x0000C56E
		public static ControlPoint operator +(ControlPoint cp, Vector3 v)
		{
			return new ControlPoint(cp.a + v, cp.b + v);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000E390 File Offset: 0x0000C590
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.a.ToString(),
				" | ",
				this.b.ToString(),
				"]"
			});
		}

		// Token: 0x04000125 RID: 293
		public Vector3 a;

		// Token: 0x04000126 RID: 294
		public Vector3 b;
	}
}
