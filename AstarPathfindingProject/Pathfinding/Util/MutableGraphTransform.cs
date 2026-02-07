using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200028B RID: 651
	public class MutableGraphTransform : GraphTransform
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x0005F438 File Offset: 0x0005D638
		public MutableGraphTransform(Matrix4x4 matrix) : base(matrix)
		{
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0005F441 File Offset: 0x0005D641
		public void SetMatrix(Matrix4x4 matrix)
		{
			base.Set(matrix);
		}
	}
}
