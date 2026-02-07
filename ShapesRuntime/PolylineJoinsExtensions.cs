using System;

namespace Shapes
{
	// Token: 0x02000062 RID: 98
	internal static class PolylineJoinsExtensions
	{
		// Token: 0x06000C9A RID: 3226 RVA: 0x000193CB File Offset: 0x000175CB
		public static bool HasJoinMesh(this PolylineJoins join)
		{
			switch (join)
			{
			case PolylineJoins.Simple:
				return false;
			case PolylineJoins.Miter:
				return false;
			case PolylineJoins.Round:
				return true;
			case PolylineJoins.Bevel:
				return true;
			default:
				throw new ArgumentOutOfRangeException("join", join, null);
			}
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x000193FE File Offset: 0x000175FE
		public static bool HasSimpleJoin(this PolylineJoins join)
		{
			switch (join)
			{
			case PolylineJoins.Simple:
				return false;
			case PolylineJoins.Miter:
				return false;
			case PolylineJoins.Round:
				return false;
			case PolylineJoins.Bevel:
				return true;
			default:
				throw new ArgumentOutOfRangeException("join", join, null);
			}
		}
	}
}
