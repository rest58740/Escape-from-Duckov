using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200028A RID: 650
	public interface ITransform
	{
		// Token: 0x06000F81 RID: 3969
		Vector3 Transform(Vector3 position);

		// Token: 0x06000F82 RID: 3970
		Vector3 InverseTransform(Vector3 position);
	}
}
