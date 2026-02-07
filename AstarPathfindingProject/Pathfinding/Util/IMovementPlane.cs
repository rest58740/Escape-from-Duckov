using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000285 RID: 645
	public interface IMovementPlane
	{
		// Token: 0x06000F58 RID: 3928
		Vector2 ToPlane(Vector3 p);

		// Token: 0x06000F59 RID: 3929
		Vector2 ToPlane(Vector3 p, out float elevation);

		// Token: 0x06000F5A RID: 3930
		Vector3 ToWorld(Vector2 p, float elevation = 0f);

		// Token: 0x06000F5B RID: 3931
		SimpleMovementPlane ToSimpleMovementPlane();
	}
}
